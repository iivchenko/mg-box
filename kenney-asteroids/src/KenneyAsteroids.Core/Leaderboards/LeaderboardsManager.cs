using KenneyAsteroids.Engine.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KenneyAsteroids.Core.Leaderboards
{
    public sealed class LeaderboardsManager
    {
        private IRepository<Collection<LeaderboardItem>> _repository;

        public LeaderboardsManager(IRepository<Collection<LeaderboardItem>> repository)
        {
            _repository = repository;
        }

        public bool CanAddLeader(int score)
        {
            var items = _repository.Read();

            return score > 0 && items.Count() < 10 || items.Any(x => x.Score < score);
        }

        public void AddLeader(string name, int score, TimeSpan playedTime)
        {
            if(!CanAddLeader(score))
            {
                throw new Exception(); // make more clean
            }

            var item = new LeaderboardItem
            {
                Name = name,
                Score = score,
                PlayedTime = playedTime,
                ScoreDate = DateTime.Now
            };

            var items =
                _repository
                    .Read()
                    .Concat(new[] { item })
                    .OrderByDescending(x => x.Score)
                    .Take(10)
                    .ToList();

            _repository.Update(new Collection<LeaderboardItem>(items));
        }

        public IEnumerable<LeaderboardItem> GetLeaders()
        {
            return _repository.Read();
        }
    }
}
