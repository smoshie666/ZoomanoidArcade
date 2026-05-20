
#if UNITY_WEBGL
using Playgama.Modules.Advertisement;
#endif
using Playgama;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;
using NUnit.Framework;

#if !UNITY_WEBGL
namespace Playgama.Modules.Advertisement
{
    public enum RewardedState { Loading, Opened, Rewarded, Closed }
    public enum InterstitialState { Loading, Opened, Closed, Failed }
}
#endif

public class GameSession : MonoBehaviour
{
    public static GameSession instance;

    public int score;
    public static int hiScore;
    //hi score here
    public static int totalLives = 5;  //want these represented by icons
    public static int currentLives;
    [SerializeField] private int startingLives = 5;
    public float _bonusTime = 5;

    [SerializeField] private List<Image> livesImages;
    [SerializeField] private Image _extraLifeImage;


    public float waitTime;
    public float waitTimeVictory;
    public ScoreUI scoreUI;
    public LifeScreenUI lifeScreenUI;
    [SerializeField] private Transform _lifeUITransform;
    public GameOverUI gameOverUI;
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private GameObject _insertCoinText;
  //  public Button continueButton;
    [SerializeField] private ContinueController _continueController;

    public TimeManager timeManager;
    public MMFeedbacks coinSound;

    [Header("Broadcasting Events")]
    public BoolGameEvent hasLostLife;
    public BoolGameEvent abilityIsNull;
    public BoolGameEvent hasBeenShot;
    public BoolGameEvent hasContinued;

    public UnityEvent OnGameStart;
    public UnityEvent OnLifeLost;
    public UnityEvent onEndOfLifeLostScreen;
    public UnityEvent onEndOfLevel;
   // public UnityEvent onGameOver;
   // public UnityEvent onGameOverScreen;
    public UnityEvent OnPointsGained;
    public UnityEvent OnVictoryScreen;
    public UnityEvent OnBonusPointsGained;  //call mm feedbacks from these events
    public UnityEvent OnInsertCoinRestart;
    public UnityEvent OnInsertCoinIntiated;
    public UnityEvent OnFinalGameOver;

    public delegate void OnNextLevelLoad();
    public static OnNextLevelLoad nextLevel;


    private Image _originalImage;
    
    private int _livesControllerNumber = 0;
    private int _scoreBonusMultiplier;
    private int _level;

    private bool _scoreBonusChosen = false;
    private bool _hasBeenShot = false;
    private bool _isAwaitingContinue = false;
    [SerializeField] private bool useAdTest = false;

    private System.Action _pendingReward;
    private bool _rewardGranted;



    // [SerializeField] private bool _isAfterContinue = false;


    public int HighScore { get; private set; }
    public bool HasBeenShot { get => _hasBeenShot; set { _hasBeenShot = value; } }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;

#if UNITY_WEBGL
        Bridge.advertisement.rewardedStateChanged -= HandleRewardedState;
        Bridge.advertisement.rewardedStateChanged += HandleRewardedState;

