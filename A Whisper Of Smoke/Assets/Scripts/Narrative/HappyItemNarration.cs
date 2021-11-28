using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappyItemNarration : MonoBehaviour
{
    [SerializeField] private GameHandler gh;

    [SerializeField] private List<string> quotesToDisplay;
    [SerializeField] private List<float> timeForEachDisplay;
    [SerializeField] private float fadeAnimationTime = 1.0f;

    [SerializeField] private GameObject smokeCageParticleSystem;
    [SerializeField] private Animator anim;
    [SerializeField] private Text textbox;
    private bool alreadyPlayed = false;

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
