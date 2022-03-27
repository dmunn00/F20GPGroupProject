using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGlider : MonoBehaviour
{

    [Header("Glider Pickup Settings")]
    public GameObject pistolObject;
    private PlayerMovement pistolScript;
    public Renderer pistol1;
    public Renderer pistol2;
    public Renderer pistol3;
    public bool pickedUp = false;
    public Transform pistolCheck;
    public LayerMask Player;
    // Start is called before the first frame update
    void Start()
    {
        pistol1 = pistol1.GetComponent<Renderer>();
        pistol2 = pistol2.GetComponent<Renderer>();
        pistol3 = pistol3.GetComponent<Renderer>();
        pistolScript = pistolObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        pickedUp = Physics.CheckSphere(pistolCheck.position, 0.4f, Player);
        if(pickedUp){
            pistol1.enabled = false;
            pistol2.enabled = false;
            pistol3.enabled = false;
            //pistolScript.pickUpRifle();
        }
        transform.parent.Rotate(Vector3.up * (40f * Time.deltaTime));
    }
}
