using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private int numberOfItemsForWin = 2;
    [SerializeField] private CinemachineVirtualCamera playerCmvc;
    [SerializeField] private CinemachineVirtualCamera objectCmvc;
    [SerializeField] private static bool playerAllowedToMove = true;
    [Header("Scene Transition Variables")]
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private int winSceneBuildIndex = 2;
    [SerializeField] private int loseSceneBuildIndex = 3;

    private int numberOfItemsPurified = 0;

    private Queue<IEnumerator> captionQueue;
    private IEnumerator activeCaptionCoroutine;
    private bool captionCoroutineActive;

    private GameObject currentlyCenteredGO;
    private GameObject objectToCenterOn;
    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        captionQueue = new Queue<IEnumerator>();
        activeCaptionCoroutine = null;
        currentlyCenteredGO = playerCmvc.Follow.gameObject;
        playerObject = currentlyCenteredGO;
    }

    // Update is called once per frame
    void Update()
    {
        if (!captionCoroutineActive)
        {
            if (captionQueue.Count > 0)
            {
                // Debug.Log("Queue count: " + captionQueue.Count);
                activeCaptionCoroutine = captionQueue.Dequeue();
                StartCoroutine(activeCaptionCoroutine);
                activeCaptionCoroutine = null;
                numberOfItemsPurified++;
                currentlyCenteredGO = objectToCenterOn;
                objectCmvc.Follow = objectToCenterOn.transform;
                playerCmvc.Priority = 0;
                objectCmvc.Priority = 1;
                playerAllowedToMove = false;
                return;
            }
            if (numberOfItemsPurified >= numberOfItemsForWin)
            {         // If the player has purified all objects w/o dying, switch to win scene
                Debug.Log("The player has purified all items and all subtitles are done running");
                levelLoader.LoadScene(winSceneBuildIndex);
            }
            if (currentlyCenteredGO != playerObject)
            {
                currentlyCenteredGO = playerObject;
                objectCmvc.Priority = 0;
                playerCmvc.Priority = 1;
                playerAllowedToMove = true;
            }
        }
    }

    public void AddCaptionCoroutineToQueue(IEnumerator c, GameObject centeredObj) {
        captionQueue.Enqueue(c);
        objectToCenterOn = centeredObj;
    }

    public void SetCaptionCoroutineActive(bool active) {
        captionCoroutineActive = active;
    }

    public static bool GetPlayerActionsEnabled() {
        return playerAllowedToMove;
    }

    public int GetNumberOfPurifications() {
        return numberOfItemsPurified;
        Debug.Log("Number purified: " + numberOfItemsPurified);
    }
}
