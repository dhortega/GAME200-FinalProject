using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.0f;
    private Animator anim;

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    public void LoadScene(int sceneIndex) {
        StartCoroutine(LoadSceneEnumerator(sceneIndex));
    }

    IEnumerator LoadSceneEnumerator(int sceneIndex) {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneIndex);
    }
}
