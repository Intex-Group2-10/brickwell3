using brickwell2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using brickwell2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using System.Drawing.Printing;
using System.Security.Cryptography;

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

        public IActionResult Index()
        {

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

            var viewModel = new ItemRecommendationViewModel
            {
                Product = productToDisplay
            };

            return View(viewModel);
        }

        public IActionResult Products(int pageNum, string? productCategory, string? productPrimaryColor)
        {
            int pageSize = 6;

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
                    TotalProducts = productQuery.Count() 
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
