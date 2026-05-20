using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{

    public SceneSO level;


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI hiScoreText;

    public TextMeshProUGUI levelNumber;
    public TextMeshProUGUI levelCompleteText1;
    public TextMeshProUGUI levelCompleteText2;

    public Button chooseBonusTime;
    public Button chooseBonusScore;
    public Button chooseXtraLife;
    public Button continueButton;
    


    private void Start()
    {
        //level = SceneManager.GetActiveScene() ;

    }

    private void Update()
    {
      //  levelNumber.text = level.sceneName;
    }
}
