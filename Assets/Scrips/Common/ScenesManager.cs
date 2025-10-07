using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneData
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneObject;
#endif
    private int id;
    private string sceneName;

#if UNITY_EDITOR
    public void SetSceneAsset(SceneAsset asset)
    {
        sceneObject = asset;
        if (sceneObject != null)
        {
            sceneName = sceneObject.name;
        }
    }
#endif

    public void SetId(int id) => this.id = id;
    public void SetSceneName(string sceneName) => this.sceneName = sceneName;
    public int GetId() => this.id;
    public string GetSceneName() => this.sceneName;

#if UNITY_EDITOR
    public SceneAsset GetSceneAsset() => this.sceneObject;
#endif
}

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    [SerializeField] private List<SceneData> scenes = new List<SceneData>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

#if UNITY_EDITOR
        // SceneAsset からシーン名を取得してセット
        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].GetSceneAsset() != null)
                scenes[i].SetSceneName(scenes[i].GetSceneAsset().name);
            scenes[i].SetId(i);
        }
#endif
    }

    /// <summary>
    /// シーン切り替え
    /// </summary>
    public void SceneMove(int id)
    {
        foreach (SceneData scene in scenes)
        {
            if (scene.GetId() == id)
            {
                Debug.Log($"Loading scene: {scene.GetSceneName()}");
                SceneManager.LoadScene(scene.GetSceneName());
                return;
            }
        }

        Debug.LogWarning($"Scene with ID {id} not found!");
    }
}
