namespace FoodRecipes.Dto
{
    public class RecipeUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
