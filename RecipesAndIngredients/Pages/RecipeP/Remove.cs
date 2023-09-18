using RecipesAndIngredients.Services;

namespace RecipesAndIngredients.Pages.RecipeP
{
    public partial class RecipePage
    {
        public static void RemoveRecipe(RecipeService recipeService)
        {
            Console.WriteLine("Введите название рецепта");

            string recName = Utils.GetAndValidateNullString();
            if (recipeService.CheckExistanceByName(recName) == false)
                Console.WriteLine("Отмена удаления");

            recipeService.Remove(recName);
            Console.WriteLine("Ингредиент успешно удален");
        }
    }
}
