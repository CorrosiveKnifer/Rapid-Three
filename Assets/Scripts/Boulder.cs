using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public GameObject projectilePrefab;
    Rigidbody2D projectileRB;

    [Header("Force Settings")]
    public float maximumForce = 5.0f;
    public float forcePerSecond = 0.5f;

    Vector2 playerLook;
    Vector2 direction;
    Vector2 myPosition;

    float Magnitude = 0.0f;
    Vector3 screenPoint;

    public PlayerController playercontro;

    // Start is called before the first frame update
    void Start()
    {
        projectileRB = projectilePrefab.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetRayDraw();
        if (playercontro.m_bIsLifting)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                Magnitude = 0.0f;


            }
            if (Input.GetButton("Fire1"))
            {
                if (Input.GetMouseButton(0))
                {
                    //using mouse mechanic
                    screenPoint = Input.mousePosition;
                    screenPoint.z = 10.0f; //distance of the plane from the camera
                    playerLook = Camera.main.ScreenToWorldPoint(screenPoint);

                    myPosition = new Vector2(transform.position.x, transform.position.y);
                    direction = GetVectorOfThrow();
                }
                else
                {
                    float x = Input.GetAxis("HorizontalAim");
                    float y = Input.GetAxis("VerticalAim");
                    if (x == 0 && y == 0)
                    {
                        x = 1;
                    }
                    myPosition = new Vector2(transform.position.x, transform.position.y);
                    direction = new Vector2(x, y);
                }

                direction.Normalize();

                //increasing the amount of force
                Magnitude = Mathf.Clamp(Magnitude + (forcePerSecond * Time.deltaTime), 0, maximumForce);
                

                //Debug.Log("Pressed left click.");
            }
            if (Input.GetButtonUp("Fire1"))
            {
                ApplyForce();
            }
        }

    }

    void ApplyForce()
    {
        playercontro.ReleaseBoulder();
        Vector2 Force = direction * Magnitude;
        projectileRB.AddForce(Force, ForceMode2D.Impulse);

    }
    void GetRayDraw()
    {
        float RayMadgnitude = Magnitude / maximumForce;
        Debug.DrawRay(myPosition, direction * (4.5f * RayMadgnitude));
    }
    public Vector2 GetVectorOfThrow()
    {
        return playerLook - myPosition;
    }
    public float GetDistanceOfThrow()
    {
        return (playerLook - myPosition).magnitude;
    }
}
