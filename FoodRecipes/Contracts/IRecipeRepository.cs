using FoodRecipes.Dto;
using FoodRecipes.Entities;

namespace FoodRecipes.Contracts
{
    public interface IRecipeRepository
    {
        public Task<IEnumerable<Recipe>> GetRecipes();
        public Task<Recipe> GetRecipeById(int id);
        public Task<Recipe> GetRecipeByTitle(string searchTitle);
        public Task<Recipe> CreateRecipe(RecipeCreateDto recipe, int userId);
        public Task<Recipe> UpdateRecipe(int id, int userId, RecipeUpdateDto recipe);
        public Task<string> DeleteRecipe(int id);
        public Task<Recipe> RateRecipe(int id, Recipe recipe, RatingDto rating);

    }
}
