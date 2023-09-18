using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.IngredientP
{
    public partial class IngredientPage
    {
        public static void GetIngredient(IngredientService ingredientService)
        {
            while (true)
            {
                Console.WriteLine("Введите название ингредиента");

                string ingName = Utils.GetAndValidateNullString();
                if (ingredientService.CheckExistanceByName(ingName) == false)
                {
                    Console.WriteLine($"Ингредиента с названием {ingName} не существует");
                    continue;
                }
                IngredientDto ingredientDto = ingredientService.GetByNameDto(ingName);
            
                Console.WriteLine($"IngredientName - {ingredientDto.IngName}, QuantityType {ingredientDto.QuantityType.Name}");
            }
        }
    }
}
