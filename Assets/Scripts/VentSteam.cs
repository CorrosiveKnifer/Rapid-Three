using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rachael work
/// </summary>
public class VentSteam : MonoBehaviour
{
    public LayerMask m_PlayerMask;
    public float Thrust = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.8f, m_PlayerMask))
        {
            Collider2D colliders = Physics2D.OverlapCircle(transform.position, 0.8f, m_PlayerMask);
            colliders.GetComponent<Rigidbody2D>().AddForce(transform.up* Thrust, ForceMode2D.Impulse);
            Debug.Log("going up");
        }

    }
}
