using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
    public partial class RecipePage
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

                int key = Utils.GetAndValidateNullInt();
                switch (key)
                {
                    case 1:
                        GetAllRecipies(recipeService);
                        break;
                    case 2:
                        AddNewRecipe(recipeService, ingredientService);
                        break;
                    case 3:
                        EditRecipe(recipeService, ingredientService);
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
            } /// такой же вопрос как ниже
        }



        public static void GetRecipe(RecipeService recipeService)
        {
            Console.WriteLine("Введите название рецепта");

            while (true) /// цикл больше не нужен, он внутри метода, а далее здесь он не используется
            {
                string recName = Utils.GetAndValidateNullString();
                RecipeDto? recipeDto = recipeService.GetByNameDto(recName); 
                if (recipeDto == null)
                {
                    Console.WriteLine("Рецепт не существует");
                    continue;
                }
                Dictionary<int, IngredientAndQuantityDto> ingredientAndQuantityDtos = new (); /// как привязать рецепт к данному экземпляру, сделать поиск с include?
                Console.WriteLine($"RecipeName - {recipeDto.RecName}, RecipeCategory - {recipeDto.Category.CategName}"); ///// ????????? перебрать foreach обратиться к values чтобы получить доступ к ингред-там

                foreach (KeyValuePair<int, IngredientAndQuantityDto> ingredientAndQuantityDto in ingredientAndQuantityDtos)
                {
                    Console.WriteLine($"{ingredientAndQuantityDto.Value.Ingredient.IngName} - {ingredientAndQuantityDto.Value.QuantityCount}" +
                        $" {ingredientAndQuantityDto.Value.Ingredient.QuantityType.Name}"); 
                }
                /// внутри foreach еще CW выставлять ingname and quantitytype
                break;
            }
        }



        public static void AddNewRecipe(RecipeService recipeService, IngredientService ingredientService)
        {
            Dictionary<int, IngredientAndQuantityDto> ingredients = new Dictionary<int, IngredientAndQuantityDto>();
            RecipeDto recipeDto = new RecipeDto();

            Console.WriteLine("Создание нового рецепта");
            Console.WriteLine("Введите название рецепта");

            string recName = Utils.GetAndValidateNullString();
            recipeDto.RecName = recName;

            Console.WriteLine("Выберите одну из категорий блюда");

            List<RecipeCategoryDto> categoryList = recipeService.GetAllCategory();
            int countCategory = categoryList.Count;  /// .Count возвращает число записей списка
            for (int i = 1; i <= countCategory; i++)
            {
                Console.WriteLine($"{i} - {categoryList[i - 1].CategName}");   /// [] позволяют обращаться по индексу (количество строк)
                    /// обращаясь по индексу порядок начинается с 0, поэтому указываем i - 1  
            }
            while (true)  ///////////// переделать на уроке с вопросом о цикле !!!!!!!
            {
                Console.WriteLine("Введите цифру категории");

                string? input  = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный ввод");///////////////
                    continue;
                }
                int inputConv = Convert.ToInt32(input);
                if (inputConv > countCategory)
                {
                    Console.WriteLine("Неверная цифра");////////////
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

                string? ingName = Utils.GetAndValidateNullString();
                if (ingredientService.CheckExistanceByName(ingName) == false)
                {
                    continue;
                }
                IngredientDto? ingredientDto = ingredientService.GetByNameDto(ingName);
                if (ingredientDto == null)
                {
                    Console.WriteLine("Ингредиент не найден");
                    continue;
                }

                Console.WriteLine($"Введите количество ингредиента ({ingredientDto.QuantityType.Name})");

                int quantity = Utils.GetAndValidateNullInt();
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

            string recName = Utils.GetAndValidateNullString();
            if (recipeService.CheckExistanceByName(recName) == false)
            {
                Console.WriteLine("Отмена удаления");
            }
            else
            {
                recipeService.Remove(recName);
                Console.WriteLine("Ингредиент успешно удален");
            }
        }
    }  
}
