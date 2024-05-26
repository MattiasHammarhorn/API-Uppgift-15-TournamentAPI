using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TournamentAPIContext _context;
        private ITournamentRepository _tournamentRepository;
        private IGameRepository _gameRepository;

        public UnitOfWork(TournamentAPIContext context,
            ITournamentRepository tournamentRepository,
            IGameRepository gameRepository)
        {
            _context = context;
            _tournamentRepository = tournamentRepository;
            _gameRepository = gameRepository;
        }

        public ITournamentRepository TournamentRepository
        {
            get
            {
                if (this._tournamentRepository is null)
                {
                    _tournamentRepository = new TournamentRepository(_context);
                }

                return _tournamentRepository;
            }
        }

        public IGameRepository GameRepository
        {
            get
            {
                if (this._gameRepository is null)
                {
                    _gameRepository = new GameRepository(_context);
                }

                return _gameRepository;
            }
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
