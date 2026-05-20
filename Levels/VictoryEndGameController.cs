using UnityEngine;

public class VictoryEndGameController : MonoBehaviour
{
    private Movement _mover;

    private void Start()
    {
        _mover = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= -2250f)
        {
            if(_mover != null)
            _mover.speed = 0;
        }
    }
}
