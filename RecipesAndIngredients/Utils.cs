using System;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients
{
    public static class Utils
    {
        public static IngredientDto? ConvertToIngredientDto(Ingredient ingredient)
        {
            IngredientDto? ingredientDto = new IngredientDto()
            {
                Id = ingredient.Id,
                IngName = ingredient.IngName,
                QuantityType = new QuantityTypeDto
                {
                    Id = ingredient.QuantityType.Id,
                    Quantity = ingredient.QuantityType.Quantity,
                }
            };
            return ingredientDto;
        }


        public static RecipeDto? ConvertToRecipeDto(Recipe recipe)
        {
            RecipeDto? recipeDto = new RecipeDto()
            {
                Id = recipe.Id,
                RecName = recipe.RecName,
                Category = recipe.RecipeCategory,
            };
        }
    }
}
