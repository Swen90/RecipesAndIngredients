using Microsoft.EntityFrameworkCore;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;

namespace RecipesAndIngredients.Services
{
    public class RecipeService
    {
        private Recipe? Get(Guid Id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = db.Recipes
                    ///.Include(c => c.RecipeCategory) убираем Include чтобы не тянуть за собой экземпляр таблицы из бд, иначе не происходит запись переменной RecipeCategoryId
                    .Include(c => c.RecipeIngredients)
                    .Where(r => r.Id == Id)
                    .Distinct() /// для дубликатов, получение уникальной строчки простого типа данных из БД
                    .FirstOrDefault();
                return recipe; 
            }
        }



        public RecipeDto? GetDto(Guid Id)
        {
            if ( Id == Guid.Empty) /// Guid.Empty возвращает нулевое значение в виде 00000000-000-0000-0000000
                return null;

            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = Get(Id);
                ///Distinct фильтрует дубликаты и оставляет только уникальные значения
                if (recipe == null)
                    return null;

                RecipeDto recipeDto = Utils.ConvertToRecipeDto(recipe);
                return recipeDto;
            }
        }

        

        public List<RecipeDto>? GetAll()
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                List<Recipe>? recipes = db.Recipes.Include(r => r.RecipeCategory)
                    .Include(c => c.RecipeIngredients)
                    .ThenInclude(i => i.Ingredient) /// thenIclude - для того чтобы присоединить запись по иерархии данных ниже, не к главной а уже к вспомогательной таблице
                    .ThenInclude(t => t.QuantityType).ToList();
                //if (recipes == null || recipes.Count == 0) /// дописать
                List<RecipeDto>? recipesDto = new List<RecipeDto>();
                foreach (Recipe recipe in recipes)
                {
                    RecipeDto? dto = Utils.ConvertToRecipeDto(recipe);
                    if (dto == null)
                        return null;

                    recipesDto.Add(dto);
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



        public void UpdateRecipe(RecipeDto recipeDto, Dictionary<IngredientAndQuantityDto, CommandEnum> ingredients)
        {
            Recipe? recipe = Get(recipeDto.Id);
            if (recipe == null)
                return;
            
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                recipe.RecName = recipeDto.RecName;
                recipe.RecipeCategoryId = recipeDto.Category.Id;

                //var uniqIngredients = ingredients.DistinctBy(x => x.Key.Ingredient.Id); /// для дубликатов, метод для выбора уникального комплексного экземпляра данных, с уточнением по какому-то конкретному значению
                foreach (KeyValuePair<IngredientAndQuantityDto, CommandEnum> ingredient in ingredients)
                {
                    switch (ingredient.Value)
                    {
                        case CommandEnum.Add:
                            RecipeIngredient recipeIngredient = new RecipeIngredient()
                            {
                                RecipeId = recipe.Id,
                                IngredientId = ingredient.Key.Ingredient.Id,
                                QuantityCount = ingredient.Key.QuantityCount,
                            };
                            recipe.RecipeIngredients.Add(recipeIngredient);
                            db.Entry(recipeIngredient).State = EntityState.Added; /// явно указывать entity что запись добавлена
                            break;
                        case CommandEnum.Edit:
                            RecipeIngredient? recipeIngredientEdit = recipe.RecipeIngredients.Where(i => i.IngredientId == ingredient.Key.Ingredient.Id).FirstOrDefault();
                            recipeIngredientEdit.QuantityCount = ingredient.Key.QuantityCount;
                            db.Update(recipeIngredientEdit);
                            db.Entry(recipeIngredientEdit).State = EntityState.Modified;
                            break;
                        case CommandEnum.Delete:
                            RecipeIngredient? recipeIngredientDelete = recipe.RecipeIngredients.Where(i => i.IngredientId == ingredient.Key.Ingredient.Id).FirstOrDefault();
                            db.Entry(recipeIngredientDelete).State = EntityState.Deleted;
                            recipe.RecipeIngredients.Remove(recipeIngredientDelete);
                            break;
                    }
                }
                db.Update(recipe);
                db.SaveChanges();
            }
        }



        public bool CheckIngredientInRecipe(string recName, int id)
        {
            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                var recipe = db.Recipes
                    .Include(x => x.RecipeIngredients)
                    .Where(r => r.RecName == recName && r.RecipeIngredients.Select(entry => entry.IngredientId) /// select дает возможность выделить 1 колонку данных (простой тип данных (int, string и т.д.)). Через выделение получаем список IngredientId из БД
                    .Contains(id))
                    .FirstOrDefault();
                if (recipe == null)
                    return false;

                return true;
            }
        }



        private Recipe? GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                return null;

            using (RecipesIngredientsContext db = new RecipesIngredientsContext())
            {
                Recipe? recipe = db.Recipes.Include(p => p.RecipeCategory)
                    .Include(o => o.RecipeIngredients)
                    .ThenInclude(i => i.Ingredient)
                    .ThenInclude(t => t.QuantityType)
                    .Where(i => i.RecName == name)
                    .FirstOrDefault();
                return recipe; 
            }
        }



        public RecipeDto? GetByNameDto(string name) /// QUE: нужен ли вообще этот метод, создает много предупреждений,
            /// и желательно разделение обязанностей, сделать отдельно в классе getbyname и отдельно конверт, если будет устраивать по условию
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            Recipe recipe = GetByName(name);
            RecipeDto recipeDto = Utils.ConvertToRecipeDto(recipe);
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
