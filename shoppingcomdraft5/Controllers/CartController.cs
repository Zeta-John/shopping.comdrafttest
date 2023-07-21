using Microsoft.AspNetCore.Mvc;
using shoppingcomdraft5.Data;
using shoppingcomdraft5.Infrastructure;
using shoppingcomdraft5.Models;
using shoppingcomdraft5.Models.ViewModels;

namespace shoppingcomdraft5.Controllers
{
    public class CartController : Controller
    {
        private readonly shoppingcomdraft5Context _context;

        public CartController(shoppingcomdraft5Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Add(int id)
        {
            Listing listing = await _context.Listing.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.FirstOrDefault(c => c.ListingId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem(listing));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.FirstOrDefault(c => c.ListingId == id);

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(l => l.ListingId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }            

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(l => l.ListingId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");
        }
    }
}
