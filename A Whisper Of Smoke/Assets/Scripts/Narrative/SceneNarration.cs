using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneNarration : MonoBehaviour
{
    [SerializeField] private List<string> quotesToDisplay;
    [SerializeField] private List<float> timeForEachDisplay;
    [SerializeField] private float fadeAnimationTime = 1.0f;
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private LevelLoader levelLoader;
    private Animator anim;
    private Text textbox;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        textbox = gameObject.GetComponent<Text>();
        if (quotesToDisplay.Count == timeForEachDisplay.Count && anim != null && textbox != null)
            StartCoroutine( PlayCutscene());
        else
            Debug.Log("Double check your game object! Something went wrong..");
    }

    IEnumerator PlayCutscene() {
        //yield return new WaitForSeconds(fadeAnimationTime);
        for (int i = 0; i < quotesToDisplay.Count; ++i) {
            textbox.text = quotesToDisplay[i];
            anim.SetBool("isVisible", true);
            yield return new WaitForSeconds(timeForEachDisplay[i]);
            anim.SetBool("isVisible", false);
            yield return new WaitForSeconds(fadeAnimationTime);
        }
        levelLoader.LoadScene(nextSceneIndex);
    }
}
