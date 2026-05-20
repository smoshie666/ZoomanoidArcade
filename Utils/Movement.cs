using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Configuration")]
    public Vector3 direction;
    public float speed;

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}