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

    [Header("Force Settings")]
    public float maximumForce = 5.0f;
    public float forcePerSecond = 0.5f;

    Vector2 playerLook;
    Vector2 direction;
    Vector2 myPosition;

    float Magnitude = 1.0f;
    Vector3 worldPosition;
    Vector3 screenPoint;

    float RayMadgnitude = 0.0f;
    public PlayerController playercontro;

    // Start is called before the first frame update
    void Start()
    {
        projectileRB = projectilePrefab.GetComponent<Rigidbody2D>();
    }
    void AddForceUp()
    {
        playercontro.ReleaseBoulder();
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
        RayMadgnitude = Magnitude / maximumForce;
        Debug.DrawRay(myPosition, direction* (4.5f* RayMadgnitude));
        //direction.Normalize();
        //Debug.Log(direction.magnitude);

        if (playercontro.m_bIsLifting)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                Magnitude = 1.0f;


            }
            if (Input.GetButton("Fire1"))
            {

                
                if (Input.GetMouseButton(0))
                {
                    screenPoint = Input.mousePosition;
                    screenPoint.z = 10.0f; //distance of the plane from the camera
                    playerLook = Camera.main.ScreenToWorldPoint(screenPoint);
                    myPosition = new Vector2(transform.position.x, transform.position.y);
                    direction = playerLook - myPosition;
                }
                else
                {
                    float x = Input.GetAxis("HorizontalAim");
                    float y = Input.GetAxis("VerticalAim");
                    if (x == 0 && y == 0)
                    {
                        x = 1;
                    }
                    direction = new Vector2(x, y);
                }

                Debug.Log(direction);

                //mousePos = Input.mousePosition;
                //AddForceUp();

                //Magnitude = (playerLook - myPosition).magnitude;
                direction.Normalize();

                //Magnitude = Mathf.Clamp(Magnitude + forcePerSecond * Time.deltaTime, 0, maximumForce);
                if (Magnitude >= maximumForce)
                {
                    Magnitude = maximumForce;
                }
                else
                {
                    Magnitude += forcePerSecond * Time.deltaTime;
                }

                //Debug.Log("Pressed left click.");
            }
            if (Input.GetButtonUp("Fire1"))
            {
                //mousePos = Input.mousePosition;
                AddForceUp();

            }
        }

    }

}
