using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 200.0f;
    [SerializeField] private bool isFacingLeft = false;
    [SerializeField] private Animator anim;
    private Rigidbody rb;
    private bool canJump = false;

    private bool askedToJump = false;
    private bool askedToUpdateMovement = false;
    private Vector3 updateToThisVelocity;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        #region PHYSICS-BASED HORIZONTAL MOVEMENT
        bool playerXInput = Input.GetButton("Horizontal");
        if (playerXInput)
        {
            float xAxisValue = Input.GetAxis("Horizontal");
            //rb.velocity = new Vector3(speed * xAxisValue, rb.velocity.y, rb.velocity.z);
            updateToThisVelocity = new Vector3(speed * xAxisValue, rb.velocity.y, rb.velocity.z);
            askedToUpdateMovement = true;
            if (xAxisValue < 0 && !isFacingLeft)
            {
                isFacingLeft = true;
                anim.SetBool("isIdle", true);
            }
            else if (xAxisValue > 0 && isFacingLeft)
            {
                isFacingLeft = false;
                anim.SetBool("isIdle", true);
            }
            else {
                if (anim.GetBool("isIdle"))
                    anim.SetBool("isIdle", false);
            }
        }
        if (!playerXInput && rb.velocity.x != 0.0f && !askedToUpdateMovement)
        {
            //rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z); 
            updateToThisVelocity =  new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
            askedToUpdateMovement = true;
            if (!anim.GetBool("isIdle"))
            {
                anim.SetBool("isIdle", true);
            }
        }
        #endregion

        #region PHYSICS-BASED VERTICAL MOVEMENT (single jump)
        bool playerYInput = Input.GetButtonDown("Jump");
        if (playerYInput && canJump) {
            canJump = false;
            askedToJump = true;
            anim.SetBool("isMidair", true);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (askedToJump) {
            rb.AddForce(Vector3.up * jumpForce);
            askedToJump = false;
        }
        if (askedToUpdateMovement) {
            rb.velocity = updateToThisVelocity;
            askedToUpdateMovement = false;
        }
    }

    public bool IsFacingLeft() {
        return isFacingLeft;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") && !canJump) {
            anim.SetBool("isMidair", false);
            canJump = true;
        }
    }
}
