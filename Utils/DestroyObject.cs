using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [Header("Configuration")]
    public float destroyTime;

    public void IntiateDestruction()
    {
        Destroy(gameObject, destroyTime);
        Debug.Log("Boooom!");
    }
}