        Bridge.advertisement.interstitialStateChanged -= HandleInterstitialState;
        Bridge.advertisement.interstitialStateChanged += HandleInterstitialState;
#endif
    }

    private void Start()
    {
        totalLives = 5;
        currentLives = totalLives;
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
               
        _scoreBonusMultiplier = 0;
        _level = 1;
        UpdateUIDisplay();
     //   CheckHiScore(); only at end?
        OnGameStart?.Invoke();

#if UNITY_WEBGL && !UNITY_EDITOR
Bridge.advertisement.SetMinimumDelayBetweenInterstitial(30);
#endif

        //   LeaderboardService.SubmitLeaderboardScore(HighScore);
    }

    private void Update()
    {
        UpdateUIDisplay();

        if (totalLives < currentLives)
        {
            currentLives = totalLives;

        } 

        if (totalLives <= 0 && !_isAwaitingContinue)
        { 
            totalLives = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Game still alive");
        }

        //    hiScore = HighScore;

        Debug.LogFormat("Current Lives in GameSession are: {0}, and Total Lives = {1}.", currentLives, totalLives);
        Debug.Log("Has Been Shot is:  " + _hasBeenShot);

        Debug.LogFormat("HighScore is {0}, Score is {1}.", HighScore, score);
        Debug.Log("Game Session Is Awaiting Continue" + _isAwaitingContinue);

        if (Input.GetMouseButtonDown(0))
        {
                Debug.Log("CLICK DETECTED");

            
                PointerEventData data = new PointerEventData(EventSystem.current);
                data.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(data, results);

                foreach (var r in results)
                {
                    Debug.Log("UI Hit: " + r.gameObject.name);
                }
            

            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("CLICKING UI");
            }
            else
            {
                Debug.Log("NOT OVER UI");
            }
        }
        
    }


    public void CheckBonusDecision()
    {

       
        //if level bool is checked change the corrseponding value
        //multiple value by relevent function below
        
    }

    //for buttons
    public void ChooseBonusTime() 
    {
        _bonusTime = NewBonusTime(_bonusTime);
        Debug.LogFormat("BonusTime should be changed. It is currently {0}. _scoeBonusChosen bool is {1}", _bonusTime, _scoreBonusChosen);
        
    }

    //for buttons
    public void ChooseScoreBoost()
    { 
        _scoreBonusChosen = true;
        Debug.LogFormat("scoreBonusChosen = {0}, and Bonus Time  = {1}", _scoreBonusChosen, _bonusTime);
        
    }

    public void EndOfLevelLife()
    {
        AddLives(1);
      //  ExtraLivesIconController(_extraLifeImage);
        //might need array of extralife icons which _extraLifeImage becomes by assiging it in a for loop

    /*    var modifier = (int)_level / 6f;

        if (modifier <= 0.5f)
        {
            AddLives(1);
            ExtraLivesIconController(_extraLifeImage);

        }
        else if (modifier <= 0.82f && modifier > 0.5f)
        {

            AddLives(2);
            ExtraLivesIconController(_extraLifeImage); //need to add 2

        }
        else if (modifier == 1)
        {
            AddLives(3);
            ExtraLivesIconController(_extraLifeImage); //need to add 3

        }*/

            
    }

    //increase bonus time - need to translate it to player
    //work it out here and send that int to player and set bonus time there

    public int NewBonusTime(float bonusTime)
    {
        int bonusTimeAdded = (int)bonusTime * _level;//new time here
        return bonusTimeAdded;
    }


    public void MoveToNextLevel() //this needs to be a button press!
    {
        nextLevel();
        CallOnEndOfLevel();
        EndOfLevelReset();
    }

    public void AddScore(int points) //call from int event listener
    {
        //if score multiplier chosen then points *= _level;
        if (_scoreBonusChosen)
        { 
            points *= _level;
            Debug.LogFormat("Score Bonus chosen {0}, and points = {1}", _scoreBonusChosen, points);            
        }

        score += points;
        CheckHiScore();
        OnPointsGained?.Invoke();//sounds, fx
    }

    public void AddBonusScore(int points) //call from int event listener
    {
        //if score multiplier chosen then points *= _level;

        score += points;
        CheckHiScore();
        OnBonusPointsGained?.Invoke();//different sounds and fx to normal score
    }


    public void CheckHiScore()
    {
        PlayerPrefs.SetInt("ScoreEntry", score);
        
        if (score >= HighScore)
        { 
            HighScore = score;
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.Save();
            return;
        }

        PlayerPrefs.Save();
    }

    public void UpdateUIDisplay()
    {
        scoreUI.scoreText.text = "SCORE: " + score.ToString("000000");
        scoreUI.livesText.text = "LIVES: " + totalLives.ToString();
        scoreUI.hiScoreText.text = "Hi-SCORE: " + HighScore.ToString("000000");
        //hi score
    }

    public void LoseLives(bool lost) //call when all balls destroyed // from event listner?
    {
        if (lost)
        {

            if (!lost) return;

            totalLives--;

            UpdateLivesUI();

            StartCoroutine(DestructionWaitTime());

           
            Debug.Log("Total Lives minus 1 in GameSession");

        }
    }


    public void LoseLifeWithBullet(bool shot)
    {
        if (shot)
        {
            if (!shot) return;

            _hasBeenShot = true;
            totalLives--;

            UpdateLivesUI();

            StartCoroutine(DestructionWaitTime());
        }
    
    }

    public bool isNotLitUp(Image image)
    {
        if (image.color.a == 0.25f)
        {
            return true;
        
        }

        return false;
    
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < livesImages.Count; i++)
        {
            Color c = livesImages[i].color;

            c.a = i < totalLives ? 1f : 0.25f;

            livesImages[i].color = c;
        }
    }

    private void LivesIconController()
    {
        for (int i = 0; i < livesImages.Count; i++)
        {
            if (livesImages[i].color.a == 1f)
            {
                Color color = Color.white;
                color.a = 0.25f;
                livesImages[i].color = color;
                Debug.LogFormat("Color {0} has been grabbed. Alpha {1} shold be changed to 0.25 in element {2}", color, color.a, livesImages[i].name);
                return;
            }       
        }
    }

    public void BonusExtraLives()
    {
        int lifeimages = livesImages.Count;

        if (lifeimages > 5)
        {
            int extras = lifeimages - 5;
            livesImages.RemoveRange(5, extras);
            Debug.Log("Removed extras number!!");
        }
        //_livesControllerNumber = 2;
        var color = Color.white;
        color.a = 1f;
        livesImages[4].color = color;
        livesImages[3].color = color;
        livesImages[2].color = color;

    }

    public void ExtraLivesIconController(Image extraLife) //could be an Image or color method??
    { 
       /* _livesControllerNumber--; //only after everthing else and check how many slots "open"
        if (_livesControllerNumber < 0)   //or could try returning out of LivesIconController
        { _livesControllerNumber = 0; }
      */
        if (_livesControllerNumber >= 1)
        {
            for(int i = livesImages.Count; i-- >= 0;)
            {
                Debug.LogFormat("Lives controller is {0}, and currently doing {1} in array ", _livesControllerNumber, i);

                if (livesImages[i].color.a == 0.25f)
                {
                    Color color = Color.white;
                    color.a = 1f;
                    livesImages[i].color = color; //if this doesn't work have to do opposite of above and recolor icon
                    Debug.LogFormat("Image was changed and was {0} in icons list", i);

                    return;
                }
              
            }
            Debug.LogFormat("Lives controller is {0}, and adding image ", _livesControllerNumber);
            livesImages.Add(extraLife);
            Instantiate(extraLife, _lifeUITransform);


        } else {

            Debug.LogFormat("Lives controller is {0}, and adding image ", _livesControllerNumber);

            livesImages.Add(extraLife);
            Instantiate(extraLife, _lifeUITransform);
        }
    }

    private IEnumerator DestructionWaitTime()
    {
        if (totalLives != 0)
        {
            yield return new WaitForSeconds(waitTime);
            hasLostLife.Raise(true);
            OnLifeLost?.Invoke(); //sounds, game state change/reset, fx
            Debug.Log("Gamesession OnLivesLost invoked");
        }
        else {
                
            yield return new WaitForSeconds(waitTime);
            
            if (!_isAwaitingContinue) 
            {
                hasContinued.Raise(true);
            }
        }
    }

    //add lives (call from listner)
    public void AddLives(int lives)
    {
        totalLives += lives;
        currentLives = totalLives;

        while (livesImages.Count < totalLives)
        {
            Image newLife = Instantiate(_extraLifeImage, _lifeUITransform);
            livesImages.Add(newLife);
        }

        UpdateLivesUI();
    }


    private void TrimExtraLifeIcons()
    {
        while (livesImages.Count > startingLives)
        {
            Image extra = livesImages[livesImages.Count - 1];

            livesImages.RemoveAt(livesImages.Count - 1);

            Destroy(extra.gameObject);
        }
    }

    public void RevivePlayer()
    {
        Debug.Log("REVIVE!");
        Debug.Log("Player revived via ad!");
        totalLives = 0;
        TrimExtraLifeIcons();
        AddLives(3);
        //add live icons
        Debug.Log("Lives added:  " + totalLives);
        

    }

    
    public void LostLifeScreenControl()
    {
        //should reset the screen elements makes sure they are set to false
        Debug.Log("Gamesession LivesLost coroutine started");
        StartCoroutine(LostLifeScreen());
        Debug.Log("Gamesession LivesLost coroutine exited");
       

    }
    private IEnumerator LostLifeScreen()
    {
        lifeScreenUI.lifeText1.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        lifeScreenUI.lifeText1.gameObject.SetActive(false);
        lifeScreenUI.lifeText2.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        lifeScreenUI.lifeText2.gameObject.SetActive(false);
        lifeScreenUI.lifeText3.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        lifeScreenUI.lifeText3.gameObject.SetActive(false);
        onEndOfLifeLostScreen?.Invoke();
        Debug.Log("End of lives Lost invoked");
        abilityIsNull.Raise(true);
        
    }

    IEnumerator LevelPassedScreen()
    { 
        //is end of level flyer properly covered ?

        scoreUI.levelCompleteText1.gameObject.SetActive(true);
        Debug.Log(scoreUI.levelCompleteText1.text);
        yield return new WaitForSecondsRealtime(1.5f);
        scoreUI.levelCompleteText1.gameObject.SetActive(false);
        scoreUI.levelCompleteText2.gameObject.SetActive(true);
        Debug.Log(scoreUI.levelCompleteText2.text);
        yield return new WaitForSecondsRealtime(1.5f);
        scoreUI.levelCompleteText2.gameObject.SetActive(false);
        _level++;
        yield return new WaitForSecondsRealtime(1.5f);
        Debug.Log("Should be setting bonus buttons on");
        scoreUI.chooseBonusTime.gameObject.SetActive(true);
        scoreUI.chooseBonusScore.gameObject.SetActive(true);
        scoreUI.chooseXtraLife.gameObject.SetActive(true);
        scoreUI.continueButton.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.5f); // small buffer

        if (_level % 2 == 0) // every 2 levels (safe frequency)
        {
            ShowInterstitialAd();
        }

        abilityIsNull.Raise(true);
        Debug.Log("Level passed coroutine exiting");
    }

    public void CallOnEndOfLevel()
    {
        onEndOfLevel?.Invoke(); //reset time and bonus choice etc
        Debug.Log("End of level invoked");
    }

    public void EndOfLevelReset()
    {
        scoreUI.chooseBonusTime.gameObject.SetActive(false);
        scoreUI.chooseBonusScore.gameObject.SetActive(false);
        scoreUI.chooseXtraLife.gameObject.SetActive(false);
        scoreUI.continueButton.gameObject.SetActive(false);
    }

    public void LevelPassedScreen(bool allBlocksGone) //called from bool listener
    {
        if (allBlocksGone) //change game state
        {
            Debug.Log("Level passed coroutine started");
          
            if (_scoreBonusChosen)
            {
                _scoreBonusChosen = false;
            }

            StartCoroutine(LevelPassedScreen());
           
            //need to change game state and Time here as well
        }
    
    }
    
    public void GameOverScreenControl(bool lost) //called from bool listener
    {
        //should reset the screen elements makes sure they are set to false
        if (lost)
        {
            Debug.Log("Starting GameOverScreenControl");
            
            StartCoroutine(GameOverScreen());
         //   OnContinueButton();
        }

        lost = false;
    }

    public void VictoryScreenControl(bool won) //called from bool listener
    {
        if (won)
        { 
        
            StartCoroutine(VictoryScreen());
         //   won = false;
        }
       
    }
    /*
    IEnumerator InsertCoinSequence() 
    {
        //call coin start event to change game state
        OnInsertCoinIntiated?.Invoke();
        _insertCoinText.SetActive(true);
        var cg = _insertCoinText.GetComponent<CanvasGroup>();

        // Cancel previous tween if it exists
        LeanTween.cancel(cg.gameObject);

        // Reset alpha fully
        cg.alpha = 1f;
        
        TextTweener(_insertCoinText, 0f, 0.2f, -1, true);
        yield return new WaitForSecondsRealtime(10.5f);
        
        
        _insertCoinText.SetActive(false);

        
        Debug.Log("Insert Coin Coroutine exiting");
    }*/

    private void TextTweener(GameObject textObject, float alphaStart)
    {
        CanvasGroup canvasgroup = textObject.GetComponent<CanvasGroup>();
        canvasgroup.alpha = alphaStart;
        LeanTween.alphaCanvas(canvasgroup, 1, 0.2f).setLoopPingPong(5).setIgnoreTimeScale(true);
   
    }

    private void TextTweener(
    GameObject textObject,
    float alphaStart = 0f,
    float duration = 0.2f,
    int loops = -1, // -1 = infinite
    bool ignoreTimeScale = true)
    {
        CanvasGroup cg = textObject.GetComponent<CanvasGroup>();
        if (cg == null) cg = textObject.AddComponent<CanvasGroup>();

        cg.alpha = alphaStart;

        var tween = LeanTween.alphaCanvas(cg, 1f, duration)
            .setLoopPingPong(loops)
            .setIgnoreTimeScale(ignoreTimeScale);

        if (loops == -1)
            tween.setLoopPingPong(); // infinite

        //Soft shimmer (0.3f, 0.5f, -1)
        //Fast arcade flash (0f, 0.1f, -1)
        //Attention grab pulse (0.5f, 0.25f, 6)
    }

    IEnumerator GameOverScreen() //change to game over state
    {

        gameOverUI.gameOverText1.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        gameOverUI.gameOverText1.gameObject.SetActive(false);
        yield return null;
        gameOverUI.gameOverText2.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        gameOverUI.gameOverText2.gameObject.SetActive(false);
        yield return null;

        //change gamestate
        OnInsertCoinIntiated?.Invoke();
        
        _continueController.ShowContinue(
                         

            onContinue: () =>
            {
                _isAwaitingContinue = true;
                Debug.Log("Game Session IsAwaitingContinue should be TRUE and is    " + _isAwaitingContinue );
                ShowRewardedAd(() =>
                {
                    RevivePlayer();
                    _isAwaitingContinue = false;

                    if (!_isAwaitingContinue)
                    {
                        OnInsertCoinRestart?.Invoke();
                    }
                    //   _isAwaitingContinue = false;

                    //   OnInsertCoinRestart?.Invoke();

                });
            },
            
            onFail: () =>
            {
                FinalGameOver();
                  
            }
        );

    }

    private IEnumerator AdTimeTester(System.Action onReward)
    {
        yield return new WaitForSecondsRealtime(30f);
        onReward?.Invoke();
        
     //   _isAwaitingContinue = false;

    }

    private void FinalGameOver()
    {
        //need to turn off continute button screen
        //and show game over with the main menu button
        OnFinalGameOver?.Invoke();
        //need coroutine here to make changes and bring restart button up
        EventSystem.current.SetSelectedGameObject(gameOverUI.restartButton.gameObject);
        gameOverUI.gameOverText3.gameObject.SetActive(true);
        gameOverUI.restartButton.gameObject.SetActive(true);
        CheckHiScore();
        LeaderboardService.SubmitLeaderboardScore(score);
        leaderboardUI.RefreshStatic();
    }

    IEnumerator VictoryScreen()
    {
        OnVictoryScreen?.Invoke();
        gameOverUI.victoryText1.gameObject.SetActive(true);
        TextTweener(gameOverUI.victoryText1.gameObject, 0.5f);
        OnVictoryScreen?.Invoke();
        yield return new WaitForSecondsRealtime(2.5f);
        gameOverUI.victoryText1.gameObject.SetActive(false);
        yield return null;
        gameOverUI.victoryText2.gameObject.SetActive(true);
        TextTweener(gameOverUI.victoryText2.gameObject, 0.5f);
        yield return new WaitForSecondsRealtime(waitTimeVictory);
        if (score < HighScore)
        {
            LeaderboardService.SubmitLeaderboardScore(score);
            leaderboardUI.RefreshStatic();
        }
       
    }


    public void ShowInterstitialAd()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    if (!Bridge.advertisement.isInterstitialSupported)
    {
        Debug.Log("Interstitial not supported");
        return;
    }

    AudioListener.pause = true;
    Bridge.advertisement.ShowInterstitial();

