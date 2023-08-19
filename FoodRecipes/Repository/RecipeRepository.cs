using Dapper;
using FoodRecipes.Context;
using FoodRecipes.Contracts;
using FoodRecipes.Dto;
using FoodRecipes.Entities;
using Microsoft.Exchange.WebServices.Data;
using System.Data;

namespace FoodRecipes.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DapperContext _context;

        public RecipeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            var query = "SELECT Id, Title, Description, Ingredients, CategoryId, Rate, UserId FROM Recipes";

            using (var connection = _context.CreateConnection())
            {
                var recipes = await connection.QueryAsync<Recipe>(query);
                return recipes.ToList();
            }
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            var query = "SELECT * FROM Recipes WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var recipe = await connection.QuerySingleOrDefaultAsync<Recipe>(query, new { id });

                return recipe;
            }
        }

        public async Task<Recipe> CreateRecipe(RecipeCreateDto recipe, int userId)
        {
            var query = "INSERT INTO Recipes (Title, Description, Ingredients, CategoryId, UserId) " +
                "VALUES (@Title, @Description, @Ingredients, @CategoryId, @UserId)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Title", recipe.Title, DbType.String);
            parameters.Add("Description", recipe.Description, DbType.String);
            parameters.Add("Ingredients", recipe.Ingredients, DbType.String);
            parameters.Add("CategoryId", recipe.CategoryId, DbType.Int64);
            parameters.Add("UserId", userId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var createdRecipe = new Recipe
                {
                    Id = id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Ingredients = recipe.Ingredients,
                    CategoryId = recipe.CategoryId,
                    UserId = userId
                };

                return createdRecipe;
            }
        }

        public async Task<Recipe> UpdateRecipe(int id, int userId, RecipeUpdateDto recipe)
        {
            var query = "UPDATE Recipes SET Title = @Title, Description = @Description, Ingredients = @Ingredients, CategoryId = @CategoryId WHERE Id = @Id AND UserId = @userId";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Title", recipe.Title, DbType.String);
            parameters.Add("Description", recipe.Description, DbType.String);
            parameters.Add("Ingredients", recipe.Ingredients, DbType.String);
            parameters.Add("CategoryId", recipe.CategoryId, DbType.Int64);
            parameters.Add("UserId", userId, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);

                var updatedRecipe = new Recipe
                {
                    Id = id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Ingredients = recipe.Ingredients,
                    CategoryId = recipe.CategoryId,
                    UserId = userId
                };

                return updatedRecipe;
            }
        }

        public async Task<string> DeleteRecipe(int id)
        {
            var query = "DELETE FROM Recipes WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
                return "Recipe successfully removed.";
            }
        }

        public async Task<Recipe> RateRecipe(int id, Recipe recipe, RatingDto rating)
        {
            var query = "UPDATE Recipes SET Rate = @Rate WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Title", recipe.Title, DbType.String);
            parameters.Add("Rate", rating.Rate, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);

                var ratedRecipe = new Recipe
                {
                    Id = id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Ingredients = recipe.Ingredients,
                    CategoryId = recipe.CategoryId,
                    UserId = recipe.UserId,
                    Rate = rating.Rate
                };

                return ratedRecipe;
            }
        }
    }
}
