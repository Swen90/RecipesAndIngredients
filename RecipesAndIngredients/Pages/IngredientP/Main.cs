using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.IngredientP
{
    public static partial class IngredientPage
    {
        public static void IngredientsPage()
        {
            bool exit = false;
            IngredientService ingredientService = new IngredientService();
            while (true)
            {
                Console.WriteLine("Выберите операцию:");
                Console.WriteLine("1 - Показать все ингредиенты");
                Console.WriteLine("2 - Добавить новый ингредиент");
                Console.WriteLine("3 - Редактировать информацию об ингредиенте");
                Console.WriteLine("4 - Получение информации об ингредиенте");
                Console.WriteLine("5 - Удаление ингредиента");
                Console.WriteLine("6 - Вернуться на главную страницу");

                int key = Utils.GetAndValidateNullInt();
                if (key <= 0 || key > 6)
                {
                    Console.WriteLine("Цифра должна соответствовать номеру из списка. Повторите ввод");
                    continue;
                }
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
    }
}