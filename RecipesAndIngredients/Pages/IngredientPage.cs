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
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ошибка ввода");
                    continue;
                }
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

            foreach (IngredientDto ingredientDto1 in ingredientsDto)
            {
                    Console.WriteLine($"Id = {ingredientDto1.Id}  \tIngredientName = {ingredientDto1.IngName}" +
                        $"\t\t QuantityType = {ingredientDto1.QuantityType.Name}"); /// break не нужен, перебрал и закончил
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
            QuantityTypeDto? quantityDto; /// знак ? уже дает понятие что здесь лежит null
            string? input;
            int input1Conv;
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
                Console.WriteLine("Выберите тип исчисления: 1 - Г, 2 - Шт");
                string? input1 = Console.ReadLine();
                if( string.IsNullOrEmpty(input1) )
                {
                    Console.WriteLine("Неверный формат");
                    continue;
                }
                else
                {
                    input1Conv = Convert.ToInt32(input1);
                    break;
                }
            }
            quantityDto = ingredientService.GetQuantityTypeById(input1Conv);

            IngredientDto? ingredientDto = new IngredientDto()
            {
                IngName = input,
                QuantityType = quantityDto,
            };
            ingredientService.Add(ingredientDto);
            Console.WriteLine("Добавлен новый ингредиент");
            Console.WriteLine($"Name - {ingredientDto.IngName}, QuantityType - {ingredientDto.QuantityType.Name}");
        }



        public void EditIngredient(IngredientService ingredientService)
        {
            IngredientDto? ingredientDto = new IngredientDto();
            Console.WriteLine("Редактирование информации об ингредиенте");
            while(true)
            {
                Console.WriteLine("Введите название ингредиента");
                string? input = Console.ReadLine();
                if( string.IsNullOrEmpty(input) )
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                bool check = ingredientService.CheckExistanceByName(input);
                if(check == false)
                {
                    Console.WriteLine($"Ингредиент с названием {input} не существует");
                    continue;
                }
                else
                {
                    ingredientDto = ingredientService.GetByNameDto(input);
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
                            Console.WriteLine("Введите новое название для ингредиента");
                            string? ingName = Console.ReadLine();
                            if (string.IsNullOrEmpty(ingName))
                            {
                                Console.WriteLine("Некорректный ввод");
                                continue;
                            }
                            ingredientDto.IngName = ingName == null ? ingredientDto.IngName : ingName;
                            break;
                        }
                        break;
                    case 2:
                        while (true)
                        {
                            Console.WriteLine("Выберите тип исчисления");
                            List<QuantityTypeDto>? quantityTypeList = ingredientService.GetAllQuantityTypes();
                            int count = quantityTypeList.Count;
                            for (int i = 1; i <= count; i++)
                            {
                                Console.WriteLine($"{i} - {quantityTypeList[i-1].Name}");
                            }
                            string? type = Console.ReadLine();
                            if (string.IsNullOrEmpty(type))
                            {
                                Console.WriteLine("Некорректный ввод");
                                continue;
                            }
                            int typeConvert = Convert.ToInt32(type);
                            ingredientDto.QuantityType.Id = quantityTypeList[typeConvert - 1].Id;

                            ingredientDto.QuantityType.Name = quantityTypeList[typeConvert - 1].Name;
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
            Console.WriteLine("Введите Id ингредиента"); /// сделать поиск ингредиента по name
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
