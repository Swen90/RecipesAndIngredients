using Microsoft.EntityFrameworkCore;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.Services
{
    public class RecipeService
    {

        public RecipeDto? GetDto(Guid Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = db.Recipes.Include(c => c.RecipeCategory).Where(r => r.Id == Id)
                    .SelectMany(s => s.RecipeIngredients.Select(m => m.Recipe)).Distinct().FirstOrDefault();
                ///Distinct фильтрует дубликаты и оставляет только уникальные значения
                
                return recipe;
            }
        }


        public RecipeDto? AddRecipe(RecipeDto recipe)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe newRecipe = new Recipe()
                {
                    Id = recipe.Id,
                    RecName = recipe.RecName,
                    RecipeCategoryId = recipe.Category.Id,
                };
                
                foreach(RecipeIngredientDto recipeIngredientDto in recipe.Ingredients)
                {
                    RecipeIngredient recipeIngredient = new RecipeIngredient()
                    {
                        IngredientId = recipeIngredientDto.Ingredient.Id,
                        QuantityCount = recipeIngredientDto.QuantityCount,
                        RecipeId = recipe.Id,
                    };
                    newRecipe.RecipeIngredients.Add();
                }
            }
        
        

        }
    }
}
