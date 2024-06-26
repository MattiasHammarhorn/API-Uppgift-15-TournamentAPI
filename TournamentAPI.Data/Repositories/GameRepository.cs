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
    public class GameRepository : IGameRepository
    {
        private readonly TournamentAPIContext _context;

        public GameRepository(TournamentAPIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync(string? title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return await _context.Games.ToListAsync();
            }

            return await _context.Games
                .Where(g => g.Title.Contains(title))
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<Game?> GetAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Games.AnyAsync(g => g.Id == id);
        }

        public void Add(Game game)
        {
            _context.Games.Add(game);
        }

        public void Update(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
        }

        public void Remove(Game game)
        {
            _context.Games.Remove(game);
        }
    }
}
