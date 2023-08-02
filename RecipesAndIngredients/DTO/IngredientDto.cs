using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAndIngredients.DTO
{
    public class IngredientDto
    {
        public int Id { get; set; }

        public string IngName { get; set; } = null!;

        public QuantityTypeDto? QuantityType { get; set; } = new QuantityTypeDto();

    }
}
