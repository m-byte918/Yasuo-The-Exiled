using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource firstPart;
    public AudioSource secondPart;
    

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        double introDuration = (double)firstPart.clip.samples / firstPart.clip.frequency;
        double startTime = AudioSettings.dspTime - 0.3;
        firstPart.PlayScheduled(startTime);
        secondPart.PlayScheduled(startTime + introDuration);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
