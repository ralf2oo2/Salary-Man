using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class MySceneManager : MonoBehaviour
{
    private static MySceneManager instance;

    // Lazy initialization
    // With this you wouldn't even need this object in the scene
    public static MySceneManager Instance
    {
        get {
            if (instance) return instance;

            instance = new MySceneManager();

            return instance;
        }
    }

// Usual instant initialization having this object in the scene
private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this);
    }



    public void LoadScene(int index)
    {
        StartCoroutine(loadNextScene(index));
    }

    private IEnumerator loadNextScene(int index)
    {

        // Instead of the index get the actual current scene instance
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        var _async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);


        _async.allowSceneActivation = false;
        yield return new WaitWhile(() => _async.progress < 0.9f);

        _async.allowSceneActivation = true;

        yield return new WaitUntil(() => _async.isDone);

        // You have to do this before otherwise you might again
        // get by index the previous scene 
        var unloadAsync = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
        yield return new WaitUntil(() => unloadAsync.isDone);

        var newScene = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(index);

        UnityEngine.SceneManagement.SceneManager.SetActiveScene(newScene);
    }
}