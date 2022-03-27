using System;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public float range = 100f;

    private int currentAmmo = 0;

    private GameTimer timer;

    public Text ammoText;

    public Camera cam;

    public bool canShootPistol = false;
    public bool canShootRifle = false;

    public Renderer pistolBody;
    public Renderer pistolHammer;
    public Renderer pistolCasingExit;
    public Renderer pistolSlider;
    public Renderer rifle;

    public Image fistImage, pistolImage, gliderImage, rifleImage;

    Animator PlayerAnimations;
    void Start()
    {
        //pistol = pistol.GetComponent<Renderer>();
        //rifle = rifle.GetComponent<Renderer>();
        pistolBody = pistolBody.GetComponent<Renderer>();
        pistolHammer = pistolHammer.GetComponent<Renderer>();
        pistolCasingExit = pistolCasingExit.GetComponent<Renderer>();
        pistolSlider = pistolSlider.GetComponent<Renderer>();

        fistImage = fistImage.GetComponent<Image>();
        pistolImage = pistolImage.GetComponent<Image>();
        gliderImage = gliderImage.GetComponent<Image>();
        rifleImage = rifleImage.GetComponent<Image>();

        PlayerAnimations = GetComponent<Animator>();

        ammoText = ammoText.GetComponent<Text>();

        timer = GetComponent<GameTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAmmo == 0){
            canShootPistol = false;
            canShootRifle = false;
        }
        if(canShootPistol == true || canShootRifle == true){
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }
    }

    public void pickUpPistol(){
        ammoText.text = "12";
        currentAmmo = 12;

        canShootPistol = true;
        pistolBody.enabled = true;
        pistolHammer.enabled = true;
        pistolCasingExit.enabled = true;
        pistolSlider.enabled = true;
        PlayerAnimations.SetTrigger("PickUpPistol");
        fistImage.enabled = false;
        pistolImage.enabled = true;
    }

    public void pickUpRifle(){
        ammoText.text = "5";
        currentAmmo = 5;
        canShootRifle = true;
        rifle.enabled = true;
        PlayerAnimations.SetTrigger("PickUpRifle");
        fistImage.enabled = false;
        rifleImage.enabled = true;
    }

    public void DropGuns(){
        ammoText.text = "0";
        currentAmmo = 0;
        canShootRifle = false;
        rifle.enabled = false;
        pistolBody.enabled = false;
        pistolHammer.enabled = false;
        pistolCasingExit.enabled = false;
        pistolSlider.enabled = false;
        PlayerAnimations.SetTrigger("DropGun");
        fistImage.enabled = true;
        pistolImage.enabled = false;
        rifleImage.enabled = false;
    }

    private void Fire()
    {
        PlayerAnimations.SetTrigger("Fire");
        currentAmmo = currentAmmo-1;
        ammoText.text = currentAmmo.ToString();
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.name.Contains("Target")){
                timer.hitTarget();
            }else {
                timer.missTarget();
            }
            
            if(hit.transform == null){
                timer.missTarget();
            }

            Targets target = hit.transform.GetComponent<Targets>();
            if (target != null)
            {
                target.TargetHit();
            }
        }
    }
}
