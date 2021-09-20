using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public AudioSource voiceLine;
    public GameObject Fade;
    public Color startColor;
    public Color mouseOverColor;

    private void OnMouseOver()
    {
        this.GetComponent<MeshRenderer>().material.SetColor("_Color", mouseOverColor);

        if (Input.GetMouseButtonUp(0))
        {
            Fade.SetActive(true);
            voiceLine.Play();
            StartCoroutine(WaitForSceneLoad());
        }
    }

    private void OnMouseExit()
    {
        this.GetComponent<MeshRenderer>().material.SetColor("_Color", startColor);
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(1);
    }
}
