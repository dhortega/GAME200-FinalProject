using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private bool isFacingLeft = false;
    [SerializeField] private Animator anim;
    private Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool playerXInput = Input.GetButton("Horizontal");
        if (playerXInput)
        {
            float xAxisValue = Input.GetAxis("Horizontal");
            if (anim.GetBool("isIdle"))
                anim.SetBool("isIdle", false);
            rb.velocity = new Vector3(speed * xAxisValue, 0.0f, 0.0f);
            if (xAxisValue < 0 && !isFacingLeft)
            {
                isFacingLeft = true;
            }
            else if (xAxisValue > 0 && isFacingLeft)
            {
                isFacingLeft = false;
            }
        }
        else if (!playerXInput && rb.velocity.x != 0.0f)
        {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
            if (!anim.GetBool("isIdle")) {
                anim.SetBool("isIdle", true);
            }
        }
    }

    public bool IsFacingLeft() {
        return isFacingLeft;
    }
}
