using UnityEngine;

public class LeaderboardEntry
{
    public string name;
    public int score;
    public int rank;
    public string playerID;
    public bool isPlayerEntry;


    public LeaderboardEntry(string name, int score, int rank, string id)
    {
        this.name = name;
        this.score = score;
        this.rank = rank;
        playerID = id;
    }

    public LeaderboardEntry(string name, int score, int rank, string id, bool isPlayer)
    {
        this.name = name;
        this.score = score;
        this.rank = rank;
        playerID = id;
        isPlayerEntry = isPlayer;

    }

}