using FoodRecipes.Contracts;
using FoodRecipes.Dto;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Azure;
using Microsoft.Exchange.WebServices.Data;

namespace FoodRecipes.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepo;

        public RecipesController(IRecipeRepository recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

        [HttpGet("GetAllRecipes")]
        public async Task<IActionResult> GetRecipes()
        {
            try
            {
                var response = await _recipeRepo.GetRecipes();
                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateRecipe")]
        public async Task<IActionResult> CreateRecipe(RecipeCreateDto recipe)
        {
            try
            {
                if (DataHelper.LoggedUserId == 0)
                {
                    return StatusCode(409, "Log in to create recipe.");
                }

                var response = await _recipeRepo.CreateRecipe(recipe, DataHelper.LoggedUserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, RecipeUpdateDto recipe)
        {
            try
            {
                var recipeById = await _recipeRepo.GetRecipeById(id);
                if (recipeById == null)
                    return NotFound();

                if (DataHelper.LoggedUserId == 0)
                {
                    return StatusCode(409, "Log in to update the recipe.");
                }

                if (DataHelper.LoggedUserId != recipeById.UserId)
                {
                    return StatusCode(409, "Only user that created the recipe can make an update.");
                }

                var response = await _recipeRepo.UpdateRecipe(id, DataHelper.LoggedUserId, recipe);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("RateRecipe/{id}")]
        public async Task<IActionResult> RateRecipe(int id, RatingDto rating)
        {
            try
            {
                var recipeById = await _recipeRepo.GetRecipeById(id);
                if (recipeById == null)
                    return NotFound();

                if (DataHelper.LoggedUserId == 0)
                {
                    return StatusCode(409, "Log in to rate the recipe.");
                }

                if (DataHelper.LoggedUserId == recipeById.UserId)
                {
                    return StatusCode(409, "You cannot rate your own recipe.");
                }

                var response = await _recipeRepo.RateRecipe(id, recipeById, rating);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                var recipeById = await _recipeRepo.GetRecipeById(id);
                if (recipeById == null)
                    return NotFound();

                if (DataHelper.LoggedUserId == 0)
                {
                    return StatusCode(409, "Log in to remove the recipe.");
                }

                if (DataHelper.LoggedUserId != recipeById.UserId)
                {
                    return StatusCode(409, "Only user that created the recipe can remove it.");
                }

                var response = await _recipeRepo.DeleteRecipe(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
