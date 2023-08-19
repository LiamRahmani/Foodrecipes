using System.ComponentModel.DataAnnotations;

namespace FoodRecipes.Dto
{
    public class RatingDto
    {
        [Range(1, 5)]
        public int Rate { get; set; }
    }
}
