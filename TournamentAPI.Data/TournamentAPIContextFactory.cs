using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data
{
    internal class TournamentAPIContextFactory :
        IDesignTimeDbContextFactory<TournamentAPIContext>
    {
        public TournamentAPIContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<TournamentAPIContext>();
            options.UseSqlServer("Not required for scaffolding");

            return new TournamentAPIContext(options.Options);
        }
    }
}
