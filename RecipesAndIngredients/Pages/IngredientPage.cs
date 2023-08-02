using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RecipesAndIngredients.Pages
{
    public class IngredientPage
    {
        public void IngredientsPage()
        {
            IngredientService ingredientService = new IngredientService();
            while (true)
            {
                Console.WriteLine("Выберите операцию: 1, 2, 3, 4, 5");
                Console.WriteLine("1 - Показать все ингредиенты");
                Console.WriteLine("2 - Добавить новый ингредиент");
                Console.WriteLine("3 - Редактировать информацию об ингредиенте");
                Console.WriteLine("4 - Получение информации об ингредиенте");
                Console.WriteLine("5 - Удаление ингредиента");
                Console.WriteLine("6 - Вернуться на главную страницу");
                string? input = Console.ReadLine();
                int key = Convert.ToInt32(input);
                switch (key)
                {
                    case 1:
                        GetAllIngredients(ingredientService);
                        break;
                    case 2:
                        AddNewIngredient(ingredientService);
                        break;
                    case 3:
                        ///EditIngredient(ingredientService);
                        break;
                    case 4:
                        ///GetIngredient(ingredientService);
                        break;
                    case 5:
                        ///RemoveIngredient(ingredientService);
                        break;
                    case 6:
                        /// bool для выхода из страницы
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }
        }



        public static void GetAllIngredients(IngredientService ingredientService)
        {
            Console.WriteLine("Список ингредиентов");
            List<IngredientDto>? ingredientsDto = ingredientService.GetAll();
            /// переделать проверку на   List<IngredientDto>? ingredientsDto

            foreach (IngredientDto ingredientDto1 in ingredientsDto)
            {
                if (ingredientDto1 == null) //// ??????
                {
                    return;
                }
                else
                {
                    Console.WriteLine($"Id = {ingredientDto1.Id}, IngredientName = {ingredientDto1.IngName}," +
                        $" QuantityType = {ingredientDto1.QuantityType.Quantity}");
                }
            }
        }



        public static void AddNewIngredient(IngredientService ingredientService)
        {
            QuantityTypeDto? quantityDto = null;
            QuantityTypeDto? quantityDto1 = null;
            string? input;
            int input2;
            Console.WriteLine("Добавление ингредиента");
            while (true)
            {
                Console.WriteLine("Введите название ингредиента");
                input = Console.ReadLine();
                if( string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный формат названия");
                    continue;
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                Console.WriteLine("Выберите тип исчисления: 1 - Кг, 2 - Шт");
                string? input1 = Console.ReadLine();
                if( string.IsNullOrEmpty(input1) )
                {
                    Console.WriteLine("Неверный формат");
                    continue;
                }
                else
                {
                    input2 = Convert.ToInt32(input1);
                    break;
                }
            }

            quantityDto = ingredientService.GetQuantityTypeById(input2);

            IngredientDto? ingredientDto = new IngredientDto()
            {
                IngName = input,
                QuantityType = quantityDto,
            };
            IngredientDto? ingredientDto1 = ingredientService.Add(ingredientDto);
            Console.WriteLine("Добавлен новый ингредиент");
            Console.WriteLine($"Name - {input}, QuantityType - {ingredientDto.QuantityType.Quantity}");
        }
    }
}
