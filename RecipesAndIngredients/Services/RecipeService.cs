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
        private Recipe? Get(Guid Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = db.Recipes.Include(c => c.RecipeCategory).Where(r => r.Id == Id)
                    .SelectMany(s => s.RecipeIngredients.Select(m => m.Recipe)).Distinct().FirstOrDefault();
                return recipe;
            }
        }



        public RecipeDto? GetDto(Guid Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = db.Recipes.Include(c => c.RecipeCategory).Where(r => r.Id == Id)
                    .SelectMany(s => s.RecipeIngredients.Select(m => m.Recipe)).Distinct().FirstOrDefault();
                ///Distinct фильтрует дубликаты и оставляет только уникальные значения
                if (recipe == null)
                {
                    return null;
                }
                else
                {
                    RecipeDto? recipeDto = Utils.ConvertToRecipeDto(recipe);
                    return recipeDto;
                }
            }
        }



        public List<RecipeDto>? GetAll()
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                List<Recipe>? recipes = db.Recipes.Include(r => r.RecipeCategory).ToList();
                List<RecipeDto>? recipesDto = new List<RecipeDto>();
                foreach (Recipe recipe in recipes)
                {
                    RecipeDto? dto = Utils.ConvertToRecipeDto(recipe);
                }
                return recipesDto;
            }
        }


        public Guid AddRecipe(RecipeDto recipeDto)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                RecipeIngredient? recipeIngredient = null;
                Guid id = Guid.NewGuid(); /// из-за того что id генерируется на этапе метода SaveChanges, при связывании записи с таблицей RecipeIngredient нужно сгенерировать Id
                                          /// Guid.NewGuid() - этот метод генерирует рандомный guid также как бы это делала SQL 
                Recipe newRecipe = new Recipe()
                {
                    Id= id,
                    RecName = recipeDto.RecName,
                    RecipeCategoryId = recipeDto.Category.Id,
                };

                foreach (KeyValuePair<int, RecipeIngredientDto> recipeIngredientDto in recipeDto.Ingredients) /// тип данных dictionary
                {
                    recipeIngredient = new RecipeIngredient()
                    {
                        IngredientId = recipeIngredientDto.Key, /// в key находится Id ингредиента
                        RecipeId = id,
                        QuantityCount = recipeIngredientDto.Value.QuantityCount, /// Value обращение к самому значению (второй столбец)
                    };
                    newRecipe.RecipeIngredients.Add(recipeIngredient); /// чтобы добавить информацию в БД связанную через FK.
                                                                       /// Важно: записи не добавятся пока е будет добавлена основная запись (в нашем случае рецепт)
                                                                       /// Как определить какая запись основная - переменная до первой точки (newRecipe)
                }
                db.Recipes.Add(newRecipe); /// здесь добавится и основная запись рецепта и запись в связующую таблицу RecipeIngredient
                db.SaveChanges();

                return id; /// почему так отвечу на следующем занятии
            }
        }
    }
}
