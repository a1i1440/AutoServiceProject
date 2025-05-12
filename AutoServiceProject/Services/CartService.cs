using AutoServiceProject.Models;

namespace AutoServiceProject.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartItem> GetCartItems()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cart = session.GetObjectFromJson<List<CartItem>>(CartKey);
            return cart ?? new List<CartItem>();
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCartItems();
            var existing = cart.FirstOrDefault(i => i.SparePartId == item.SparePartId);
            if (existing != null)
                existing.Quantity += item.Quantity;
            else
                cart.Add(item);

            _httpContextAccessor.HttpContext.Session.SetObjectAsJson(CartKey, cart);
        }

        public void RemoveFromCart(int sparePartId)
        {
            var cart = GetCartItems();
            cart.RemoveAll(i => i.SparePartId == sparePartId);
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson(CartKey, cart);
        }

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove(CartKey);
        }
    }
}
