﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class TournamentRepository: ITournamentRepository
    {
        private readonly TournamentAPIContext _context;

        public TournamentRepository(TournamentAPIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync(bool includeGames)
        {
            if (includeGames)
            {
                return await _context.Tournaments.Include(t => t.Games).ToListAsync();
            }
            return await _context.Tournaments.ToListAsync();
        }

        public async Task<Tournament?> GetAsync(int id, bool includeGames)
        {
            if (includeGames)
            {
                return await _context.Tournaments.Include(t => t.Games).Where(t => t.Id == id).FirstOrDefaultAsync();
            }

            return await _context.Tournaments.FindAsync(id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Tournaments.AnyAsync(e => e.Id == id);
        }

        public void Add(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
        }

        public void Update(Tournament tournament)
        {
            _context.Entry(tournament).State = EntityState.Modified;
        }

        public void Remove(Tournament tournament)
        {
            _context.Tournaments.Remove(tournament);
        }
    }
}
