using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using brickwell2.Models;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using brickwell2.Models.ViewModels;
using SQLitePCL;
using Microsoft.AspNetCore.Authorization;

namespace brickwell2.Controllers;

[Authorize (Roles = "Admin")]
public class AdminController : Controller
{

    private ILegoRepository _repo;
    private ILegoSecurityRepository _securityRepo;

    public AdminController(ILegoRepository legoInfo, ILegoSecurityRepository secureInfo)
    {
        _repo = legoInfo;
        _securityRepo = secureInfo;
    }

    [HttpGet]
    public IActionResult AddProduct(int id)
    {
        var newProduct = new Product();
        return View(newProduct);
    }
    
    [HttpPost]
    public IActionResult AddProduct(Product response)
    {
        
        _repo.AddProduct(response);

        return RedirectToAction("AdminProducts");
    }
    
    
    [HttpGet]
    public IActionResult AddUser(int id)
    {
        var newUser = new AspNetUser();
        return View(newUser);
    }
    
    [HttpPost]
    public IActionResult AddUser(AspNetUser user)
    {
        _securityRepo.AddUser(user);

        return RedirectToAction("AdminUsers");
    }
    
    

    public IActionResult AdminProducts(int pageNum)
    {
        int pageSize = 10;

        pageNum = Math.Max(pageNum, 1);

        var productQuery = _repo.Products
                            .OrderBy(x => x.ProductId);

        var product = new PaginationListViewModel
        {
            Products = productQuery
                        .Skip((pageNum - 1) * pageSize)
                        .Take(pageSize), 

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize,
                TotalProducts = productQuery.Count() 
            }
        };

        return View(product);
    }

    public IActionResult AdminOrders(int pageNum)
    {
        int pageSize = 150;


        pageNum = Math.Max(pageNum, 1);

        var orderQuery = _repo.Orders
                              .Where(x => x.Fraud == 1) 
                              .OrderByDescending(x => x.TransactionId);

        var order = new PaginationListViewModel
        {

            Orders = orderQuery
                        .Skip((pageNum - 1) * pageSize)
                        .Take(pageSize),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize,
                TotalProducts = orderQuery.Where(x => x.Fraud == 1).Count()
            }
        };

        return View(order);
    }

    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        var recordToEdit = _repo.Products
            .Single(x => x.ProductId == id);

        return View(recordToEdit);
    }

    [HttpPost]
    public IActionResult EditProduct(Product product)
    {
        _repo.EditProduct(product);
        return RedirectToAction("AdminProducts");
    }

    [HttpGet]
    public IActionResult DeleteProduct(int id)
    {
        var recordToDelete = _repo.Products
            .Single(x => x.ProductId == id);

        return View(recordToDelete);
    }

    [HttpPost]
    public IActionResult DeleteProduct(Product delete)
    {
        _repo.DeleteProduct(delete);

        return RedirectToAction("AdminProducts");

    }


    public IActionResult AdminUsers(int pageNum)
    {
        int pageSize = 10;

        pageNum = Math.Max(pageNum, 1);

        var userQuery = _securityRepo.AspNetUsers
                          .OrderBy(x => x.UserName);

        var user = new PaginationListViewModel
        {
            AspNetUsers = userQuery
                            .Skip((pageNum - 1) * pageSize)
                            .Take(pageSize), 

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize, 
                TotalProducts = userQuery.Count() 
            }
        };

        return View(user);
    }

    [HttpGet]
    public IActionResult EditUser(string id)
    {
        var recordToEdit = _securityRepo.AspNetUsers
            .Single(x => x.Id == id);
        return View(recordToEdit);
    }

    [HttpPost]
    public IActionResult EditUser(Models.AspNetUser user)
    {
        _securityRepo.EditUser(user);
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
    public IActionResult DeleteUser(AspNetUser user)
    {
        _securityRepo.DeleteUser(user);
        return RedirectToAction("AdminUsers");
    }
    
    public class UserController : Controller
    {
        public IActionResult CreateUser()
        {
            string uniqueId = GenerateUniqueId.UniqueIdGenerator();

            return View(uniqueId);
        }
    }

	//public IActionResult AdminUsers(int pageNum)
	//{
	//    int pageSize = 10;
	//    var user = new PaginationListViewModel
	//    {
	//        AspNetUsers = _securityRepo.AspNetUsers
	//            .OrderBy(x => x.UserName)
	//            .Skip((pageNum - 1) * pageSize)
	//            .Take(pageSize),

	//        PaginationInfo = new PaginationInfo
	//        {
	//            CurrentPage = pageNum,
	//            ProductsPerPage = pageSize,
	//            TotalProducts = _securityRepo.AspNetUsers.Count()
	//        }
	//    };
	//    return View(user);

}