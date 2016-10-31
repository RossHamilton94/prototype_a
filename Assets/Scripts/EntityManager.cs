using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
    public Transform entity_container;
    public Transform spawn_points_list;
    public Transform player_prefab;
    public int current_player_count = 0;    // 0 indexed

    public List<GameObject> players;
    private Vector3[] spawn_points;

    // Called when this object is instantiated in the scene
    void Awake()
    {
        Init();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        // This will run in the game manager logic loop, check here for deaths/game states/things not tied to the players/physics step
        Process();
    }

    public void Init()
    {
        if (entity_container == null) Debug.Log("Error: Please attach a container object to store the entities in to this script.");
        if (spawn_points_list == null) Debug.Log("Error: Please attach a spawn points object to this script.");
        if (player_prefab == null) Debug.Log("Error: Please attach a player prefab object to this script.");

        int i = 0;
        spawn_points = new Vector3[spawn_points_list.childCount];
        for (i = 0; i < spawn_points_list.childCount; i++)
        {
            spawn_points[i] = spawn_points_list.GetChild(i).transform.position;
        }
    }

    Vector3 RandomSpawnPoint()
    {
        return spawn_points[UnityEngine.Random.Range(0, spawn_points.Length)];
    }

    public void SpawnPlayers(int player_count)
    {
        GameObject temp_player = Instantiate(player_prefab.gameObject, RandomSpawnPoint(), Quaternion.identity) as GameObject;
        temp_player.transform.parent = entity_container.transform;
        if (temp_player.GetComponent<PlayerInput>() != null) temp_player.GetComponent<PlayerInput>().playerid = current_player_count;
        players.Add(temp_player);
        current_player_count++;
    }

    public void SpawnBoss()
    {
        throw new NotImplementedException();
    }

    public void SpawnLevel()
    {
        throw new NotImplementedException();
    }

    public void Process()
    {
        if (GameManager.instance.GetState() == GameManager.GameState.READY)
        {
            foreach (GameObject p in players)
            {
                PlayerInput temp_p = p.GetComponent<PlayerInput>();
                BossController temp_b = p.GetComponent<BossController>();

                // If the player we've accessed is a player and not a boss
                if (temp_p != null)
                {
                    if (temp_p.health <= 0)
                    {
                        GameManager.instance.SetState(GameManager.GameState.GAMEOVER);
                        Application.LoadLevel("Game_Over");     // TODO: User specific win logic, what happens when the players win?
                    }
                }

                // If the player we'eve accessed is a boss and not a player
                if (temp_b != null)
                {
                    if (temp_b.base_health <= 0)
                    {
                        GameManager.instance.SetState(GameManager.GameState.GAMEOVER);
                        Application.LoadLevel("Game_Over");     // TODO: User specific win logic, what happens when the boss wins?
                    }
                }
            }
        }
    }
}
