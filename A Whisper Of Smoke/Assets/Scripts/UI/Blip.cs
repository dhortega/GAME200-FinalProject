using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blip : MonoBehaviour
{
    public Transform target;
    MiniMap1 map;
    RectTransform localRectTransform;

    void Start()
    {
        map = GetComponentInParent<MiniMap1>();
        localRectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Vector2 newPosition = map.TransformPosition(target.position);

        localRectTransform.localPosition = newPosition;
    }
}
