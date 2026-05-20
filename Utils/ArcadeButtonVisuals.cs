using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class ArcadeButtonVisuals : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public RectTransform visual;
    [SerializeField] private Image frame;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject glow;

    public UnityEvent onSelect;

    private Vector3 _defaultScale;

    private void Awake()
    {
        _defaultScale = visual.localScale;

        if (glow != null)
            glow.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Kill ALL tweens
        LeanTween.cancel(visual.gameObject);
        LeanTween.cancel(gameObject);

        visual.localScale = _defaultScale;

        onSelect?.Invoke();
        Debug.Log("SELECTED: " + gameObject.name);
        LeanTween.scale(visual.gameObject, _defaultScale * 1.08f, 0.18f).setEaseOutBack().setIgnoreTimeScale(true);

        if (glow != null)
            glow.SetActive(true);

        LeanTween.value(gameObject, 0f, 1f, 0.4f)
            .setLoopPingPong()
            .setOnUpdate((float t) =>
            {
                frame.color = Color.Lerp(Color.white, Color.cyan, t);
                label.color = Color.Lerp(Color.white, Color.yellow, t);
            }).setIgnoreTimeScale(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Kill ALL tweens
        LeanTween.cancel(visual.gameObject);
        LeanTween.cancel(gameObject);

        visual.localScale = _defaultScale;

        frame.color = Color.white;
        label.color = Color.white;

        if (glow != null)
            glow.SetActive(false);
    }

}
