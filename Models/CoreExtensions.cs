using System;
using System.Globalization;
using System.Linq;
using Core.Models.Identity;
using Core.Models.Pool;
using Microsoft.EntityFrameworkCore;

namespace Core.Models
{
    public static class CoreExtensions
    {
        public static void SeedData(this CoreContext context, bool isDevelopment = false, string ownerId = null)
        {
            if (context.AllMigrationsApplied())
            {

                if (!context.Users.Any()) {
                    var adminUser = context.Users.Add(new User { FirstName = "Admin", LastName = "User", OwnerId = ownerId }).Entity;
                    context.Posts.Add(new Post { UserId = adminUser.Id, Content = "Welcome to the app!" });

                    if (isDevelopment) {
                        var testUser1 = context.Users.Add(new User { FirstName = "Jan", LastName = "Janssen" }).Entity;
                        var testUser2 = context.Users.Add(new User { FirstName = "Karel", LastName = "Karels" }).Entity;
                        context.Posts.AddRange(
                            new Post { UserId = testUser1.Id, Content = "What a piece of junk!" },
                            new Post { UserId = testUser2.Id, Content = "More junk!" },
                            new Post { UserId = testUser2.Id, Content = "A lot of junk!" }
                        );
                    }
                    context.SaveChanges();
                }
                if (!context.Country.Any()) {
                    context.Country.Add(new Country { Id = 1, Name = "Frankrijk", Code = "FR", Group = "A" });
                    context.Country.Add(new Country { Id = 2, Name = "Roemenië", Code = "RO", Group = "A" });
                    context.Country.Add(new Country { Id = 3, Name = "Albanië", Code = "AL", Group = "A" });
                    context.Country.Add(new Country { Id = 4, Name = "Zwitserland", Code = "CH", Group = "A" });
                    context.Country.Add(new Country { Id = 5, Name = "Wales", Code = "GB-WLS", Group = "B" });
                    context.Country.Add(new Country { Id = 6, Name = "Slowakije", Code = "SK", Group = "B" });
                    context.Country.Add(new Country { Id = 7, Name = "Engeland", Code = "GB-ENG", Group = "B" });
                    context.Country.Add(new Country { Id = 8, Name = "Rusland", Code = "RU", Group = "B" });
                    context.Country.Add(new Country { Id = 9, Name = "Polen", Code = "PL", Group = "C" });
                    context.Country.Add(new Country { Id = 10, Name = "Noord-Ierland", Code = "GB-NIR", Group = "C" });
                    context.Country.Add(new Country { Id = 11, Name = "Duitsland", Code = "DE", Group = "C" });
                    context.Country.Add(new Country { Id = 12, Name = "Oekraïne", Code = "UA", Group = "C" });
                    context.Country.Add(new Country { Id = 13, Name = "Turkije", Code = "TR", Group = "D" });
                    context.Country.Add(new Country { Id = 14, Name = "Kroatië", Code = "HR", Group = "D" });
                    context.Country.Add(new Country { Id = 15, Name = "Spanje", Code = "ES", Group = "D" });
                    context.Country.Add(new Country { Id = 16, Name = "Tsjechië", Code = "CZ", Group = "D" });
                    context.Country.Add(new Country { Id = 17, Name = "Ierland", Code = "IE", Group = "E" });
                    context.Country.Add(new Country { Id = 18, Name = "Zweden", Code = "SE", Group = "E" });
                    context.Country.Add(new Country { Id = 19, Name = "België", Code = "BE", Group = "E" });
                    context.Country.Add(new Country { Id = 20, Name = "Italië", Code = "IT", Group = "E" });
                    context.Country.Add(new Country { Id = 21, Name = "Oostenrijk", Code = "AT", Group = "F" });
                    context.Country.Add(new Country { Id = 22, Name = "Hongarije", Code = "HU", Group = "F" });
                    context.Country.Add(new Country { Id = 23, Name = "Portugal", Code = "PT", Group = "F" });
                    context.Country.Add(new Country { Id = 24, Name = "IJsland", Code = "IS", Group = "F" });
                    context.SaveChanges();
                }
                if (!context.Finals.Any()) {
                    context.Finals.Add(new Finals { Id = 1, LevelName = "Achtste finales", LevelNumber = 1 });
                    context.Finals.Add(new Finals { Id = 2, LevelName = "Kwart finales", LevelNumber = 2 });
                    context.Finals.Add(new Finals { Id = 3, LevelName = "Halve finales", LevelNumber = 3 });
                    context.Finals.Add(new Finals { Id = 4, LevelName = "Finale", LevelNumber = 4 });
                    context.Finals.Add(new Finals { Id = 5, LevelName = "Winnaar", LevelNumber = 5 });
                    context.SaveChanges();
                }
                if (!context.Match.Any()) {
                    string format = "yyyy-MM-dd HH:mm:ss";
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    context.Match.Add(new Match { Id = 1, Country1Id = 1, Country2Id = 2, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-10 21:00:00", format, culture), Location = "Saint-Denis" });
                    context.Match.Add(new Match { Id = 2, Country1Id = 3, Country2Id = 4, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-11 15:00:00", format, culture), Location = "Lens" });
                    context.Match.Add(new Match { Id = 3, Country1Id = 5, Country2Id = 6, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-11 18:00:00", format, culture), Location = "Bordeaux" });
                    context.Match.Add(new Match { Id = 4, Country1Id = 7, Country2Id = 8, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-11 21:00:00", format, culture), Location = "Marseille" });
                    context.Match.Add(new Match { Id = 5, Country1Id = 13, Country2Id = 14, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-12 15:00:00", format, culture), Location = "Parijs" });
                    context.Match.Add(new Match { Id = 6, Country1Id = 9, Country2Id = 10, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-12 18:00:00", format, culture), Location = "Nice" });
                    context.Match.Add(new Match { Id = 7, Country1Id = 11, Country2Id = 12, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-12 21:00:00", format, culture), Location = "Villeneuve-d'Ascq" });
                    context.Match.Add(new Match { Id = 8, Country1Id = 15, Country2Id = 16, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-13 15:00:00", format, culture), Location = "Toulouse" });
                    context.Match.Add(new Match { Id = 9, Country1Id = 17, Country2Id = 18, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-13 18:00:00", format, culture), Location = "Saint-Denis" });
                    context.Match.Add(new Match { Id = 10, Country1Id = 19, Country2Id = 20, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-13 21:00:00", format, culture), Location = "Lyon" });
                    context.Match.Add(new Match { Id = 11, Country1Id = 21, Country2Id = 22, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-14 18:00:00", format, culture), Location = "Bordeaux" });
                    context.Match.Add(new Match { Id = 12, Country1Id = 23, Country2Id = 24, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-14 21:00:00", format, culture), Location = "Saint-Étienne" });
                    context.Match.Add(new Match { Id = 13, Country1Id = 8, Country2Id = 6, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-15 15:00:00", format, culture), Location = "Villeneuve-d'Ascq" });
                    context.Match.Add(new Match { Id = 14, Country1Id = 2, Country2Id = 4, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-15 18:00:00", format, culture), Location = "Parijs" });
                    context.Match.Add(new Match { Id = 15, Country1Id = 1, Country2Id = 3, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-15 21:00:00", format, culture), Location = "Marseille" });
                    context.Match.Add(new Match { Id = 16, Country1Id = 7, Country2Id = 5, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-16 15:00:00", format, culture), Location = "Lens" });
                    context.Match.Add(new Match { Id = 17, Country1Id = 12, Country2Id = 10, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-16 18:00:00", format, culture), Location = "Lyon" });
                    context.Match.Add(new Match { Id = 18, Country1Id = 11, Country2Id = 9, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-16 21:00:00", format, culture), Location = "Saint-Denis" });
                    context.Match.Add(new Match { Id = 19, Country1Id = 20, Country2Id = 18, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-17 15:00:00", format, culture), Location = "Toulouse" });
                    context.Match.Add(new Match { Id = 20, Country1Id = 16, Country2Id = 14, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-17 18:00:00", format, culture), Location = "Saint-Étienne" });
                    context.Match.Add(new Match { Id = 21, Country1Id = 15, Country2Id = 13, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-17 21:00:00", format, culture), Location = "Nice" });
                    context.Match.Add(new Match { Id = 22, Country1Id = 19, Country2Id = 17, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-18 15:00:00", format, culture), Location = "Bordeaux" });
                    context.Match.Add(new Match { Id = 23, Country1Id = 24, Country2Id = 22, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-18 18:00:00", format, culture), Location = "Marseille" });
                    context.Match.Add(new Match { Id = 24, Country1Id = 23, Country2Id = 21, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-18 21:00:00", format, culture), Location = "Parijs" });
                    context.Match.Add(new Match { Id = 25, Country1Id = 2, Country2Id = 3, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-19 21:00:00", format, culture), Location = "Lyon" });
                    context.Match.Add(new Match { Id = 26, Country1Id = 4, Country2Id = 1, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-19 21:00:00", format, culture), Location = "Villeneuve-d'Ascq" });
                    context.Match.Add(new Match { Id = 27, Country1Id = 6, Country2Id = 7, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-20 21:00:00", format, culture), Location = "Toulouse" });
                    context.Match.Add(new Match { Id = 28, Country1Id = 8, Country2Id = 5, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-20 21:00:00", format, culture), Location = "Saint-Étienne" });
                    context.Match.Add(new Match { Id = 29, Country1Id = 12, Country2Id = 9, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-21 18:00:00", format, culture), Location = "Marseille" });
                    context.Match.Add(new Match { Id = 30, Country1Id = 10, Country2Id = 11, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-21 18:00:00", format, culture), Location = "Parijs" });
                    context.Match.Add(new Match { Id = 31, Country1Id = 16, Country2Id = 13, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-21 21:00:00", format, culture), Location = "Lens" });
                    context.Match.Add(new Match { Id = 32, Country1Id = 14, Country2Id = 15, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-21 21:00:00", format, culture), Location = "Bordeaux" });
                    context.Match.Add(new Match { Id = 33, Country1Id = 24, Country2Id = 21, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-22 18:00:00", format, culture), Location = "Saint-Denis" });
                    context.Match.Add(new Match { Id = 34, Country1Id = 22, Country2Id = 23, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-22 18:00:00", format, culture), Location = "Lyon" });
                    context.Match.Add(new Match { Id = 35, Country1Id = 20, Country2Id = 17, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-22 21:00:00", format, culture), Location = "Villeneuve-d'Ascq" });
                    context.Match.Add(new Match { Id = 36, Country1Id = 18, Country2Id = 19, GoalsCountry1 = -1, GoalsCountry2 = -1, StartDate = DateTime.ParseExact("2016-06-22 21:00:00", format, culture), Location = "Nice" });
                    context.SaveChanges();
                }
                if (!context.MatchFinals.Any()) {
                    string format = "yyyy-MM-dd HH:mm:ss";
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    context.MatchFinals.Add(new MatchFinals { Id = 37, StartDate = DateTime.ParseExact("2016-06-25 15:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Saint-Étienne", Country1Text = "2e Poule A", Country2Text = "2e Poule C", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 38, StartDate = DateTime.ParseExact("2016-06-25 18:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Parijs", Country1Text = "Winnaar Poule B", Country2Text = "3e Poule A/C/D", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 39, StartDate = DateTime.ParseExact("2016-06-25 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Lens", Country1Text = "Winnaar Poule D", Country2Text = "3e Poule B/E/F", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 40, StartDate = DateTime.ParseExact("2016-06-26 15:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Lyon", Country1Text = "Winnaar Poule A", Country2Text = "3e Poule C/D/E", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 41, StartDate = DateTime.ParseExact("2016-06-26 18:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Villeneuve-d'Ascq", Country1Text = "Winnaar Poule C", Country2Text = "3e Poule A/B/F", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 42, StartDate = DateTime.ParseExact("2016-06-26 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Toulouse", Country1Text = "Winnaar Poule F", Country2Text = "2e Poule E", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 43, StartDate = DateTime.ParseExact("2016-06-27 18:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Saint-Denis", Country1Text = "Winnaar Poule E", Country2Text = "2e Poule D", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 44, StartDate = DateTime.ParseExact("2016-06-27 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Nice", Country1Text = "2e Poule B", Country2Text = "2e Poule F", LevelNumber = 1 });
                    context.MatchFinals.Add(new MatchFinals { Id = 45, StartDate = DateTime.ParseExact("2016-06-30 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Marsaille", Country1Text = "Winnaar 37", Country2Text = "Winnaar 39", LevelNumber = 2 });
                    context.MatchFinals.Add(new MatchFinals { Id = 46, StartDate = DateTime.ParseExact("2016-07-01 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Villeneuve-d'Ascq", Country1Text = "Winnaar 38", Country2Text = "Winnaar 42", LevelNumber = 2 });
                    context.MatchFinals.Add(new MatchFinals { Id = 47, StartDate = DateTime.ParseExact("2016-07-02 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Bordeaux", Country1Text = "Winnaar 41", Country2Text = "Winnaar 43", LevelNumber = 2 });
                    context.MatchFinals.Add(new MatchFinals { Id = 48, StartDate = DateTime.ParseExact("2016-07-03 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Saint-Denis", Country1Text = "Winnaar 40", Country2Text = "Winnaar 44", LevelNumber = 2 });
                    context.MatchFinals.Add(new MatchFinals { Id = 49, StartDate = DateTime.ParseExact("2016-07-06 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Lyon", Country1Text = "Winnaar 45", Country2Text = "Winnaar 46", LevelNumber = 3 });
                    context.MatchFinals.Add(new MatchFinals { Id = 50, StartDate = DateTime.ParseExact("2016-07-07 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Marseille", Country1Text = "Winnaar 47", Country2Text = "Winnaar 48", LevelNumber = 3 });
                    context.MatchFinals.Add(new MatchFinals { Id = 51, StartDate = DateTime.ParseExact("2016-07-10 21:00:00", format, culture), GoalsCountry1 = -1, GoalsCountry2 = -1, Location = "Parijs", Country1Text = "Winnaar 49", Country2Text = "Winnaar 50", LevelNumber = 4 });
                    context.SaveChanges();
                }
                if (!context.PoolPlayer.Any() && isDevelopment) {
                  var user = context.Users.SingleOrDefault(u => u.OwnerId == ownerId);
                  context.PoolPlayer.Add(new PoolPlayer { Id = 1, Name = "Admin", UserId = user.Id, OpenQuestions = "Open Questions" });
                  context.SaveChanges();
                }
                if (!context.PoolMessage.Any() && isDevelopment) {
                  context.PoolMessage.Add(new PoolMessage { Id = 1, PoolPlayerId = 1, PlacedDate = DateTime.UtcNow, Message = "Welcome to the pool", Status = Core.Models.Pool.PoolMessage.MessageStatus.Approved });
                  context.SaveChanges();
                }
                if (!context.MatchPrediction.Any() && isDevelopment) {
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 1, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 2, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 3, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 4, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 5, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 6, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 7, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 8, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 9, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 10, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 11, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 12, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 13, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 14, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 15, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 16, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 17, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 18, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 19, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 20, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 21, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 22, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 23, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 24, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 25, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 26, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 27, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 28, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 29, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 30, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 31, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 32, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 33, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 34, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 35, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.MatchPrediction.Add(new MatchPrediction { MatchId = 36, PoolPlayerId = 1, GoalsCountry1 = 0, GoalsCountry2 = 0 });
                  context.SaveChanges();
                }
                if (!context.FinalsPrediction.Any() && isDevelopment) {
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 1 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 2 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 3 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 4 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 5 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 6 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 7 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 8 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 9 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 10 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 11 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 12 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 13 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 14 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 15 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 1, CountryId = 16 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 1 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 2 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 3 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 4 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 5 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 6 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 7 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 2, CountryId = 8 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 3, CountryId = 1 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 3, CountryId = 2 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 3, CountryId = 3 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 3, CountryId = 4 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 4, CountryId = 1 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 4, CountryId = 2 });
                  context.FinalsPrediction.Add(new FinalsPrediction { PoolPlayerId = 1, FinalsId = 5, CountryId = 1 });
                  context.SaveChanges();
                }
            }
        }

/*DELETE FROM [dbo].[MatchFinals]
GO
DELETE FROM [dbo].[Match]
GO
DELETE FROM [dbo].[Country]
GO
DELETE FROM [dbo].[Finals]
GO

SET IDENTITY_INSERT [dbo].[Country] ON
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (1, N'Frankrijk', N'A')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (2, N'Roemenië', N'A')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (3, N'Albanië ', N'A')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (4, N'Zwitserland', N'A')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (5, N'Wales', N'B')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (6, N'Slowakije', N'B')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (7, N'Engeland', N'B')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (8, N'Rusland', N'B')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (9, N'Polen', N'C')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (10, N'Noord-Ierland', N'C')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (11, N'Duitsland ', N'C')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (12, N'Oekraïne', N'C')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (13, N'Turkije', N'D')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (14, N'Kroatië', N'D')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (15, N'Spanje', N'D')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (16, N'Tsjechië', N'D')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (17, N'Ierland', N'E')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (18, N'Zweden', N'E')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (19, N'België', N'E')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (20, N'Italië', N'E')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (21, N'Oostenrijk', N'F')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (22, N'Hongarije', N'F')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (23, N'Portugal', N'F')
INSERT INTO [dbo].[Country] ([ID], [Name], [Group]) VALUES (24, N'IJsland', N'F')
SET IDENTITY_INSERT [dbo].[Country] OFF
SET IDENTITY_INSERT [dbo].[Finals] ON
INSERT INTO [dbo].[Finals] ([ID], [LevelName], [LevelNumber]) VALUES (1, N'Achtste finales', 1)
INSERT INTO [dbo].[Finals] ([ID], [LevelName], [LevelNumber]) VALUES (2, N'Kwartfinales', 2)
INSERT INTO [dbo].[Finals] ([ID], [LevelName], [LevelNumber]) VALUES (3, N'Halve finales', 3)
INSERT INTO [dbo].[Finals] ([ID], [LevelName], [LevelNumber]) VALUES (4, N'Finale', 4)
INSERT INTO [dbo].[Finals] ([ID], [LevelName], [LevelNumber]) VALUES (5, N'Winnaar', 5)
SET IDENTITY_INSERT [dbo].[Finals] OFF
SET IDENTITY_INSERT [dbo].[Match] ON
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (1, 1, 2, N'2016-06-10 21:00:00', NULL, NULL, N'Saint-Denis')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (2, 3, 4, N'2016-06-11 15:00:00', NULL, NULL, N'Lens')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (3, 5, 6, N'2016-06-11 18:00:00', NULL, NULL, N'Bordeaux')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (4, 7, 8, N'2016-06-11 21:00:00', NULL, NULL, N'Marseille')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (5, 13, 14, N'2016-06-12 15:00:00', NULL, NULL, N'Parijs')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (6, 9, 10, N'2016-06-12 18:00:00', NULL, NULL, N'Nice')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (7, 11, 12, N'2016-06-12 21:00:00', NULL, NULL, N'Villeneuve-d'Ascq')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (8, 15, 16, N'2016-06-13 15:00:00', NULL, NULL, N'Toulouse')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (9, 17, 18, N'2016-06-13 18:00:00', NULL, NULL, N'Saint-Denis')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (10, 19, 20, N'2016-06-13 21:00:00', NULL, NULL, N'Lyon')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (11, 21, 22, N'2016-06-14 18:00:00', NULL, NULL, N'Bordeaux')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (12, 23, 24, N'2016-06-14 18:00:00', NULL, NULL, N'Saint-Étienne')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (13, 8, 6, N'2016-06-15 15:00:00', NULL, NULL, N'Villeneuve-d'Ascq')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (14, 2, 4, N'2016-06-15 18:00:00', NULL, NULL, N'Parijs')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (15, 1, 3, N'2016-06-15 21:00:00', NULL, NULL, N'Marseille')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (16, 7, 5, N'2016-06-16 15:00:00', NULL, NULL, N'Lens')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (17, 12, 10, N'2016-06-16 18:00:00', NULL, NULL, N'Lyon')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (18, 11, 9, N'2016-06-16 21:00:00', NULL, NULL, N'Saint-Denis')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (19, 20, 18, N'2016-06-17 15:00:00', NULL, NULL, N'Toulouse')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (20, 16, 14, N'2016-06-17 18:00:00', NULL, NULL, N'Saint-Étienne')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (21, 15, 13, N'2016-06-17 21:00:00', NULL, NULL, N'Nice')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (22, 19, 17, N'2016-06-18 15:00:00', NULL, NULL, N'Bordeaux')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (23, 24, 22, N'2016-06-18 18:00:00', NULL, NULL, N'Marseille')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (24, 23, 21, N'2016-06-18 21:00:00', NULL, NULL, N'Parijs')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (25, 2, 3, N'2016-06-19 21:00:00', NULL, NULL, N'Lyon')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (26, 4, 1, N'2016-06-19 21:00:00', NULL, NULL, N'Villeneuve-d'Ascq')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (27, 6, 7, N'2016-06-20 21:00:00', NULL, NULL, N'Toulouse')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (28, 8, 5, N'2016-06-20 21:00:00', NULL, NULL, N'Saint-Étienne')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (29, 12, 9, N'2016-06-21 18:00:00', NULL, NULL, N'Marseille')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (30, 10, 11, N'2016-06-21 18:00:00', NULL, NULL, N'Parijs')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (31, 16, 13, N'2016-06-21 21:00:00', NULL, NULL, N'Lens')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (32, 14, 15, N'2016-06-21 21:00:00', NULL, NULL, N'Bordeaux')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (33, 24, 21, N'2016-06-22 18:00:00', NULL, NULL, N'Saint-Denis')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (34, 22, 23, N'2016-06-22 18:00:00', NULL, NULL, N'Lyon')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (35, 20, 17, N'2016-06-22 21:00:00', NULL, NULL, N'Villeneuve-d'Ascq')
INSERT INTO [dbo].[Match] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location]) VALUES (36, 18, 19, N'2016-06-22 21:00:00', NULL, NULL, N'Nice')
SET IDENTITY_INSERT [dbo].[Match] OFF
SET IDENTITY_INSERT [dbo].[MatchFinals] ON
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (37, NULL, NULL, N'2016-06-25 15:00:00', NULL, NULL, N'Saint-Étienne', N'2e Poule A', N'2e Poule C', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (38, NULL, NULL, N'2016-06-25 18:00:00', NULL, NULL, N'Parijs', N'Winnaar Poule B', N'3e Poule A/C/D', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (39, NULL, NULL, N'2016-06-25 21:00:00', NULL, NULL, N'Lens', N'Winnaar Poule D', N'3e Poule B/E/F', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (40, NULL, NULL, N'2016-06-26 15:00:00', NULL, NULL, N'Lyon', N'Winnaar Poule A', N'3e Poule C/D/E', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (41, NULL, NULL, N'2016-06-26 18:00:00', NULL, NULL, N'Villeneuve-d'Ascq', N'Winnaar Poule C', N'3e Poule A/B/F', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (42, NULL, NULL, N'2016-06-26 21:00:00', NULL, NULL, N'Toulouse', N'Winnaar Poule F', N'2e Poule E', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (43, NULL, NULL, N'2016-06-27 18:00:00', NULL, NULL, N'Saint-Denis', N'Winnaar Poule E', N'2e Poule D', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (44, NULL, NULL, N'2016-06-27 21:00:00', NULL, NULL, N'Nice', N'2e Poule B', N'2e Poule F', 1)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (45, NULL, NULL, N'2016-06-30 21:00:00', NULL, NULL, N'Marsaille', N'Winnaar 37', N'Winnaar 39', 2)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (46, NULL, NULL, N'2016-07-01 21:00:00', NULL, NULL, N'Villeneuve-d'Ascq', N'Winnaar 38', N'Winnaar 42', 2)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (47, NULL, NULL, N'2016-07-02 21:00:00', NULL, NULL, N'Bordeaux', N'Winnaar 41', N'Winnaar 43', 2)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (48, NULL, NULL, N'2016-07-03 21:00:00', NULL, NULL, N'Saint Denis', N'Winnaar 40', N'Winnaar 44', 2)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (49, NULL, NULL, N'2016-07-06 21:00:00', NULL, NULL, N'Lyon', N'Winnaar 45', N'Winnaar 46', 3)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (50, NULL, NULL, N'2016-07-07 21:00:00', NULL, NULL, N'Marseille', N'Winnaar 47', N'Winnaar 48', 3)
INSERT INTO [dbo].[MatchFinals] ([ID], [Country1], [Country2], [Start], [GoalsCountry1], [GoalsCountry2], [Location], [Country1Text], [Country2Text], [LevelNumber]) VALUES (51, NULL, NULL, N'2016-07-10 21:00:00', NULL, NULL, N'Parijs', N'Winnaar 49', N'Winnaar 50', 4)
SET IDENTITY_INSERT [dbo].[MatchFinals] OFF */

/*SET IDENTITY_INSERT [dbo].[Schedule] ON
INSERT INTO [dbo].[Schedule] ([ID], [MatchNumber], [Start], [Location], [Country1], [Country2], [GoalsCountry1], [GoalsCountry2], [LevelNumber]) VALUES ()
SET IDENTITY_INSERT [dbo].[Schedule] OFF*/

/*
Speelschema Groep A
#	DATUM	WAAR	THUIS	UIT	UITSLAG 
1	10 juni 2016 om 21:00	Saint-Denis	Frankrijk	Roemenië
2	11 juni 2016 om 15:00	Lens	Albanië	Zwitserland 
3	15 juni 2016 om 18:00	Paris	Roemenië	Zwitserland 
4	15 juni 2016 om 21:00	Marseille	Frankrijk	Albanië 
5	19 juni 2016 om 21:00	Lyon	Roemenië	Albanië 
6	19 juni 2016 om 21:00	Villeneuve-d'Ascq	Zwitserland	Frankrijk 
Speelschema Groep B
# 	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG 
7	11-06-16 om 18:00	Bordeaux	Wales	Slowakije 
8	11-06-16 om 21:00	Marseille	Engeland	Rusland 
9	15-06-16 om 15:00	Villeneuve-d'Ascq	Rusland	Slowakije 
10	16-06-16 om 15:00	Lens	Engeland	Wales 
11	20-06-16 om 21:00	Toulouse	Slowakije	Engeland 
12	20-06-16 om 21:00	Saint-Étienne	Rusland	Wales 
Speelschema Groep C
# 	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG 
13	12-06-16 om 18:00	Nice	Polen	Noord-Ierland 
14	12-06-16 om 21:00	Villeneuve-d'Ascq	Duitsland	Oekraïne 
15	16-06-16 om 18:00	Lyon	Oekraïne	Noord-Ierland 
16	16-06-16 om 21:00	Saint-Denis	Duitsland	Polen 
17	21-06-16 om 18:00	Marseille	Oekraïne	Polen 
18	21-06-16 om 18:00	Paris	Noord-Ierland	Duitsland 
Speelschema Groep D
#	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG 
19	12-06-16 om 15:00	Paris	Turkije	Kroatië 
20	13-06-16 om 15:00	Toulouse	Spanje	Tsjechië 
21	17-06-16 om 18:00	Saint-Étienne	Tsjechië	Kroatië 
22	17-06-16 om 21:00	Nice	Spanje	Turkije 
23	21-06-16 om 21:00	Lens	Tsjechië	Turkije 
24	21-06-16 om 21:00	Bordeaux	Kroatië	Spanje 
Speelschema Groep E
 #	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG 
25	13-06-16 om 18:00	Saint-Denis	Ierland	Zweden 
26	13-06-16 om 21:00	Lyon	België	Italië 
27	17-06-16 om 15:00	Toulouse	Italië	Zweden 
28	18-06-16 om 15:00	Bordeaux	België	Ierland 
29	22-06-16 om 21:00	Villeneuve-d'Ascq	Italië	Ierland 
30	22-06-16 om 21:00	Nice	Zweden	België 
Speelschema Groep F
 #	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG 
31	14-06-16 om 18:00	Bordeaux	Oostenrijk	Hongarije 
32	14-06-16 om 21:00	Saint-Étienne	Portugal	IJsland 
33	18-06-16 om 18:00	Marseille	IJsland	Hongarije 
34	18-06-16 om 21:00	Paris	Portugal	Oostenrijk 
35	22-06-16 om 18:00	Saint-Denis	IJsland	Oostenrijk 
36	22-06-16 om 18:00	Lyon	Hongarije	Portugal 
Speelschema Achtste Finales EK 2016
 #	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG 
37	25-06-16 om 15:00	Saint-Étienne	2e Poule A	2e Poule C 
38	25-06-16 om 18:00	Paris	Winnaar Poule B	3e Poule A/C/D 
39	25-06-16 om 21:00	Lens	Winnaar Poule D	3e Poule B/E/F 
40	26-06-16 om 15:00	Lyon	Winnaar Poule A	3e Poule C/D/E 
41	26-06-16 om 18:00	Villeneuve-d'Ascq	Winnaar Poule C	3e Poule A/B/F 
42	26-06-16 om 21:00	Toulouse	Winnaar Poule F	2e Poule E 
43	27-06-16 om 18:00	Saint-Denis	Winnaar Poule E	2e Poule D 
44	27-06-16 om 21:00	Nice	2e Poule B	2e Poule F 
Speelschema Kwart Finales EK 2016
 #	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG 
45	30-06-16 om 21:00	Marseille	Winnaar 37	Winnaar 39 
46	01-07-16 om 21:00	Villeneuve-d'Ascq	Winnaar 38	Winnaar 42 
47	02-07-16 om 21:00	Bordeaux	Winnaar 41	Winnaar 43 
48	03-07-16 om 21:00	Saint-Denis	Winnaar 40	Winnaar 44 
Halve Finales EK 2016
 #	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG
49	06-07-16 om 21:00	Lyon	Winnaar 45	Winnaar 46 
50	07-07-16 om 21:00	Marseille	Winnaar 47	Winnaar 48 
Finale EK 2016
#	DATUM	SPEELSTAD	THUIS	UIT	UITSLAG
 	10 juli 2016 om 21:00 uur	Parijs, Stade de France	Winnaar 49	Winnaar 50 
*/

        public static void SetLoggedIn(this CoreContext context, ApplicationUser user, bool login) {
            var selectedDbUser = context.Users.SingleOrDefault(u => u.OwnerId == user.Id);
            if (selectedDbUser == null) {
                selectedDbUser = context.Users.Add(new User { FirstName = "New", LastName = "User", OwnerId = user.Id }).Entity;
            }

            selectedDbUser.IsLoggedIn = login;
            if (login) {
                selectedDbUser.LastLoginDate = DateTime.UtcNow;
            }
            context.SaveChanges();

        }
    }
}