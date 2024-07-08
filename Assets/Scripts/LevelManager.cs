using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void FirstLevel()
    {
        int levelNumber = 0;
        LoadLevel(levelNumber);
        UIController.Instance.ResetLives();
    }

    public void LastLevel()
    {
        int levelNumber = SceneManager.sceneCountInBuildSettings - 1;
        LoadLevel(levelNumber);
    }
}