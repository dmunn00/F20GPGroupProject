using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRifle : MonoBehaviour
{
    [Header("Gun Pickup Settings")]
    public GameObject pistolObject;
    private Gun pistolScript;
    public Renderer pistol;
    public bool pickedUp = false;
    public Transform pistolCheck;
    public LayerMask Player;

    // Start is called before the first frame update
    void Start()
    {   
        pistol = pistol.GetComponent<Renderer>();
        pistolScript = pistolObject.GetComponent<Gun>();
    }


    // Update is called once per frame
    void Update()
    {
        pickedUp = Physics.CheckSphere(pistolCheck.position, 0.4f, Player);
        if(pickedUp){
            pistol.enabled = false;
            pistolScript.pickUpRifle();
        }

        transform.Rotate(Vector3.up * (40f * Time.deltaTime));
    }
}