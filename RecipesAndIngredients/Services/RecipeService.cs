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
            if ( Id == Guid.Empty) /// Guid.Empty возвращает нулевое значение в виде 00000000-000-0000-0000000
            {
                return null;
            }   
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = Get(Id);
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
                Guid id = Guid.NewGuid(); /// из-за того что id генерируется на этапе метода SaveChanges, при связывании записи с таблицей RecipeIngredient нужно сгенерировать Id
                                          /// Guid.NewGuid() - этот метод генерирует рандомный guid также как бы это делала SQL 
                Recipe newRecipe = new Recipe()
                {
                    Id= id,
                    RecName = recipeDto.RecName,
                    RecipeCategoryId = recipeDto.Category.Id,
                };

                foreach (KeyValuePair<int, IngredientAndQuantityDto> recipeIngredientDto in recipeDto.Ingredients) /// тип данных dictionary
                {
                    RecipeIngredient recipeIngredient = new RecipeIngredient()
                    {
                        IngredientId = recipeIngredientDto.Key, /// в key находится Id ингредиента
                        RecipeId = id,
                        QuantityCount = recipeIngredientDto.Value.QuantityCount, /// Value обращение к самому значению (второй столбец)
                    };
                    newRecipe.RecipeIngredients.Add(recipeIngredient); /// чтобы добавить информацию в БД связанную через FK.
                                                                       /// Важно: записи не добавятся пока не будет добавлена основная запись (в нашем случае рецепт)
                                                                       /// Как определить какая запись основная - переменная до первой точки (newRecipe)
                }
                db.Recipes.Add(newRecipe); /// здесь добавится и основная запись рецепта и запись в связующую таблицу RecipeIngredient
                db.SaveChanges();

                return id; /// почему так отвечу на следующем занятии
            }
        }



        //public bool UpdateRecipe(RecipeDto recipeDto, string name, Dictionary<int, CommandEnum> keyValues)
        //{
        //    RecipeIngredient? recipe = GetByName(name);
        //    using (RecipesIngredientsContext db = new RecipesIngredientsContext())
        //    {
        //        recipe.RecName = recipeDto.RecName;
        //        recipe.RecipeCategoryId = recipeDto.Category.Id;
        //        Guid recipeId = recipe.Id;
        //        RecipeDto recipeDto1 = new RecipeDto()
        //        {

        //        };

        //        foreach (KeyValuePair<int, RecipeIngredientDto> recipeIngredientDto in recipeDto.Ingredients) /// тип данных dictionary
        //        {

        //            RecipeIngredient recipeIngredient = recipe.RecipeIngredients.Where((r => r.IngredientId == ingredientId && r.RecipeId == recipeId)); 
        //            {
        //                IngredientId = recipeIngredientDto.Key, /// в key находится Id ингредиента
        //                QuantityCount = recipeIngredientDto.Value.QuantityCount, /// Value обращение к самому значению (второй столбец)
        //            };
        //                                                               /// чтобы добавить информацию в БД связанную через FK.
        //                                                               /// Важно: записи не добавятся пока не будет добавлена основная запись (в нашем случае рецепт)
        //                                                               /// Как определить какая запись основная - переменная до первой точки (newRecipe)
        //        }
        //        switch (keyValues)
        //        {
        //            case Dictionary<int, CommandEnum> edit:

        //                break;
        //            case Dictionary<int, CommandEnum> add:

        //                break;
        //            case Dictionary<int, CommandEnum> delete:

        //                break;
        //        }
        //        db.Update(recipe); 
        //        var isUpdated = db.Recipes != null;
        //        db.SaveChanges();

        //        return true;
        //    }
        //}


        private Recipe? GetByName(string name)
        {
            if (string.IsNullOrEmpty(name)) 
                return null;
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = db.Recipes.Include(p => p.RecipeCategory).Include(o => o.RecipeIngredients).Where(i => i.RecName == name).FirstOrDefault();
                return recipe; 
            }
        }



        public RecipeDto? GetByNameDto(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            Recipe? recipe = GetByName(name);
            if (recipe == null)  /// снова лишняя проверка ???
            {
                return null;
            }
            RecipeDto? recipeDto = Utils.ConvertToRecipeDto(recipe);
            return recipeDto;
        }



        public List<RecipeCategoryDto> GetAllCategory()
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                List<RecipeCategoryDto> recipeCategoryDtos = new List<RecipeCategoryDto>();
                List<RecipeCategory> recipeCategories = db.RecipeCategories.ToList();
                foreach (RecipeCategory category in recipeCategories)
                {
                    RecipeCategoryDto recipeCategoryDto = Utils.ConvertToRecipeCategoryDto(category);
                    recipeCategoryDtos.Add(recipeCategoryDto);
                }
                return recipeCategoryDtos;
            }
        }


        public bool CheckExistanceByName(string name)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = GetByName(name);
                if (recipe == null)
                {
                    return false;
                }
                return true;
            }
        }



        public void Remove(string name)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = GetByName(name);
                if (recipe == null)
                {
                    return;
                }
                else
                {
                    db.Remove(recipe);
                    db.SaveChanges();
                }
            }
        }
    }
}
