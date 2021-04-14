using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeModel : MonoBehaviour
{
    public GameObject PlayerAnim;

    public GameObject PlayerCarryAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Grounded(bool ground)
    {
        PlayerAnim.GetComponent<Animator>().SetBool("Grounded", ground);
        PlayerCarryAnim.GetComponent<Animator>().SetBool("Grounded", ground);
     
    }
    public void Walking(bool walking)
    {
        PlayerAnim.GetComponent<Animator>().SetBool("Walk", walking);
        PlayerCarryAnim.GetComponent<Animator>().SetBool("Walk", walking);

    }
    public void Jump()
    {
        PlayerAnim.GetComponent<Animator>().SetTrigger("Jump");

    }
    public void Throw()
    {
        PlayerCarryAnim.GetComponent<Animator>().SetTrigger("Throw");

    }
    public void Carrying()
    {
        //PlayerCarryAnim.SetActive(true);
        //PlayerAnim.SetActive(false);
        SetGameObjectVisibility(PlayerCarryAnim, true);
       
        SetGameObjectVisibility(PlayerAnim, false);
    }
    public void NotCarrying()
    {
        //PlayerCarryAnim.SetActive(false);
        //PlayerAnim.SetActive(true);

        SetGameObjectVisibility(PlayerCarryAnim, false);
        SetGameObjectVisibility(PlayerAnim, true);
    }
    public void SetGameObjectVisibility(GameObject _object, bool _isVisible)
    {
        SpriteRenderer[] renderers = _object.GetComponentsInChildren<SpriteRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.enabled = _isVisible;
        }
    }
}
