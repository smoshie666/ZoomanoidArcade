using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class WebGLInputFix : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void FocusCanvas();
#endif

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        FocusCanvas();
#endif
    }



}
