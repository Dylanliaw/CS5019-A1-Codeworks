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
            const price = parseFloat(row.querySelector("td:nth-child(2)").textContent.replace('$', '').trim());
            const quantity = parseInt(row.querySelector("td:nth-child(3)").textContent.trim());
            total += price * quantity;
        });

        // Update the total element
        const totalElement = document.querySelector("h3");
        if (totalElement) {
            totalElement.textContent = "Total: $" + total.toFixed(2);
        }
    }
});
