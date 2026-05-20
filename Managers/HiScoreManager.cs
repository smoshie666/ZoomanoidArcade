using UnityEngine;


public static class HiScoreManager
{
    public static int highScore;
    public static int score;
    public static bool gameStarted = false;

    public static void HiScore()
    {
        score = PlayerPrefs.GetInt("ScoreEntry", 0);
        
        if (!gameStarted)
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            LeaderboardService.SubmitLeaderboardScore(highScore);
            gameStarted = true;
        }
    }

}
