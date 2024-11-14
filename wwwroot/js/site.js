document.addEventListener("DOMContentLoaded", function () {
    // Select all remove buttons
    const removeButtons = document.querySelectorAll(".remove-item");

    // Add event listener to each remove button
    removeButtons.forEach(button => {
        button.addEventListener("click", function () {
            const productId = this.getAttribute("data-product-id");

            // Make an AJAX POST request to remove the item
            fetch('/Customer/Cart?handler=RemoveItem', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({ productId: productId })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        // Remove the item from the cart table
                        const cartItemRow = document.getElementById("cart-item-" + productId);
                        if (cartItemRow) {
                            cartItemRow.remove();
                        }

                        // Update the total price
                        updateTotal();
                    } else {
                        alert("Error: " + data.message);
                    }
                })
                .catch(error => {
                    alert("An error occurred while removing the item.");
                });
        });
    });

    // Update the total price after an item is removed
    function updateTotal() {
        let total = 0;
        const rows = document.querySelectorAll("tr[id^='cart-item-']");
        rows.forEach(row => {
            const price = parseFloat(row.querySelector("td:nth-child(3)").textContent.replace('$', '').trim());
            const quantity = parseInt(row.querySelector("td:nth-child(4)").textContent.trim());
            total += price * quantity;
        });

        // Update the total element
        const totalElement = document.querySelector("h3:contains('Total')");
        if (totalElement) {
            totalElement.textContent = "Total: $" + total.toFixed(2);
        }
    }

    // Live search function for products (NavBar)
    window.liveSearch = function () {
        const searchQuery = document.getElementById("searchInput").value.trim();

        // Send the search query to the server via AJAX (using Fetch API)
        fetch(`/Customer/Search?searchQuery=${encodeURIComponent(searchQuery)}`)
            .then(response => response.json())
            .then(data => {
                // Update the search results dropdown
                displaySearchResults(data);
            })
            .catch(error => {
                console.error("Error fetching filtered products:", error);
            });
    };

    // Display search results in the dropdown
    function displaySearchResults(products) {
        const resultsContainer = document.getElementById("searchResults");
        resultsContainer.innerHTML = ''; // Clear previous results

        if (products.length > 0) {
            products.forEach(product => {
                const resultItem = document.createElement('div');
                resultItem.classList.add('dropdown-item');
                resultItem.innerHTML = `
                    <a href="/Customer/ProductDetail?id=${product.id}">
                        <img src="${product.imageUrl}" alt="${product.name}" width="30" height="30" />
                        ${product.name} - ${product.price}
                    </a>
                `;
                resultsContainer.appendChild(resultItem);
            });

            // Show the dropdown menu
            resultsContainer.style.display = 'block';
        } else {
            resultsContainer.innerHTML = '<div class="dropdown-item">No results found</div>';
            resultsContainer.style.display = 'block';
        }
    }

    // Live search function for Admin products (Admin Page)
    if (document.getElementById("adminSearchInput")) {
        document.getElementById("adminSearchInput").addEventListener('input', function () {
            const searchQuery = this.value.trim();

            // Send the search query to the server via AJAX (using Fetch API)
            fetch(`/Admin?searchQuery=${encodeURIComponent(searchQuery)}`)
                .then(response => response.text())
                .then(data => {
                    // Update the products table in the admin page
                    const tableContainer = document.querySelector(".admin-table-container");
                    tableContainer.innerHTML = data;
                })
                .catch(error => {
                    console.error("Error fetching filtered products:", error);
                });
        });
    }
});

// site.js

document.addEventListener("DOMContentLoaded", function () {
    // Live search for navbar
    window.liveSearch = function () {
        const searchQuery = document.getElementById("navSearchInput").value.trim();

        // Send the search query to the server via AJAX (using Fetch API)
        fetch(`/Customer/Product?searchQuery=${encodeURIComponent(searchQuery)}`)
            .then(response => response.json())
            .then(data => {
                // Update the search results dropdown
                displaySearchResults(data);
            })
            .catch(error => {
                console.error("Error fetching filtered products:", error);
            });
    };

    // Display search results in the dropdown
    function displaySearchResults(products) {
        const resultsContainer = document.getElementById("navSearchResults");
        resultsContainer.innerHTML = ''; // Clear previous results

        if (products.length > 0) {
            products.forEach(product => {
                const resultItem = document.createElement('div');
                resultItem.classList.add('dropdown-item');
                resultItem.innerHTML = `
                    <a href="/Customer/ProductDetail?id=${product.productId}">
                        <img src="${product.imageUrl}" alt="${product.itemName}" width="30" height="30" />
                        ${product.itemName} - ${product.sellingPrice}
                    </a>
                `;
                resultsContainer.appendChild(resultItem);
            });

            // Show the dropdown menu
            resultsContainer.style.display = 'block';
        } else {
            resultsContainer.innerHTML = '<div class="dropdown-item">No results found</div>';
            resultsContainer.style.display = 'block';
        }
    }
});


// Search functionality (AJAX-based)
document.getElementById("searchQuery").addEventListener("input", function () {
    var query = this.value;

    fetch(`/Customer/Product?searchQuery=${query}`)
        .then(response => response.text())
        .then(html => {
            document.getElementById("productGrid").innerHTML = html;
        });
});