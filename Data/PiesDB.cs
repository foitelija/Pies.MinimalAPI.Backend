

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


//проверяем, что БД создана

// Configure the HTTP request pipeline.







//Middleware HTTP -> HTTPs

public class PiesDB : DbContext
{
    public PiesDB(DbContextOptions<PiesDB> options) : base(options) { }
    public DbSet<Pies> Pies => Set<Pies>();
}

