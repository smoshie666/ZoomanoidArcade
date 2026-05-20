using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;


public class LeaderboardUI : MonoBehaviour
{
    /*  [Header("UI References")]
      [SerializeField] private RectTransform rootPanel;      // LeaderboardUI object
      [SerializeField] private RectTransform entryContainer;
      [SerializeField] private GameObject entryPrefab;
      [SerializeField] private GameObject newRecordPanel;

      [Header("Layout Control")]
      [SerializeField] private VerticalLayoutGroup layoutGroup;
      // [SerializeField] private ContentSizeFitter sizeFitter;
    */

    [Header("UI References")]
    public GameObject title;
    public GameObject insertCoinText;
    public GameObject leaderboardPanel;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private CanvasGroup canvasGroup;


    [Header("Pre-made Slots (8)")]
    [SerializeField] private List<LeaderboardEntryUI> slots;

    [Header("Auto Refresh")]
    [SerializeField] private float refreshInterval = 5f;



  //  private List<GameObject> spawnedEntries = new();

    private void OnEnable()
    {
        StartCoroutine(AttractLoop());
        RefreshStatic();
        StartCoroutine(AutoRefresh());

        // Show();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0.95f + UnityEngine.Random.Range(0f, 0.05f);
    }

    public void RefreshStatic()
    {
        LeaderboardService.GetEntries((entries) =>
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < entries.Count)
                    slots[i].Set(entries[i]);
            }
        });
    }

    private void TextTweener(
    GameObject textObject,
    float alphaStart = 0.5f,
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


    private void Hide()
    {
        //hide UI objects at start
        //then show one by one in AttractLoop()
    }


    IEnumerator AutoRefresh()
    {
        while (true)
        {
            yield return new WaitForSeconds(refreshInterval);
            RefreshStatic();
        }

    }

    IEnumerator CoinFlash()
    {
        insertCoinText.SetActive(true);
        CanvasGroup cg = insertCoinText.GetComponent<CanvasGroup>();
        if (cg == null) cg = insertCoinText.AddComponent<CanvasGroup>();

        // reset alpha and cancel any old tweens
        cg.alpha = 1f;
        LeanTween.cancel(insertCoinText);

        while (gameObject.activeSelf)
        {
            TextTweener(insertCoinText, 0f, _duration, -1, true);
            yield return new WaitForSecondsRealtime(_duration * 2f);
        }

        insertCoinText.SetActive(false);

    }

    IEnumerator AttractLoop()
    {        
            ShowTitle();
            yield return new WaitForSeconds(3f);

            ShowLeaderboard();
            yield return new WaitForSeconds(6f);

            FlashInsertCoin();
            yield return new WaitForSeconds(3f);
        
    }

    private void FlashInsertCoin()
    {
        StartCoroutine(CoinFlash());
    }

    private void ShowTitle()
    {
        title.gameObject.SetActive(true);

    }

    private void ShowLeaderboard()
    {
        leaderboardPanel.gameObject.SetActive(true);
    }

    







    /*

    public void Show()
    {
        var position = new Vector2(500f, 0);
        layoutGroup.enabled = false;
        //  sizeFitter.enabled = false;
        
        // Move panel offscreen left
        rootPanel.anchoredPosition = new Vector2(-1200f, 0);

        // Slide in with NeoGeo bounce
        LeanTween.move(rootPanel, position, 1f)
                 .setEaseOutExpo()
                 .setOnComplete(() =>
                 {
                     // Re-enable layout AFTER animation
                     layoutGroup.enabled = true;
               //      sizeFitter.enabled = true;

                     LayoutRebuilder.ForceRebuildLayoutImmediate(entryContainer);
                 }); 
        Refresh();
    }


    public void Refresh()
    {
      
         //Destroy existing
        foreach (var e in spawnedEntries)
            Destroy(e);

        spawnedEntries.Clear();

            // Load entries from leaderboard service
            LeaderboardService.GetEntries((entries) =>
        {
            int index = 0;
            int playerIndex = -1;

            foreach (var e in entries)
            {
                GameObject item = Instantiate(entryPrefab, entryContainer);
                spawnedEntries.Add(item);

                ApplyData(item, e);
                              
                AnimateEntry(item, index);
                
                if (e.isPlayerEntry)
                    playerIndex = index;

                index++;
            }

            if (playerIndex >= 0)
            {
                HighlightNewScore(spawnedEntries[playerIndex]);
                ShowNewRecordPanel();
            }

        });
                
    }


    
   
    private void ShowNewRecordPanel()
    {
        newRecordPanel.SetActive(true);

        RectTransform rt = newRecordPanel.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;

        LeanTween.scale(rt, Vector3.one, 0.45f)
                 .setEaseOutBack();
    }


    private void ApplyData(GameObject item, LeaderboardEntry e)
    {
        item.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = e.rank.ToString();
        item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = e.name;
        item.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = e.score.ToString();
    }


    public void AnimateEntry(GameObject entry, int index)
    {
         
        float delay = index * 0.06f;

        CanvasGroup cg = entry.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = entry.AddComponent<CanvasGroup>();

        cg.alpha = 0f;


        LeanTween.alphaCanvas(cg, 1f, 0.25f)
         .setDelay(delay);

        // OPTIONAL: subtle pop on a child visual
        Transform visual = entry.transform.Find("Visual");
        if (visual != null)
        {
            visual.localScale = Vector3.one * 0.95f;

            LeanTween.scale(visual.gameObject, Vector3.one, 0.25f)
                     .setDelay(delay)
                     .setEaseOutBack();
        }
    }

    private void HighlightNewScore(GameObject entry)
    {

        Transform visual = entry.transform.Find("Visual");
        if (visual == null) return;

        LeanTween.scale(visual.gameObject, Vector3.one * 1.15f, 0.2f)
                 .setEaseOutExpo()
                 .setLoopPingPong(1);

        Image img = visual.GetComponent<Image>();
        if (img == null) return;

        Color orig = img.color;

        LeanTween.value(entry, 0f, 1f, 0.4f)
            .setLoopPingPong(1)
            .setOnUpdate(t =>
            {
                img.color = Color.Lerp(orig, Color.white, t);
            });
       
    }
    */
}
