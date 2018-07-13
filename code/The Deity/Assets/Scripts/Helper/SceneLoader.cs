﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private bool loadScene = false;

    [SerializeField]
    private int scene;
    [SerializeField]
    private Text loadingText;
    private Image loadingPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        loadingPanel = loadingText.transform.parent.GetComponent<Image>();
        loadingPanel.enabled = false;
    }

    public void LoadLevel()
    {
        // ...set the loadScene boolean to true to prevent loading a new scene more than once...
        loadScene = true;

        // ...change the instruction text to read "Loading..."
        loadingText.text = "Prepairing your world, please wait...";
        loadingPanel.enabled = true;

        // ...and start a coroutine that will load the desired scene.
        StartCoroutine(LoadNewScene());
    }

    public void LoadLevel(int sceneIdx)
    {
        scene = sceneIdx;

        LoadLevel();
    }

    public void LoadLevel(string sceneName)
    {
        scene = SceneManager.GetSceneByName(sceneName).buildIndex;

        LoadLevel();
    }

    // Updates once per frame
    void Update()
    {
        // If the new scene has started loading...
        if (loadScene == true)
        {
            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(0.3f+Time.time, 1));
        }
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {
        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(3);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }

}