using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.RecipeP
{
    public static partial class RecipePage
    {
        public static void RecipesPage()
        {
            bool exit = false;
            RecipeService recipeService = new RecipeService();
            IngredientService ingredientService = new IngredientService();
            while (true)
            {
                Console.WriteLine("\tВыберите операцию:");
                Console.WriteLine("1 - Показать весь список рецептов");
                Console.WriteLine("2 - Создать новый рецепт");
                Console.WriteLine("3 - Редактировать состав рецепта");
                Console.WriteLine("4 - Получение информации о рецепте");
                Console.WriteLine("5 - Удаление рецепта из списка");
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







       



        



        
    }  
}
