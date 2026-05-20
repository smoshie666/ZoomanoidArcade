
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeaderboardEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image background;
    [SerializeField] private float flickerTime = 3;

    private Color _normalColor = Color.white;
    private Color _highlightColor = new Color(1f, 0.9f, 0.3f); // arcade gold


    public void Set(LeaderboardEntry entry)
    { 
        rankText.text = entry.rank.ToString("00");
        nameText.text = entry.name;
        scoreText.text = entry.score.ToString("000000");

               
            
            if (entry.isPlayerEntry)
            Highlight();
        else
            ResetVisual();

           // if (gameObject.SetActive(true))
            StartCoroutine(BoardFlicker());
    }

    private void Highlight()
    {
        nameText.color = _highlightColor;
        scoreText.color = _highlightColor;
        rankText.color = _highlightColor;
        nameText.colorGradient = new VertexGradient(new Color(1f, 0.9f, 0.3f), new Color(1f, 0.8f, 0f), new Color(0.8f, 0.7f, 0.2f), new Color(1f, 0.9f, 0.4f));
        scoreText.colorGradient = new VertexGradient(new Color(1f, 0.9f, 0.3f), new Color(1f, 0.8f, 0f), new Color(0.8f, 0.7f, 0.2f), new Color(1f, 0.9f, 0.4f));
        rankText.colorGradient = new VertexGradient(new Color(1f, 0.9f, 0.3f), new Color(1f, 0.8f, 0f), new Color(0.8f, 0.7f, 0.2f), new Color(1f, 0.9f, 0.4f));

        LeanTween.cancel(this.gameObject);
        LeanTween.alphaText(nameText.rectTransform, 0.3f, 0.4f).setLoopPingPong();
    }

    private void ResetVisual()
    {
        nameText.color = _normalColor;
        scoreText.color = _normalColor;
        rankText.color = _normalColor;
    }

    private IEnumerator BoardFlicker()
    {
        while (true)
        {
            scoreText.alpha = 0.7f + Random.Range(0, 1);
            rankText.alpha = 0.6f + Random.Range(0, 1);
            nameText.alpha = 0.5f + Random.Range(0, 1);
            yield return new WaitForSeconds(flickerTime);
            Debug.Log("text alpha updating in LBEUI");
        }
    
    }
    


}
