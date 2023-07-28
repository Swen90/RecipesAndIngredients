using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("1 - Показать все заказы");
                Console.WriteLine("2 - Создать новый заказ");
                Console.WriteLine("3 - Редактировать информацию о заказе");
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
            List<IngredientDto>? ingredientDto = ingredientService.GetAll();
            foreach (IngredientDto ingredientDto1 in ingredientDto)
                /// проверку на null
            {
                Console.WriteLine($"Id = {ingredientDto1.Id}, IngredientName = {ingredientDto1.IngName}," +
                    $" QuantityType = {ingredientDto1.QuantityType.Quantity}");
            }
        }



        public static void AddNewIngredient(IngredientService ingredientService)
        {
            Console.WriteLine("Добавление ингредиента");
            while (true)
            {
                Console.WriteLine("Введите название ингредиента");
                string? input = Console.ReadLine();
                if( string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Неверный формат названия");
                    continue;
                }

                /// Дописать
                IngredientDto ingredientDto = new IngredientDto() {IngName = input};


            }
        }
    }
}
