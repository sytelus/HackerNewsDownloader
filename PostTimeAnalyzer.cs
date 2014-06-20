using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils;
using Newtonsoft.Json.Linq;

namespace HackerNewsDownloader
{
    public static class PostTimeAnalyzer
    {
        private class StoryRating
        {
            public DateTime CreatedOn { get; set; }
            public int Points { get; set; }
            public int Comments { get; set; }
        }

        public static void AnalyzePostTimes(string storiesFilePath, string postTimeStatsFilePath)
        {
            //Get posts and convert to date
            var posts = JsonNetUtils.DeserializeSequenceFromJson<JObject>(storiesFilePath)
                .SelectMany(responseJson => responseJson["hits"])
                .Select(hitJson => new StoryRating
                {
                    CreatedOn = Utils.FromUnixTime(hitJson["created_at_i"].Value<long>()).ToLocalTime(),
                    Points = hitJson["points"].Value<int>(),
                    Comments = hitJson["num_comments"].Value<int>()
                }).ToList();

            var postsPerMinutes = posts
                .GroupBy(post => post.CreatedOn.ToString("ddd hh:mm tt"))
                .OrderBy(g => g.Key);

            using (var outputFile = File.CreateText(postTimeStatsFilePath))
            {
                outputFile.WriteLine("Time\tPoints\tComments");
                foreach (var postsPerMinute in postsPerMinutes)
                {
                    outputFile.WriteLine("{0}\t{1}\t{2}".FormatEx(postsPerMinute.Key,
                        postsPerMinute.Select(p => p.Points.ToStringInvariant()).ToDelimitedString(","),
                        postsPerMinute.Select(p => p.Comments.ToStringInvariant()).ToDelimitedString(",")
                        ));
                }
            }
        }
    }
}
