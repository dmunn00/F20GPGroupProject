using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReplayGame : MonoBehaviour
{
    public Image finishOneStar, finishTwoStar, finishThreeStar;
    public Text finishTime;
    // Start is called before the first frame update
    void Start()
    {
     finishOneStar = finishOneStar.GetComponent<Image>();
        finishTwoStar = finishTwoStar.GetComponent<Image>();
        finishThreeStar = finishThreeStar.GetComponent<Image>();
        finishTime = finishTime.GetComponent<Text>();
    }

    public void Restart(){
        finishOneStar.enabled = false;
        finishTwoStar.enabled = false;
        finishThreeStar.enabled = false;
        finishTime.enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
