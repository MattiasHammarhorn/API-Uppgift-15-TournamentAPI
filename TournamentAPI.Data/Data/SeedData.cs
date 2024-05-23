using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public class SeedData
    {
        private static Faker faker;

        public static async Task InitAsync(TournamentAPIContext context)
        {
            if (await context.Tournaments.AnyAsync()) return;

            faker = new Faker("sv");

            var tournaments = GenerateTournaments(3);
            await context.AddRangeAsync(tournaments);

            await context.SaveChangesAsync();
        }

        private static IEnumerable<Tournament> GenerateTournaments(int numberOfTournaments)
        {
            var tournaments = new List<Tournament>();

            for (int i = 0; i < numberOfTournaments; i++)
            {
                Random rand = new Random();

                var title = faker.Company.Bs();
                var startDate = DateTime.Now.AddDays(3);
                var endDate = startDate.AddDays(rand.Next(2,6));
                var games = GenerateGames(rand.Next(2, 5), startDate, endDate);

                var tournament = new Tournament
                {
                    Title = title,
                    StartDate = startDate,
                    Games = games.ToList()
                };
                tournaments.Add(tournament);
            }

            return tournaments;
        }

        private static IEnumerable<Game> GenerateGames(int numberOfGames, DateTime startDate, DateTime endDate)
        {
            var games = new List<Game>();

            for (int i = 0; i < numberOfGames; i++)
            {
                games.Add(new Game
                {
                    Title = faker.Company.Bs(),
                    Time = faker.Date.Between(startDate, endDate)
                });
            }

            return games;
        }
    }
}
