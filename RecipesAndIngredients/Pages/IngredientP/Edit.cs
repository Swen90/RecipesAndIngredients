using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.IngredientP
{
    public partial class IngredientPage
    {
        public static void EditIngredient(IngredientService ingredientService)
        {
            IngredientDto? ingredientDto;
            Console.WriteLine("Редактирование информации об ингредиенте");
            while (true)
            {
                Console.WriteLine("Введите название ингредиента");

                string ingName = Utils.GetAndValidateNullString();
                if (ingredientService.CheckExistanceByName(ingName) == false) /// метод возвращает bool значит можно сократить на 1 строчку и сравнивать сразу в условии
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

                int choice = Utils.GetAndValidateNullInt();
                if (choice <= 0 || choice > 2)
                {
                    Console.WriteLine("Цифра должна соответствовать номеру из списка. Повторите ввод");
                    continue;
                }
                string ingName; /// "это как пример!" объявление переменной внутри цикла будет видна и далее в нем как для свича так и для других конструкций
                                /// область существования переменной может быть только в области {} скобок
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Введите новое название для ингредиента");

                        ingName = Utils.GetAndValidateNullString();
                        ingredientDto.IngName = ingName == null ? ingredientDto.IngName : ingName;

                        Console.WriteLine($"Новое название \"{ingredientDto.IngName}\" успешно сохранено");
                        break;
                    case 2:
                        Console.WriteLine("Выберите тип исчисления");

                        List<QuantityTypeDto>? quantityTypeList = ingredientService.GetAllQuantityTypes();
                        /// QUE: возможно ли в теории полчение пустого списка из БД. На мой взгляд нет, а значит возврат из сервиса null невозможен
                        //if (quantityTypeList == null)
                        //{
                        //    Console.WriteLine("Типы исчисления не найдены");
                        //}
                        int count = quantityTypeList.Count;
                        for (int i = 1; i <= count; i++)
                            Console.WriteLine($"{i} - {quantityTypeList[i - 1].Name}");

                        while (true)
                        {
                            int type = Utils.GetAndValidateNullInt();
                            if (type < 1 || type > 2)
                                Console.WriteLine("Цифра должна соответствовать номеру из списка. Повторите ввод");

                            ingredientDto.QuantityType.Id = quantityTypeList[type - 1].Id;
                            ingredientDto.QuantityType.Name = quantityTypeList[type - 1].Name;

                            Console.WriteLine($"Тип исчисления ингредиента \"{ingredientDto.QuantityType.Name}\" успешно сохранен");
                            break;
                        }
                        break;
                }
                break;
            }
            if (ingredientService.UpdateIngredient(ingredientDto) == false)
                Console.WriteLine("Редактирование отменено");

            Console.WriteLine("Данные успешно сохранены");
        }
    }
}
