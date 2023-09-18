
namespace RecipesAndIngredients.DTO
{
    public class IngredientAndQuantityDto
    {
        public int? QuantityCount { get; set; }

        public  IngredientDto Ingredient { get; set; } = null!;
    }
}
