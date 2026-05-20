using UnityEngine;

public class TextCrawler : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;

    //could put gameobjects here and shrink them as they move up if can't do 2D scroller on slanted canvas

    private void Update()
    {
        transform.Translate(Camera.main.transform.up * _scrollSpeed * Time.deltaTime );
    }


}
