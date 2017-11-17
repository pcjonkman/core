using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.Models.Identity;
using Core.Models.Pool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Core.Models.Pool.PoolMessage;

namespace Core.Controllers
{
    [Route("api/[controller]")]
    public class PoolController : Controller
    {
        private readonly CoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Query query;

        public PoolController(CoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            query = new Query(_context);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await GetUser();

            var currentUser = await GetCurrentUserAsync();
            PoolPlayer poolPlayer = null;
            if (currentUser != null) {
              var selectedDbUser = _context.Users.SingleOrDefault(u => u.OwnerId == currentUser.Id);
              if (selectedDbUser != null && selectedDbUser.IsLoggedIn && checkWithCurrentUser(selectedDbUser)) {
                poolPlayer = _context.PoolPlayer.SingleOrDefault(u => u.UserId == selectedDbUser.Id);
              }
            }

            return Json(new { user = user, poolPlayer = poolPlayer, messages = query.poolMessage().ToList() });
        }

        [HttpGet("PoolPlayers")]
        public async Task<IActionResult> PoolPlayers()
        {
          var user = await GetUser();

          var currentUser = await GetCurrentUserAsync();
          PoolPlayer poolPlayer = null;
          if (currentUser != null) {
            var selectedDbUser = _context.Users.SingleOrDefault(u => u.OwnerId == currentUser.Id);
            if (selectedDbUser != null && selectedDbUser.IsLoggedIn && checkWithCurrentUser(selectedDbUser)) {
              poolPlayer = _context.PoolPlayer.SingleOrDefault(u => u.UserId == selectedDbUser.Id);
            }
          }

          return Json(new { user = user, poolPlayer = poolPlayer, poolPlayers = query.poolPlayer().ToList() });
        }

        [HttpGet("Country")]
        public async Task<IActionResult> Poule(string name)
        {
          var user = await GetUser();

          return Json(new { user = user, country = query.country().ToList() });
        }

        [HttpGet("Ranking")]
        public async Task<IActionResult> PoolRanking()
        {
          var user = await GetUser();

          return Json(new { user = user, ranking = query.poolRanking().ToList() });
        }

        [HttpGet("Schedule")]
        public async Task<IActionResult> PoolSchedule()
        {
          var user = await GetUser();

          return Json(new { user = user, schedule = query.poolSchedule().ToList() });
        }

        [HttpGet("Prediction")]
        public async Task<IActionResult> PoolPrediction()
        {
          int id = 0;
          var user = await GetUser();
          var currentUser = await GetCurrentUserAsync();
          if (currentUser != null) {
            var selectedDbUser = _context.Users.SingleOrDefault(u => u.OwnerId == currentUser.Id);
            if (selectedDbUser != null && selectedDbUser.IsLoggedIn && checkWithCurrentUser(selectedDbUser)) {
              var poolPlayer = _context.PoolPlayer.SingleOrDefault(u => u.UserId == selectedDbUser.Id);
              if (poolPlayer != null) {
                id = poolPlayer.Id;
              }
            }
          }

          var mp = query.poolMatchPrediction(id).ToList();
          var fp = query.poolFinalsPrediction(id).ToList();

          return Json(new { user = user, match = mp, finals = fp });
        }

        [HttpGet("Prediction/{id}")]
        public async Task<IActionResult> PoolPredictionById(int? id)
        {
          var user = await GetUser();
          if (!id.HasValue || id == 0) {
            return Json(new { user = user, match = new List<dynamic>(), finals = new List<dynamic>() });
          }
          var mp = query.poolMatchPrediction(id.Value).ToList();
          var fp = query.poolFinalsPrediction(id.Value).ToList();

          if (mp.Count() > 0 && mp[0].PredictedGoals1 == -1 && mp[0].PredictedGoals2 == -1 && mp[0].Subscore == -1) {
            return Json(new { user = user, match = new List<dynamic>(), finals = new List<dynamic>() });
          }

          return Json(new { user = user, match = mp, finals = fp });
        }

        [HttpPost("Prediction")]
        public async Task<IActionResult> PostPrediction([FromBody] PoolPredictions pool)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Json(pool);
            }

            List<dynamic> mpList = new List<dynamic>();
            List<dynamic> fpList = new List<dynamic>();
            
