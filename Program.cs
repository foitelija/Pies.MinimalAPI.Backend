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
app.MapGet("/pies", async (IPieService pieService) => Results.Ok(await pieService.GetPiesAsync()));

app.MapGet("/pies/{id}", async (int id, IPieService pieService) => await pieService.GetPieAsync(id) is Pies pies ? Results.Ok(pies) : Results.NotFound());

app.MapPost("/pies", async([FromBody] Pies pies, IPieService pieService) =>
{
    await pieService.InsertPieAsync(pies);
    await pieService.SaveAsync();
    return Results.Created($"/pies/{pies.Id}", pies);
});

app.MapPut("/pies", async ([FromBody] Pies pie, IPieService pieService) =>
{
    await pieService.UpdatePieAsync(pie);
    await pieService.SaveAsync();
    return Results.NoContent();
});

app.MapDelete("/hotels/{id}", async (int id, IPieService pieService) =>
{
    await pieService.DeletePieAsync(id);
    await pieService.SaveAsync();
    return Results.NoContent();
});

//Middleware HTTP -> HTTPs
app.UseHttpsRedirection();

app.Run();

