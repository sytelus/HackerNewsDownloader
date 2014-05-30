using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonUtils;

namespace HackerNewsDownloader
{
    public static class WpAnalyzer
    {
        static Regex endsWithExtensionRegEx = new Regex(@".*?\..{2,4}$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        static Regex wpThemesRegEx = new Regex(@"wp-content\/themes\/(.*?)\/", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        static Regex wpPluginsRegEx = new Regex(@"wp-content\/plugins\/(.*?)\/", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        public static void AnalyzeWordPressUrl(StoryUrlStats urlStats)
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

    }
}
