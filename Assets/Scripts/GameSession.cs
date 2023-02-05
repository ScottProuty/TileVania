using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int deathCamSecs = 3;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int playerScore = 0;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void addScore(int amount)
    {
        playerScore += amount;
        scoreText.text = playerScore.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            StartCoroutine(TakeLife());
        } 
        else
        {
            ResetGameSession();
        }
    }

    private IEnumerator TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        yield return new WaitForSecondsRealtime(deathCamSecs);
        // Reload current level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().resetPersistance();
        SceneManager.LoadScene(0); // Load first level
        Destroy(gameObject);
    }

    void Update()
    {
    }
}
