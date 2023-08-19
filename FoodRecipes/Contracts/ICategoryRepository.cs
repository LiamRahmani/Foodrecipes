using FoodRecipes.Entities;

namespace FoodRecipes.Contracts
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetCategories();
    }
}
