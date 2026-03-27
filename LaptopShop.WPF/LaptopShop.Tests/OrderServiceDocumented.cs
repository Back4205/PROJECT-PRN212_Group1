// Lấy Order từ User
user = new User();
customer = customer.GetCustomerByUserId(userId);
cart = cart.GetOrDataByCustomer(customer.CustomerId);
cartItems = new List<Order>(orderItem);
totalAmount = cart.GetAllProductByCustomerID(cart.CartId);
print("Order: " + orderRepository.GetOrDataByUserId(customer.CustomerId));