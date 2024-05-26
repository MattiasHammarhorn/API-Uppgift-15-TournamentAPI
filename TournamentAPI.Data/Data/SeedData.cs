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

            //var tournaments = GenerateTournaments(3);
            var tournaments = new List<Tournament>
            {
                new Tournament
                {
                    Title = "Turnering 1",
                    StartDate = DateTime.Now,
                    Games = new List<Game>
                    {
                        new Game
                        {
                            Title = "T1 - Match 1",
                            Time = DateTime.Now.AddMinutes(10)
                        },
                        new Game
                        {
                            Title = "T1 - Match 2",
                            Time = DateTime.Now.AddMinutes(20)
                        }
                    }
                },
                new Tournament
                {
                    Title = "Turnering 2",
                    StartDate = DateTime.Now,
                    Games = new List<Game>
                    {
                        new Game
                        {
                            Title = "T2 - Match 3",
                            Time = DateTime.Now.AddMinutes(30)
                        },
                        new Game
                        {
                            Title = "T2 - Match 4",
                            Time = DateTime.Now.AddMinutes(40)
                        }
                    }
                },
            };
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
                var endDate = startDate.AddMonths(3);
                var games = GenerateGames(2, startDate, endDate);

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
