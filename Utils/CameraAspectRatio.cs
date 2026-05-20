using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // Change if needed (e.g. 9f/16f for portrait)


    private void Start()
    {

        SetAspect();
        
    }

    void LateUpdate()
    {

        SetAspect();

    }


    private void SetAspect()
    { 
        Camera cam = GetComponent<Camera>();

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1)
        {
            // Add letterbox (top & bottom)
            Rect rect = cam.rect;

            rect.width = 1;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1 - scaleHeight) / 2;

            cam.rect = rect;

        }
        else {

            // Add pillarbox (left & right)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1;

            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;

        }



    }

}
