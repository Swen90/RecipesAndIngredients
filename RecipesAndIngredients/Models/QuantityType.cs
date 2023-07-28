using System;
using System.Collections.Generic;

namespace RecipesAndIngredients.Models;

public partial class QuantityType
{
    public int Id { get; set; }

    public string? Quantity { get; set; }

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
