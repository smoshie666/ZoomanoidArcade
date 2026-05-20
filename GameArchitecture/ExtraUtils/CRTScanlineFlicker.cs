using UnityEngine;
using UnityEngine.UI;

public class CRTScanlineFlicker : MonoBehaviour
{
    public RawImage scanlines;
    public float scrollSpeed = 0.5f;

    private void Awake()
    {
        if (scanlines == null)
        {
            scanlines = GetComponent<RawImage>();
        }

        if (scanlines == null)
        {
            Debug.LogError("CRTScanlineFlicker: No RawImage assigned or found!");
            enabled = false;
        }
    }

    private void Update()
    {
        Rect uv = scanlines.uvRect;
        uv.y += Time.unscaledDeltaTime * scrollSpeed;
        scanlines.uvRect = uv;

    }


}
