using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatScript : MonoBehaviour
{
    public GameObject PlayerObject;
    public GameObject BoulderObject;

    private SpriteRenderer FlagSprite;
    // Start is called before the first frame update
    void Start()
    {
        FlagSprite = GetComponent<SpriteRenderer>();
        SetSavePoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            SetSavePoint();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadSavePoint();
        }
    }

    private void SetSavePoint()
    {
        float closestDist = 10000.0f;
        RaycastHit2D closestHits = new RaycastHit2D();
        RaycastHit2D[] hits = Physics2D.RaycastAll(PlayerObject.transform.position, Vector2.down, 5.0f);
        foreach (var hit in hits)
        {
            if (hit.distance < closestDist && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                closestDist = hit.distance;
                closestHits = hit;
            }
        }

        if (closestDist != 10000.0f)
        {
            transform.position = new Vector3(closestHits.point.x, closestHits.point.y, 0.0f);
            transform.up = closestHits.normal;
        }
    }
    private void LoadSavePoint()
    {
        PlayerObject.transform.up = transform.up;
        BoulderObject.transform.up = transform.up;

        PlayerObject.transform.position = transform.position + (transform.up * 1.5f);
        BoulderObject.transform.position = transform.position + (transform.up * 1.5f);

        PlayerObject.GetComponent<Rigidbody2D>().velocity = new Vector2();
        BoulderObject.GetComponent<Rigidbody2D>().velocity = new Vector2();
    }
}
