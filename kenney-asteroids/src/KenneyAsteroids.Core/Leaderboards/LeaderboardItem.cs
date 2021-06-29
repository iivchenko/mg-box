using System;

namespace KenneyAsteroids.Core.Leaderboards
{
    public sealed class LeaderboardItem
    {
        public LeaderboardItem() { }

        public string Name { get; set; }
        public int Score { get; set; }
        public TimeSpan PlayedTime { get; set; }
        public DateTime ScoreDate { get; set; }
    }
}
