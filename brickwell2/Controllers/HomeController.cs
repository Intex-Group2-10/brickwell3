using brickwell2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using brickwell2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace brickwell2.Controllers
{ 
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ILegoRepository _repo;
        private ILegoSecurityRepository _securityRepository;
        public HomeController(ILegoRepository temp, ILegoSecurityRepository securetemp)
        {
            _repo = temp;
            _securityRepository = securetemp;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            //List<Product> productsToDisplay = new List<Product>();
            //if (User.Identity.IsAuthenticated)
            //{
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userExists = await _repo.UserCusts.AnyAsync(u => u.UserId == userId);

            // Use the result in an if statement
            if (userExists)
            {
                // Code to execute if the user ID exists in the UserCust table
                var custid = await _repo.UserCusts.Where(u => u.UserId == userId).Select(u => u.CustomerId).FirstOrDefaultAsync();
                var transid = await _repo.Orders.Where(u => u.CustomerId == custid).Select(u => u.TransactionId).FirstOrDefaultAsync();
                var prodid = await _repo.LineItems.Where(u => u.TransactionId == transid).Select(u => u.ProductId).FirstOrDefaultAsync();

                var recommendation = _repo.UserBasedRecommendations
                    .Where(r => r.ProductId == prodid)
                    .FirstOrDefault();

                // Fetch details for each recommendation
                ViewBag.Recommendation1 = _repo.Products.Single(p => p.ProductId == recommendation.RecommendedProduct1);
                ViewBag.Recommendation2 = _repo.Products.Single(p => p.ProductId == recommendation.RecommendedProduct2);
                ViewBag.Recommendation3 = _repo.Products.Single(p => p.ProductId == recommendation.RecommendedProduct3);

                var viewModel = new UserRecommendationViewModel();

                return View(viewModel);

            }
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult About()
        {
            return View();
        }
        
        public IActionResult Cart()
        {
            return View();
        }
        
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ProductDetail(int id)
        {
            var productToDisplay = _repo.Products.Single(x => x.ProductId == id);

            var recommendations = _repo.ItemBasedRecommendations
                .Where(r => r.ProductId == id)
                .FirstOrDefault();

            // Fetch details for each recommendation
            ViewBag.Recommendation1 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation1);
            ViewBag.Recommendation2 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation2);
            ViewBag.Recommendation3 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation3);
            ViewBag.Recommendation4 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation4);
            ViewBag.Recommendation5 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation5);
            ViewBag.Recommendation6 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation6);
            ViewBag.Recommendation7 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation7);
            ViewBag.Recommendation8 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation8);
            ViewBag.Recommendation9 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation9);
            ViewBag.Recommendation10 = _repo.Products.Single(p => p.ProductId == recommendations.Recommendation10);
            // Similarly fetch and store details for Recommendation3 to Recommendation10

            var viewModel = new ItemRecommendationViewModel
            {
                Product = productToDisplay
            };

            return View(viewModel);
        }

        // public IActionResult ProductCart(int id)
        // {
        //     var productToDisplay = _repo.Products
        //         .Single(x => x.ProductId == id);
        //     return View("ProductDetail");
        // }

        public IActionResult Test()
        {
            var viewUsers = _securityRepository.AspNetUsers.ToList();
            return View(viewUsers);
        }

        //public IActionResult Products(int pageNum, string? productCategory)
        //{
        //    int pageSize = 3;

        //    var productObject = new PaginationListViewModel
        //    {
        //        Products = _repo.Products
        //            .Where(x => x.Category == productCategory || productCategory == null)
        //            .OrderBy(x => x.Name)
        //            .Skip((pageNum - 1) * pageSize)
        //            .Take(pageSize),

        //        PaginationInfo = new PaginationInfo
        //        {
        //            CurrentPage = pageNum,
        //            ProductsPerPage = pageSize,
        //            TotalProducts = productCategory == null ? _repo.Products.Count() : _repo.Products.Where(x => x.Category == productCategory).Count()
        //        },

        //    };
        //    return View(productObject);
        //}

        public IActionResult Products(int pageNum, string? productCategory, string? productPrimaryColor)
        {
            int pageSize = 6;

            // Ensure pageNum is at least 1
            pageNum = Math.Max(pageNum, 1);

            var productQuery = _repo.Products
                                    .Where(x => (x.Category == productCategory || productCategory == null)
                                                && (x.PrimaryColor == productPrimaryColor || productPrimaryColor == null))
                                    .OrderBy(x => x.Name);

            var productObject = new PaginationListViewModel
            {
                Products = productQuery
                            .Skip((pageNum - 1) * pageSize)
                            .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ProductsPerPage = pageSize,
                    TotalProducts = productCategory == null
                                     ? productQuery.Count()
                                     : productQuery.Where(x => x.Category == productCategory).Count()
                },
            };
            return View(productObject);
        }




        public IActionResult ContactUs()
        {
            return View();
        }
        
    }
}
