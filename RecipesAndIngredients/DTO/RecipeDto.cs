using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.DTO
{
    public class RecipeDto
    {
        public Guid Id { get; set; }

        public string RecName { get; set; } = null!;

        public RecipeCategoryDto? Category { get; set; } = new RecipeCategoryDto();

        public List<RecipeIngredientDto> Ingredients { get; set; } = new List<RecipeIngredientDto>();
    }
}
