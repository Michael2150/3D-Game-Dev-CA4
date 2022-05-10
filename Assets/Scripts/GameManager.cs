using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ExternalScene")
        {
            player = GameObject.FindWithTag("Player");
            isPlayerDead = false;

            //Set the mouse cursor to not be visible and locked
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            firesLit = 0;
        }
        else
        {
            player = null;

            //Set the mouse cursor to be visible and unlocked
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //Handle Game State
    [SerializeField] private GameObject player;
    private bool isPlayerDead = false;
    
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
        if (!isPlayerDead)
        {
            isPlayerDead = true;
            StartCoroutine(PlayerDie());
        }
    }
    
    private IEnumerator PlayerDie()
    {
        //Fade to black, Show "You died", and wait then load the Game Over scene
        if (player)
        {
            var ui = player.GetComponent<PlayerInteractTextScript>();
            player.GetComponent<Movement>().enabled = false;
            player.GetComponent<MouseLook>().enabled = false;
            ui.FadeToBlack();
            ShowStory("You died");
            yield return new WaitForSeconds(Mathf.Max(ui.fadeSpeed+1f, storyTime+1f));
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
        try
        {
            text.text = story;
            text.gameObject.SetActive(true);
            text.CrossFadeAlpha(1, 0.5f, false);
        }
        catch (MissingReferenceException ex)
        {
            Console.WriteLine();
        }

        yield return new WaitForSeconds(storyTime);
        
        try
        {
            text.CrossFadeAlpha(0, 0.5f, false);
            text.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    
}
