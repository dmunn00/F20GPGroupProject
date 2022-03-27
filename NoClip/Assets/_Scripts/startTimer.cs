using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startTimer : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Started");
            GameTimer.start();
        }
    }
}
