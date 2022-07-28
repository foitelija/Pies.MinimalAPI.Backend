namespace Bakery.MinimalAPI.Backend.Data
{
    public class PieService : IPieService
    {
        private readonly PiesDB _context;

        public PieService(PiesDB context)
        {
            _context = context;
        }

        public Task<List<Pies>> GetPiesAsync()
        {
            return _context.Pies.ToListAsync();
        }
        public async Task<Pies> GetPieAsync(int piesId)
        {
            return await _context.Pies.FirstOrDefaultAsync(p => p.Id == piesId);
        }
        public async Task InsertPieAsync(Pies pies)
        {
            await _context.Pies.AddAsync(pies);
        }

        public async Task UpdatePieAsync(Pies pies)
        {
            var piesFromDb = await _context.Pies.FindAsync(new object[]
            {
                pies.Id
            });
            if(piesFromDb == null)
            {
                return;
            }
            piesFromDb.Name = pies.Name;
            piesFromDb.Description = pies.Description;
            piesFromDb.Weight = pies.Weight;
            piesFromDb.imageUrl = pies.imageUrl;
            piesFromDb.Price = pies.Price;
        }

        public async Task DeletePieAsync(int pieId)
        {
            var piesFromDb = await _context.Pies.FindAsync(new object[] { pieId });
            if (piesFromDb == null)
            {
                return;
            }
            _context.Pies.Remove(piesFromDb);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
    }
}
