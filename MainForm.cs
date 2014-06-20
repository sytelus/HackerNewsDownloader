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
using Newtonsoft.Json;
using System.Diagnostics;

namespace HackerNewsDownloader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Stopwatch sw = new Stopwatch();
        private void buttonFetch_Click(object sender, EventArgs e)
        {
            SaveHnItems("story", textBoxStoriesFilePath.Text, this.UpdateProgress);
        }

        private void SaveHnItems(string itemType, string filePath, Action<JObject, long> progress)
        {
            sw.Start();

            var stories = GetHnItems(itemType);

            using (var saveFile = File.CreateText(filePath))
                JsonNetUtils.SerializeSequenceToJson(stories, saveFile, progress);

            sw.Stop();
        }

        private void UpdateProgress(JObject response, long requestCount)
        {
            labelrequestCount.Text = requestCount.ToString();
            labelTimeElapsed.Text = sw.Elapsed.TotalSeconds.ToStringInvariant();
        }

        private static IEnumerable<JObject> GetHnItems(string itemType)
        {
            const string baseUrl = @"https://hn.algolia.com/api/v1/search_by_date?tags={2}&hitsPerPage={0}&numericFilters=created_at_i<{1}";
            var restClient = new RestClient();
            var offset = DateTime.UtcNow.ToUnixTime();
            var limit = 1000;

            var hitCount = 0;
            do
            {
                var request = new RestRequest(baseUrl.FormatEx(limit, offset, itemType), Method.GET);

                var response = restClient.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = JObject.Parse(response.Content);
                    var hits = responseJson["hits"];
                    hitCount = hits.Count();

                    yield return responseJson;

                    if (hitCount > 0)
                    {
                        offset = hits.Min(h => h["created_at_i"].Value<long>());
                        Thread.Sleep(4000);
                    }
                }
                else
                    throw new Exception("Recieved Response {0}-{1}, Body {2} for request {3}, Error: {4}".FormatEx(response.StatusCode, response.StatusDescription, response.Content, response.ResponseUri, response.ErrorMessage));
            } while (hitCount > 0);
        }

        private void buttonAnalyzeUniqueUrls_Click(object sender, EventArgs e)
        {
            var storyUrlsStats = new Dictionary<string, StoryUrlStats>();
            int storyCount = 0, blankUrls = 0, badUrls = 0;

            //Consider stories upto 3 years old for this analysis
            var minUnixDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(365*3)).ToUnixTime();

            foreach (var responseJson in JsonNetUtils.DeserializeSequenceFromJson<JObject>(textBoxStoriesFilePath.Text))
            {
                try
                {
                    foreach (var hitJson in responseJson["hits"])
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
                        catch (UriFormatException)
                        {
                            badUrls++;
                        }
                    }
                }
                catch{}
            }
            endFileReading:

            Task.WaitAll(storyUrlsStats.Values.Select(s => s.AnalyzeWpTask).ToArray());

            using (var urlStatsFile = File.CreateText(textBoxUrlAnalysisFilePath.Text))
            {
                foreach (var urlStatsKvp in storyUrlsStats)
                {
                    if (string.IsNullOrWhiteSpace(urlStatsKvp.Value.ThemeNames) && string.IsNullOrWhiteSpace(urlStatsKvp.Value.PluginNames))
                        continue;

                    //Tab seperated Column Header
                    //Host	URLSample	PointsSum	ThemeNames	PluginNames	StoryCount	CommentsSum	MaxDate	MinDate
                    var values = new string[] {urlStatsKvp.Key, urlStatsKvp.Value.Url, urlStatsKvp.Value.PointsSum.ToStringInvariant(), 
                        urlStatsKvp.Value.ThemeNames, urlStatsKvp.Value.PluginNames,
                        urlStatsKvp.Value.StoryCount.ToStringInvariant(), urlStatsKvp.Value.CommentsSum.ToStringInvariant(), 
                        urlStatsKvp.Value.CreatedUnixTimeMax.ToString(), urlStatsKvp.Value.CreatedUnixTimeMin.ToString()};

                    urlStatsFile.WriteLine(values.ToDelimitedString("\t"));
                }
            }

            MessageBox.Show("Total: {0}, Blank: {1}, Bad: {2}".FormatEx(storyCount, blankUrls, badUrls));
        }

        private void buttonRegExText_Click(object sender, EventArgs e)
        {
            var testStats = StoryUrlStats.Create("http://www.ShitalShah.com");
            WpAnalyzer.AnalyzeWordPressUrl(testStats);
            MessageBox.Show("Themes: {0}, Plugins: {1}".FormatEx(testStats.ThemeNames, testStats.PluginNames));
        }

        private void buttonFetchComents_Click(object sender, EventArgs e)
        {
            SaveHnItems("comment", textBoxCommentsFilePath.Text, this.UpdateProgress);
        }

        private void buttongetStats_Click(object sender, EventArgs e)
        {
            foreach(var filePath in new[]{textBoxStoriesFilePath.Text, textBoxCommentsFilePath.Text})
            {
                var createDatesUnix = JsonNetUtils.DeserializeSequenceFromJson<JObject>(filePath)
                    .SelectMany(responseJson => responseJson["hits"])
                    .Select(hitJson => hitJson["created_at_i"].Value<long>());

                long minDateUnix = long.MaxValue, maxDateUnix = long.MinValue, count = 0;
                foreach(var createDateUnix in createDatesUnix)
                {
                    minDateUnix = Math.Min(minDateUnix, createDateUnix);
                    maxDateUnix = Math.Max(maxDateUnix, createDateUnix);
                    count++;
                }

                MessageBox.Show("{3}\n{0} items from {1} to {2}".FormatEx(count, Utils.FromUnixTime(minDateUnix).ToString("r"), Utils.FromUnixTime(maxDateUnix).ToString("r"), filePath));
            }
        }
        private void buttonAnalyzePostTimes_Click(object sender, EventArgs e)
        {
            PostTimeAnalyzer.AnalyzePostTimes(textBoxStoriesFilePath.Text, textBoxPostTimeStatsFilePath.Text);
        }
    }
}
