using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawns : MonoBehaviour
{
    public CharacterController controller;

    public Transform spawn;
    public Transform check1;
    public Transform check2;

    public Transform groundCheck;


    public LayerMask checkPad1;
    public LayerMask checkPad2;

    public float groundDistance = 0.4f;
    public float deathPoint = -1f;



    private Transform lastCheck;



    // Start is called before the first frame update
    void Start()
    {
        lastCheck = spawn;
    }

    // Update is called once per frame
    void Update()
        {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, checkPad1))
        {
            lastCheck = check1;
        }
        if (Physics.CheckSphere(groundCheck.position, groundDistance, checkPad2))
        {
            lastCheck = check2;
        }

        if(transform.position.y < deathPoint)
        {
            controller.enabled = false;
            transform.position = lastCheck.position;
            controller.enabled = true;

        }


    }
}
