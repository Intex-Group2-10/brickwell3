namespace brickwell2.Models.ViewModels;

public class ItemRecommendationViewModel
{
    public Product Product { get; set; }
    public IEnumerable<ItemBasedRecommendation>? ItemBasedRecommendations { get; set; }
}