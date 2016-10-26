﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;      // Singleton to help access global vars from other scripts
    public EntityManager em;                        // Drag the script from the same object onto here
    public int player_count = 1;                    // How many players are gonna be playing this game?

    // State flags help pin down what went wrong in what stage for easier debugging
    public enum GameState
    {
        DEFAULT,        // Default state 
        INITIALISING,   // Getting the game ready
        READY,          // Play state, start running logic here
        PAUSED,         // Set this flag is we're paused, stop logic
        LOADING,        // If the level needs loading before the players can move, set this
        SAVING,         // If the level is being saved/serialized, handle this
        PROCESSING,      // Any post processing done after loading, trigger this flag
        GAMEOVER
    };
    public GameState state = GameState.INITIALISING;

    // Called when this object is instantiated in the scene
    void Awake()
    {
        #region Singleton Check

        bool reinit = false;
        if (instance != null) {
            reinit = true;
        }

        if (instance == null)
        {
            instance = this;
        } 
        else if (instance != this)
        {
            Destroy(gameObject); // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
        }

        #endregion

        // If an instance already exists, the start function won't be called, lets call it manually
        if (reinit)
        {
            Debug.Log("The game manager has restarted the game");
            Init();
        }

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("The game manager has started the game");
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        em.Process();
    }

    void Init()
    {
        // We're initialising the game scene
        SetState(GameState.INITIALISING);

        // Init the scene manager first
        em.Init();

        // Spawn players
        em.SpawnPlayers(player_count);

        // Spawn boss
        // em.SpawnBoss();

        // Spawn any extra geometry
        // em.SpawnLevel();

        // We're good to go
        SetState(GameState.READY);

    }

    public GameState GetState()
    {
        return state;
    }

    public void SetState(GameState _state)
    {
        state = _state;
    }
}
