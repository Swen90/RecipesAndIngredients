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
                    Name = ingredient.QuantityType.Quantity,
                }
            };
            return ingredientDto;
        }



        public static QuantityTypeDto? ConvertToQuantityTypeDto(QuantityType quantityType)
        {
            QuantityTypeDto? quantityTypeDto = new QuantityTypeDto()
            {
                Id = quantityType.Id,
                Name = quantityType.Quantity,
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



        public static RecipeCategoryDto ConvertToRecipeCategoryDto(RecipeCategory recipeCategory)
        {
            RecipeCategoryDto categoryDto = new RecipeCategoryDto()
            {
                Id = recipeCategory.Id,
                CategName = recipeCategory.CategName,
            };
            return categoryDto;
        }



        public static string GetAndValidateNullString(string? title)
        {
            while (true)
            {
                if (string.IsNullOrWhiteSpace(title) == false)  /// оператор сравнения bool внутри условия на проверку null
                {
                    Console.WriteLine(title);
                }
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) == true)  /// то же самое что без bool == true
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                return input;
            }
        }



        public static int GetAndValidateNullInt(string? title = null) /// название метода то что отдает(желательно точнее и короче), не писать промежуточные процессы
            /// если в параметре идет присвоение через = то это означает что будет приниматься дефолтное значение для метода 
        {
            int convertInput = Convert.ToInt32(GetAndValidateNullString(title));
            /// int convertInput = int.Parse(GetAndValidateNullString(title)); то же самое
            return convertInput;
        }
    }
}
