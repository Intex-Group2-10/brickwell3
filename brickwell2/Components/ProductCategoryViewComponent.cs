using Microsoft.AspNetCore.Mvc;
using brickwell2.Models;

namespace brickwell2.Components
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private ILegoRepository _legoRepo;

        public ProductCategoryViewComponent(ILegoRepository temp) 
        {
            _legoRepo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectproductCategory = RouteData?.Values["productCategory"];

            var productCategories = _legoRepo.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(productCategories);
        }

    }
}
