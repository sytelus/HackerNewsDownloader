using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsDownloader
{
    public class StoryUrlStats
    {
        public string Url { get; private set; }
        public int PointsSum { get; private set; }
        public int CommentsSum { get; private set; }
        public int StoryCount { get; private set; }
        public long CreatedUnixTimeMin { get; private set; }
        public long CreatedUnixTimeMax { get; private set; }
        public Task AnalyzeWpTask { get; private set; }

        private StoryUrlStats()
        {
        }
        public static StoryUrlStats Create(string url)
        {
            var stats = new StoryUrlStats();
            stats.Url = url;
            stats.CreatedUnixTimeMin = long.MaxValue;
            stats.CreatedUnixTimeMax = long.MinValue;

            stats.AnalyzeWpTask = Task.Run(() => WpAnalyzer.AnalyzeWordPressUrl(stats));

            return stats;
        }
        public void AddStory(JToken storyJson)
        {
            this.PointsSum += storyJson["points"].Value<int>();
            this.CommentsSum += storyJson["num_comments"].Value<int>();
            this.StoryCount++;

            var createdUnixTime = storyJson["created_at_i"].Value<long>();
            if (createdUnixTime < this.CreatedUnixTimeMin)
                this.CreatedUnixTimeMin = createdUnixTime;
            if (createdUnixTime > this.CreatedUnixTimeMax)
                this.CreatedUnixTimeMax = createdUnixTime;
        }

        public string FetchErrorMessage { get; set; }
        public string FecthErrorStatus { get; set; }
        public string ThemeNames { get; set; }
        public string PluginNames { get; set; }
    }
}
