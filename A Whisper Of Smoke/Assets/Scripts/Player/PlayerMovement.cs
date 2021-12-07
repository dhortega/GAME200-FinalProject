using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement Variables
    [Header("Movement Variables")]
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 200.0f;
    [SerializeField] private bool isFacingLeft = false;
    [SerializeField] private float repulseTime = 0.5f;
    // Purification Variables
    [Header("Purification Variables")]
    [SerializeField] private Text purificationAmountText;
    [SerializeField] private int numberOfPurifiesAvalible = 6;
    [SerializeField] private int amountRecoveredFromCandy = 2;
    [SerializeField] private int turnToGrayAt = 3;
    [SerializeField] private GameObject whiteBun;
    [SerializeField] private GameObject grayBun;
    [SerializeField] private GameObject blackBun;
    private bool purificationAmountChanged = false;
    // Component Variables
    [Header("Component Variables")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject purifier;
    private Rigidbody rb;
    // Bool Checkers
    private bool canJump = false;
    private Vector3 lastPosition;
    // Fixed Update Request Variables
    private bool askedToJump = false;
    private bool askedToUpdateMovement = false;
    private Vector3 updateToThisVelocity;

    private bool repuseRequested = false;
    private float repulseTimer = 0.0f;
    private float[] repulses = { 0.0f, 0.0f, 0.0f };

    private AudioManager am;
    private void OnEnable()
    {
        if (am == null)
        {
            am = FindObjectOfType<AudioManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        canJump = true;
        SwitchToBun(true, false, false);
        lastPosition = gameObject.transform.position;
        purificationAmountText.text = "x" + GetNumOfPurificationsAvalible();
    }

    // Update is called once per frame
    void Update()
    {
        bool playerXInput = Input.GetButton("Horizontal");
        bool playerYInput = Input.GetButtonDown("Jump");
        if (GameHandler.GetPlayerActionsEnabled()) {
            #region PHYSICS-BASED HORIZONTAL MOVEMENT INPUT
            
            if (playerXInput && !anim.GetBool("isPurifying"))
            {
                float xAxisValue = Input.GetAxis("Horizontal");
                //rb.velocity = new Vector3(speed * xAxisValue, rb.velocity.y, rb.velocity.z);
                updateToThisVelocity = new Vector3(speed * xAxisValue, rb.velocity.y, rb.velocity.z);
                askedToUpdateMovement = true;
                if (xAxisValue < 0 && !isFacingLeft)
                {
                    isFacingLeft = true;
                    anim.SetBool("isIdle", true);
                    gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y - 180, gameObject.transform.eulerAngles.z);
                }
                else if (xAxisValue > 0 && isFacingLeft)
                {
                    isFacingLeft = false;
                    anim.SetBool("isIdle", true);
                    gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y + 180, gameObject.transform.eulerAngles.z);
                }
                else
                {
                    if (anim.GetBool("isIdle"))
                        anim.SetBool("isIdle", false);
                }
            }
            if (!playerXInput && rb.velocity.x != 0.0f && !askedToUpdateMovement)
            {
                //rb.velocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z); 
                updateToThisVelocity = new Vector3(0.0f, rb.velocity.y, rb.velocity.z);
                askedToUpdateMovement = true;
                if (!anim.GetBool("isIdle"))
                {
                    anim.SetBool("isIdle", true);
                }
            }
            #endregion

            #region PHYSICS-BASED VERTICAL MOVEMENT INPUT (single jump)
            
            if (playerYInput && canJump && !anim.GetBool("isPurifying"))
            {
                if (am == null)
                {
                    Debug.Log("AudioManager is missing! Cannot play sounds.");
                }
                else
                {
                    am.Play("Jump");
                }
                canJump = false;
                askedToJump = true;
                anim.SetBool("isMidair", true);
            }
            #endregion

            #region PURIFY INPUT
            if (Input.GetButtonDown("Purify") && !purifier.activeInHierarchy)
            {
                purifier.SetActive(true);
            }
            #endregion
        }

        #region Switch Buns based on purification amount
        if (purificationAmountChanged && !anim.GetBool("isPurifying")) {
            if (numberOfPurifiesAvalible > turnToGrayAt && !whiteBun.activeInHierarchy){
                SwitchToBun(true, false, false);
            }
            else if (numberOfPurifiesAvalible <= turnToGrayAt && numberOfPurifiesAvalible > 0 && !grayBun.activeInHierarchy) {
                SwitchToBun(false, true, false);
            }else if(numberOfPurifiesAvalible <= 0 && !blackBun.activeInHierarchy) {
                SwitchToBun(false, false, true);
            }
            purificationAmountChanged = false;
        }
        #endregion

        #region Clean up animation bugs at the end
        if ((!playerXInput && !playerYInput) && gameObject.transform.position == lastPosition) {
            if (!anim.GetBool("isIdle"))
                anim.SetBool("isIdle", true);
        }
        lastPosition = gameObject.transform.position;
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

        if (repuseRequested) {
            if (repulseTimer == 0.0f) {
                repulses[0] = -1.0f * rb.velocity.x / repulseTime;
                if (Mathf.Abs(repulses[0]) < 1.0f && repulses[0] != 0) {
                    repulses[0] = 1.0f * Mathf.Sign(repulses[0]);
                }
                repulses[1] = -1.0f * rb.velocity.y / repulseTime;
                if (Mathf.Abs(repulses[1]) < 1.0f && repulses[1] != 0) {
                    repulses[1] = 1.0f * Mathf.Sign(repulses[1]);
                }
                repulses[2] = -1.0f * rb.velocity.z / repulseTime;
                if (Mathf.Abs(repulses[2]) < 1.0f && repulses[2] != 0) {
                    repulses[2] = 1.0f * Mathf.Sign(repulses[2]);
                }
            }
            repulseTimer += Time.deltaTime;
            rb.velocity -= new Vector3(repulses[0], repulses[1], repulses[2]) * Time.deltaTime;
            if (repulseTimer >= repulseTime) {
                rb.velocity = Vector3.zero;
                repulseTimer = 0.0f;
                repuseRequested = false;
                GameHandler.SetPlayerActionsEnabled(true);
            }
        }
        //
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Candy"))
        {
            // PLAY CHOMP SOUND
            if (am == null)
            {
                Debug.Log("AudioManager is missing! Cannot play sounds.");
            }
            else
            {
                am.Play("Munch");
            }
            other.gameObject.SetActive(false);
            IncreaseNumOfPurifications(amountRecoveredFromCandy);
            Debug.Log("Nom nom nom! I now have " + numberOfPurifiesAvalible + " purifications left!");
            purificationAmountText.text = "x" + GetNumOfPurificationsAvalible();
        }
        else if (other.gameObject.CompareTag("Purifiable")) {
            Debug.Log("PUrified hit");
            if (!purifier.activeInHierarchy) {
                if (GameHandler.GetPlayerActionsEnabled())
                {
                    GameHandler.SetPlayerActionsEnabled(false);
                    rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * 10.0f, Mathf.Sign(rb.velocity.y) * 5.0f, 0.0f) * -1.0f;
                    repuseRequested = true;
                }
            }
            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Purifiable"))
        {
            //GameHandler.SetPlayerActionsEnabled(true);
            Debug.Log("Exited smoke collider");
        }
    }

    public Animator GetAnimator() {
        return anim;
    }

    public int GetNumOfPurificationsAvalible() {
        return numberOfPurifiesAvalible;
    }

    public void DecreaseNumOfPurifications(int dec = 1) {
        numberOfPurifiesAvalible -= dec;
        purificationAmountChanged = true;
        purificationAmountText.text = "x" + GetNumOfPurificationsAvalible();
    }

    public void IncreaseNumOfPurifications(int inc = 1) {
        numberOfPurifiesAvalible += inc;
        purificationAmountChanged = true;
    }

    private void SwitchToBun(bool whiteActive, bool grayActive, bool blackActive) {
        whiteBun.SetActive(whiteActive);
        grayBun.SetActive(grayActive);
        blackBun.SetActive(blackActive);
    }
}
