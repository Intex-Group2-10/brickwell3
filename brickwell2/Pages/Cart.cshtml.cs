using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using brickwell2.Infrastructure;
using brickwell2.Models;

namespace brickwell2.Pages
{
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        // public FraudPrediction FraudPrediction;

        public Cart Cart { get; set; }

        public CartModel(ILegoRepository temp, Cart cartService)
        {
            _repo = temp;
            Cart = cartService;
        }
        
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }
        public IActionResult OnPost(int ProductId, string returnUrl) 
        {
            Product prod = _repo.Products
                .FirstOrDefault(x => x.ProductId == ProductId);
            if (prod != null)
            {
                //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(prod, 1);
                //HttpContext.Session.SetJson("cart", Cart);
            }
            return RedirectToPage (new {returnUrl = returnUrl});
        }

        public IActionResult OnPostRemove (int productId, string returnUrl) 
        {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == productId).Product);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
