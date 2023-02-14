using Newtonsoft.Json;


public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int page = 1;
        int goals = 0;
        for (int i = 1; i <= 2; i++)
        {
            while (true)
            {
                var current = GetMatches(year.ToString(), team, page.ToString(), $"team{i}");
                foreach (Match m in current.data)
                {
                    if (m.team1 == team) goals += m.team1goals;
                    if (m.team2 == team) goals += m.team2goals;
                }
                page++;
                if (page > current.total_pages) break;
            }
            page = 1;
        }

        return goals;
    }

    public static MatchResult GetMatches(string year, string team, string page, string teamSearch)
    {
        HttpClient client = new HttpClient();
        string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamSearch}={team}&page={page}";
        var current = client.GetAsync(url).Result;
        MatchResult result = JsonConvert.DeserializeObject<MatchResult>(current.Content.ReadAsStringAsync().Result);
        return result;

    }

    public class Match
    {
        public string team1 { get; set; }
        public string team2 { get; set; }
        public int team1goals { get; set; }
        public int team2goals { get; set; }
    }

    public class MatchResult
    {
        public List<Match> data { get; set; }
        public int total_pages { get; set; }
    }



}