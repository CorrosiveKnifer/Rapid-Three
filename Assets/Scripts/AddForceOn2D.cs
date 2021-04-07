using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// rachael work
/// </summary>
public class AddForceOn2D : MonoBehaviour
{
    public GameObject projectilePrefab;
    Rigidbody2D projectileRB;

    Vector2 playerLook;
    Vector2 direction;
    Vector2 myPosition;

    float Magnitude = 1.0f;
    Vector3 worldPosition;
    Vector3 screenPoint;

    // Start is called before the first frame update
    void Start()
    {
        projectileRB = projectilePrefab.GetComponent<Rigidbody2D>();
    }
    void AddForceUp()
    {
        Vector2 Force = direction * Magnitude;
        projectileRB.AddForce(Force, ForceMode2D.Impulse);

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
        
        Debug.DrawRay(myPosition, direction* Magnitude);
        //direction.Normalize();
        Debug.Log(direction.magnitude);
        if (Input.GetMouseButtonDown(0))
        {
            //mousePos = Input.mousePosition;
            //AddForceUp();
            screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera
            playerLook = Camera.main.ScreenToWorldPoint(screenPoint);
            myPosition = new Vector2(transform.position.x, transform.position.y);

            direction = playerLook - myPosition;
            //Magnitude = (playerLook - myPosition).magnitude;
            Magnitude = 1.0f;
            direction.Normalize();


        }
        if (Input.GetMouseButton(0))
        {
            
            if(Magnitude>=5)
            {
                Magnitude = 5.0f;
            }
            else
            {
                Magnitude += 0.01f;
            }
            
            //Debug.Log("Pressed left click.");
        }
        if (Input.GetMouseButtonUp(0))
        {
            //mousePos = Input.mousePosition;
            AddForceUp();

        }

    }

}
