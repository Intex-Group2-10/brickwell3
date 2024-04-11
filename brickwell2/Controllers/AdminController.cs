using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using brickwell2.Models;
using System;
using System.Diagnostics;
using brickwell2.Models.ViewModels;

namespace brickwell2.Controllers;

public class AdminController : Controller
{
    private ILegoRepository _repo;
    private ILegoSecurityRepository _securityRepo;
    
    public AdminController(ILegoRepository legoInfo, ILegoSecurityRepository secureInfo) 
    {
        _repo = legoInfo;
        _securityRepo = secureInfo;
    }
    public IActionResult AdminOrders()
    {
        var viewOrders = _repo.Orders.ToList();
        return View(viewOrders);
    }

    public IActionResult AdminProducts(int pageNum)
    {
        int pageSize = 10;
        var product = new PaginationListViewModel
        {
            Products = _repo.Products
                .OrderBy(x => x.ProductId)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize,
                TotalProducts = _repo.Products.Count()
            }
        };
        return View(product);
    }
    
    [HttpGet]
    public IActionResult EditProduct ()
    {
        ViewBag.categories = _repo.Products.ToList();
        return View("AdminUsers");
    }
    
    [HttpPost]
    public IActionResult EditProducts (Models.Product product)
    {
        _repo.AddProduct(product);
        return RedirectToAction("AdminUsers");
    }
    
    [HttpGet]
    public IActionResult DeleteProduct(int id)
    {
        var recordToDelete = _repo.Products
            .Single(x => x.ProductId == id);

        return View(recordToDelete);
    }

    [HttpPost]
    public IActionResult DeleteProduct (Models.Customer deleteInfo)
    {
        _repo.DeleteCustomer(deleteInfo);

        return RedirectToAction("AdminUsers");
    }

    public IActionResult AdminUsers(int pageNum)
    {
        int pageSize = 10;
        var user = new PaginationListViewModel
        {
            AspNetUsers = _securityRepo.AspNetUsers
                .OrderBy(x => x.UserName)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize,
                TotalProducts = _securityRepo.AspNetUsers.Count()
            }
        };
        return View(user);
    }
    
    [HttpGet]
    public IActionResult EditCustomer ()
    {
        ViewBag.categories = _repo.Customers.ToList();
        return View("AdminUsers");
    }
    
    [HttpPost]
    public IActionResult EditCustomer (Models.Customer customer)
    {
            return RedirectToAction("AdminUsers");
    }
    
    [HttpGet]
    public IActionResult DeleteCustomer(int id)
    {
        var recordToDelete = _repo.Customers
            .Single(x => x.CustomerId == id);

        return View(recordToDelete);
    }

    [HttpPost]
    public IActionResult DeleteCustomer (Models.Customer deleteInfo)
    {
        return RedirectToAction("AdminUsers");
    }
}