using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameLoader : MonoBehaviour
{
    private VideoPlayer player;
    bool starttime = true;

    private void Start()
    {
        player = GetComponent<VideoPlayer>();
        StartCoroutine(loadScene());
        StartCoroutine(CheckVideoFinished());
    }

    private AsyncOperation async;

    private IEnumerator CheckVideoFinished()
    {
        while (player.isPlaying)
        {
            yield return null;
        }
        print("VideoDone");
        starttime = false;
    }

    private IEnumerator loadScene()
    {
        async = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (!async.isDone && starttime)
        {
            print(async.progress);
            yield return null;
        }
        showScene();
        print("SceneLoadDone");
    }

    public void showScene()
    {
        async.allowSceneActivation = true;
    }
}
