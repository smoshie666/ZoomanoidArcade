using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroller : MonoBehaviour
{
    public float scrollSpeed;

    [SerializeField]
    private MeshRenderer _mesh;


    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(0, Time.time * scrollSpeed);
        _mesh.material.mainTextureOffset = offset;
    }


}
