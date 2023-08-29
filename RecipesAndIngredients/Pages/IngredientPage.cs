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

                int key = Utils.GetAndValidateNullInt();
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

            string ingName = Utils.GetAndValidateNullString();
            IngredientDto? ingredientDto = ingredientService.GetByNameDto(ingName);
            if (ingredientDto == null)
            {
                Console.WriteLine("Ингредиент не найден");
            }
            Console.WriteLine($"IngredientName - {ingredientDto.IngName}, QuantityType {ingredientDto.QuantityType.Name}");
        }



        public static void AddNewIngredient(IngredientService ingredientService)
        {
            int quantityType;
            Console.WriteLine("Добавление ингредиента");
            Console.WriteLine("Введите название ингредиента");

            string ingName = Utils.GetAndValidateNullString();

            while (true) //////////// новое, показать спросить
            {
                Console.WriteLine("Выберите тип исчисления: 1 - Г, 2 - Шт"); 
                quantityType = Utils.GetAndValidateNullInt();
                if (quantityType < 1 || quantityType > 2)
                {
                    Console.WriteLine("Цифра должна соответствовать номеру из списка. Повторите ввод");
                }
                break;
            }

            QuantityTypeDto? quantityDto = ingredientService.GetQuantityTypeById(quantityType);
            IngredientDto? ingredientDto = new IngredientDto()
            {
                IngName = ingName,
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
            while (true)
            {
                Console.WriteLine("Введите название ингредиента");

                string ingName = Utils.GetAndValidateNullString();
                if(ingredientService.CheckExistanceByName(ingName) == false) /// метод возвращает bool значит можно сократить на 1 строчку и сравнивать сразу в условии
                {
                    Console.WriteLine($"Ингредиент с названием {ingName} не существует");
                    continue;
                }
                ingredientDto = ingredientService.GetByNameDto(ingName);
                break;
            }
            while (true)
            {
                Console.WriteLine("1 - Редактировать название");
                Console.WriteLine("2 - Редактировать тип исчисления");

                int input = Utils.GetAndValidateNullInt();
                string ingName; /// "это как пример!" объявление переменной внутри цикла будет видна и далее в нем как для свича так и для других конструкций
                /// область существования переменной может быть только в области {} скобок
                switch (input)
                {
                    case 1:
                        Console.WriteLine("Введите новое название для ингредиента");

                        ingName = Utils.GetAndValidateNullString();
                        ingredientDto.IngName = ingName == null ? ingredientDto.IngName : ingName;
                        break;
                    case 2:
                        Console.WriteLine("Выберите тип исчисления");

                        List<QuantityTypeDto>? quantityTypeList = ingredientService.GetAllQuantityTypes();
                        int count = quantityTypeList.Count;
                        for (int i = 1; i <= count; i++)
                        {
                            Console.WriteLine($"{i} - {quantityTypeList[i-1].Name}");
                        }
                        while (true)
                        {
                            int type = Utils.GetAndValidateNullInt();
                            if(type < 1 || type > 2)
                            {
                                Console.WriteLine("Цифра должна соответствовать номеру из списка. Повторите ввод");
                            }
                            /////////////////// сделать проверку на 1 и на 2, на 0 и выше.
                            ingredientDto.QuantityType.Id = quantityTypeList[type - 1].Id;
                            ingredientDto.QuantityType.Name = quantityTypeList[type - 1].Name;
                            break;
                        }
                        break;
                }
                break;
            }
            if (ingredientService.UpdateIngredient(ingredientDto) == false)
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
            Console.WriteLine("Введите название ингредиента");

            string? ingName = Utils.GetAndValidateNullString();
            if(ingredientService.CheckExistanceByName(ingName) == false )
            {
                Console.WriteLine("Отмена удаления");
            }
            else
            {
                ingredientService.Remove(ingName);
                Console.WriteLine("Ингредиент успешно удален");
            }
        }
    }
}