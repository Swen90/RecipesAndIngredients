using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Models;
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
            bool exit = false;
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
                        EditIngredient(ingredientService);
                        break;
                    case 4:
                        GetIngredient(ingredientService);
                        break;
                    case 5:
                        RemoveIngredient(ingredientService);
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



        public static void GetAllIngredients(IngredientService ingredientService)
        {
            Console.WriteLine("Список ингредиентов");
            List<IngredientDto>? ingredientsDto = ingredientService.GetAll();
            if(ingredientsDto == null)
            {
                Console.WriteLine("Список не найден");
            }
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
                        $" QuantityType = {ingredientDto1.QuantityType.Name}");
                }
                ///break;     ???????????
            }
        }



        public static void GetIngredient(IngredientService ingredientService)
        {
            Console.WriteLine("Введите название ингредиента");
            while (true)
            {
                string? ing = Console.ReadLine();
                if (string.IsNullOrEmpty(ing))
                {
                    Console.WriteLine("Неверный ввод");
                    continue;             
                }
                IngredientDto? ingredientDto = ingredientService.GetByNameDto(ing);
                if (ingredientDto == null)
                {
                    Console.WriteLine("Ингредиент не найден");
                }
                Console.WriteLine($"IngredientName - {ingredientDto}");
            }

        }


        public static void AddNewIngredient(IngredientService ingredientService)
        {
            QuantityTypeDto? quantityDto = null;
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
            Console.WriteLine($"Name - {input}, QuantityType - {ingredientDto.QuantityType.Name}");
        }




        public void EditIngredient(IngredientService ingredientService)
        {
            IngredientDto? ingredientDto = new IngredientDto();
            Console.WriteLine("Редактирование информации об ингредиенте");
            while(true)
            {
                Console.WriteLine("Введите Id ингредиента");
                string? input = Console.ReadLine();
                if( string.IsNullOrEmpty(input) )
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                ingredientDto = ingredientService.GetByNameDto(input);
                if( ingredientDto == null )
                {
                    Console.WriteLine("Ингредиент не найден");
                    continue;
                }
                break;
            }
            while (true)
            {
                Console.WriteLine("1 - Редактировать название");
                Console.WriteLine("2 - Редактировать тип исчисления");
                string? input2 = Console.ReadLine();
                if (string.IsNullOrEmpty(input2))
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                int inpuy2Conv = Convert.ToInt32(input2);
                switch (inpuy2Conv)
                {
                    case 1:
                        while (true)
                        {
                            string? ingName = Console.ReadLine();
                            if (string.IsNullOrEmpty(ingName))
                            {
                                Console.WriteLine("Некорректный ввод");
                                continue;
                            }
                            ingredientDto.IngName = ingName != null ? ingredientDto.IngName : ingName;
                            break;
                        }
                        break;
                    case 2:
                        while (true)
                        {
                            string? quantityType = Console.ReadLine();
                            if (string.IsNullOrEmpty(quantityType))
                            {
                                Console.WriteLine("Некорректный ввод");
                                continue;
                            }
                            ingredientDto.QuantityType.Name = quantityType != null ? ingredientDto.QuantityType.Name : quantityType;
                            break;
                        }
                        break;
                }
                break;
            }
            bool editValid = ingredientService.Edit(ingredientDto);
            if (editValid == false)
            {
                Console.WriteLine("Редактирование отменено");
            }
            else
            {
                Console.WriteLine("Данные успешно сохранены");
            }
        }


        public static void RemoveIngredient(IngredientService ingredientService)
        {
            Console.WriteLine("Введите Id ингредиента");
            while(true)
            {
                string? input = Console.ReadLine();
                if( string.IsNullOrEmpty(input) )
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                int inputConv = Convert.ToInt32(input);
                bool isExisted = ingredientService.CheckExistance(inputConv);
                if( isExisted == false )
                {
                    Console.WriteLine("Отмена удаления");
                }
                else
                {
                    ingredientService.Remove(inputConv);
                    Console.WriteLine("Ингредиент успешно удален");
                }
                break;
            }
        }
    }
}
