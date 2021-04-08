using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        bool lifting = Input.GetButtonDown("Fire3");

        Debug.Log(move);

        playerController.Move(move, jump);
        playerController.Lift(lifting);
    }
}
