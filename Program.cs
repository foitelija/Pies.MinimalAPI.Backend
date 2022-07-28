var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PiesDB>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MrMYSQL"));
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPieService, PieService>();

var app = builder.Build();

// Configure the HTTP request pipeline. // DB EXIST
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PiesDB>();
    db.Database.EnsureCreated();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapGet("/pies", async (IPieService pieService) => Results.Ok(await pieService.GetPiesAsync()))
    .Produces<List<Pies>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetAllPies")
    .WithTags("Getters");

app.MapGet("/pies/{id}", async (int id, IPieService pieService) => await pieService.GetPieAsync(id) is Pies pies ? Results.Ok(pies) : Results.NotFound())
    .Produces<Pies>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetSinglePie")
    .WithTags("Getters");

app.MapPost("/pies", async([FromBody] Pies pies, IPieService pieService) =>
    {
        await pieService.InsertPieAsync(pies);
        await pieService.SaveAsync();
        return Results.Created($"/pies/{pies.Id}", pies);
    })
    .Accepts<Pies>("application/json")
    .Produces<Pies>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("CreatePie")
    .WithTags("Creators");

app.MapPut("/pies", async ([FromBody] Pies pie, IPieService pieService) =>
    {
        await pieService.UpdatePieAsync(pie);
        await pieService.SaveAsync();
        return Results.NoContent();
    })
    .Accepts<Pies>("application/json")
    .WithName("UpdateHotel")
    .WithTags("Updaters");

app.MapDelete("/hotels/{id}", async (int id, IPieService pieService) =>
    {
        await pieService.DeletePieAsync(id);
        await pieService.SaveAsync();
        return Results.NoContent();
    })
    .WithName("DeletePie")
    .WithTags("Deletes");

app.MapGet("/pies/search/name/{query}", async (string query, IPieService pieService) =>
    await pieService.GetPiesAsync(query) is IEnumerable<Pies> pies
    ? Results.Ok(pies)
    : Results.NotFound(Array.Empty<Pies>()))
    .Produces<List<Pies>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("SearchPies")
    .WithTags("Getters");

//Middleware HTTP -> HTTPs
app.UseHttpsRedirection();

app.Run();

