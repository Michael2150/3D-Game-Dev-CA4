using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Handle Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
        private set {
            instance = value;
            DontDestroyOnLoad(Instance);
        }
    }
    private void Awake()
    {
        if (GameManager.Instance == null)
            GameManager.Instance = this;
        else
            Destroy(gameObject);
    }

    //Handle Scenes
    private string curLevelName = "";
    public void loadLevel(string levelName)
    {
        if (levelName != curLevelName)
        {
            curLevelName = levelName;
            SceneManager.LoadScene(levelName);
        }
    }
    
    
}
