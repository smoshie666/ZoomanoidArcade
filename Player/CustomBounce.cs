using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBounce : MonoBehaviour
{
    private CapsuleCollider2D _caps2D;

    private void Awake()
    {
        _caps2D = GetComponentInChildren<CapsuleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            float ballRelativePosition = (other.transform.position.x - this.transform.position.x) / _caps2D.bounds.size.x;
            other.rigidbody.linearVelocity = new Vector2(ballRelativePosition, 1).normalized * other.rigidbody.linearVelocity.magnitude;
        }


    }

}