#else
        Debug.Log("EDITOR: Interstitial Ad");
#endif
    }


    public void ShowRewardedAd(System.Action onReward)
    {
        _pendingReward = onReward;
        _rewardGranted = false;

#if UNITY_WEBGL && !UNITY_EDITOR
    if (!Bridge.advertisement.isRewardedSupported)
    {
        Debug.Log("Rewarded ads not supported.");
        return;
    }


    AudioListener.pause = true;
    Bridge.advertisement.ShowRewarded();

        
#else
        Debug.Log("EDITOR: Rewarded Ad (auto reward)");

        if (useAdTest && Application.platform != RuntimePlatform.WebGLPlayer)
        {

            StartCoroutine(AdTimeTester(onReward));

        }

        else
        {
            onReward?.Invoke();
          //  _isAwaitingContinue = false;
        }


        
       
#endif
    }

    private void PlayCoinSound()
    {
        coinSound?.PlayFeedbacks();
    }

#if UNITY_WEBGL
    private void HandleRewardedState(RewardedState state)
    {
        Debug.Log("Rewarded state: " + state);

        switch (state)
        {
            case RewardedState.Opened:
               
                AudioListener.pause = true;
                break;

            case RewardedState.Rewarded:

                _rewardGranted = true;
                PlayCoinSound();
                _pendingReward?.Invoke();
               
                break;

            case RewardedState.Closed:

                Debug.Log("Ad CLOSED callback reached.");
                AudioListener.pause = false;
                Debug.Log("Audio unpaused.");

                Time.timeScale = 1f;
                Debug.Log("TimeScale is: " + Time.timeScale);

                if (!_rewardGranted)
                {
                    Debug.Log("Ad closed without reward.");
                }

                Debug.Log("Reward flow cleanup complete.");

                _rewardGranted = false;
                _pendingReward = null;
                break;
        }
    }

#endif


#if UNITY_WEBGL
    private void HandleInterstitialState(InterstitialState state)
    {
        Debug.Log("Interstitial state: " + state);

        switch (state)
        {
            case InterstitialState.Opened:
                AudioListener.pause = true;
                break;

            case InterstitialState.Closed:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                break;
        }
    }
#endif



    //add bonus score (call from listner)

    //control end of game state

    //load next level communicating with scene loader manager
}
