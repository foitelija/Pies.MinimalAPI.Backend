public class PiesDB : DbContext
{
    public PiesDB(DbContextOptions<PiesDB> options) : base(options) { }
    public DbSet<Pies> Pies => Set<Pies>();
}

