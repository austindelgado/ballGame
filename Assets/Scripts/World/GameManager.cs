using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum GameState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class GameManager : MonoBehaviour
{
    // GameManager should be used for managing inGame events and contain the events, such as hit, that things can subscribe to
    // Difference between events and UnityEvents, look into this

    public static GameManager manager;

    // UI
    public GameObject gameOverUI;
    public GameObject winUI;
    public GameObject inGameUI;

    public GameState state;

    void Awake()
    {
        if (manager == null)
        {
            //DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(LevelStart());
    }

    IEnumerator LevelStart()
    {
        //Debug.Log("Level Start");
        state = GameState.START;

        // This should be any level gen setup and animations before the player gets to move
        // Animation of player and camera descending

        //yield return new WaitForSeconds(.5f);
        yield return null;

        // Once this is done, it's the player's turn
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        //Debug.Log("Player turn started");

        // This should have any setup for the player's turn
        // Once state is flipped to PLAYERTURN, player can move
        yield return null;

        state = GameState.PLAYERTURN;
    }

    // Called from ball manager when player turn is complete?
    public void PlayerTurnOver()
    {
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        //Debug.Log("Enemy turn started");
        state = GameState.ENEMYTURN;

        yield return new WaitForSeconds(.1f);

        // Call world move and wait
        //Debug.Log("World start moving");
        yield return StartCoroutine(GridManager.manager.MoveBlocks(false));
        //Debug.Log("World done moving");

        //yield return new WaitForSeconds(.5f);

        // Call enemy move and wait
        //Debug.Log("Enemy start moving");
        if (state == GameState.ENEMYTURN)
            yield return StartCoroutine(GridManager.manager.MoveEnemies());
        //Debug.Log("Enemy done moving");

        // End enemy turn
        if (state == GameState.ENEMYTURN)
            StartCoroutine(PlayerTurn());
    }

    // Won and Lost should be called from GridManager
    public IEnumerator LevelWon()
    {
        state = GameState.WON;

        //Debug.Log("Level Won");

        //winUI.SetActive(true);
        //inGameUI.SetActive(false);

        // Move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        yield return null;
    }

    public IEnumerator LevelLost()
    {
        state = GameState.LOST;

        //Debug.Log("Level Lost");

        //gameOverUI.SetActive(true);
        //inGameUI.SetActive(false);

        // Back to menu
        SceneManager.LoadScene(0);

        yield return null;
    }
}
