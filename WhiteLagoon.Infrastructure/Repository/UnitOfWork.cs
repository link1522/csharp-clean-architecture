﻿using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        public IVillaRepository Villa { get; private set; }

        public IVillaNumberRepository VillaNumber { get; private set; }

        public IAmenityRepository Amenity {  get; private set; }

        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext context) {
            _context = context;
            Villa = new VillaRepository(_context);
            VillaNumber = new VillaNumberRepository(_context);
            Amenity = new AmenityRepository(_context);
            User = new UserRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
