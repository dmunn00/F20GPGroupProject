using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTimer : MonoBehaviour
{
  public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Finished");
            GameTimer.finished();
        }
    }
}
