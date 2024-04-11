namespace brickwell2.Models.ViewModels;

public class ItemRecommendationViewModel
{
    public Product Products { get; set; }
    public IQueryable<ItemBasedRecommendation>? ItemBasedRecommendations { get; set; }
}