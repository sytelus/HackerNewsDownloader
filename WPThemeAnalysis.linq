<Query Kind="Statements">
  <Reference Relative="..\DotNetCommonUtils\CommonUtils\bin\Debug\CommonUtils.dll">C:\GithubSrc\DotNetCommonUtils\CommonUtils\bin\Debug\CommonUtils.dll</Reference>
  <Namespace>CommonUtils</Namespace>
</Query>

var dates = File.ReadLines(@"C:\temp\WPUrlAnalysis.txt")
.Select(line => line.Split('\t')) 
.Where(cols => cols.Length == 9)
.SelectMany(cols => Utils.AsEnumerable(long.Parse(cols[7]), long.Parse(cols[8])))
.ToList();

var minDate = dates.Min();
var maxDate = dates.Max();
var deltaDate = maxDate - minDate;

var unranked = File.ReadLines(@"C:\temp\WPUrlAnalysis.txt")
.Select(line => line.Split('\t')) 
.Where(cols => cols.Length == 9)
//.Dump()
.GroupBy(cols => cols[3])
.Select(g => new { 
	Theme = g.Key, 
	PointsSum = g.Sum(cols => Utils.ParseOrDefault(cols[2],0,true)) ,
	CommentsSum = g.Sum(cols => Utils.ParseOrDefault(cols[6],0,true)),
	StroryCountsum = g.Sum(cols => Utils.ParseOrDefault(cols[5],0,true)),
	HostCount = g.Select(cols => cols[0]).Distinct().Count(),
	HostExamples = g.Select(cols => cols[0]).Distinct().Take(5).ToDelimitedString(", "),
	AvgStoryPoints = g.Average(cols => Utils.ParseOrDefault(cols[2],0,true) / (double) Utils.ParseOrDefault(cols[5],0,true)) ,
	AvgStoryComments = g.Average(cols => Utils.ParseOrDefault(cols[6],0,true) / (double) Utils.ParseOrDefault(cols[5],0,true)),
	RelativeDateMin = g.Min(cols => (long.Parse(cols[8]) - minDate) / (double) deltaDate),
	RelativeDateMax = g.Max(cols => (long.Parse(cols[7])- minDate) / (double) deltaDate),
	AvgStoryPointsWeighted = g.Average(cols => Math.Pow(Math.E, (long.Parse(cols[8]) - minDate) / (double) deltaDate) *
		Utils.ParseOrDefault(cols[2],0,true) * 10.0 / (double) Utils.ParseOrDefault(cols[5],0,true)),
	AvgStoryCommentsWeighted = g.Average(cols => Math.Pow(Math.E, (long.Parse(cols[8]) - minDate) / (double) deltaDate) *
		Utils.ParseOrDefault(cols[6],0,true) * 10.0 / (double) Utils.ParseOrDefault(cols[5],0,true)),
	ExplicitBlogCount = g.Count(cols => cols[0].Contains("blog")),
}).ToList();

var maxWeight = unranked.Max(t => t.AvgStoryPointsWeighted + t.AvgStoryCommentsWeighted*3);
var minWeight = unranked.Min(t => t.AvgStoryPointsWeighted + t.AvgStoryCommentsWeighted*3);
var deltaWeight = maxWeight - minWeight;

var maxExplicitBlogCount = unranked.Max(t => t.ExplicitBlogCount) + 1.0;

unranked
.Where(t => t.HostCount > 2 && t.RelativeDateMax > 0.85)
.Select(u => new {Data = u, NormalizedWeight = ((u.AvgStoryPointsWeighted + u.AvgStoryCommentsWeighted*3) - minWeight) / deltaWeight})
.Select(u => new {Data = u.Data, Rank = u.NormalizedWeight + ((u.Data.ExplicitBlogCount + 1.0)/(maxExplicitBlogCount+1)) + 2*u.Data.RelativeDateMax})
//.Count()
.OrderByDescending(u => u.Rank)
.Select(u => u.Data)
//.Select((r,i) => Tuple.Create(r.Data, r.Rank, i))
//.Where(t => t.Item1.Theme == "omega" || t.Item1.Theme == "able" || t.Item1.Theme == "decode" || t.Item1.Theme == "ryu" || t.Item3 < 5)
.Dump();