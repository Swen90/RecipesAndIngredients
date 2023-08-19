using Microsoft.Identity.Client;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
using RecipesAndIngredients.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.Pages
{
    public class RecipePage
    {
        public void RecipesPage()
        {
            bool exit = false;
            RecipeService recipeService = new RecipeService();
            IngredientService ingredientService = new IngredientService();
            while (true)
            {
                Console.WriteLine("Выберите операцию: 1, 2, 3, 4, 5");
                Console.WriteLine("1 - Показать весь список рецептов");
                Console.WriteLine("2 - Создать новый рецепт");
                Console.WriteLine("3 - Редактировать состав рецепта");
                Console.WriteLine("4 - Получение информации о рецепте");
                Console.WriteLine("5 - Удаление рецепта из списка");
                Console.WriteLine("6 - Вернуться на главную страницу");
                string? input = Console.ReadLine();
                int key = Convert.ToInt32(input);
                switch (key)
                {
                    case 1:
                        GetAllRecipies(recipeService);
                        break;
                    case 2:
                        AddNewRecipe(recipeService, ingredientService);
                        break;
                    case 3:
                        ///EditRecipe(recipeService);
                        break;
                    case 4:
                        GetRecipe(recipeService);
                        break;
                    case 5:
                        RemoveRecipe(recipeService);
                        break;
                    case 6:
                        exit = true;
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
                if (exit == true)
                {
                    break;
                }
            }
        }



        public static void GetAllRecipies(RecipeService recipeService)
        {
            Console.WriteLine("Список всех рецептов");
            List<RecipeDto>? recipesDto = recipeService.GetAll();
            foreach(RecipeDto recipeDto in recipesDto)
            {
                Console.WriteLine($"Id = {recipeDto.Id}, RecipeName = {recipeDto.RecName}, Category = {recipeDto.Category.CategName}");
            }
        }


        public static void GetRecipe(RecipeService recipeService)
        {
            Console.WriteLine("Введите название рецепта");
            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                RecipeDto? recipeDto = recipeService.GetByNameDto(input);
                if (recipeDto == null)
                {
                    Console.WriteLine("Рецепт не существует");
                }
                Console.WriteLine($"RecipeName - {recipeDto.RecName}, RecipeCategory - {recipeDto.Category}, Ingredients - {recipeDto.Ingredients.Values}, " +
                    $"IngredientCount - {recipeDto.Ingredients}");///// ????????? перебрать foreach обратиться к values чтобы получить доступ к ингред-там
                /// внутри foreach еще CW выставлять ingname and quantitytype
            }
        }



        public static void AddNewRecipe(RecipeService recipeService, IngredientService ingredientService)
        {
            Dictionary<int, IngredientAndQuantityDto> ingredients = new Dictionary<int, IngredientAndQuantityDto>();
            RecipeDto recipeDto = new RecipeDto();
            Console.WriteLine("Создание нового рецепта");
            while (true)
            {
                Console.WriteLine("Введите название рецепта");
                string? inp = Console.ReadLine();
                if (string.IsNullOrEmpty(inp))
                {
                    Console.WriteLine("Неверный формат ввода");
                    continue;
                }
                recipeDto.RecName = inp;
                break;
            }
            Console.WriteLine("Выберите одну из категорий блюда");
            List<RecipeCategoryDto> categoryList = recipeService.GetAllCategory();
            int countCategory = categoryList.Count;  /// .Count возвращает число записей списка
            for (int i = 1; i <= countCategory; i++)
            {
                Console.WriteLine($"{i} - {categoryList[i - 1].CategName}");   /// [] позволяют обращаться по индексу (количество строк)
                    /// обращаясь по индексу порядок начинается с 0, поэтому указываем i - 1  
            }
            while (true)
            {
                Console.WriteLine("Введите цифру категории");
                string? input  = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                int inputConv = Convert.ToInt32(input);
                if (inputConv > countCategory)
                {
                    Console.WriteLine("Неверная цифра");
                    continue;
                }
                ///RecipeCategoryDto recipeCategoryDto = categoryList[inputConv - 1];
                RecipeCategoryDto recipeCategoryDto = categoryList.ElementAt(inputConv - 1); /// ElementAt позволяет обратиться к выбранной записи по индексу(аналог предыдущей строчки)

                recipeDto.Category = recipeCategoryDto;
                break;
            }

            while (true)
            {
                Console.WriteLine("Введите название ингредиента");
                string? inp = Console.ReadLine();
                if (string.IsNullOrEmpty(inp))
                {
                    Console.WriteLine("Неверный формат ввода");
                    continue;
                }
                bool check = ingredientService.CheckExistanceByName(inp);
                if (check == false)
                {
                    continue;
                }
                IngredientDto? ingredientDto = ingredientService.GetByNameDto(inp);

                Console.WriteLine($"Введите количество ингредиента ({ingredientDto.QuantityType.Name})");
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный ввод количества");
                    continue;
                }
                int quantity = Convert.ToInt32(input);

                IngredientAndQuantityDto ingredientAndQuantityDto = new IngredientAndQuantityDto()
                {
                    QuantityCount = quantity,
                    Ingredient = ingredientDto,
                };
                ingredients.Add(ingredientDto.Id, ingredientAndQuantityDto);
                Console.WriteLine("Желаете ли продолжить добавление ингредиента?");
                Console.WriteLine("Да - Y   Нет - N");
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Y)
                {
                    continue;
                }
                recipeDto.Ingredients = ingredients;
                break;
            }
            recipeService.AddRecipe(recipeDto);
            Console.WriteLine("Создан новый рецепт");
            Console.WriteLine($"{recipeDto.RecName}, {recipeDto.Category.CategName}: ");
            foreach(var ingredient in ingredients)
            {
                Console.WriteLine($"{ingredient.Value.Ingredient.IngName} - {ingredient.Value.QuantityCount}, {ingredient.Value.Ingredient.QuantityType.Name}");
            }
        }



        public static void RemoveRecipe(RecipeService recipeService)
        {
            Console.WriteLine("Введите название рецепта");
            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                bool isExisted = recipeService.CheckExistance(input);
                if (isExisted == false)
                {
                    Console.WriteLine("Отмена удаления");
                }
                else
                {
                    recipeService.Remove(input);
                    Console.WriteLine("Ингредиент успешно удален");
                }
                break;
            }
        }
    }  
}
