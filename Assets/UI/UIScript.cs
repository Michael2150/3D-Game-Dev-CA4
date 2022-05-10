using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Text TitleText;
    
    //Start
    public void Start()
    {
        //IF this scene is the "GameOverScene" then set the title text to "Game Over"
        if (SceneManager.GetActiveScene().name == "GameOverScene")
        {
            TitleText.text = "You died \n You got " + GameManager.Instance.FiresLit + "/" + GameManager.Instance.FiresToLight + " of the fires lit \n Try again";
        }
    }


    public void StartGame()
    {
        GameManager.Instance.loadLevel("ExternalScene");
    }

    public void GameOver()
    {
        GameManager.Instance.loadLevel("GameOverScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
