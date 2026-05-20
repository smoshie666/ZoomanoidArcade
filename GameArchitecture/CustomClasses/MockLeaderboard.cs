using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

public static class MockLeaderboard
{

    private static List<LeaderboardEntry> mockEntries = new List<LeaderboardEntry>(8)
    {
        new LeaderboardEntry("JMT", 1000000, 1, "id_001"),
        new LeaderboardEntry("DGT", 875000, 2, "id_002"),
        new LeaderboardEntry("CMD", 750000, 3, "id_003" ),
        new LeaderboardEntry("ZMA", 600000, 4, "id_004"),
        new LeaderboardEntry("DEG", 550000, 5, "id_005"),
        new LeaderboardEntry("TTT", 450000, 6, "id_006"),
        new LeaderboardEntry("GST", 300000, 7, "id_007"),
        new LeaderboardEntry("CTK", 275000, 8, "id_008")
    };

    public static void CheckMockScore(int score)
    {
      //  score.ToString("000000");

        foreach (var entry in mockEntries)
        {
            if (score >= entry.score)
            {
                //add
                mockEntries.Add(new LeaderboardEntry("YOU", score, 0, "local", true));
                //break;
                return;
            }

        }

    }

    public static void AddMockScore(int score)
    {

        //want this so it only adds high score once and adds to appropriate pos until beaten etc

        //need to: check if player is in leaderboard, and what position. Store reference(s)
        //check if he beats mock scores and place him in that position
        //if not set to false
        //if so set to true



        //so score has to reflect Hi Score and actual Score from game
        //save reference to score at end of game
        //check if it is HIGH SCORE
        //if not check if it is OTHER HIGH SCORE
        //place in table accordingly and make sure it only happens that one time

        //also want to LIMIT table to 10 entries = make it an array and replace or set list limit
        ////or remove from list
        ///
        // Remove previous player entries
//      mockEntries.RemoveAll(e => e.isPlayerEntry);
        mockEntries.Add(new LeaderboardEntry("YOU", score, 0, "local", true));
        SortLeaderboard();
        Debug.Log("SortLeaderboard called via AddMockScore()");
    }
     
    public static List<LeaderboardEntry> GetMockEntries()
    {
        SortLeaderboard();
        return new List<LeaderboardEntry>(mockEntries);
    }


    private static void SortLeaderboard()
    {
        //this is where we can compare entries and add or remove accordingly

        mockEntries.Sort((a, b) => b.score.CompareTo(a.score));

        for (int i = 0; i < mockEntries.Count; i++)
        {
            mockEntries[i].rank = i + 1;
            Debug.Log("SortLeaderboard SORTED!!");
        }

        if (mockEntries.Count > 8)
        {
            mockEntries.RemoveAt(8);
           
        }
    }
}
