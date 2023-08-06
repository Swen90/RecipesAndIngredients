using Microsoft.Identity.Client;
using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
using RecipesAndIngredients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Dictionary<int, RecipeIngredientDto> ingredients = new Dictionary<int, RecipeIngredientDto>();

            Console.WriteLine("Создание нового рецепта");
            
            while (true)
            {
                Console.WriteLine("Введите название ингредиента");
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный формат ввода");
                    continue;
                }
                IngredientDto? ingredient = ingredientService.GetByName(input);
                if (ingredient == null)
                {
                    Console.WriteLine("Ингредиент не найден");
                    continue;
                }
                else
                {
                    /// запросить через консоль quantitycount
                    
                    ingredients.Add(ingredient.Id, new RecipeIngredientDto() {Ingredient = ingredient, QuantityCount = 50});
                }
                Console.WriteLine("Хотите добавить еще ингредиент?");
                Console.WriteLine("Введите '1' чтобы добавить ингредиент. Введите любую другую цифру чтобы закончить");
                string? key = Console.ReadLine();
                if (string.IsNullOrEmpty(key))
                {
                    break;
                }
                int key1 = Convert.ToInt32(key);
                if (key1 == 1)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            RecipeDto? recipeDto = new RecipeDto()
            {
                RecName = "Борщ",
                Category = new RecipeCategoryDto()
                {
                    Id = 1,
                    CategName = "Первое блюдо",
                },
                Ingredients = ingredients
            };
            recipeService.AddRecipe(recipeDto);

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
