using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rachael work
/// </summary>
/// 
public class Boulder : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject indicatorPrefab;
    Rigidbody2D projectileRB;

    [Header("Ground")]
    public LayerMask m_GroundMask;
    public bool m_bGrounded;


    [Header("Force Settings")]
    public float maximumForce = 5.0f;
    public float forcePerSecond = 0.5f;
    public float maximumShakeDistance = 20.0f;

    Vector2 playerLook;
    Vector2 direction;
    Vector2 myPosition;

    public float Magnitude = 0.0f;
    Vector3 screenPoint;

    public LayerMask m_PlayerMask;
    public float radius = 0.0f;

    public PlayerController playercontro;
    public GameObject director;
    // Start is called before the first frame update
    void Start()
    {
        projectileRB = projectilePrefab.GetComponent<Rigidbody2D>();
        indicatorPrefab.transform.localScale *= radius;
        indicatorPrefab.SetActive(false);
    }

    private void FixedUpdate()
    {
        GroundedUpdate();
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
                
                if (Input.GetMouseButton(0))
                {
                    //using mouse mechanic
                    screenPoint = Input.mousePosition;
                    screenPoint.z = 10.0f; //distance of the plane from the camera
                    playerLook = Camera.main.ScreenToWorldPoint(screenPoint);

                    myPosition = GetPlayerPositon();
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

                    myPosition = GetPlayerPositon();
                    
                    //direction = new Vector2(x, y);
                    //Debug.Log(direction);
                    //GetRayDrawDirection(x, y);
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
        else
        {
            Magnitude = 0.0f;
        }

        GameManager.instance.SetPower(Magnitude, maximumForce);
    }
    
    void GroundedUpdate()
    {
        bool wasGrounded = m_bGrounded;
        m_bGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.8f, m_GroundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject) // If found ground near ground check
            {
                m_bGrounded = true; // Set grounded to true.
                if (!wasGrounded && projectileRB.velocity.y < 0)
                {
                    float distanceFromPlayer = Mathf.Clamp(1.0f - (Vector2.Distance(playercontro.playerSprite.transform.position, transform.position) / maximumShakeDistance), 0.0f, 1.0f);
                    float fCamShake = (Mathf.Abs(projectileRB.velocity.y) / 30) * distanceFromPlayer;
                    Debug.Log(fCamShake);
                    CameraController.instance.StartShake(fCamShake, fCamShake);
                }
                break;
            }
        }

        if (m_bGrounded)
        {
            projectileRB.drag = 1.0f;
        }
        else
        {
            projectileRB.drag = 0.0f;
        }

        if (projectileRB.velocity.y < 0)
        {
            projectileRB.gravityScale = 1.5f;
        }
        else
        {
            projectileRB.gravityScale = 1.0f;
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
