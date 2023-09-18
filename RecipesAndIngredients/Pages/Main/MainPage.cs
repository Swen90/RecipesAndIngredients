using RecipesAndIngredients.Pages.IngredientP;
using RecipesAndIngredients.Pages.RecipeP;

namespace RecipesAndIngredients.Pages.Main
{
    public class MainPage
    {
        public void TheMainPage()
        {
            while (true)
            {
                Console.WriteLine("Выберите страницу");
                Console.WriteLine("1 - Ингредиенты, 2 - Рецепты");

                int pageChoose = Utils.GetAndValidateNullInt();
                switch (pageChoose)
                {
                    case 1:
                        Console.Clear();
                        IngredientPage.IngredientsPage();
                        break;
                    case 2:
                        Console.Clear();
                        RecipePage.RecipesPage();
                        break;
                }
            }
        }
    }
}
