using Microsoft.AspNetCore.Mvc;
using brickwell2.Models;

namespace brickwell2.Components
{
    public class ProductColorViewComponent : ViewComponent
    {
        private ILegoRepository _legoRepo;

        public ProductColorViewComponent(ILegoRepository temp)
        {
            _legoRepo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectproductColor = RouteData?.Values["productColor"];

            var productColor = _legoRepo.Products
                .Select(x => x.PrimaryColor)
                .Distinct()
                .OrderBy(x => x);

            return View(productColor);
        }

    }
}
