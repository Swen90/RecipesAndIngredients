using RecipesAndIngredients.DTO;
using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.RecipeP
{
    public partial class RecipePage
    {
        public static void GetAllRecipies(RecipeService recipeService)
        {
            Console.WriteLine("Список всех рецептов");

            List<RecipeDto> recipesDto = recipeService.GetAll()!;

            foreach (RecipeDto recipeDto in recipesDto)
            {
                Console.WriteLine($"RecipeName = {recipeDto.RecName}, Category = {recipeDto.Category.CategName}");
                foreach (KeyValuePair<int, IngredientAndQuantityDto> ingredientAndQuantityDto in recipeDto.Ingredients)
                {
                    Console.WriteLine($"{ingredientAndQuantityDto.Value.Ingredient.IngName} - {ingredientAndQuantityDto.Value.QuantityCount}" +
                        $" {ingredientAndQuantityDto.Value.Ingredient.QuantityType.Name}");
                }
                Console.WriteLine();
            }
        }
    }
}
