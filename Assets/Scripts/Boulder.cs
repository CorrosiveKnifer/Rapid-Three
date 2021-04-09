using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject indicatorPrefab;
    Rigidbody2D projectileRB;

    [Header("Force Settings")]
    public float maximumForce = 5.0f;
    public float forcePerSecond = 0.5f;

    Vector2 playerLook;
    Vector2 direction;
    Vector2 myPosition;

    float Magnitude = 0.0f;
    Vector3 screenPoint;

    public LayerMask m_PlayerMask;
    public float radius = 0.0f;

    public PlayerController playercontro;

    // Start is called before the first frame update
    void Start()
    {
        projectileRB = projectilePrefab.GetComponent<Rigidbody2D>();
        indicatorPrefab.transform.localScale *= radius;
        indicatorPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Making the indicator appear and dissapear
        bool boulderRadius = Physics2D.OverlapCircle(transform.position, radius, m_PlayerMask);
        if (boulderRadius == true)
        {
            indicatorPrefab.SetActive(false);
        }
        if (boulderRadius == false)
        {
            indicatorPrefab.SetActive(true);
        }

        //using mouse mechanic
        screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera
        playerLook = Camera.main.ScreenToWorldPoint(screenPoint);

        
        GetRayDrawWhenAim();
        if (playercontro.m_bIsLifting)
        {
            Debug.DrawRay(GetPlayerPositon(), (playerLook - GetPlayerPositon()), Color.green);
            if (Input.GetButtonDown("Fire1"))
            {

                Magnitude = 0.0f;


            }
            if (Input.GetButton("Fire1"))
            {
                
                float x = Input.GetAxis("HorizontalAim");
                float y = Input.GetAxis("VerticalAim");
                //GetRayDrawDirection(x*4.5f, y*4.5f);
                if(x != 0 || y != 0)
                {
                    Debug.Log("move the x");
                    Debug.Log(x);
                    Debug.Log("move the y");
                    Debug.Log(y);
                }
                if (Input.GetKey("joystick button 8"))
                {
                    direction = new Vector2(x, y);
                    
                }
                else if (Input.GetMouseButton(0))
                {
                    //using mouse mechanic
                    screenPoint = Input.mousePosition;
                    screenPoint.z = 10.0f; //distance of the plane from the camera
                    playerLook = Camera.main.ScreenToWorldPoint(screenPoint);
                    
                    myPosition = GetPlayerPositon();
                    direction = GetVectorOfThrow();
                    
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
    void GetRayDrawWhenAim()
    {
        float RayMadgnitude = Magnitude / maximumForce;
        Debug.DrawRay(myPosition, direction * (4.5f * RayMadgnitude));
    }
    void GetRayDrawDirection(float x, float y)
    {
        Vector2 pointer = new Vector2(x, y);
        Vector2 myPosition = new Vector2(transform.position.x, transform.position.y);


        Vector2 direction = pointer - myPosition;
        Debug.DrawRay(myPosition, direction, Color.red);
    }
    public Vector2 GetPlayerPositon()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        return position;
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
