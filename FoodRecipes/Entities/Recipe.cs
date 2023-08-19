namespace FoodRecipes.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
        public int? Rate { get; set; } = 0;
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
