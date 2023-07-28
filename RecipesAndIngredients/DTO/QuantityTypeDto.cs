using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.DTO
{
    public class QuantityTypeDto
    {
        public int Id { get; set; }

        public string? Quantity { get; set; }

        public List<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
    }
}
