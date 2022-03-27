using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    public Text timerText;
    private static float startTime;
    public static bool fin;
    
    public float seconds;
    public int minutes;

    public PlayerMovement moveScript;
    public MouseLook lookScript;

    public Image finishOneStar, finishTwoStar, finishThreeStar;
    public Text finishTime;
    // Start is called before the first frame update
    void Start()
    {
        finishOneStar = finishOneStar.GetComponent<Image>();
        finishTwoStar = finishTwoStar.GetComponent<Image>();
        finishThreeStar = finishThreeStar.GetComponent<Image>();
        finishTime = finishTime.GetComponent<Text>();
        moveScript = moveScript.GetComponent<PlayerMovement>();
        lookScript = lookScript.GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fin == false)
        {
            float t = Time.time - startTime;

            minutes = ((int) t/60);
            seconds = (t % 60);

            timerText.text = minutes.ToString() + ":" + seconds.ToString("f1");
        }
        if(fin == true){
            lookScript.finish();
            moveScript.finish();
            finishTime.enabled = true;
            finishTime.text = minutes.ToString() + "mins " + seconds.ToString("f1") + "secs";
            if(minutes < 1){
                finishThreeStar.enabled = true;
            }
            if(minutes >= 1 && minutes < 2){
                finishTwoStar.enabled = true;
            }
            if(minutes >= 2){
                finishOneStar.enabled = true;
            }
        }

    }

    public void hitTarget(){
        startTime = startTime + 4f;
    }

    public void missTarget(){
        startTime = startTime - 4f;
    }

    public static void start()
    {
        fin = false;
        startTime = Time.time;
    }
    public static void finished()
    {
        fin = true;
    }
}
