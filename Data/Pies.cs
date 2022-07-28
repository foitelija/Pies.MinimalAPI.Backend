public class Pies
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Weight { get; set; }
    public string imageUrl { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}

