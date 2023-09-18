using RecipesAndIngredients.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.Pages.IngredientP
{
    public partial class IngredientPage
    {
        public static void RemoveIngredient(IngredientService ingredientService)
        {
            Console.WriteLine("Введите название ингредиента");

            string? ingName = Utils.GetAndValidateNullString();
            if (ingredientService.CheckExistanceByName(ingName) == false)
                Console.WriteLine("Отмена удаления");

            ingredientService.Remove(ingName);
            Console.WriteLine("Ингредиент успешно удален");
        }
    }
}
