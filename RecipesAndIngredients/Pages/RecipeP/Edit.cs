using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.RecipeP
{
    public partial class RecipePage /// partial - деление класса (одинаковые модификаторы и название класса)
    {
        public static bool ContinueOrBreak()
        {
            Console.WriteLine("Желаете ли продолжить редактирование ингредиентов?");
            Console.WriteLine("Да - Y   Нет - N");

            ConsoleKey key2 = Console.ReadKey().Key;
            if (key2 == ConsoleKey.Y)
            {
                return true;
            }
            return false;
        }



        public static void EditRecipe(RecipeService recipeService, IngredientService ingredientService)
        {
            bool exit = false;
            Dictionary<IngredientAndQuantityDto, CommandEnum> ingredients = new Dictionary<IngredientAndQuantityDto, CommandEnum>();
            RecipeDto? recipeDto = new RecipeDto();

            Console.WriteLine("Редактирование рецепта");
            Console.WriteLine("Введите название");

            string recipeName = Utils.GetAndValidateNullString();
            while (true)
            {
                if (recipeService.CheckExistanceByName(recipeName) == false)
                    Console.WriteLine($"Рецепт {recipeName} не найден");
                
                recipeDto = recipeService.GetByNameDto(recipeName);
                break;
            }
            while (true)
            {
                Console.WriteLine("Выберите параметр редактирования");
                Console.WriteLine("1 - Название рецепта");
                Console.WriteLine("2 - Категория рецепта");
                Console.WriteLine("3 - Добавить ингредиент к рецепту");
                Console.WriteLine("4 - Редактирование количества ингредиента в рецепте");
                Console.WriteLine("5 - Удалить ингредиент из рецепта");
                Console.WriteLine("6 - Сохранение и завершение редактирования");

                int key = Utils.GetAndValidateNullInt();
                if (key <= 0 || key > 6)
                {
                    Console.WriteLine("Цифра должна соответствовать номеру из списка. Повторите ввод");
                    continue;
                }
                switch (key)
                {
                    case 1:
                        Console.WriteLine("Введите новое название рецепта");

                        string recName = Utils.GetAndValidateNullString();
                        recipeDto.RecName = recName;
                        Console.WriteLine($"Название рецепта успешно изменено на {recName}");
                        continue;
                    case 2:
                        Console.WriteLine("Выберите категорию блюда");

                        List<RecipeCategoryDto> categoryList = recipeService.GetAllCategory();
                        int countCategory = categoryList.Count;
                        for (int i = 1; i <= countCategory; i++)
                            Console.WriteLine($"{i} - {categoryList[i - 1].CategName}");   /// [] позволяют обращаться по индексу (количество строк)
                                                                                           /// обращаясь по индексу порядок начинается с 0, поэтому указываем i - 1  
                        int newCategory = Utils.GetAndValidateNullInt();
                        if (newCategory > countCategory && newCategory < 0)
                        {
                            Console.WriteLine("Неверная цифра");
                            continue;
                        }
                        RecipeCategoryDto recipeCategoryDto = categoryList[newCategory - 1];
                        recipeDto.Category = recipeCategoryDto;
                        Console.WriteLine($"Категория рецепта изменена на {recipeCategoryDto.CategName}");
                        continue;
                    case 3:
                    case 4:
                    case 5:
                        while (true)
                        {
                            Console.WriteLine("Впишите название ингредиента");

                            string ingName = Utils.GetAndValidateNullString();
                            if (ingredientService.CheckExistanceByName(ingName) == false)
                            {
                                Console.WriteLine($"Ингредиента с названием {ingName} не существует");
                                continue;
                            }
                            Ingredient ingredient = ingredientService.GetByName(ingName)!;
                            IngredientDto ingredientDto = Utils.ConvertToIngredientDto(ingredient);
                            
                            Console.WriteLine($"{ingredientDto.IngName} - {ingredientDto.QuantityType.Name}");

                            if (key == 3)
                            {
                                if (ingredients.Any(i => i.Value == CommandEnum.Add && i.Key.Ingredient.Id == ingredient.Id) /// any позволяет определить хранятся ли значения в коллекции по заданным условиям
                                    || recipeService.CheckIngredientInRecipe(recipeName, ingredient.Id))
                                {
                                    Console.WriteLine($"Ингредиент {ingredient.IngName} уже существует В рецепте");
                                    continue;
                                }

                                Console.WriteLine($"Впишите количество, ({ingredientDto.QuantityType.Name}:)");

                                int quantity = Utils.GetAndValidateNullInt();
                                IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto()
                                {
                                    QuantityCount = quantity,
                                    Ingredient = ingredientDto,
                                };
                                ingredients.Add(ingredientAndQuantityDto, CommandEnum.Add); /// использовать для сервиса
                            }
                            /// 1 - ингредиент добавлен в локальное хранилище (пометка Add), но его нет в базе
                            /// 2 - ингредиента нет в базе
                            /// 3 - ингредиент есть в базе и есть в локальном хранилище с пометкой Edit (замена значения quantity)
                            /// 4 - ингредиент не добавлен в локальное хранилище и есть в базе (новая запись с пометкой Edit)
                            else if (key == 4)
                            {
                                IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto();
                                var existAddIngredient = ingredients.Where(i => i.Value == CommandEnum.Add && i.Key.Ingredient.Id == ingredient.Id).FirstOrDefault().Key;

                                if (existAddIngredient != null) /// 1
                                {
                                    Console.WriteLine("Введите новое количество ингредиента:");
                                    int quantity = Utils.GetAndValidateNullInt();
                                    existAddIngredient.QuantityCount = quantity;
                                }
                                else
                                {
                                    if (recipeService.CheckIngredientInRecipe(recipeName, ingredient.Id) == false) /// 2
                                    {
                                        Console.WriteLine($"Ингредиента {ingredient.IngName} нет в рецепте");
                                        continue;
                                    }
                                    var existIngredientEdit = ingredients.Where(i => i.Value == CommandEnum.Edit && i.Key.Ingredient.Id == ingredient.Id).FirstOrDefault().Key;
                                    Console.WriteLine("Введите новое количество ингредиента:");
                                    int quantity = Utils.GetAndValidateNullInt();
                                    if (existIngredientEdit != null) /// 3
                                    {
                                        existIngredientEdit.QuantityCount = quantity;
                                    }
                                    else /// 4
                                    {
                                        ingredientAndQuantityDto = new IngredientAndQuantityDto()
                                        {
                                            QuantityCount = quantity,
                                            Ingredient = ingredientDto
                                        };
                                        ingredients.Add(ingredientAndQuantityDto, CommandEnum.Edit);
                                    }
                                }
                            }
                            /// 1 ингредиент есть в локальном хранилище, пометка Add
                            /// 2 ингредиент есть в базе, но нет в локальном хранилище
                            /// 3 ингредиента нет в базе
                            else if (key == 5)
                            {
                                IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto();

                                if (ingredients.Any(i => i.Value == CommandEnum.Add && i.Key.Ingredient.Id == ingredient.Id) == false) /// 1
                                {
                                    Console.WriteLine($"Ингредиент {ingredient.IngName} не существует в рецепте");
                                    continue;
                                }
                                else
                                {
                                    ingredientAndQuantityDto = new IngredientAndQuantityDto()
                                    {
                                        QuantityCount = 0,
                                        Ingredient = ingredientDto
                                    };
                                    ingredients.Add(ingredientAndQuantityDto, CommandEnum.Delete);
                                }

                                if (recipeService.CheckIngredientInRecipe(recipeName, ingredient.Id) == false) /// 3
                                {
                                    Console.WriteLine($"Ингредиента {ingredient.IngName} нет в рецепте");
                                    continue;
                                }
                                else /// 2
                                {
                                    ingredientAndQuantityDto = new IngredientAndQuantityDto()
                                    {
                                        QuantityCount = 0,
                                        Ingredient = ingredientDto
                                    };
                                    ingredients.Add(ingredientAndQuantityDto, CommandEnum.Delete);
                                }
                            }
                            bool repeatAction = ContinueOrBreak();
                            if (repeatAction == true)
                                continue;
                            break;
                        }
                        break;
                    case 6:
                        exit = true;
                        break;
                }
                if (exit == true)
                    break;
            }
            recipeService.UpdateRecipe(recipeDto, ingredients);
        }
    }
}
