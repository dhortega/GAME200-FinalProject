using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPurification : MonoBehaviour
{
    bool vacuumTurnedOn = false;
    ParticleSystem ps;
    // Update is called once per frame
    void Update()
    {
        if (ps != null && vacuumTurnedOn) {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ps.time -= Time.deltaTime;
            if (ps.time <= 0) {
                ps.gameObject.SetActive(false);
                ps = null;
                vacuumTurnedOn = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Purifiable")) {
            Debug.Log("This can be purified!");
            if (!vacuumTurnedOn) {
                vacuumTurnedOn = true;
                ps = other.gameObject.GetComponent<ParticleSystem>();
            }
        }
    }
}
