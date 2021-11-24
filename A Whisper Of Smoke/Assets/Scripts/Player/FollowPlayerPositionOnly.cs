using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerPositionOnly : MonoBehaviour
{
    private bool isFacingLeft = false;

    [SerializeField] private GameObject player;
    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (player.GetComponent<PlayerMovement>().IsFacingLeft() && !isFacingLeft|| !player.GetComponent<PlayerMovement>().IsFacingLeft() && isFacingLeft) {
            isFacingLeft = !isFacingLeft;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y * -1, transform.localEulerAngles.z);
        }
        transform.position = player.transform.position;
    }

    public Animator GetAnimator() {
        return anim;
    }
}
