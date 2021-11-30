using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject Player;
    public CinemachineVirtualCamera playerCamera;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 initialPosition = Player.transform.position;
        
        Player.transform.position = teleportTarget.transform.position;
        playerCamera.OnTargetObjectWarped(Player.transform, initialPosition - teleportTarget.transform.position);
        playerCamera.PreviousStateIsValid = false;
    }
}
