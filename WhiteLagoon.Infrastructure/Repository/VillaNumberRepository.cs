using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {

        private readonly ApplicationDbContext _context;

        public VillaNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(VillaNumber entity)
        {
            _context.Update(entity);
        }

        public bool Exists(VillaNumber entity)
        {
            return _context.VillaNumbers.Any(v => v.Villa_Number == entity.Villa_Number);
        }
    }
}
