using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TurretSystem : MonoBehaviour {

    public Transform objectiveSpawnList;
    public Transform objectivePrefab;

    public List<GameObject> objectives;
    public int currentObjectiveCount = 0;

    private Vector3[] objectiveSpawns;

    void Start()
    {
        //Debug.Log("TurretSystem started");

        SpawnObjectives();
    }

    void Awake()
    {
        //Debug.Log("TurretSystem awake");
        Init();
    }

    void Init()
    {
        //Debug.Log("TurretSystem initialising");

        if (objectiveSpawns == null) Debug.Log("Error: Please attach a container object to store the objective spawns in to this script.");
        if (objectivePrefab == null) Debug.Log("Error: Please attach an objective prefab object to this script.");

        int i = 0;
        objectiveSpawns = new Vector3[objectiveSpawnList.childCount];

        for (i = 0; i < objectiveSpawnList.childCount; i++)
        {
            objectiveSpawns[i] = objectiveSpawnList.GetChild(i).transform.position;
        }
    }

    public void SpawnObjectives()
    {
        //Debug.Log("TurretSystem spawning");

        foreach (Vector3 spawnPoint in objectiveSpawns)
        {
            GameObject tempObj = Instantiate(objectivePrefab.gameObject, spawnPoint, Quaternion.Euler(new Vector3(0, 90, 0))) as GameObject;

            objectives.Add(tempObj);
            currentObjectiveCount++;
        }
    }

    public void Update()
    {
        int numCharged = 0;

        if (objectives != null)
        {
            foreach (GameObject objective in objectives)
            {
                Objective tempObj = objective.GetComponent<Objective>();
                
                if (tempObj.GetPercentageCharge() >= 0.99f)
                {
                    numCharged++;
                }

            }
        }

        //Debug.Log("Number of charged turrets = " + numCharged.ToString());

        if (numCharged == objectives.Count)
        {
            //Debug.Log("All turrets charged");
        }
    }
}
