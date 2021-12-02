using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurification : MonoBehaviour
{
    [SerializeField] private float fadeoutDuration = 0.1f;
    [SerializeField] private float castDuration = 0.5f;
    [SerializeField] private PlayerMovement playerMovement;
    
    private float startCastTimer = 0.0f;
    private ParticleSystem ps;
    private ParticleSystem.MainModule psmm;
    private bool isPurifying = false;

    private AudioManager am;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void OnEnable() {
        startCastTimer = Time.time;
        if (am == null)
        {
            am = FindObjectOfType<AudioManager>();
        } 
    }
    void Update()
    {
        if (ps != null) {
            if (!ps.gameObject.activeInHierarchy)
            {
                playerMovement.GetAnimator().SetBool("isPurifying", false);
                ps = null;
                isPurifying = false;
                this.gameObject.SetActive(false);
            }
            else if (ps.gameObject.activeInHierarchy && ps.time <= 0.0f) {
                ps.gameObject.SetActive(false);
            }
        }
        else if (Time.time - startCastTimer >= castDuration && this.gameObject.activeInHierarchy) {
            this.gameObject.SetActive(false);
        }
    }

    public bool IsPurifying() {
        return isPurifying;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.tag);
        // NOTE: ONCE THE PARTICLE SYSTEM'S TIME RUNS OUT, IT WILL DISABLE ITSELF. 
        if (other.gameObject.CompareTag("Purifiable"))                                  // If collided with purifiable object...
        {
            if (playerMovement.GetNumOfPurificationsAvalible() > 0) {                   // If there is tummy space for more purifications...
                am.Play("Purify");
                if (ps == null) {
                    ps = other.gameObject.GetComponent<ParticleSystem>();
                    if (ps != null) {
                        Debug.Log("Starting purification process!");
                        playerMovement.DecreaseNumOfPurifications();
                        
                        if (!playerMovement.GetAnimator().GetBool("isPurifying"))
                            playerMovement.GetAnimator().SetBool("isPurifying", true);
                        isPurifying = true;
                        for (int i = 0; i < ps.gameObject.transform.childCount; i++) {
                            GameObject childGO = ps.gameObject.transform.GetChild(i).gameObject;
                            if (childGO != null) {
                                if (childGO.activeInHierarchy)
                                    childGO.SetActive(false);
                            }
                        }
                        psmm = ps.main;
                        ps.Pause();
                        psmm.duration = fadeoutDuration;
                        ps.Play();
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    }
                    else {
                        Debug.Log("There were no particle systems found on this Purifiable object. Was this intentional?");
                    }
                }
            }
            else  {                                                                      // Else if the Deater is too full...
                Debug.Log("I'm too full! Can't purify any more smoke :(");
                // PLAY BURP SOUND HERE
                if (am == null)
                {
                    Debug.Log("AudioManager is missing! Cannot play sounds.");
                }
                else
                {
                    am.Play("Burp");
                }
            }
        }
    }

    public bool GetIsPurifying() {
        return isPurifying;
    }
}