            if (ModelState.IsValid)
            {
              var selectedDbUser = _context.Users.SingleOrDefault(u => u.OwnerId == pool.user.user.Id);
              if (selectedDbUser != null && selectedDbUser.IsLoggedIn && checkWithCurrentUser(selectedDbUser)) {
                var poolPlayer = _context.PoolPlayer.SingleOrDefault(u => u.UserId == selectedDbUser.Id);
                if (poolPlayer == null) {
                    poolPlayer = _context.PoolPlayer.Add(new PoolPlayer { Id = _context.PoolPlayer.Count() + 1, Name = selectedDbUser.FirstName + ' ' + selectedDbUser.LastName, UserId = selectedDbUser.Id }).Entity;
                }

                foreach(var match in pool.match) {

                  if (match.PredictedGoals1 == -1 || match.PredictedGoals2 == -1) {
                    continue;
                  }

                  var matchPrediction = _context.MatchPrediction.SingleOrDefault(mp => mp.MatchId == match.MatchId && mp.PoolPlayerId == poolPlayer.Id);
                  if (matchPrediction == null) {
                    matchPrediction = _context.MatchPrediction.Add(new MatchPrediction { PoolPlayerId = poolPlayer.Id, MatchId = match.MatchId }).Entity;
                  }
                  matchPrediction.GoalsCountry1 = match.PredictedGoals1;
                  matchPrediction.GoalsCountry2 = match.PredictedGoals2;
                }

                var finalLevels = (from p in _context.Finals
                                    select p.LevelNumber);
                foreach(var finalLevel in finalLevels) {
                  List<FinalsPrediction> finalPrediction = new List<FinalsPrediction>();
                  int finalId = _context.Finals.Where(fn => fn.LevelNumber == finalLevel).First().Id;
                  var curr = from fp in _context.FinalsPrediction
                              where fp.FinalsId == finalId && fp.PoolPlayerId == poolPlayer.Id
                              select fp;
                  foreach (var finals in pool.finals.Where(f => f.Level == finalLevel)) {
                    FinalsPrediction f = new FinalsPrediction() { FinalsId = finalId, PoolPlayerId = poolPlayer.Id };
                    f.CountryId = finals.CountryId;
                    finalPrediction.Add(f);
                    if (curr.Where(c => c.CountryId == f.CountryId).Count() == 0)
                    {
                      _context.FinalsPrediction.Add(f);
                    }
                  }
                  var removequery = from c in curr
                                      // where (finalPrediction.Where(fp => fp.CountryId == c.CountryId).SingleOrDefault() != null)
                                      where !(finalPrediction.Any(fpCurr => fpCurr.CountryId == c.CountryId))
                                      select c;
                  _context.FinalsPrediction.RemoveRange(removequery);

                }
                _context.SaveChanges();

                mpList = query.poolMatchPrediction(poolPlayer.Id).ToList();
                fpList = query.poolFinalsPrediction(poolPlayer.Id).ToList();

              }
            }

