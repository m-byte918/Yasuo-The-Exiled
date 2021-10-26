using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLoad : MonoBehaviour
{
    public GameObject Fade;

   private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag.Equals("Player"))
        {
            Fade.SetActive(true);
            StartCoroutine(WaitForSceneLoad());
        }
       
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(2);
    }
}
