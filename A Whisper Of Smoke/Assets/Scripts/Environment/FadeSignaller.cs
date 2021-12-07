using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSignaller : MonoBehaviour
{
    // Currently triggers from a mouse click, check if this permitted - 11/30/2021

    // Objects to iterate over that have the FadeDisable script
    public GameObject[] targetObjects;
    
    // Upon entering the trigger where this script is attached to, call their respective FadeOut from FadeDisable
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null) {
            for (int i = 0; i < targetObjects.Length; i++)
            {
                Debug.Log("Entered Fade Collision");
                StartCoroutine(targetObjects[i].GetComponent<FadeDisable>().FadeOut());
            }
        }
        
    }
}
