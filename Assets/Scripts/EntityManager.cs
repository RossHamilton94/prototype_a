using UnityEngine;
using System.Collections;
using System;

public class EntityManager : MonoBehaviour
{
    public Transform entity_container;
    public Transform spawn_points_list;
    public Transform player_prefab;
    public int current_player_count = 0;    // 0 indexed

    private Vector3[] spawn_points;

    // Called when this object is instantiated in the scene
    void Awake()
    {
        if (entity_container == null) Debug.Log("Error: Please attach a container object to store the entities in to this script.");
        if (spawn_points_list == null) Debug.Log("Error: Please attach a spawn points object to this script.");
        if (player_prefab == null) Debug.Log("Error: Please attach a player prefab object to this script.");

        int i = 0;
        spawn_points = new Vector3[spawn_points_list.childCount];
        for (i = 0; i < spawn_points_list.childCount; i++) {
            spawn_points[i] = spawn_points_list.GetChild(i).transform.position;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 RandomSpawnPoint()
    {
        return spawn_points[UnityEngine.Random.Range(0, spawn_points.Length)];
    }

    public void SpawnPlayers(int player_count)
    {
        GameObject temp_player = Instantiate(player_prefab.gameObject, RandomSpawnPoint(), Quaternion.identity) as GameObject;
        temp_player.transform.parent = entity_container.transform;
        temp_player.GetComponent<PlayerInput>().playerid = current_player_count;
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
        throw new NotImplementedException();
    }
}
