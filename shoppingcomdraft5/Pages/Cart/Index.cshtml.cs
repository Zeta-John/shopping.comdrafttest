using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Infrastructure;
using shoppingcomdraft5.Models;
using shoppingcomdraft5.Models.ViewModels;

namespace shoppingcomdraft5.Pages.Listings
{

    public class CartModel : PageModel

    {
        private readonly shoppingcomdraft5Context _context;

        public CartModel(shoppingcomdraft5Context context)
        {
            _context = context;
        }

        public CartViewModel CartVM { get; set; }

        public void OnGet()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };
        }
    }
}
