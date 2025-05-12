using AutoServiceProject.Models;
using AutoServiceProject.Services;

public static class CartHelper
{
    public static int GetCartCount(HttpContext context)
    {
        var cart = context.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        return cart.Sum(item => item.Quantity);
    }
}