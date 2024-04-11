using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using brickwell2.Models;
using System;
using System.Diagnostics;
using brickwell2.Models.ViewModels;
using SQLitePCL;

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
    
    public IActionResult AdminOrders(int pageNum)
    {
        int pageSize = 150;
        var order = new PaginationListViewModel
        {
            Orders = _repo.Orders
                .Where(x => x.Fraud == 1) // Filter orders where fraud equals 1
                .OrderByDescending(x => x.TransactionId) // Order by TransactionId (most recent first)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize,
                TotalProducts = _repo.Orders.Where(x => x.Fraud == 1).Count() // Count of all orders (including those where fraud != 1)
            }
        };
        return View(order);
    }
    
    [HttpGet]
    public IActionResult EditProduct (int id)
    {
        var recordToEdit = _repo.Products
            .Single(x => x.ProductId == id);

        return View("AdminUsers", recordToEdit);
        // ViewBag.categories = _repo.Products.ToList();
        // return View("AdminUsers");
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
        // _repo.DeleteCustomer(deleteInfo);

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
    public IActionResult EditUser (string id)
    {
        var recordToEdit = _securityRepo.AspNetUsers
            .Single(x => x.Id == id);
        // ViewBag.users = _repo.Users.ToList();
        return View(recordToEdit);
    }
    
    [HttpPost]
    public IActionResult EditUser(Models.AspNetUser user)
    {
        _securityRepo.EditUser(user);
        // _securityRepo.SaveChanges();
            return RedirectToAction("AdminUsers");
    }
    
    [HttpGet]
    public IActionResult DeleteUser(string id)
    {
        var recordToDelete = _securityRepo.AspNetUsers
            .Single(x => x.Id == id);

        return View(recordToDelete);
    }

    [HttpPost]
    public IActionResult DeleteUser (Models.AspNetUser deleteInfo)
    {
        return RedirectToAction("AdminUsers");
    }
}