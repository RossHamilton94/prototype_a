using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

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
        PROCESSING      // Any post processing done after loading, trigger this flag
    };
    private GameState state = GameState.INITIALISING;

    // Called when this object is instantiated in the scene
    void Awake()
    {
        #region Singleton Check

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject); // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.

        #endregion

        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {

        Debug.Log("The game manager has started the game");

        // We're initialising the game scene
        SetState(GameState.INITIALISING);

        // Spawn players
        em.SpawnPlayers(player_count);

        // Spawn boss
        // em.SpawnBoss();

        // Spawn any extra geometry
        // em.SpawnLevel();
        
        // We're good to go
        SetState(GameState.READY);

    }
	
	// Update is called once per frame
	void Update () {

        // This will run in the game manager logic loop, check here for deaths/game states/things not tied to the players/physics step
        // em.Process();

	}

    void SetState(GameState _state)
    {
        state = _state;
    }
}
