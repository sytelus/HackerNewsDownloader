using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace CommonUtils
{
    public class UrlComponents
    {
        public UrlComponents(string url)
        {
            string hostPrefix = string.Empty;
            string query = string.Empty;
            string path = string.Empty;
            string file = string.Empty;
            string host = string.Empty;
            string hostBase = string.Empty;
            NameValueCollection queryParameters;
            this.HasPathOrQuery = false;

            if (!string.IsNullOrEmpty(url))
            {
                Uri uri;
                
                url = FixUrlErrorsAndGetUriObject(url.ToLowerInvariant(), out uri, out queryParameters);

                host = uri.Host;

                hostBase = host;

                int lastHostDotIndex = host.LastIndexOf(".", StringComparison.Ordinal);
                if (lastHostDotIndex > 0)
                {
                    int lastToLastDotIndex = host.Substring(0, lastHostDotIndex).LastIndexOf(".", StringComparison.Ordinal);
                    if (lastToLastDotIndex > 0)
                    {
                        hostPrefix = host.Substring(0, lastToLastDotIndex);
                        hostBase = host.Substring(lastToLastDotIndex + 1);
                    }
                }

                if (GetUriPathAndQuery(uri) == "/")
                {
                    //nothing more to do
                }
                else
                {
                    this.HasPathOrQuery = true;

                    path = HttpUtility.UrlDecode(uri.AbsolutePath ?? string.Empty);
                    int lastPathDelimiter = path.LastIndexOf("/", StringComparison.Ordinal);
                    int lastPathDotIndex = path.LastIndexOf(".", StringComparison.Ordinal);
                    if (lastPathDelimiter >= 0 && lastPathDotIndex >= 0 && lastPathDotIndex > lastPathDelimiter)
                    {
                        file = path.Substring(lastPathDelimiter + 1);
                        path = path.Substring(0, lastPathDelimiter);
                    }

                    query = uri.Query ?? string.Empty;
                }
            }
            else
            {
                queryParameters = null;
            }

            this.Host = host;
            this.HostBase = hostBase;
            this.File = file;
            this.Query = query;
            this.Path = path;
            this.HostPrefix = hostPrefix;
            this.Url = url;
            this.QueryParameters = queryParameters;
        }

        public string Host { get; private set; }
        public string HostBase { get; private set; }
        public string HostPrefix { get; private set; }
        public string Path { get; private set; }
        public string Query { get; private set; }
        public NameValueCollection QueryParameters { get; private set; }
        public string File { get; private set; }
        public string Url { get; private set; }
        public bool HasPathOrQuery
        {
            get;
            private set;
        }



        private static string GetUriPathAndQuery(Uri uri)
        {
            try
            {
                return uri.PathAndQuery;
            }
            catch (IndexOutOfRangeException) //This seems to be thrown by very long encoded URLs
            {
                return "/"; //ignore bad URL paths
            }
            catch (ArgumentOutOfRangeException)
            {
                return "/"; //ignore bad URL paths
            }
        }

        private static string FixUrlErrorsAndGetUriObject(string url, out Uri uri, out NameValueCollection queryParameters)
        {
            if (!url.Contains("://"))
                url = "http://" + url;  //otherwise Uri object can't do parsing

            url = url.Trim().ToLowerInvariant();

            if (url.IndexOf("http", StringComparison.Ordinal) > 0)    //Some url string have bugs, for example, "abc df http://www.example.com"
                url = url.Substring(url.IndexOf("http", StringComparison.Ordinal));

            url = url.Replace("/#!/", "/"); //This type of facebook URLs confuses Uri object parsing

            if (url.EndsWith("/", StringComparison.Ordinal) && url.Length > 1)
                url = url.Substring(0, url.Length - 1);

            url = HttpUtility.UrlDecode(url);   //Some urls are encoded and Uri object would fail to parse them if they don't get decoded first
            try
            {
                uri = new Uri(url);

                queryParameters = HttpUtility.ParseQueryString(uri.Query);

                string urlInUrl = queryParameters["url"] ?? queryParameters["u"];   //websites like Yelp has URL in URL. However we must avoid taking URL from parameters like urlref. f param is used by doubleclick
                if (urlInUrl == null)
                {
                    int doubleClickStyleUrl = url.LastIndexOf("f?http://", StringComparison.Ordinal);
                    if (doubleClickStyleUrl >= 0)
                    {
                        urlInUrl = url.Substring(doubleClickStyleUrl + 2);
                    }
                }

                if (!string.IsNullOrEmpty(urlInUrl) && (urlInUrl.StartsWith("http://", StringComparison.Ordinal) ||
                    urlInUrl.StartsWith("https://", StringComparison.Ordinal) || urlInUrl.StartsWith("www.", StringComparison.Ordinal)))
                {
                    url = FixUrlErrorsAndGetUriObject(urlInUrl, out uri, out queryParameters);
                }
            }
            catch(UriFormatException)
            {
                url = "http://UrlFormatException/" + HttpUtility.UrlEncode(url);
                uri =  new Uri(url);
                queryParameters = HttpUtility.ParseQueryString(uri.Query);
            }
            

            return url;
        }

        public static string GetMatchHash(string url)
        {
            return (new UrlComponents(url)).GetMatchHash();
        }

        public static byte[] GetEmptyBinary()
        {
            return null;
        }

        public static string GetQueryParameterValue(string url, string queryParameterName, string matchHostLowerCased)
        {
            var urlComponents = new UrlComponents(url);
            if (!string.IsNullOrEmpty(matchHostLowerCased))
            {
                if (urlComponents.Host != matchHostLowerCased)
                    return null;
                else
                    return urlComponents.QueryParameters[queryParameterName];
            }
            else
                return null;
        }

        private readonly static char[] UrlSlashDelimiter = new char[] {'/'};
        public static string GetHost(string url, bool fastAndDirty)
        {
            if (!fastAndDirty)
            {
                var urlComponents = new UrlComponents(url);
                return urlComponents.Host;
            }
            else
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var splttedUrl = url.ToLowerInvariant().Split(UrlSlashDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    if (splttedUrl.Length > 0)
                    {
                        if (splttedUrl.Length >= 2)
                            return splttedUrl[1];
                        else
                            return splttedUrl[0];
                    }
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
        }

        public static IEnumerable<KeyValuePair<string, string>> GetUrlComponents(string url, string returnQueryParameterName, bool returnPath, bool returnQuery, bool returnFile, bool returnHostPrefix, bool returnHostBase, bool returnHost)
        {
            var urlComponents = new UrlComponents(url);

            if (returnHost)
                yield return new KeyValuePair<string, string>("Host", urlComponents.Host);
            if (returnHostBase)
                yield return new KeyValuePair<string, string>("HostBase", urlComponents.HostBase);
            if (returnHostPrefix)
                yield return new KeyValuePair<string, string>("HostPrefix", urlComponents.HostPrefix);
            if (returnPath)
                yield return new KeyValuePair<string, string>("Path", urlComponents.Path);
            if (returnQuery)
                yield return new KeyValuePair<string, string>("Query", urlComponents.Query);
            if (returnFile)
                yield return new KeyValuePair<string, string>("File", urlComponents.File);
            if (returnQueryParameterName != null && urlComponents.QueryParameters != null && urlComponents.QueryParameters.Count > 0)
                yield return new KeyValuePair<string, string>("q:" + returnQueryParameterName, urlComponents.QueryParameters[returnQueryParameterName]);
        }

        public string GetMatchHash()
        {
            StringBuilder urlHash = new StringBuilder(255);

            urlHash.Append(this.HostBase);

            if (!string.IsNullOrEmpty(HostPrefix) && !CommonTerms.CommonHostPrefixes.Contains(this.HostPrefix))
            {
                urlHash.Append("|");
                urlHash.Append(this.HostPrefix);
            }

            string path = this.Path;
            if (!string.IsNullOrEmpty(path))
            {
                string[] pathParts = path.Split('/');
                foreach (string pathPart in pathParts)
                {
                    if (!string.IsNullOrEmpty(pathPart) && !CommonTerms.CommonPaths.Contains(pathPart))
                    {
                        urlHash.Append("|");
                        urlHash.Append(pathPart);
                    }
                }
            }

            string file = this.File;
            if (!string.IsNullOrEmpty(file))
            {
                string[] fileParts = file.Split('.');
                foreach (string filePart in fileParts)
                {
                    if (!string.IsNullOrEmpty(filePart) && !CommonTerms.CommonFileParts.Contains(filePart))
                    {
                        urlHash.Append("|");
                        urlHash.Append(filePart);
                    }
                }
            }


            if (this.QueryParameters != null && this.QueryParameters.Count > 0)
            {
                foreach (var queryParamName in this.QueryParameters.AllKeys)
                {
                    if (CommonTerms.includeQueryParameterExceptionNames.Contains(queryParamName))
                    {
                        string paramValue = this.QueryParameters[queryParamName];
                        if (!string.IsNullOrEmpty(paramValue))
                        {
                            urlHash.Append("|");
                            urlHash.Append(queryParamName);
                            urlHash.Append("=");
                            urlHash.Append(paramValue);
                            break;
                        }
                    }
                }
            }

            return urlHash.ToString();
        }

        [Obsolete]
        public string GetMatchHash1()
        {
            StringBuilder urlHash = new StringBuilder(255);

            urlHash.Append(this.HostBase);

            if (CommonTerms.includeHostPrefixForHostBases.Contains(this.HostBase) && !CommonTerms.CommonHostPrefixes.Contains(this.HostPrefix))
            {
                urlHash.Append("|");
                urlHash.Append(this.HostPrefix);
            }

            if (CommonTerms.includePathExceptionHosts.Contains(this.HostBase))
            {
                urlHash.Append("|");
                urlHash.Append(this.Path);
            }

            if (this.QueryParameters != null && this.QueryParameters.Count > 0)
            {
                foreach (var queryParamName in this.QueryParameters.AllKeys)
                {
                    if (CommonTerms.includeQueryParameterExceptionNames.Contains(queryParamName))
                    {
                        string paramValue = this.QueryParameters[queryParamName];
                        if (!string.IsNullOrEmpty(paramValue))
                        {
                            urlHash.Append("|");
                            urlHash.Append(queryParamName);
                            urlHash.Append("=");
                            urlHash.Append(paramValue);
                            break;
                        }
                    }
                }
            }

            return urlHash.ToString();
        }

    }
}
