using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;

    private float xOffset;
    private float yOffset;
    private float zOffset;

    private void Start()
    {
        xOffset = transform.position.x - player.position.x;
        yOffset = transform.position.y - player.position.y;
        zOffset = transform.position.z - player.position.z;
    }

    private void Update()
    {
        gameObject.transform.position = new Vector3(
            player.position.x + xOffset,
            player.position.y + yOffset,
            player.position.z + zOffset);
    }
    //private void LateUpdate()
    //{
    //    Vector3 newPosition = player.position;
    //    //newPosition.y = transform.position.y;
    //    transform.position = newPosition;
    //}

}
