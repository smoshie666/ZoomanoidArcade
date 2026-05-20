
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class ContinueController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private TextMeshProUGUI _continueText;
    [SerializeField] private GameObject _adButton;
    [SerializeField] private GameObject _insertCoinText;

    [Header("Settings")]
    [SerializeField] private float _countdownStart = 9f;
    [SerializeField] private float _countdownInterval = 1f;
   // [SerializeField] private float _insertCoinFlashDuration = 0.2f;

    [Header("Insert Coin Flash Settings")]
    [SerializeField] private float _baseFlashDuration = 0.2f;


    private float _countdown;
    private System.Action _onContinue;
    private System.Action _onFail;

    private Coroutine _insertCoinCoroutine;
    private Coroutine _countdownCoroutine;
  //  private Coroutine _flashCoroutine;

    public void ShowContinue(System.Action onContinue, System.Action onFail)
    {
        EventSystem.current.SetSelectedGameObject(_adButton);

        _onContinue = onContinue;
        _onFail = onFail;
        this.gameObject.SetActive(true);


        StartCoroutine(StaggeredShowUI());
        StartCountdown();
        StartInsertCoinFlash();
        _insertCoinText.transform.localScale = Vector3.one;
        LeanTween.scale(_insertCoinText, Vector3.one * 1.1f, 0.3f).setLoopPingPong(1);

        /* StartCoroutine(ContinueSequence());
         StartCoroutine(CountdownRoutine(onContinue, onFail));
        */

    }
    IEnumerator ContinueSequence()
    {
        _countdown = 9;
        _canvasGroup.alpha = 1;
        yield return new WaitForSecondsRealtime(0.2f);

        _continueText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.4f);
        _countdownText.gameObject.SetActive(true);
       

        yield return new WaitForSecondsRealtime(0.6f);
        _adButton.gameObject.SetActive(true);

    }

    public void OnContinuePressed()
    {
        Debug.Log("Continue Pressed");
        _onContinue?.Invoke();
        StopAllInsertCoinFlash();
        gameObject.SetActive(false);


    }

    public void OnFailTriggered()
    {
        _onFail?.Invoke();
        StopAllInsertCoinFlash();
        gameObject.SetActive(false);
    }

    private void StartCountdown()
    {
        _countdown = _countdownStart;
        if (_countdownCoroutine != null)
            StopCoroutine(_countdownCoroutine);

        _countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        yield return new WaitForSecondsRealtime(0.4f);

        while (_countdown > 0)
        {
            _countdownText.text = Mathf.Ceil(_countdown).ToString();
            yield return new WaitForSecondsRealtime(_countdownInterval);
            _countdown--;

        }
        // Countdown finished
        OnFailTriggered();

    }

    private void StartInsertCoinFlash()
    {

        if (_insertCoinCoroutine != null)
            StopCoroutine(_insertCoinCoroutine);
     //   _flashCoroutine = StartCoroutine(InsertCoinFlashRoutine());
            _insertCoinCoroutine = StartCoroutine(InsertCoinFlashRoutine());
    }

    private void StopAllInsertCoinFlash()
    {
        if (_insertCoinCoroutine != null)
        {
            StopCoroutine(_insertCoinCoroutine);
            _insertCoinCoroutine = null;
        }

        if (_insertCoinText != null)
        {
            LeanTween.cancel(_insertCoinText);
            _insertCoinText.SetActive(false);
        }
    }

    private IEnumerator InsertCoinFlashRoutine()
    {
        
        _insertCoinText.SetActive(true);
        CanvasGroup cg = _insertCoinText.GetComponent<CanvasGroup>();
        if (cg == null) cg = _insertCoinText.AddComponent<CanvasGroup>();

        // reset alpha and cancel any old tweens
        cg.alpha = 1f;
        LeanTween.cancel(_insertCoinText);


        while (gameObject.activeSelf)
        {
            // Flash speed up logic based on countdown
            float speedMultiplier = 1f;
            if (_countdown < 5f) speedMultiplier = 0.6f;   // double speed
            if (_countdown < 3f) speedMultiplier = 0.4f;   // triple speed
            if (_countdown < 1.5f) speedMultiplier = 0.25f; // super fast

            // Clamp minimum duration so it never becomes a blur
            float duration = Mathf.Max(_baseFlashDuration * speedMultiplier, 0.08f);

            TextTweener(_insertCoinText, 0f, duration, 1, true);
            yield return new WaitForSecondsRealtime(duration * 2f);
        }

        _insertCoinText.SetActive(false);


/*
        // infinite flash
        TextTweener(_insertCoinText, 0f, _insertCoinFlashDuration, -1, true);

        // optional: auto-hide after X seconds (10.5 in your original)
        yield return new WaitForSecondsRealtime(10.5f);
*/
  //      _insertCoinText.SetActive(false);

    
    }



    private IEnumerator StaggeredShowUI()
    {
        // small initial delay for polish
        yield return new WaitForSecondsRealtime(0.2f);

        _continueText.gameObject.SetActive(true);
        LeanTween.scale(_continueText.gameObject, Vector3.one * 1.2f, 0.2f)
                 .setLoopPingPong(1);

        yield return new WaitForSecondsRealtime(0.6f);

        _countdownText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.4f);

        _adButton.gameObject.SetActive(true);
        LeanTween.scale(_adButton, Vector3.one * 1.1f, 0.2f).setEasePunch();
    }


    private void TextTweener(
       GameObject textObject,
       float alphaStart = 0f,
       float duration = 0.2f,
       int loops = -1, // infinite
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


}
