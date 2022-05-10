using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    //Handle Scenes
    private string curLevelName = "";
    public void loadLevel(string levelName)
    {
        if (levelName != curLevelName)
        {
            curLevelName = levelName;
            SceneManager.LoadScene(levelName);

            if (levelName == "ExternalScene")
            {
                player = GameObject.FindGameObjectWithTag("Player");
                
                firesLit = 0;
                
                
            }
                
        }
    }
    
    //Handle Game State
    [SerializeField] private GameObject player;
    
    private int firesLit = 0;
    public int FiresToLight { get => 3; }
    public int FiresLit { get => firesLit; }

    public void lightFire()
    {
        firesLit++;
        if (firesLit == FiresToLight)
        {
            
        } else if (firesLit == 1)
        {
            StartCoroutine(doFirstFireStory());
        }
    }

    private IEnumerator doFirstFireStory()
    {
        ShowStory("You lit the first fire.");
        yield return new WaitForSeconds(4.5f);
        ShowStory("Light all 3 fires to rid the forest of the corruption.");
    }
    
    public void PlayerDead()
    {
        StartCoroutine(PlayerDie());
    }
    
    private IEnumerator PlayerDie()
    {
        //Fade to black, Show "You died", and wait then load the Game Over scene
        if (player)
        {
            var ui = player.GetComponent<PlayerInteractTextScript>();
            ui.FadeToBlack();
            ShowStory("You died");
            yield return new WaitForSeconds(Mathf.Max(ui.fadeSpeed, storyTime));
            loadLevel("GameOverScene");
        }
    }
    
    public void ShowStory(string story)
    {
        if (player)
        {
            Text text = player.GetComponent<PlayerInteractTextScript>().StoryText;
            //Show the story text for a few seconds
            StartCoroutine(showStoryText(text, story));
        }
    }
    
    private float storyTime = 4.0f;
    private IEnumerator showStoryText( Text text, string story)
    {
        if (text)
        {
            text.text = story;
            text.gameObject.SetActive(true);
            text.CrossFadeAlpha(1, 0.5f, false);
            yield return new WaitForSeconds(storyTime);
            text.CrossFadeAlpha(0, 0.5f, false);
            text.gameObject.SetActive(false);
        }
    }
}
