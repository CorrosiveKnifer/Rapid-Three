using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float closestDist = 10000.0f;
        RaycastHit2D closestHits = new RaycastHit2D();
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.parent.transform.position, Vector2.down, 5.0f);
        foreach (var hit in hits)
        {
            if (hit.distance < closestDist && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                closestDist = hit.distance;
                closestHits = hit;
            }
        }

        GetComponent<SpriteRenderer>().enabled = closestDist != 10000.0f;

        if (closestDist != 10000.0f)
        {
            transform.position = new Vector3(closestHits.point.x, closestHits.point.y, 0.0f);
            transform.up = closestHits.normal;
        }
    }
}
