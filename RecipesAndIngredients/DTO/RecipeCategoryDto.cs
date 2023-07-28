using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.DTO
{
    public class RecipeCategoryDto
    {
        public int Id { get; set; }

        public string? CategName { get; set; }

        public List<RecipeDto> Recipes { get; set; } = new List<RecipeDto>();
    }
}
