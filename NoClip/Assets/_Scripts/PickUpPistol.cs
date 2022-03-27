using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPistol : MonoBehaviour
{
    [Header("Gun Pickup Settings")]
    public GameObject PlayerObject;
    private Gun pistolScript;
    public Renderer pistolBody;
    public Renderer pistolHammer;
    public Renderer pistolCasingExit;
    public Renderer pistolSlider;
    public bool pickedUp = false;
    public Transform pistolCheck;
    public LayerMask Player;

    // Start is called before the first frame update
    void Start()
    {   
        pistolBody = pistolBody.GetComponent<Renderer>();
        pistolHammer = pistolHammer.GetComponent<Renderer>();
        pistolCasingExit = pistolCasingExit.GetComponent<Renderer>();
        pistolSlider = pistolSlider.GetComponent<Renderer>();
        pistolScript = PlayerObject.GetComponent<Gun>();
    }


    // Update is called once per frame
    void Update()
    {
        pickedUp = Physics.CheckSphere(pistolCheck.position, 0.4f, Player);
        if(pickedUp){
            pistolBody.enabled = false;
            pistolHammer.enabled = false;
            pistolCasingExit.enabled = false;
            pistolSlider.enabled = false;
            pistolScript.pickUpPistol();
        }

        transform.Rotate(Vector3.up * (40f * Time.deltaTime));
    }
}
