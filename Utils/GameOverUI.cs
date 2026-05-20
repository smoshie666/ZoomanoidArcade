using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText1;
    public TextMeshProUGUI gameOverText2;
    public TextMeshProUGUI gameOverText3;
    public Button restartButton;

    public TextMeshProUGUI victoryText1;
    public TextMeshProUGUI victoryText2;


    private void Awake()
    {
      //  restartButton.gameObject.SetActive(false);
    }

    private void Start()
    {
       // StartCoroutine(RestartButton());
    }

    private IEnumerator RestartButton()
    { 
        yield return new WaitForSeconds(1f);
        restartButton.gameObject.SetActive(true);
    }
}
