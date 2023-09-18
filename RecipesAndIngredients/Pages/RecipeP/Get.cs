using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.RecipeP
{
    public partial class RecipePage
    {
        public static void GetRecipe(RecipeService recipeService)
        {
            Console.WriteLine("Введите название рецепта");

            while (true)
            {
                string recName = Utils.GetAndValidateNullString();
                RecipeDto? recipeDto = recipeService.GetByNameDto(recName);
                if (recipeDto == null)
                {
                    Console.WriteLine("Рецепт не существует");
                    continue;
                }
                Console.WriteLine($"RecipeName - {recipeDto.RecName}, RecipeCategory - {recipeDto.Category.CategName}");

                foreach (KeyValuePair<int, IngredientAndQuantityDto> ingredientAndQuantityDto in recipeDto.Ingredients)
                {
                    Console.WriteLine($"{ingredientAndQuantityDto.Value.Ingredient.IngName} - {ingredientAndQuantityDto.Value.QuantityCount}" +
                        $" {ingredientAndQuantityDto.Value.Ingredient.QuantityType.Name}");
                }
                break;
            }
        }
    }
}
