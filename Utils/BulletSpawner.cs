using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BulletSpawner : MonoBehaviour
{
    public enum SpawnerType { Straight, Spin }

    [Header("Bullet Attributes")]   
    public float bulletLife;
    public float bulletSpeed;
    public GameObject bullet;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType _spawnerType;
    [SerializeField] private float _fireRate;    
    [SerializeField] private float _target;    
    [SerializeField] private float minRandomRange;
    [SerializeField] private float maxRandomRange;


    public bool isRandom;
    public float timeatstart;
    public UnityEvent onFire;

    private GameObject _spawnedBullet;
    private float _timer;
    [SerializeField] private float _eulerangler;


    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        _eulerangler = Random.Range(225f, 315f);
        _timer += Time.deltaTime;
        if (_spawnerType == SpawnerType.Spin)
        { transform.eulerAngles = new Vector3(0, 0, _eulerangler); }
            //else { transform.position = Vector3.down; }
        if (_timer >= _fireRate)
        {
                Fire();
                _timer = 0;

        }
    }


    private void Fire()
    {
        if (bullet)
        {
            onFire?.Invoke();
            _spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            _spawnedBullet.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
            _spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            _spawnedBullet.transform.rotation = transform.rotation;

        }

    }

    private float Sweeper(float euler)
    {
        var neweuler = euler;

        neweuler += _target;

        return neweuler;
    }

    private float Rotator(float euler)
    {
        var neweuler = euler;

        neweuler++;

        return neweuler;

    }

    private float Spinner(float euler)
    { 
    
        var neweuler = euler;
        neweuler--; //maybe ++ depending on Sweeper
        return neweuler;
    }

    private float PingPonger(float euler)
    { 
        var neweuler = Mathf.PingPong(Time.time, euler);
        return neweuler;
    }

    private IEnumerator PingPongerGo(float euler)
    {
        yield return timeatstart;
        Debug.Log("Time at start gone");
              
            while (true)
            {
                Debug.Log("While loop initiated");
                euler = PingPonger(_eulerangler);
                yield return null;
            }
    }
}
