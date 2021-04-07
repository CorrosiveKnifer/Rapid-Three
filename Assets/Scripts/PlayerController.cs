using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Values")]
    public float m_fJumpForce = 600.0f;
    public float m_fRunSpeed = 10.0f;
    public float m_fAirSpeed = 10.0f;
    private float m_fMovementSmooth = 0.1f;

    [Header("Ground")]
    public LayerMask m_GroundMask;
    public Transform m_GroundCheck;
    public bool m_bGrounded;

    private Rigidbody2D m_Rigidbody;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_bGrounded;
        m_bGrounded = false;
        // Ground check
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.2f, m_GroundMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_bGrounded = true;
                if (!wasGrounded && m_Rigidbody.velocity.y < 0)
                {
                    //float shakeAmount = m_Rigidbody.velocity.y / 10.0f;
                    CameraController.instance.StartShake(0.3f, 0.3f);
                    Debug.Log("Bonk!");
                    // Screen Shake!
                }
                break;
            }
        }
    }

    // Update is called once per frame
    public void Move(float _move, bool _jump)
    {
        float speed = m_fAirSpeed;
        if (m_bGrounded)
        {
            speed = m_fRunSpeed;
            if (_jump)
            {
                m_bGrounded = false;
                m_Rigidbody.AddForce(new Vector2(0.0f, m_fJumpForce));
            }
        }
        Vector3 targetVelocity = new Vector2(_move * speed, m_Rigidbody.velocity.y);

        // Move the character by finding the target velocity
        // And then smoothing it out and applying it to the character
        m_Rigidbody.velocity = Vector3.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_fMovementSmooth);

        if (m_Rigidbody.velocity.y < 0)
        {
            m_Rigidbody.gravityScale = 5.0f;
        }
        else
        {
            m_Rigidbody.gravityScale = 2.0f;
        }

        if (_move < 0 && m_FacingRight)
        {
            Flip();
        }
        else if (_move > 0 && !m_FacingRight)
        {
            Flip();
        }
    }
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
