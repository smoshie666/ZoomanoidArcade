using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AssignCanvasCamera : MonoBehaviour
{
    
    private void Start()
    {
        StartCoroutine(WaitForStart());

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit: " + hit.collider.name);
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }

    private IEnumerator WaitForStart()
    {

        while (MainCameraProvider.Instance == null)
        {
            yield return null;
        }

        yield return null; // extra safety frame

        Canvas canvas = GetComponent<Canvas>();

        if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

        

        canvas.worldCamera = MainCameraProvider.Instance; 

        Debug.Log("Canvas camera assigned: " + MainCameraProvider.Instance.name);

        canvas.overrideSorting = true;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 100;
        
        //Canvas.ForceUpdateCanvases();
        Debug.Log("Canvas Camera: " + canvas.worldCamera);

        
    }

}
