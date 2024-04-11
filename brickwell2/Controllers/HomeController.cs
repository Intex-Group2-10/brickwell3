using brickwell2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using brickwell2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using System.Drawing.Printing;

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
            var productToDisplay = _repo.Products
                .Single(x => x.ProductId == id);
            return View(productToDisplay);
        }

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

        public IActionResult Products(int pageNum, string? productCategory)
        {
            int pageSize = 10;

            // Ensure pageNum is at least 1
            pageNum = Math.Max(pageNum, 1);

            var productQuery = _repo.Products
                                .Where(x => x.Category == productCategory || productCategory == null)
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


    }
}
