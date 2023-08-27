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
            string recipeName = Utils.GetAndValidateNullString("Введите название");
            while (true)
            {
                bool check = recipeService.CheckExistanceByName(recipeName);
                if (check == false)
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
                        while (true)
                        {
                            Console.WriteLine("Введите новое название рецепта");
                            string? inp2 = Console.ReadLine();
                            if (string.IsNullOrEmpty(inp2))
                            {
                                Console.WriteLine("Неверный ввод");
                                continue;
                            }
                            recipeDto.RecName = inp2;
                            break;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Выберите категорию блюда");
                        List<RecipeCategoryDto> categoryList = recipeService.GetAllCategory();
                        int countCategory = categoryList.Count;
                        for (int i = 1; i <= countCategory; i++)
                        {
                            Console.WriteLine($"{i} - {categoryList[i - 1].CategName}");   /// [] позволяют обращаться по индексу (количество строк)
                                                                                           /// обращаясь по индексу порядок начинается с 0, поэтому указываем i - 1  
                        }
                        while (true)
                        {
                            string? inp3 = Console.ReadLine();
                            if (string.IsNullOrEmpty(inp3))
                            {
                                Console.WriteLine("Неверный ввод");
                                continue;
                            }
                            int inp3Conv = Convert.ToInt32(inp3);
                            if (inp3Conv > countCategory)
                            {
                                Console.WriteLine("Неверная цифра");
                                continue;
                            }
                            RecipeCategoryDto recipeCategoryDto = categoryList[inp3Conv - 1];
                            recipeDto.Category = recipeCategoryDto;
                            break;
                        }
                        break;
                    case 3:

                        while (true)
                        {
                            Console.WriteLine("Впишите название ингредиента");
                            string? inp4 = Console.ReadLine();
                            if (string.IsNullOrEmpty(inp4))
                            {
                                Console.WriteLine("Неверный ввод");
                                continue;
                            }
                            bool check = ingredientService.CheckExistanceByName(inp4);
                            if (check == false)
                            {
                                Console.WriteLine($"Ингредиента с названием {inp4} не существует");
                                continue;
                            }
                            IngredientDto? ingredientDto = ingredientService.GetByNameDto(inp4);

                            Console.WriteLine($"{ingredientDto.IngName} - {ingredientDto.QuantityType.Name}");

                            Console.WriteLine($"Впишите количество ({ingredientDto.QuantityType.Name})");
                            string? quan = Console.ReadLine();
                            if (string.IsNullOrEmpty(quan))
                            {
                                Console.WriteLine("Неверный ввод");
                            }
                            int quantity = Convert.ToInt32(quan);

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
                            string? inp5 = Console.ReadLine();
                            if (string.IsNullOrEmpty(inp5))
                            {
                                Console.WriteLine("Неверный ввод");
                                continue;
                            }
                            bool check1 = ingredientService.CheckExistanceByName(inp5);
                            if (check1 == false)
                            {
                                Console.WriteLine($"Ингредиента с названием {inp5} не существует");
                                continue;
                            }
                            IngredientDto? ingredientDto1 = ingredientService.GetByNameDto(inp5);
                            Console.WriteLine($"{ingredientDto1.IngName} - {ingredientDto1.QuantityType.Name}");

                            Console.WriteLine("Введите новое количество ингредиента:");
                            string? quan1 = Console.ReadLine();
                            if (string.IsNullOrEmpty(quan1))
                            {
                                Console.WriteLine("Неверный ввод");
                            }
                            int quantity1 = Convert.ToInt32(quan1);
                            IngredientAndQuantityDto ingredientAndQuantityDto1 = new IngredientAndQuantityDto()
                            {
                                QuantityCount = quantity1,
                                Ingredient = ingredientDto1
                            };
                            ingredients.Add(ingredientAndQuantityDto1, CommandEnum.Edit); /// использовать для сервиса
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
                            string? inp6 = Console.ReadLine();
                            if (string.IsNullOrEmpty(inp6))
                            {
                                Console.WriteLine("Неверный ввод");
                                continue;
                            }
                            bool check = ingredientService.CheckExistanceByName(inp6);
                            if (check == false)
                            {
                                Console.WriteLine($"Ингредиента с названием {inp6} не существует");
                                continue;
                            }
                            IngredientDto? ingredientDto2 = ingredientService.GetByNameDto(inp6);
                            break;
                        }
                        break;
                }
            }
        }
    }
}
