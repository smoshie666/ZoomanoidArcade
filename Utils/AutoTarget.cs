using UnityEngine;

public class AutoTarget : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        var targetPosition = target.position;
        Vector3 direction = targetPosition - transform.position;
        direction.Normalize();

        float rot2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot2 - 90f);
    }
}
