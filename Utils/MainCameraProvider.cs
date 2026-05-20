using System.Collections;
using UnityEngine;

public class MainCameraProvider : MonoBehaviour
{
    public static Camera Instance;
   

    private void Awake()
    {
            Instance = GetComponent<Camera>();
    }

}
