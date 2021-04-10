using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("TEMP SPRITES")]
    public Sprite m_NoCarry;
    public Sprite m_Carry;

    [Header("Attached")]
    public GameObject playerSprite;

    [Header("Mobility Test")]
    public int m_iAirJumps = 1;
    private int m_iJumpsLeft = 1;
    public float m_fGrapplePull = 5.0f;

    [Header("Movement Values")]
    public float m_fJumpForce = 12.0f;
    public float m_fRunSpeed = 10.0f;
    public float m_fAirSpeed = 10.0f;
    public float m_fCarrySpeed = 1.0f;
    private float m_fMovementSmooth = 0.3f;

    [Header("Jump Forgiveness")]
    public float m_fJumpForgiveTime = 0.4f;
    public float m_fJumpTimer = 0.4f;
    public bool m_bCanJump = true;


    [Header("Ground")]
    public LayerMask m_GroundMask;
    public Transform m_GroundCheck;
    public bool m_bGrounded;

    [Header("Life Timer")]
    public float m_fLife = 100.0f;
    public float m_fLifeTetherRadius = 10.0f;
    public bool m_bIsRegening = false;
    public float m_fRegenRate = 1.0f;
    public float m_fDecayRate = 1.0f;

    [Header("Boulder")]
    public float m_fBoulderLerpSpeed = 1.0f;
    public float m_fLiftRadius = 1.5f;
    public GameObject m_Boulder;
    public Transform m_BoulderAnchor;
    public bool m_bIsLifting = false;

    private Rigidbody2D m_Rigidbody;

    private bool m_FacingRight = false;
    private Vector3 m_Velocity = Vector3.zero;
    private float m_eulerZVelocity = 0.0f;
    private float m_fRotMovementSmooth = 0.1f;


    private void Awake()
    {
        // Set player and boulder collisions to ignore.
        Physics2D.IgnoreLayerCollision(11, 12);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody2D
        m_Rigidbody = GetComponent<Rigidbody2D>();

        // Get m_fLifeTetherRadius from boulder
        m_fLifeTetherRadius = m_Boulder.GetComponent<Boulder>().radius;
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_bGrounded;
        m_bCanJump = false;
        Quaternion newRotation = Quaternion.identity;
        // Ground check
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.2f, m_GroundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject) // If found ground near ground check
            {
                m_bGrounded = true; // Set grounded to true.
                m_fJumpTimer = 0;
                m_iJumpsLeft = m_iAirJumps;
                m_bCanJump = true;

                //newRotation = colliders[i].gameObject.transform.rotation;
                if (!wasGrounded && m_Rigidbody.velocity.y < 0)
                {
                    //float shakeAmount = m_Rigidbody.velocity.y / 10.0f;
                    //CameraController.instance.StartShake(0.3f, 0.3f);
                }
                break;
            }
        }

        if (m_bGrounded && colliders.Length == 0)
        {
            m_fJumpTimer += Time.fixedDeltaTime;

            if (m_fJumpForgiveTime <= m_fJumpTimer)
            {
                m_bGrounded = false;
            }
        }


        RaycastHit2D[] hits = Physics2D.RaycastAll(m_GroundCheck.position, transform.TransformDirection(Vector3.down), 0.1f, m_GroundMask);

        // If more than 0 hits
        if (hits.Length > 0)
        {
            RaycastHit2D hit = hits[0];
            for (int i = 1; i < hits.Length; i++)
            {
                // get shortest distance
                if (hits[i].distance < hit.distance)
                {
                    hit = hits[i];
                }
            }

            // Get angle from hit normal.
            Vector2 normal = hit.normal;
            float z = -Vector2.SignedAngle(normal, transform.TransformDirection(Vector2.up));
            newRotation = Quaternion.Euler(0.0f, 0.0f, z);
        }

        // Smoothly move to target rotation.
        float delta = Quaternion.Angle(playerSprite.transform.rotation, newRotation);
        if (delta > 0f)
        {
            float t = Mathf.SmoothDampAngle(delta, 0.0f, ref m_eulerZVelocity, m_fRotMovementSmooth);
            t = 1.0f - (t / delta);
            playerSprite.transform.rotation = Quaternion.Slerp(playerSprite.transform.rotation, newRotation, t);
        }
    }

    // Update is called once per frame
    public void Move(float _move, bool _jump)
    {
        // Set speed to the air time speed.
        float speed = m_fAirSpeed;
        if (m_bGrounded)
        {
            speed = m_fRunSpeed; // If the player is grounded change speed to normal
            if (_move == 0)
            {
                m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y);
            }

        }
       
        if (_jump && ((m_iJumpsLeft > 0 && m_iAirJumps != 0) || (m_iAirJumps == 0 && m_bCanJump))) // Check for jump input and if have enough jumps left.
        {
            --m_iJumpsLeft;
            m_bIsLifting = false;
            m_bGrounded = false; // Apply jump.
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0.0f);
            m_Rigidbody.AddForce(new Vector2(0.0f, m_fJumpForce), ForceMode2D.Impulse);
        }

        if (m_bIsLifting) // Check if lifting.
        {
            speed = m_fCarrySpeed; // Set speed to the carry speed.
        }

        // Set target velocity
        Vector3 targetVelocity = new Vector2(_move * speed, m_Rigidbody.velocity.y);

        // Smoothly set to target velocity.
        m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_fMovementSmooth);

        // If falling
        if (m_Rigidbody.velocity.y < 0) // Increase gravity
        {
            m_Rigidbody.gravityScale = 5.0f;
        }
        else // Normal gravity
        {
            m_Rigidbody.gravityScale = 2.0f;
        }

        // Sprite facing direction
        if (_move < 0 && m_FacingRight)
        {
            Flip();
        }
        else if (_move > 0 && !m_FacingRight)
        {
            Flip();
        }
    }

    public void Lift(bool _lifting)
    {
        if (m_bIsLifting) // Temp switch sprites
        {
            playerSprite.GetComponentInChildren<SpriteRenderer>().sprite = m_Carry;
        }
        else
        {
            playerSprite.GetComponentInChildren<SpriteRenderer>().sprite = m_NoCarry;
        }

        if (m_Boulder == null) // Check if boulder exists in world.
        {
            Debug.Log("Boulder does not exist!");
            return;
        }
        // Check if boulder is in range to be picked up.
        bool inRange = false;      
        if (Vector3.Distance(transform.position, m_Boulder.transform.position) <= m_fLiftRadius)
        {
            inRange = true;
        }
        if (m_fLifeTetherRadius < Vector3.Distance(transform.position, m_Boulder.transform.position))
        {
            // Drain timer
            LifeTimer(-m_fDecayRate);
            m_bIsRegening = false;
        }
        else
        {
            // Regen timer
            LifeTimer(m_fRegenRate);
            m_bIsRegening = true;
        }
        if (_lifting) // Check if button is being pressed.
        {
            if (!m_bIsLifting) // Check if currently lifting.
            {
                if (inRange && m_bGrounded) 
                {
                    m_bIsLifting = true; // Lift boulder.
                }
            }
            else
            {
                m_bIsLifting = false; // Drop boulder.
            }
        }

        if (m_bIsLifting) // While lifting
        {
            // Force boulder transformation
            m_Boulder.transform.position += m_fBoulderLerpSpeed * Time.deltaTime * (m_BoulderAnchor.position - m_Boulder.transform.position);
            m_Boulder.transform.rotation = m_BoulderAnchor.rotation;
            // Set boulder velocity to zero.
            m_Boulder.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            //Physics2D.IgnoreLayerCollision(11, 12, true);
        }
        else
        {
            //Physics2D.IgnoreLayerCollision(11, 12, false);
        }

    }

    private void LifeTimer(float _changeModifier)
    {
        m_fLife += _changeModifier * Time.deltaTime;
        if (m_fLife > 100.0f)
        {
            m_fLife = 100.0f;
        }
        else if (m_fLife < 0.0f)
        {
            m_fLife = 0.0f;
        }
        if (GameManager.instance != null)
        {
            GameManager.instance.SetLife(m_fLife);
        }
    }


    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        playerSprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -playerSprite.transform.rotation.z);

    }

    public void ReleaseBoulder()
    {
        m_bIsLifting = false;
    }

}
