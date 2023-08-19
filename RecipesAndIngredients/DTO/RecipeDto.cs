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

        public Dictionary<int, IngredientAndQuantityDto> Ingredients { get; set; } = new Dictionary<int, IngredientAndQuantityDto>();
    }
}
