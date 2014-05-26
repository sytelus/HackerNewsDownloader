using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtils;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace HackerNewsDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetHnItems();
        }

        private static void GetHnItems()
        {
            const string baseUrl = @"https://hn.algolia.com/api/v1/search_by_date?tags=story&hitsPerPage={0}&numericFilters=created_at_i<{1}";
            var restClient = new RestClient();
            var offset = DateTime.UtcNow.ToUnixTime();
            var limit = 100;

            var hitCount = 0;
            using(var saveFile = File.CreateText(@"c:\temp\test.txt"))
            {
                do
                {
                    var request = new RestRequest(baseUrl.FormatEx(limit, offset), Method.GET);

                    var response = restClient.Execute(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseJson = JObject.Parse(response.Content);
                        saveFile.WriteLine(responseJson.ToString().Replace('\n', ' ').Replace('\r', ' '));

                        var hits = responseJson["hits"];
                        hitCount = hits.Count();

                        if (hitCount > 0)
                        {
                            var minDate = hits.Min(h => h["created_at"].Value<System.DateTime>());
                            offset = minDate.ToUnixTime();
                            Thread.Sleep(4000);
                        }
                    }
                    else
                        throw new Exception("Recieved Response {0}-{1}, Body {2} for request {3}, Error: {4}".FormatEx(response.StatusCode, response.StatusDescription, response.Content, response.ResponseUri, response.ErrorMessage));
                } while (hitCount > 0);
            }

            MessageBox.Show("done!");
        }


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

                stats.AnalyzeWpTask = Task.Run(() => AnalyzeWordPressUrl(stats));

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

        static Regex endsWithExtensionRegEx = new Regex(@".*?\..{2,4}$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        static Regex wpThemesRegEx = new Regex(@"wp-content\/themes\/(.*?)\/", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        static Regex wpPluginsRegEx = new Regex(@"wp-content\/plugins\/(.*?)\/", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static void AnalyzeWordPressUrl(StoryUrlStats urlStats)
        {
            if (endsWithExtensionRegEx.IsMatch(urlStats.Url))
                return; //URL has extension not WordPress blog

            using (var client = new WebClient())
            {
                try
                {
                    var pageContent = client.DownloadString(urlStats.Url);

                    var themeNames = new HashSet<string>();
                    foreach (Match match in wpThemesRegEx.Matches(pageContent))
                        themeNames.Add(match.Groups[1].Value);

                    var pluginNames = new HashSet<string>();
                    foreach (Match match in wpPluginsRegEx.Matches(pageContent))
                        pluginNames.Add(match.Groups[1].Value);

                    urlStats.ThemeNames = themeNames.ToDelimitedString("/");
                    urlStats.PluginNames = pluginNames.ToDelimitedString("/");
                }
                catch (WebException wex)
                {
                    urlStats.FecthErrorStatus = wex.Status.ToString();
                    urlStats.FetchErrorMessage = wex.Message;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var storyUrlsStats = new Dictionary<string, StoryUrlStats>();
            int storyCount = 0, blankUrls = 0, badUrls = 0;
            var minUnixDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(365*3)).ToUnixTime();
            foreach (var hitsResponseText in File.ReadLines(@"c:\temp\test.txt"))
            {
                try
                {
                    var hitsJson = JObject.Parse(hitsResponseText);
                    foreach (var hitJson in hitsJson["hits"])
                    {
                        storyCount++;

                        if (hitJson["created_at_i"].Value<long>() < minUnixDate)
                            goto endFileReading;

                        var urlString = hitJson["url"].ToString();
                        if (string.IsNullOrWhiteSpace(urlString))
                        {
                            blankUrls++;
                            continue;
                        }
                        try
                        {
                            var uri = new Uri(urlString);
                            var host = uri.Host;
                            var stats = storyUrlsStats.AddOrGetValue(host, () => StoryUrlStats.Create(urlString));
                            stats.AddStory(hitJson);
                        }
                        catch (UriFormatException ex)
                        {
                            badUrls++;
                        }
                    }
                }
                catch{}
            }
            endFileReading:

            Task.WaitAll(storyUrlsStats.Values.Select(s => s.AnalyzeWpTask).ToArray());

            using (var urlStatsFile = File.CreateText(@"c:\temp\WPUrlStats.tsv"))
            {
                foreach (var urlStatsKvp in storyUrlsStats)
                {
                    if (string.IsNullOrWhiteSpace(urlStatsKvp.Value.ThemeNames) && string.IsNullOrWhiteSpace(urlStatsKvp.Value.PluginNames))
                        continue;

                    var values = new string[] {urlStatsKvp.Key, urlStatsKvp.Value.Url, urlStatsKvp.Value.PointsSum.ToStringInvariant(), 
                        urlStatsKvp.Value.ThemeNames, urlStatsKvp.Value.PluginNames,
                        urlStatsKvp.Value.StoryCount.ToStringInvariant(), urlStatsKvp.Value.CommentsSum.ToStringInvariant(), 
                        urlStatsKvp.Value.CreatedUnixTimeMax.ToString(), urlStatsKvp.Value.CreatedUnixTimeMin.ToString()};

                    urlStatsFile.WriteLine(values.ToDelimitedString("\t"));
                }
            }

            MessageBox.Show("Total: {0}, Blank: {1}, Bad: {2}".FormatEx(storyCount, blankUrls, badUrls));
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var testStats = StoryUrlStats.Create("http://www.ShitalShah.com");
            AnalyzeWordPressUrl(testStats);
            MessageBox.Show("Themes: {0}, Plugins: {1}".FormatEx(testStats.ThemeNames, testStats.PluginNames));
        }

    }
}
