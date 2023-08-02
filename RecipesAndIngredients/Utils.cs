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



        public static QuantityTypeDto? ConvertToQuantityTypeDto(QuantityType quantityType)
        {
            QuantityTypeDto? quantityTypeDto = new QuantityTypeDto()
            {
                Id = quantityType.Id,
                Quantity = quantityType.Quantity,
            };
            return quantityTypeDto;
        }



        public static RecipeDto? ConvertToRecipeDto(Recipe recipe)
        {
            RecipeDto? recipeDto = new RecipeDto()
            {
                Id = recipe.Id,
                RecName = recipe.RecName,
                Category = new RecipeCategoryDto
                {
                    Id = recipe.RecipeCategoryId.Value,  /// Когда ставлю знак ? добавляется новый функционал, где в свойстве Value хранится значение переменной
                    CategName = recipe.RecipeCategory.CategName,
                }
            };
            return recipeDto;
        }
    }
}
