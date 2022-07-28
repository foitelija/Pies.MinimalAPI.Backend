using System.ComponentModel.DataAnnotations.Schema;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var pies = new List<Pies>();

app.UseHttpsRedirection();
app.MapGet("/pies", () => pies);
app.MapGet("/pies/{id}", (int id) => pies.FirstOrDefault(p => p.Id == id));

app.MapPost("pies", (Pies pie) => pies.Add(pie));

app.MapPut("/pies", (Pies pie) =>
{
    var index = pies.FindIndex(p => p.Id == pie.Id);
    if(index < 0)
    {
        throw new Exception("Pie for update not found.");
    }
    pies[index] = pie;
});

app.MapDelete("/hotels/{id}", (int id) =>
{
    var index = pies.FindIndex(p=>p.Id == id);
    if(index < 0)
    {
        throw new Exception("Pie for delete not found.");
    }
    pies.RemoveAt(index);
});


app.Run();


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

