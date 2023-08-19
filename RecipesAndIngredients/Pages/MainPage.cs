using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.Pages
{
    public class MainPage
    {
        public void TheMainPage()
        {
            IngredientPage ingredientPage = new IngredientPage();
            RecipePage recipePage = new RecipePage();
            while (true)
            {
                Console.WriteLine("Выберите страницу");
                Console.WriteLine("1 - Ингредиенты, 2 - Рецепты");
                string? input = Console.ReadLine();
                if(string.IsNullOrEmpty(input) )
                {
                    Console.WriteLine("Неверный ввод");
                    continue;
                }
                int inputConv = Convert.ToInt32(input);
                switch(inputConv)
                {
                    case 1:
                        ingredientPage.IngredientsPage();
                        break;
                    case 2:
                        recipePage.RecipesPage();
                        break;
                }
            }
        }
    }
}
