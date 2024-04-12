namespace brickwell2.Models.ViewModels
{
	public class UserRecommendationViewModel
	{
		//public List<Product> Product { get; set; }
		public IEnumerable<UserBasedRecommendation>? UserBasedRecommendations { get; set; }
	}

}