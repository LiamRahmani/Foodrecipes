using Dapper;
using FoodRecipes.Context;
using FoodRecipes.Contracts;
using FoodRecipes.Entities;

namespace FoodRecipes.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _context;

        public CategoryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var query = "SELECT * FROM Categories";

            using (var connection = _context.CreateConnection())
            {
                var categories = await connection.QueryAsync<Category>(query);
                return categories.ToList();
            }
        }
    }
}
