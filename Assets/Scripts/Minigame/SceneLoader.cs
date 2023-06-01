using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [HideInInspector] public int SelectedSceneIndex;

    private AsyncOperation async;

    private void OnMouseDown()
    {
        print("Mouse down on" + gameObject.name);
        StartCoroutine(loadScene());
    }

    private IEnumerator loadScene()
    {
        async = SceneManager.LoadSceneAsync(SelectedSceneIndex);
        async.allowSceneActivation = true;

        while (!async.isDone)
        {
            print(async.progress + "");
            yield return null;
        }

    }
}

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
        if (GUILayout.Button("Current SceneToLoad Index: "+ sceneLoader.SelectedSceneIndex))
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