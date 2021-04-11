using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Roger Painting, Michael Jordan (edits)
/// </summary>
public class Parallax : MonoBehaviour
{
    private Vector2 length;
    private Vector3 startpos;
    public float parallaxEffect;

    //I did dome quick edits to reduce the public variable reliance on the camera. ~ Michael
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float tempX = (mainCam.transform.position.x * (1 - parallaxEffect));
        float dist = mainCam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startpos.x + dist, startpos.y); //Fixed bug where z was used instead of y

        if (tempX > startpos.x + length.x) startpos.x += length.x;
        else if (tempX < startpos.x - length.x) startpos.x -= length.x;

    }
}
