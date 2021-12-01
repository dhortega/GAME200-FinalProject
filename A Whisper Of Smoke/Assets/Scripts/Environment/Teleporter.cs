using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject Player;
    public CinemachineVirtualCamera playerCamera;

    private AudioManager am;
    private void OnEnable()
    {
        if (am == null)
        {
            am = FindObjectOfType<AudioManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null) {
            if (am == null)
            {
                Debug.Log("AudioManager is missing! Cannot play sounds.");
            }
            else
            {
                am.Play("Teleport");
            }

            Vector3 initialPosition = Player.transform.position;

            Player.transform.position = teleportTarget.transform.position;
            playerCamera.OnTargetObjectWarped(Player.transform, initialPosition - teleportTarget.transform.position);
            playerCamera.PreviousStateIsValid = false;
        }
    }
}
