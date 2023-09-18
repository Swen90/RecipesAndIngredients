using System;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace RecipesAndIngredients
{
    public static class Utils
    {
        public static IngredientDto ConvertToIngredientDto(Ingredient ingredient)
        {
            IngredientDto ingredientDto = new IngredientDto()
            {
                Id = ingredient.Id,
                IngName = ingredient.IngName,
                QuantityType = ConvertToQuantityTypeDto(ingredient.QuantityType),
            };
            return ingredientDto;
        }



        public static QuantityTypeDto ConvertToQuantityTypeDto(QuantityType quantityType)
        {
            QuantityTypeDto quantityTypeDto = new QuantityTypeDto()
            {
                Id = quantityType.Id,
                Name = quantityType.Quantity,
            };
            return quantityTypeDto;
        }



        public static RecipeDto? ConvertToRecipeDto(Recipe? recipe)
        {
            if (recipe == null) return null;

            RecipeDto recipeDto = new RecipeDto() /// только один new должен быть в методе (только создание экзмепляра RecipeDto)
            { /// разбить все на много маленьких методов, для практики и для правильного написания конструкции кода (принципы solid)
                Id = recipe.Id,
                RecName = recipe.RecName,
                Category = ConvertToRecipeCategoryDto(recipe.RecipeCategory)
            };

            foreach (RecipeIngredient recipeIngredient in recipe.RecipeIngredients)
            {
                IngredientAndQuantityDto recipeIngredientDto = ConvertToIngredientAndQuantityDto(recipeIngredient);
                recipeDto.Ingredients.Add(recipeIngredient.IngredientId, recipeIngredientDto);
            };

            return recipeDto;
        }



        public static IngredientAndQuantityDto ConvertToIngredientAndQuantityDto(RecipeIngredient recipeIngredient)
        {
            IngredientAndQuantityDto ingredientAndQuantityDto = new()
            {
                QuantityCount = recipeIngredient.QuantityCount,
                Ingredient = ConvertToIngredientDto(recipeIngredient.Ingredient),
            };
            return ingredientAndQuantityDto;
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



        public static string GetAndValidateNullString()
        {
            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) == true)  /// то же самое что без bool == true (оператор сравнения bool внутри условия на проверку null)
                {
                    Console.WriteLine("Неверный ввод");
                    Console.WriteLine("Повторите действие");
                    continue;
                }
                return input; /// return работает одновременно как break для цикла и как return для завершения метода(возврат переменной)
            }
        }



        public static int GetAndValidateNullInt() /// название метода то что отдает(желательно точнее и короче), не писать промежуточные процессы
                                                  ///string? title = null если в параметре идет присвоение через = то это означает что будет приниматься дефолтное значение для метода 
        {
            int convertInput = Convert.ToInt32(GetAndValidateNullString());
            /// int convertInput = int.Parse(GetAndValidateNullString(title)); то же самое
            return convertInput;
        }



        ///ConvertToRecipeDto в развернутом варианте

        //public static RecipeDto? ConvertToRecipeDto(Recipe recipe)
        //{
        //    if (recipe == null) return null;

        //    RecipeDto recipeDto = new RecipeDto() /// только один new должен быть в методе (только создание экзмепляра RecipeDto)
        //    { /// разбить все на много маленьких методов, для практики и для правильного написания конструкции кода (принципы solid)
        //        Id = recipe.Id,
        //        RecName = recipe.RecName,
        //        Category = new()
        //        {
        //            Id = recipe.RecipeCategory.Id,
        //            CategName = recipe.RecipeCategory.CategName,
        //        }
        //        ///ConvertToRecipeCategoryDto(recipe.RecipeCategory)
        //    };

        //    foreach (RecipeIngredient recipeIngredient in recipe.RecipeIngredients)
        //    {
        //        IngredientAndQuantityDto recipeIngredientDto = new()
        //        {
        //            QuantityCount = recipeIngredient.QuantityCount,
        //            Ingredient = new()
        //            {
        //                Id = recipeIngredient.IngredientId,
        //                IngName = recipeIngredient.Ingredient.IngName,
        //                QuantityType = new()
        //                {
        //                    Id = recipeIngredient.Ingredient.QuantityType.Id,
        //                    Name = recipeIngredient.Ingredient.QuantityType.Quantity,
        //                }
        //            }
        //        };
        //        recipeDto.Ingredients.Add(recipeIngredient.IngredientId, recipeIngredientDto);
        //    };
        //    return recipeDto;
        //}
    }
}
