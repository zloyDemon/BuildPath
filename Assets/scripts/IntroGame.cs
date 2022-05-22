using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroGame : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        var operation = SceneManager.LoadSceneAsync(1);
        while (!operation.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
