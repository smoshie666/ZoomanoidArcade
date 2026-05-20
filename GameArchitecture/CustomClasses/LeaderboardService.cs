using Playgama;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public static class LeaderboardService
{
    private const string LeaderboardID = "highscore";

    public static void SubmitLeaderboardScore(int score)
    {
        // Always update local board
        MockLeaderboard.AddMockScore(score);

#if UNITY_WEBGL && !UNITY_EDITOR
    Bridge.leaderboards.SetScore(
        LeaderboardID,
        score,
        (bool success) => {
            Debug.Log("Leaderboard score submitted: " + success);
        }
    );

   

#else 
        Debug.Log($"[EDITOR MOCK] Would submit score {score} to leaderboard '{LeaderboardID}'.");
       // MockLeaderboard.AddMockScore(score);
      
#endif

        
    }


    public static void GetEntries(System.Action<List<LeaderboardEntry>> callback)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        
 Bridge.leaderboards.GetEntries(

        LeaderboardID,

        (bool success, List<Dictionary<string, string>> entries) =>
        {
            List<LeaderboardEntry> combined = new();

            if (success)
            {
             
                foreach (var e in entries)
                {
                    combined.Add(new LeaderboardEntry(
                    e["name"],
                    int.Parse(e["score"]),
                    int.Parse(e["rank"]),
                    e["id"]
                 ));
                
                }

            }

            combined.AddRange(MockLeaderboard.GetMockEntries());

            combined.Sort((a, b) => b.score.CompareTo(a.score));

            for (int i = 0; i < combined.Count; i++)
                combined[i].rank = i + 1;

            if (combined.Count > 8)
                combined.RemoveRange(8, combined.Count - 8);

            callback(combined);

        }

        );
                

#else
        Debug.Log("[EDITOR MOCK] Returning mock leaderboard data");
        callback(MockLeaderboard.GetMockEntries());

       
#endif
       

    }

}
    

 