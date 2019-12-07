using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : CustomAudioSource
{
    private void Awake()
    {
        loop = false;
        playOnAwake = false;
        randomize = true;
        noDuplicate = false;
        playRandomly = false;

        base.Awake();
    }

    public void LoadLevelByIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        StartCoroutine(playSound(0));
    }

    public void LoadLevelByName(string levelName)
    {
        SceneManager.LoadScene(levelName);
        StartCoroutine(playSound(0));
    }

    public void CloseGame()
    {
        Application.Quit();
        StartCoroutine(playSound(0));
    }
}
