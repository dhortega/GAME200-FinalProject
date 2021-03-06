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
    [SerializeField] private Animator UIanim;
    [SerializeField] private Text textbox;
    [SerializeField] private bool isScrapbook;

    [SerializeField] private bool isTutorial = false;
    private bool alreadyPlayed = false;

    private AudioManager am;
    private Animator objAnimator;
    private void OnEnable()
    {
        if (am == null) {
            am = FindObjectOfType<AudioManager>();
        }
        if (objAnimator == null) {
            objAnimator = this.gameObject.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (am == null)
        {
            am = FindObjectOfType<AudioManager>();
        }
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
        if (gh.GetNumberOfPurifications() == 0) {
            bgAnim.SetTrigger("fade1");
            am.Stop("Fire", 2.0f);
        }
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
        if (objAnimator != null){
            objAnimator.SetTrigger("vanish");
        }
        yield return new WaitForSeconds(1.0f);
        if (UIanim != null)
            UIanim.SetTrigger("collected");
        gh.SetCaptionCoroutineActive(false);
    }
}
