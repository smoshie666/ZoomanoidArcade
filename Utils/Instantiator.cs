using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject prefab;

    public void InitiateInstantiate()
    { 
        if (prefab != null)
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
