using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
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
