using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private string[] audioToPlayOnAwake;
    [SerializeField] private string[] audioToStopOnExit;
    [SerializeField] private bool allowedToPause = false;
    [SerializeField] private GameObject restartMenu;
    private Animator anim;

    private AudioManager am;

    private void Awake()
    {
        
    }

    private void Start() {
        restartMenu.SetActive(false);
        anim = gameObject.GetComponent<Animator>();
        if (am == null)
        {
            am = FindObjectOfType<AudioManager>();
        }
        foreach (string str in audioToPlayOnAwake) {
            am.Play(str);
        }
    }

    private void Update()
    {
        if (am == null)
        {
            am = FindObjectOfType<AudioManager>();
        }
        if (allowedToPause) {
            if (Input.GetButtonUp("Cancel")) {
                PauseGame();
            }
        }
    }
    public void LoadScene(int sceneIndex) {
        foreach (string str in audioToStopOnExit) {
            am.Stop(str, 0.5f);
        }
        StartCoroutine(LoadSceneEnumerator(sceneIndex));
    }

    IEnumerator LoadSceneEnumerator(int sceneIndex) {
        if (Time.timeScale == 0.0f)
            Time.timeScale = 1.0f;
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(fadeDuration);
        restartMenu.SetActive(false);
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame() {
        Debug.Log("Exiting game.");
        Application.Quit();
    }

    public void PauseGame() {
        restartMenu.SetActive(!restartMenu.activeInHierarchy);
        if (Time.timeScale == 0.0f)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;
    }
}
