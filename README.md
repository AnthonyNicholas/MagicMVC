# MagicMVC
MagicMVC

This is a .net core MVC project that provides a stock system for a franchise.  

Features:

Login as Owner/Customer/Franchisee with roles.  Users can login via social media (Google).

Owner can:
1. Display owner’s inventory.
2. Display stock requests.
3. Process a stock request - a stock request that can’t be satisfied should again be
rejected (but not deleted from the database).
4. Set owner inventory item stock - user can enter the amount to set the stock level.

Franchise Holders can:
1. Display store’s inventory.
2. Create a new stock request - user can enter the quantity for the stock request.
3. Create stock request for a new inventory item - user can enter the quantity for the
stock request.

Customers can:
1. Display stores.
2. Display a store’s inventory and allow filtering of the products by name.
3. Add a product to the shopping cart - the shopping cart should support having multiple different products of varying quantity and products from different stores.
4. Shopping cart - display the products added to the cart and allow removing items from the cart. With at least one product in the cart the customer can checkout.
5. Checkout - review the products being purchased and enter credit card details. The credit card details only need to be validated and don’t have to be saved to the
database.
6. Order confirmation showing a receipt of what was bought, including the newly generated Order ID.
7. Order history - display orders made by the customer. The data processing for this is implemented using AngularJS.
