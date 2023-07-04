using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneLoader : MonoBehaviour
{
    [HideInInspector] public int SelectedSceneIndex;

    private VideoPlayer player;
    private AsyncOperation async;
    private bool starttime = true;

    public void LoadScene()
    {
        StartCoroutine(loadScene());
    }

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
        async = SceneManager.LoadSceneAsync(SelectedSceneIndex, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (!async.isDone && starttime)
        {
            print(async.progress);
            yield return null;
        }
        showScene();
        print("SceneLoadDone");
    }

    private void OnVideoStarted(VideoPlayer source)
    {
        StartCoroutine(CheckVideoFinished());
    }

    private void showScene()
    {
        async.allowSceneActivation = true;
    }

    private void Start()
    {
        if (TryGetComponent(out VideoPlayer videoPlayer))
        {
            player = videoPlayer;
            player.started += OnVideoStarted;
            StartCoroutine(loadScene());
        }
        else
        {
            starttime = false;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SceneLoader))]
class ActivateNPCDialogueEditor : Editor
{
    private SceneLoader sceneLoader;

    public override void OnInspectorGUI()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                scenes.Add(scene.path);
            }
        }
        DrawDefaultInspector();
        if (GUILayout.Button("Current SceneToLoad Index: " + sceneLoader.SelectedSceneIndex))
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < scenes.Count; i++)
            {
                int index = i;
                menu.AddItem(new GUIContent(scenes[i]), false, () =>
                {
                    sceneLoader.SelectedSceneIndex = index;
                    Debug.Log(index);
                });
            }
            menu.ShowAsContext();
        }
    }
}
#endif
