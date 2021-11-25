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
    // Update is called once per frame
    private void OnEnable() {
        startCastTimer = Time.time;
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
        // NOTE: ONCE THE PARTICLE SYSTEM'S TIME RUNS OUT, IT WILL DISABLE ITSELF. 
        if (other.gameObject.CompareTag("Purifiable"))
        {
            if (ps == null)
            {
                ps = other.gameObject.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    Debug.Log("Starting purification process!");
                    if(!playerMovement.GetAnimator().GetBool("isPurifying"))
                        playerMovement.GetAnimator().SetBool("isPurifying", true);
                    isPurifying = true;
                    psmm = ps.main;
                    ps.Pause();
                    psmm.duration = fadeoutDuration;
                    ps.Play();
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
                else
                {
                    Debug.Log("There were no particle systems found on this Purifiable object. Was this intentional?");
                }
            }
        }
    }
}
