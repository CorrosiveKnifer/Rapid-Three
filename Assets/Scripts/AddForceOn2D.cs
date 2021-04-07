using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOn2D : MonoBehaviour
{
    public GameObject projectilePrefab;
    Rigidbody2D projectileRB;

    Vector2 playerLook;
    Vector2 direction;
    Vector2 myPosition;

    Vector3 worldPosition;

    // Start is called before the first frame update
    void Start()
    {
        projectileRB = projectilePrefab.GetComponent<Rigidbody2D>();
    }
    void AddForceUp()
    {
        projectileRB.AddForce(direction, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 mousePos = Input.mousePosition;
        //mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0.0f;
        transform.position = worldPosition;
        */
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        playerLook = Camera.main.ScreenToWorldPoint(screenPoint);
        myPosition = new Vector2(transform.position.x, transform.position.y);

        direction = playerLook - myPosition;
        //direction.Normalize();
        Debug.DrawRay(myPosition, direction);
        //direction.Normalize();
        Debug.Log(transform.position);
        if (Input.GetMouseButtonDown(0))
        {
            //mousePos = Input.mousePosition;
            AddForceUp();
          
        }
    }
}
