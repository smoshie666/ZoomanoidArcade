using UnityEngine;

public class HiScore : MonoBehaviour
{

    private void Awake()
    {
        HiScoreManager.HiScore();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("GameStarted is " + HiScoreManager.gameStarted);
    }
}
