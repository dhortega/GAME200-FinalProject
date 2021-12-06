using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap1 : MonoBehaviour
{
    public Transform target;

    public Vector2 TransformPosition(Vector3 position)
    {
        Vector3 offset = position - target.position;

        Vector2 newPosition = new Vector2(offset.x, offset.z);

        return newPosition;
    }
}
