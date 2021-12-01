using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyItemNarration : MonoBehaviour
{
    [SerializeField] private GameHandler gh;
    [SerializeField] private Animator bgAnim;

    [SerializeField] private List<string> quotesToDisplay;
    [SerializeField] private List<float> timeForEachDisplay;
    [SerializeField] private float fadeAnimationTime = 1.0f;

    [SerializeField] private GameObject smokeCageParticleSystem;
    [SerializeField] private Animator anim;
    [SerializeField] private Text textbox;
    [SerializeField] private bool isScrapbook;
    private bool alreadyPlayed = false;

    private AudioManager am;

    private void OnEnable()
    {
        if (am == null) {
            am = FindObjectOfType<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!alreadyPlayed && !smokeCageParticleSystem.GetComponent<ParticleSystem>().isEmitting) {
            gh.AddCaptionCoroutineToQueue(PlayQuotes(), gameObject.transform.parent.gameObject);
            alreadyPlayed = true;
        }
            
    }

    IEnumerator PlayQuotes()
    {
        gh.SetCaptionCoroutineActive(true);
        //yield return new WaitForSeconds(fadeAnimationTime);
        if (am == null) {
            Debug.Log("AudioManager is missing! Cannot play sounds.");
        }
        else {
            if (isScrapbook)
                am.Play("PurifyScrapbook");
            else
                am.Play("PurifyFireExtinguisher");
        }
        if (gh.GetNumberOfPurifications() == 0)
            bgAnim.SetTrigger("fade1");
        else if (gh.GetNumberOfPurifications() == 1)
            bgAnim.SetTrigger("fade2");

        for (int i = 0; i < quotesToDisplay.Count; ++i)
        {
            textbox.text = quotesToDisplay[i];
            anim.SetBool("isVisible", true);
            yield return new WaitForSeconds(timeForEachDisplay[i]);
            anim.SetBool("isVisible", false);
            yield return new WaitForSeconds(fadeAnimationTime);
        }
        gh.SetCaptionCoroutineActive(false);
    }
}
