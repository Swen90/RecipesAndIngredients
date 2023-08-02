using System;
using RecipesAndIngredients.Pages;






namespace RecipesAndIngredients
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //IngredientPage ingPage = new IngredientPage();

            //ingPage.IngredientsPage();

            RecipePage recipePage = new RecipePage();

            recipePage.RecipesPage();
        }
    }
}
