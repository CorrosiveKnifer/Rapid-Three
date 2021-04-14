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

    public List<GameObject> gameObjects;
    private float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        maxDistance = GetComponent<Collider2D>().bounds.size.y;
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
        foreach (var item in gameObjects)
        {
            float distance = Vector3.Distance(transform.parent.position, item.transform.position);
            float ratio = Mathf.Clamp(1.0f - distance / maxDistance, 0.0f, 1.0f);

            if(item.layer == LayerMask.NameToLayer("Boulder"))
            {
                item.GetComponent<Rigidbody2D>().AddForce(Ventlocation.up * BoulderThrust * ratio);
            }
            else if (item.layer == LayerMask.NameToLayer("Player"))
            {
                item.GetComponent<Rigidbody2D>().AddForce(Ventlocation.up * PlayerThrust * ratio);
                item.GetComponent<PlayerController>().ReleaseBoulder();
            }
        }
        //if (Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_PlayerMask))
        //{
        //    Collider2D colliders = Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_PlayerMask);
        //    colliders.GetComponent<Rigidbody2D>().AddForce(Ventlocation.up * PlayerThrust, ForceMode2D.Impulse);
        //    colliders.GetComponent<PlayerController>().ReleaseBoulder();
        //    Debug.Log("going up");
        //}
        //if (Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_BoulderMask))
        //{
        //    Collider2D colliders = Physics2D.OverlapCircle(Ventlocation.position, 0.8f, m_BoulderMask);
        //    colliders.GetComponent<Rigidbody2D>().AddForce(Ventlocation.up * BoulderThrust, ForceMode2D.Impulse);
        //    Debug.Log("going up");
        //}
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        gameObjects.Add(collision.gameObject);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        gameObjects.Remove(collision.gameObject);
    }
}
