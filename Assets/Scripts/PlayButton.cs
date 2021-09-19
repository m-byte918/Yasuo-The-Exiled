using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public AudioSource voiceLine;

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonUp(0))
        {
            voiceLine.Play();

            StartCoroutine(WaitForSceneLoad());
        }
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(1);
    }
}
