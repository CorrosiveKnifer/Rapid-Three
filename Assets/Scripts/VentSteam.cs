using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rachael work
/// </summary>
public class VentSteam : MonoBehaviour
{
    public LayerMask m_PlayerMask;
    public LayerMask m_BoulderMask;
    public float PlayerThrust = 1.0f;
    public float BoulderThrust = 1.0f;
    public Transform Ventlocation;
    bool SteamRisen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SteamRisen)
        {
            ForceUP();
        }
    }

    public void SteamStart()
    {
        SteamRisen = true;
    }
    public void SteamEnd()
    {
        SteamRisen = false;
    }
    public void ForceUP()
    {
        if (Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_PlayerMask))
        {
            Collider2D colliders = Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_PlayerMask);
            colliders.GetComponent<Rigidbody2D>().AddForce(Ventlocation.up * PlayerThrust, ForceMode2D.Impulse);
            colliders.GetComponent<PlayerController>().ReleaseBoulder();
            Debug.Log("going up");
        }
        if (Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_BoulderMask))
        {
            Collider2D colliders = Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_BoulderMask);
            colliders.GetComponent<Rigidbody2D>().AddForce(Ventlocation.up * BoulderThrust, ForceMode2D.Impulse);
            Debug.Log("going up");
        }
    }
}
