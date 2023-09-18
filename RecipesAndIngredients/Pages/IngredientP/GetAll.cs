using RecipesAndIngredients.DTO;
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
        public static void GetAllIngredients(IngredientService ingredientService)
        {
            Console.WriteLine("Список ингредиентов");

            List<IngredientDto>? ingredientsDto = ingredientService.GetAll();
            if (ingredientsDto == null)
            {
                Console.WriteLine("Список не найден");
                return; /// при void, return пишется без возвратного значения
            }

            foreach (IngredientDto ingredientDto1 in ingredientsDto)
            {
                Console.WriteLine($"Id = {ingredientDto1.Id}  \tIngredientName = {ingredientDto1.IngName}" +
                    $"\t\t QuantityType = {ingredientDto1.QuantityType.Name}"); /// break не нужен, перебрал и закончил
            }
        }
    }
}
