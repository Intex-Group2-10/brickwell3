using brickwell2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using brickwell2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using System.Drawing.Printing;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace brickwell2.Controllers
{ 
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ILegoRepository _repo;
        private ILegoSecurityRepository _securityRepository;
        private readonly LegoDbContext _context;
        private readonly InferenceSession _session;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILegoRepository temp, ILegoSecurityRepository securetemp, LegoDbContext context, InferenceSession session, ILogger<HomeController> logger)
        {
            _repo = temp;
            _securityRepository = securetemp;
            _context = context;
            _session = session;
            _logger = logger;
        }

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
        [HttpGet]
        public IActionResult Checkout(string total)
        {
            // Parse the currency value
            if (int.TryParse(total, NumberStyles.Currency, CultureInfo.CurrentCulture, out int parsedTotal))
            {
                var order = new FraudPrediction
                {
                    Amount = parsedTotal
                };
                return View(order);
            }
            return View(new FraudPrediction()); // fallback if parsing fails
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

        // public IActionResult Test()
        // {
        //     var viewUsers = _securityRepository.AspNetUsers.ToList();
        //     return View(viewUsers);
        // }

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
        
        [HttpPost]
        public IActionResult Predict(FraudPrediction fraudPrediction, int time, int amount, int day_of_week_Mon, int day_of_week_Sat, int day_of_week_Sun, int day_of_week_Thu, int day_of_week_Tue, int day_of_week_Wed, int entry_mode_PIN, int entry_mode_Tap, int type_of_transaction_Online, int type_of_transaction_POS, int country_of_transaction_India, int country_of_transaction_Russia, int country_of_transaction_USA, int shipping_address_India, int shipping_address_Russia, int shipping_address_USA, int bank_HSBC, int bank_Halifax, int bank_Lloyds, int bank_Metro, int bank_Monzo, int bank_RBS, int type_of_card_Visa)
        {
            // Dictionary mapping the numeric prediction to an animal type
            var class_type_dict = new Dictionary<int, string>
            {
                { 0, "Not Fraudulent" },
                { 1, "Fraudulent" }
            };

            try
            {
                var input = new List<float> { time, amount, day_of_week_Mon, day_of_week_Sat, day_of_week_Sun, day_of_week_Thu, day_of_week_Tue, day_of_week_Wed, entry_mode_PIN, entry_mode_Tap, type_of_transaction_Online, type_of_transaction_POS, country_of_transaction_India, country_of_transaction_Russia, country_of_transaction_USA, shipping_address_India, shipping_address_Russia, shipping_address_USA, bank_HSBC, bank_Halifax, bank_Lloyds, bank_Metro, bank_Monzo, bank_RBS, type_of_card_Visa  };
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };

                using (var results = _session.Run(inputs)) // makes the prediction with the inputs from the form (i.e. class_type 1-7)
                {
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    if (prediction != null && prediction.Length > 0)
                    {
                        // Use the prediction to get the animal type from the dictionary
                        var fraudType = class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                        ViewBag.Prediction = fraudType;
                    }
                    else
                    {
                        ViewBag.Prediction = "Error: Unable to make a prediction.";
                    }
                }

                _logger.LogInformation("Prediction executed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during prediction: {ex.Message}");
                ViewBag.Prediction = "Error during prediction.";
            }
            if (ModelState.IsValid)
            {
                _context.FraudPredictions.Add(fraudPrediction);
                _context.SaveChanges();
                return RedirectToAction("ShowPredictions");
            }

            // If model state is not valid, return the view with errors
            return View("ShowPredictions");

        }
        
        public IActionResult ShowPredictions()
        {
            var records = _context.FraudPredictions.ToList();
            
            var predictions = new List<FraudPred>();  
            
            var class_type_dict = new Dictionary<int, string>
            {
                { 0, "Not Fraudulent" },
                { 1, "Fraudulent" }
            };

            foreach (var record in records)
            {
                var input = new List<float>
                {
                    (float)record.Amount, record.EntryModePin,
                    record.EntryModeTap, record.TypeOfTransactionOnline, record.TypeOfTransactionPos,
                    record.CountryOfTransactionIndia, record.CountryOfTransactionRussia,
                    record.CountryOfTransactionUsa,
                    // record.CountryOfTransactionUnitedKingdom,
                    record.ShippingAddressIndia,
                    record.ShippingAddressRussia,
                    record.ShippingAddressUsa, 
                    // record.ShippingAddressUnitedKingdom,
                    record.BankHsbc, record.BankHalifax, record.BankLloyds, record.BankMetro, record.BankMonzo,
                    record.BankRbs, record.TypeOfCardVisa
                };
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };

                string predictionResult;
                using (var results = _session.Run(inputs))
                {
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    predictionResult = prediction != null && prediction.Length > 0 ? class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
                }

                predictions.Add(new FraudPred() { FraudPrediction = record, Prediction = predictionResult }); // Adds the fraud information and prediction 
            }

            

            return View(predictions);
        }

    }
}
