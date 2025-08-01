﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// William de Beer
/// </summary>
public class PlayerController : MonoBehaviour
{
    public ParticleSystem dust;

    [Header("TEMP SPRITES")]
    public Sprite m_NoCarry;
    public Sprite m_Carry;

    [Header("Attached")]
    public GameObject playerSprite;

    [Header("Mobility Test")]
    public int m_iAirJumps = 1;
    public  int m_iJumpsLeft = 1;
    public float m_fGrapplePull = 5.0f;

    [Header("Movement Values")]
    public float m_fJumpForce = 12.0f;
    public float m_fRunSpeed = 10.0f;
    public float m_fAirSpeed = 10.0f;
    public float m_fCarrySpeed = 1.0f;
    private float m_fMovementSmooth = 0.1f;

    [Header("Jump Forgiveness")]
    public float m_fJumpTimer = 0.3f;
    float m_fJumpCooldown = 0.15f;

    public float m_fForgiveTimer = 0.2f;
    float m_fJumpForgiveTime = 0.14f;
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
    public float m_fBoulderLerpSpeed = 5.0f;
    public float m_fLiftRadius = 1.5f;
    public GameObject m_Boulder;
    public Transform m_BoulderAnchor;
    public bool m_bIsLifting = false;

    private Rigidbody2D m_Rigidbody;

    public bool m_IsMoving = false;
    private bool m_FacingRight = false;
    private Vector3 m_Velocity = Vector3.zero;
    private float m_eulerZVelocity = 0.0f;
    private float m_fRotMovementSmooth = 0.25f;

    public GameObject director;
    public GameObject directorThrowing;
    private Animator controller;

    private ChangeModel Animated;

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
        Animated = GetComponent<ChangeModel>();
        Animated.NotCarrying();

        // Get m_fLifeTetherRadius from boulder
        m_fLifeTetherRadius = m_Boulder.GetComponent<Boulder>().radius;

        // Warning
        if (m_fForgiveTimer > m_fJumpTimer)
        {
            Debug.LogError("Having the jumping forgiveness cooldown greater than the jump cooldown timer will create issues with jumping.\n - William de Beer");
        }
    }
    private void Update()
    {
        // Animations
        Animated.Grounded(m_bGrounded);
        Animated.Walking(m_IsMoving);

        if (m_bIsLifting) // Throw arrow
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = Camera.main.transform.position.z * -1; //distance of the plane from the camera
            Vector3 temp = Camera.main.ScreenToWorldPoint(screenPoint);
            
            directorThrowing.transform.up = (temp - m_Boulder.transform.position).normalized;
            Vector3 angles = directorThrowing.transform.rotation.eulerAngles;
            directorThrowing.transform.rotation = Quaternion.Euler(0, 0, angles.z);
            directorThrowing.SetActive(true);
        }
        else // Find boulder arrow
        {
            directorThrowing.SetActive(false);
            SetDirection((m_Boulder.transform.position - transform.position).normalized);
            director.SetActive(!m_bIsRegening);
        }
        

    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_bGrounded;
        m_fJumpTimer += Time.fixedDeltaTime;
        if (m_fJumpTimer > m_fJumpCooldown)
        {
            m_fJumpTimer = m_fJumpCooldown;
        }

        m_bCanJump = false;
        Quaternion newRotation = Quaternion.identity;
        // Ground check
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.5f, m_GroundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject) // If found ground near ground check
            {
                m_bGrounded = true; // Set grounded to true.
                m_bCanJump = true;
                m_fForgiveTimer = 0;
                m_iJumpsLeft = m_iAirJumps;

                if (!wasGrounded && m_Rigidbody.linearVelocity.y < 0)
                {
                    // Hi Callan
                }
                break;
            }
        }

        if (m_bGrounded && colliders.Length == 0) // Jump forgiveness
        {
            m_fForgiveTimer += Time.fixedDeltaTime;

            if (m_fJumpForgiveTime <= m_fForgiveTimer)
            {
                m_bGrounded = false;
            }
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(m_GroundCheck.position, transform.TransformDirection(Vector3.down), 1.0f, m_GroundMask);

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

        if (m_bIsLifting) // While lifting
        {
            // Force boulder transformation
            m_Boulder.transform.position += m_fBoulderLerpSpeed * Time.fixedDeltaTime * (m_BoulderAnchor.position - m_Boulder.transform.position);
            m_Boulder.transform.rotation = m_BoulderAnchor.rotation;

            // Set boulder velocity to zero.
            m_Boulder.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
            m_Boulder.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
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
                m_IsMoving = false;
            }
            else
            {
                m_IsMoving = true;
            }
        }

        if (_jump && (m_fJumpTimer >= m_fJumpCooldown) && ((m_iJumpsLeft > 0 && m_iAirJumps != 0) || (m_iAirJumps == 0 && m_bGrounded))) // Check for jump input and if have enough jumps left.
        {
            float jumpMultiplier = 1.0f;
            if (!m_bGrounded)
                jumpMultiplier = 0.8f; // Air jump weaker

            if (!m_bGrounded && !m_bCanJump) // Air jump
            {
                --m_iJumpsLeft;
            }

            m_fJumpTimer = 0.0f;
            m_bIsLifting = false; // Drop rock
            m_fForgiveTimer = m_fJumpForgiveTime;
            m_bGrounded = false; // Apply jump.

            //making the jump animation
            Animated.Jump();
            Animated.NotCarrying();

            m_Rigidbody.linearVelocity = new Vector2(m_Rigidbody.linearVelocity.x, m_fJumpForce * jumpMultiplier);
            CreateDust();
        }

        if (m_bIsLifting) // Check if lifting.
        {
            speed = m_fCarrySpeed; // Set speed to the carry speed.
        }

        // Set target velocity
        Vector3 targetVelocity = new Vector2(_move * speed, m_Rigidbody.linearVelocity.y);

        // Smoothly set to target velocity.
        m_Rigidbody.linearVelocity = Vector3.SmoothDamp(m_Rigidbody.linearVelocity, targetVelocity, ref m_Velocity, m_fMovementSmooth);

        // If falling
        if (m_bCanJump)
        {
            m_Rigidbody.gravityScale = 0.3f;
        }
        else if (m_Rigidbody.linearVelocity.y < 0) // Increase gravity
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
                    Animated.Carrying();
                    m_bIsLifting = true; // Lift boulder.
                }
            }
            else
            {
                Animated.NotCarrying();
                m_bIsLifting = false; // Drop boulder.
            }
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
            //GameManager.instance.SetLife(m_fLife);
        }
    }


    private void Flip()
    {
        CreateDust();
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        playerSprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -playerSprite.transform.rotation.z);

    }

    public void ReleaseBoulder()
    {
        m_bIsLifting = false;

        Animated.Throw();
    }

    public void SetDirection(Vector3 dir)
    {
        director.transform.up = dir;
    }

    void CreateDust() {
        dust.Play();
        
    }

}
