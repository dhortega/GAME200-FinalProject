using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public GameObject fogOfWarPlane;
    public Transform playerTransform;
    public LayerMask fogLayer;
    public float radius;
    private float radiusSqr { get { return radius + radius; } }

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // Ray(origin, direction)
        Ray ray = new Ray(transform.position, playerTransform.position - transform.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, fogLayer, QueryTriggerInteraction.Collide)) //1000, Vector3.Distance(transform.position,playerTransform.position)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = fogOfWarPlane.transform.TransformPoint(vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < radiusSqr)
                {
                    float alpha = Mathf.Min(colors[i].a, dist / radiusSqr);
                    Debug.Log("alpha: " + alpha);
                    colors[i].a = alpha;
                    Debug.Log("colors[i].a: " + colors[i].a);
                }
            }
            UpdateColor();
        }
    }

    private void Initialize()
    {
        mesh = fogOfWarPlane.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
        for(int i = 0; i < colors.Length; i++) 
        {
            colors[i] = Color.black;
        }
        UpdateColor();
    }

    void UpdateColor()
    {
        mesh.colors = colors;
    }
}
