using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.Pages
{
    public partial class RecipePage /// partial - деление класса (одинаковые модификаторы и название класса)
    {
        public static void EditRecipe(RecipeService recipeService, IngredientService ingredientService)
        {
            Dictionary<IngredientAndQuantityDto, CommandEnum> ingredients = new Dictionary<IngredientAndQuantityDto, CommandEnum>();
            RecipeDto? recipeDto = new RecipeDto();

            Console.WriteLine("Редактирование рецепта");
            Console.WriteLine("Введите название");

            string recipeName = Utils.GetAndValidateNullString();
            while (true)
            {
                if (recipeService.CheckExistanceByName(recipeName) == false)
                {
                    Console.WriteLine($"Рецепт {recipeName} не найден");
                }
                recipeDto = recipeService.GetByNameDto(recipeName);
                break;
            }
            while (true)
            {
                Console.WriteLine("Выберите параметр редактирования");
                Console.WriteLine("1 - Название рецепта");
                Console.WriteLine("2 - Категория рецепта");
                Console.WriteLine("3 - Добавить ингредиент к рецепту");
                Console.WriteLine("4 - Редактирование ингредиента в рецепте");
                Console.WriteLine("5 - Удалить ингредиент из рецепта");

                int key = Utils.GetAndValidateNullInt();
                switch (key)
                {
                    case 1:
                        Console.WriteLine("Введите новое название рецепта");

                        string recName = Utils.GetAndValidateNullString();
                        recipeDto.RecName = recName;
                        continue;
                    case 2:
                        Console.WriteLine("Выберите категорию блюда");

                        List<RecipeCategoryDto> categoryList = recipeService.GetAllCategory();
                        int countCategory = categoryList.Count;
                        for (int i = 1; i <= countCategory; i++) /////////// можно вывести в метод, встречается больше 2-х раз
                        {
                            Console.WriteLine($"{i} - {categoryList[i - 1].CategName}");   /// [] позволяют обращаться по индексу (количество строк)
                                                                                           /// обращаясь по индексу порядок начинается с 0, поэтому указываем i - 1  
                        }
                        int newCategory = Utils.GetAndValidateNullInt();
                        if (newCategory > countCategory)
                        {
                            Console.WriteLine("Неверная цифра");
                            continue;
                        }
                        RecipeCategoryDto recipeCategoryDto = categoryList[newCategory - 1];
                        recipeDto.Category = recipeCategoryDto;
                        continue;
                    case 3:
                        while (true)
                        {
                            Console.WriteLine("Впишите название ингредиента");

                            string ingName = Utils.GetAndValidateNullString();
                            if (ingredientService.CheckExistanceByName(ingName) == false)
                            {
                                Console.WriteLine($"Ингредиента с названием {ingName} не существует");
                                continue;
                            }
                            IngredientDto? ingredientDto = ingredientService.GetByNameDto(ingName);
                            Console.WriteLine($"{ingredientDto.IngName} - {ingredientDto.QuantityType.Name}");

                            Console.WriteLine($"Впишите количество, ({ingredientDto.QuantityType.Name}:)");

                            int quantity = Utils.GetAndValidateNullInt();
                            IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto()
                            {
                                QuantityCount = quantity,
                                Ingredient = ingredientDto,
                            };
                            ingredients.Add(ingredientAndQuantityDto, CommandEnum.Add); /// использовать для сервиса

                            Console.WriteLine("Желаете ли продолжить добавление ингредиента?");
                            Console.WriteLine("Да - Y   Нет - N");

                            ConsoleKey key1 = Console.ReadKey().Key;
                            if (key1 == ConsoleKey.Y)
                            {
                                continue;
                            }
                            ///recipeDto.Ingredients = ingredients; //// нужен сервис
                            break;
                        }
                        break;
                    case 4:
                        while (true)
                        {
                            Console.WriteLine("Впишите название ингредиента");

                            string ingName = Utils.GetAndValidateNullString();
                            if (ingredientService.CheckExistanceByName(ingName) == false)
                            {
                                Console.WriteLine($"Ингредиента с названием {ingName} не существует");
                                continue;
                            }
                            IngredientDto? ingredientDto = ingredientService.GetByNameDto(ingName);
                            Console.WriteLine($"{ingredientDto.IngName} - {ingredientDto.QuantityType.Name}");
                            Console.WriteLine("Введите новое количество ингредиента:");

                            int quantity = Utils.GetAndValidateNullInt();
                            IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto()
                            {
                                QuantityCount = quantity,
                                Ingredient = ingredientDto
                            };
                            ingredients.Add(ingredientAndQuantityDto, CommandEnum.Edit); /// использовать для сервиса

                            Console.WriteLine("Желаете ли продолжить редактирование ингредиентов?");
                            Console.WriteLine("Да - Y   Нет - N");
                            ConsoleKey key2 = Console.ReadKey().Key;
                            if (key2 == ConsoleKey.Y)
                            {
                                continue;
                            }
                            break;
                        }
                        break;
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
                            IngredientDto? ingredientDto = ingredientService.GetByNameDto(ingName);
                            Console.WriteLine($"{ingredientDto.IngName} - {ingredientDto.QuantityType.Name}");

                            IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto()
                            {
                                QuantityCount = 0,
                                Ingredient = ingredientDto
                            };
                            ingredients.Add(ingredientAndQuantityDto, CommandEnum.Delete); /// использовать для сервиса

                            Console.WriteLine("Желаете ли продолжить редактирование ингредиентов?");
                            Console.WriteLine("Да - Y   Нет - N");

                            ConsoleKey key2 = Console.ReadKey().Key;
                            if (key2 == ConsoleKey.Y)
                            {
                                continue;
                            }
                            break;
                        }
                        break;
                }
                break;
            }
            recipeService.UpdateRecipe(recipeDto, ingredients);
        }
    }
}