            return Json(new { user = pool.user, match = mpList, finals = fpList });
        }

        [HttpPost("PoolPlayer")]
        public async Task<IActionResult> Post([FromBody] PoolPlayer poolPlayer)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
              return Json(new Array[] {});
            }

            if (ModelState.IsValid)
            {
              if (poolPlayer != null) {
                var pp = _context.PoolPlayer.SingleOrDefault(u => u.Id == poolPlayer.Id);
                if (pp != null) {
                  pp.Name = poolPlayer.Name;
                  _context.SaveChanges();
                }
              }
            }
 
            var user = await GetUser();

            return Json(new { user = user, poolPlayer = poolPlayer, messages = query.poolMessage().ToList() });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PoolMessage message)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return Json(new Array[] {});
            }

            if (ModelState.IsValid)
            {
                if (message.userId != null) {
                    var selectedDbUser = _context.Users.SingleOrDefault(u => u.OwnerId == message.userId);
                    if (selectedDbUser != null && selectedDbUser.IsLoggedIn && checkWithCurrentUser(selectedDbUser)) {
                        var poolPlayer = _context.PoolPlayer.SingleOrDefault(u => u.UserId == selectedDbUser.Id);
                        if (poolPlayer == null) {
                            poolPlayer = _context.PoolPlayer.Add(new PoolPlayer { Id = _context.PoolPlayer.Count() + 1, Name = selectedDbUser.FirstName + ' ' + selectedDbUser.LastName, UserId = selectedDbUser.Id }).Entity;
                        }
                        var selectedMessage = _context.PoolMessage.SingleOrDefault(u => u.Id == message.id);
                        if (selectedMessage == null) {
                          selectedMessage = _context.PoolMessage.Add(new Models.Pool.PoolMessage { PoolPlayerId = poolPlayer.Id, PlacedDate = DateTime.UtcNow, Message = message.message }).Entity;
                        }
                        selectedMessage.Status = message.status;
                        _context.SaveChanges();
                    }
                }
            }
 
          var user = await GetUser();

          return Json(new { user = user, messages = query.poolMessage().ToList() });
        }

        public class PoolMessage {
          public int id { get; set; }
          public string message { get; set; }
          public string userId { get; set; }
          public MessageStatus status { get; set; }
        }

        public class PoolPredictions {
          public PoolUser user { get; set; }
          public List<PoolMatchPredicion> match { get; set; }
          public List<PoolFinalsPrediction> finals { get; set; }
        }

        public class PoolFinalsPrediction {
          public string Country { get; set; }
          public string CountryCode { get; set; }
          public int CountryId { get; set; }
          public int Level { get; set; }
          public int SubScore { get; set; }
        }

        public class PoolMatchPredicion {
          public int MatchId { get; set; }
          public string Group { get; set; }
          public string Country1 { get; set; }
          public string Country2 { get; set; }
          public int Country1Id { get; set; }
          public int Country2Id { get; set; }
          public string Country1Code { get; set; }
          public string Country2Code { get; set; }
          public DateTime StartDate { get; set; }
          public string Location { get; set; }
          public int Goals1 { get; set; }
          public int Goals2 { get; set; }
          public int PredictedGoals1 { get; set; }
          public int PredictedGoals2 { get; set; }
          public int SubScore { get; set; }
        }

        public class PoolUser {
          public string firstName { get; set; }
          public string lastName { get; set; }
          public bool isLoggedIn { get; set; }
          public string lastLoginDate { get; set; }
          public ApplicationUser user { get; set; }
          public List<string> roles { get; set; }
        }

        private async Task<object> GetUser() {
            var currentUser = await GetCurrentUserAsync();
            var dbUsers = await _context.Users.ToArrayAsync();
            var users = (from user in _userManager.Users
                        select user).ToList();

            return dbUsers.Select(dbu => new
            {
                firstName = dbu.FirstName,
                lastName = dbu.LastName,
                isLoggedIn = dbu.OwnerId != null ? users.FirstOrDefault(u => u.Id == dbu.OwnerId) == currentUser ? true : false : false,
                lastLoginDate = dbu.LastLoginDate,
                user = dbu.OwnerId != null ? users.FirstOrDefault(u => u.Id == dbu.OwnerId) : null,
                roles = dbu.OwnerId != null ? _userManager.GetRolesAsync(users.FirstOrDefault(u => u.Id == dbu.OwnerId)).GetAwaiter().GetResult() : null
            }).Where(u => u.isLoggedIn).SingleOrDefault();
        }

        private bool checkWithCurrentUser(Models.User user) {
            // var lastLoginDbUser = _context.Users.OrderByDescending(u => u.LastLoginDate).FirstOrDefault();
            var currentUserId = _userManager.GetUserId(User);
            var currentDbUser = _context.Users.FirstOrDefault(u => u.OwnerId == currentUserId);

            return user == currentDbUser;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }

    public class Query {
      private CoreContext _context;

      public Query(CoreContext context) {
        _context = context;
      }

      public IEnumerable<dynamic> poolRanking() {
        var query = (from pl in _context.PoolPlayer.Include(pp => pp.MatchPredictions).Include(pp => pp.FinalsPredictions)
                      where pl.MatchPredictions.Count() > 0 & pl.FinalsPredictions.Count() > 0
                      select new {
                        Id = pl.Id,
                        Name = pl.Name,
                        Score = pl.MatchPredictions.Sum(s => s.SubScore) + pl.FinalsPredictions.Sum(s => s.SubScore) + pl.SubScore
                      });
        var rankquery = (from rp in query
                          select new {
                            Rank = query.Count(p2 => p2.Score > rp.Score) + 1,
                            Id = rp.Id,
                            Name = rp.Name,
                            Score = rp.Score
                          });
                          // select new { Rank = query.Count(p2 => p2.Score > rp.Score.GetValueOrDefault(0)) + 1, Name = rp.Name, Score = rp.Score.GetValueOrDefault(0) });
        return rankquery;
      }

      public IEnumerable<dynamic> poolMessage() {
        return (from pm in _context.PoolMessage
                join poolp in _context.PoolPlayer on pm.PoolPlayerId equals poolp.Id
                orderby pm.PlacedDate descending
                select new {
                  Id = pm.Id,
                  Name = poolp.Name,
                  PlacedDate = pm.PlacedDate,
                  Message = pm.Message,
                  Status = pm.Status
                });
      }

      public IEnumerable<Country> country() {
        return (from pl in _context.Country
                orderby pl.Id
                select pl);
      }

      public IEnumerable<PoolPlayer> poolPlayer() {
        return (from pl in _context.PoolPlayer
                // where pl.Name.Contains("Admin") == false
                orderby pl.Name
                select pl);
      }

      public IEnumerable<dynamic> poolSchedule() {
        var matches = (from m in _context.Match.OrderBy(m => m.StartDate)
                    join c1 in _context.Country on m.Country1Id equals c1.Id
                    join c2 in _context.Country on m.Country2Id equals c2.Id
                    select new {
                      MatchId = m.Id,
                      Group = c1.Group,
                      Country1 = c1.Name,
                      Country2 = c2.Name,
                      Country1Code = c1.Code,
                      Country2Code = c2.Code,
                      Country1Id = c1.Id,
                      Country2Id = c2.Id,
                      StartDate= m.StartDate, 
                      Location = m.Location,
                      Goals1 = m.GoalsCountry1,
                      Goals2 = m.GoalsCountry2
                    });

        var matchfinals = (from m in _context.MatchFinals.OrderBy(m => m.StartDate)
                        join _c1 in _context.Country on m.Country1Id equals _c1.Id into country1
                        from c1 in country1.DefaultIfEmpty()
                        join _c2 in _context.Country on m.Country2Id equals _c2.Id into country2
                        from c2 in country2.DefaultIfEmpty()
                        join f in _context.Finals on m.LevelNumber equals f.LevelNumber
                        select new {
                          MatchId = m.Id,
                          Group = f.LevelName,
                          Country1 = (c1 == null ? m.Country1Text : c1.Name),
                          Country2 = (c2 == null ? m.Country2Text : c2.Name),
                          Country1Code = c1.Code,
                          Country2Code = c2.Code,
                          Country1Id = (c1 == null ? 0 : c1.Id),
                          Country2Id = (c2 == null ? 0 : c2.Id),
                          StartDate = m.StartDate,
                          Location = m.Location,
                          Goals1 = m.GoalsCountry1,
                          Goals2 = m.GoalsCountry2
                        });

        return matches.Concat(matchfinals);
        // lvSchedule.DataSource = matches.Concat(matchfinals).ToList();
      }

      public IEnumerable<dynamic> poolMatchPrediction(int playerId) {
        var matchPrediction = (from m in _context.Match
                              from mpred in _context.MatchPrediction.Where(mp => mp.MatchId == m.Id && mp.PoolPlayerId == playerId).DefaultIfEmpty()
                              join c1 in _context.Country on m.Country1Id equals c1.Id
                              join c2 in _context.Country on m.Country2Id equals c2.Id
                              select new {
                                MatchId = m.Id,
                                Group = c1.Group,
                                Country1 = c1.Name,
                                Country2 = c2.Name,
                                Country1Id = c1.Id,
                                Country2Id = c2.Id,
                                Country1Code = c1.Code,
                                Country2Code = c2.Code,
                                StartDate = m.StartDate,
                                Location = m.Location,
                                Goals1 = m.GoalsCountry1,
                                Goals2 = m.GoalsCountry2,
                                PredictedGoals1 = (mpred == null ? -1 : mpred.GoalsCountry1),
                                PredictedGoals2 = (mpred == null ? -1 : mpred.GoalsCountry2),
                                SubScore = (mpred == null ? -1 : mpred.SubScore)
                              });

        return matchPrediction;
      }

      public IEnumerable<dynamic> poolFinalsPrediction(int playerId) {
        var finalsPrediction = (from fp in _context.FinalsPrediction
                              join f in _context.Finals on fp.FinalsId equals f.Id
                              join c in _context.Country on fp.CountryId equals c.Id
                              where fp.PoolPlayerId == playerId
                              select new {
                                Country = c.Name,
                                CountryCode = c.Code,
                                CountryId = fp.CountryId,
                                Level = f.LevelNumber,
                                SubScore = fp.SubScore
                              });

        return finalsPrediction;
      }
    }
}