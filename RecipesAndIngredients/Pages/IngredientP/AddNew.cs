using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.IngredientP
{
    public partial class IngredientPage
    {
        public static void AddNewIngredient(IngredientService ingredientService)
        {
            int quantityType;
            Console.WriteLine("Добавление ингредиента");
            Console.WriteLine("Введите название ингредиента");

            string ingName = Utils.GetAndValidateNullString();
            while (true)
            {
                Console.WriteLine("Выберите тип исчисления: 1 - Г, 2 - Шт");
                quantityType = Utils.GetAndValidateNullInt();
                if (quantityType < 1 || quantityType > 2) /// QUE: нужно ли сделать так же как с категориями рецепта, в список, конвертировать, вывести через for
                {
                    Console.WriteLine("Цифра должна соответствовать номеру из списка. Повторите ввод");
                    continue;
                }
                break;
            }

            QuantityTypeDto? quantityDto = ingredientService.GetQuantityTypeById(quantityType);
            if (quantityDto == null)
            {
                Console.WriteLine("Не найден тип исчисления");
                return; /// QUE: return?
            }
            IngredientDto ingredientDto = new IngredientDto()
            {
                IngName = ingName,
                QuantityType = quantityDto,
            };
            ingredientService.Add(ingredientDto);

            Console.WriteLine("Добавлен новый ингредиент");
            Console.WriteLine($"Name - {ingredientDto.IngName}, QuantityType - {ingredientDto.QuantityType.Name}");
        }
    }
}
