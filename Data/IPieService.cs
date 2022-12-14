namespace Bakery.MinimalAPI.Backend.Data
{
    public interface IPieService : IDisposable
    {
        Task<List<Pies>> GetPiesAsync();
        Task<List<Pies>> GetPiesAsync(string name);
        Task<Pies> GetPieAsync(int piesId);
        Task InsertPieAsync(Pies pies);
        Task UpdatePieAsync(Pies pies);
        Task DeletePieAsync(int pieId);
        Task SaveAsync();
    }
}
