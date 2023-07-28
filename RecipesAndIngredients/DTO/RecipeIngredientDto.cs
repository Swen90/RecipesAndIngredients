using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.DTO
{
    public class RecipeIngredientDto
    {
        public int? QuantityCount { get; set; }

        public  IngredientDto Ingredient { get; set; } = null!;
    }
}
