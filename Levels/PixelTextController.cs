using PixelBattleText;
using UnityEngine;

public class PixelTextController : MonoBehaviour
{
    public TextAnimation textAnimation;
    public float lifeTime = 2.5f;
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Spawns above player
    public float fadeSpeed = 2.0f;
    //public Canvas bonusCanvas;
    private CanvasGroup canvasGroup;


    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Automatically destroy the text after lifeTime seconds
        Destroy(gameObject, lifeTime);
        
    }

    void Update()
    {
        // Optional: Make it float upwards slowly
        transform.position += Vector3.up * Time.deltaTime * 0.5f;

        // Start fading out when life is almost over (e.g., last 1 second)
        if (lifeTime < 1.0f)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime * fadeSpeed);
        }

        lifeTime -= Time.deltaTime;

        /* if (Input.GetKeyDown(KeyCode.Space))
             PixelBattleTextController.DisplayText(
            "Hello World!", textAnimation, new Vector3(0.5f, 0.25f, 0.5f));*/
    }

}
