
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletLife;
    public float bulletSpeed;
    public float bulletRotation;

    private Vector2 _spawnOrigin;
    private float _timer;


    // Start is called before the first frame update
    void Start()
    {
        _spawnOrigin = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > bulletLife) { Destroy(this.gameObject); }
        _timer += Time.deltaTime;
        transform.position = Movement(_timer);
    }

    private Vector2 Movement(float timer)
    {
        float x = timer * bulletSpeed * transform.right.x;
        float y = timer * bulletSpeed * transform.right.y;

        return new Vector2(x + _spawnOrigin.x, y + _spawnOrigin.y);
    
    }
}
