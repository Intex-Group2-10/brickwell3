using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using brickwell2.Models;
using System;
using System.Diagnostics;
using System.Xml.Linq;
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

    //public IActionResult AdminProducts(int pageNum)
    //{
    //    int pageSize = 10;
    //    var product = new PaginationListViewModel
    //    {
    //        Products = _repo.Products
    //            .OrderBy(x => x.ProductId)
    //            .Skip((pageNum - 1) * pageSize)
    //            .Take(pageSize),

    //        PaginationInfo = new PaginationInfo
    //        {
    //            CurrentPage = pageNum,
    //            ProductsPerPage = pageSize,
    //            TotalProducts = _repo.Products.Count()
    //        }
    //    };
    //    return View(product);
    //}
    [HttpGet]
    public IActionResult AddProduct()
    {
        return View("AdminProducts", model: new Product());
    }
    
    [HttpPost]
    public IActionResult AddProduct(Product response)
    {
        if (ModelState.IsValid)
        {
            _repo.AddProduct(response);
            return View("AdminProducts", response);
        }
        else
        {
            return View(response);
        }
    }

    public IActionResult AdminProducts(int pageNum)
    {
        int pageSize = 10;

        // Ensure pageNum is at least 1
        pageNum = Math.Max(pageNum, 1);

        var productQuery = _repo.Products
                            .OrderBy(x => x.ProductId);

        var product = new PaginationListViewModel
        {
            Products = productQuery
                        .Skip((pageNum - 1) * pageSize)
                        .Take(pageSize), // No ToList() since we want an IQueryable

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize,
                TotalProducts = productQuery.Count() // Use the count of productQuery
            }
        };

        return View(product);
    }


    //public IActionResult AdminOrders(int pageNum)
    //{
    //    int pageSize = 150;
    //    var order = new PaginationListViewModel
    //    {
    //        Orders = _repo.Orders
    //            .Where(x => x.Fraud == 1) // Filter orders where fraud equals 1
    //            .OrderByDescending(x => x.TransactionId) // Order by TransactionId (most recent first)
    //            .Skip((pageNum - 1) * pageSize)
    //            .Take(pageSize),

    //        PaginationInfo = new PaginationInfo
    //        {
    //            CurrentPage = pageNum,
    //            ProductsPerPage = pageSize,
    //            TotalProducts = _repo.Orders.Where(x => x.Fraud == 1).Count() // Count of all orders (including those where fraud != 1)
    //        }
    //    };
    //    return View(order);
    //}
    public IActionResult AdminOrders(int pageNum)
    {
        int pageSize = 150;

        // Ensure pageNum is at least 1
        pageNum = Math.Max(pageNum, 1);

        var orderQuery = _repo.Orders
                              .Where(x => x.Fraud == 1) // Filter orders where fraud equals 1
                              .OrderByDescending(x => x.TransactionId); // Order by TransactionId (most recent first)

        var order = new PaginationListViewModel
        {
            // Assuming Orders is of type IQueryable<Order>
            Orders = orderQuery
                        .Skip((pageNum - 1) * pageSize)
                        .Take(pageSize),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ProductsPerPage = pageSize,
                TotalProducts = orderQuery.Count() // Count of all orders where fraud equals 1
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
        // _repo.Products.Remove(deleteinfo);
        // _repo.SaveChanges();
        //
        // return RedirectToAction("AdminUsers");
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
    //}

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
}