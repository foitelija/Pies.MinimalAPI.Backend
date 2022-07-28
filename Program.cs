

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PiesDB>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MrMYSQL"));
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//проверяем, что БД создана
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PiesDB>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapGet("/pies", async (PiesDB db) => await db.Pies.ToListAsync());

app.MapGet("/pies/{id}", async (int id, PiesDB db) => await db.Pies.FirstOrDefaultAsync(p => p.Id == id) is Pies pies ? Results.Ok(pies) : Results.NotFound());

app.MapPost("/pies", async([FromBody] Pies pies, PiesDB db) =>
{
    db.Pies.Add(pies);
    await db.SaveChangesAsync();
    return Results.Created($"/pies/{pies.Id}", pies);
});

app.MapPut("/pies", async ([FromBody] Pies pie, PiesDB db) =>
{
    var piesFromDb = await db.Pies.FindAsync(new object[]
    {
        pie.Id
    });
    if(piesFromDb == null)
    {
        return Results.NotFound();
    }
    piesFromDb.Name = pie.Name;
    piesFromDb.Description = pie.Description;
    piesFromDb.Weight = pie.Weight;
    piesFromDb.imageUrl = pie.imageUrl;
    piesFromDb.Price = pie.Price;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/hotels/{id}", async (int id, PiesDB db) =>
{
    var piesFromDb = await db.Pies.FindAsync(new object[] { id });
    if(piesFromDb == null)
    {
        return Results.NotFound();
    }
    db.Pies.Remove(piesFromDb);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//Middleware HTTP -> HTTPs
app.UseHttpsRedirection();

app.Run();


public class PiesDB : DbContext
{
    public PiesDB(DbContextOptions<PiesDB> options) : base(options) { }
    public DbSet<Pies> Pies => Set<Pies>();
}

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

