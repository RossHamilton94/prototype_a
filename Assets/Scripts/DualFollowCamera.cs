using UnityEngine;
using System.Collections;

public class DualFollowCamera : MonoBehaviour
{
    public bool isOrthographic;
    public GameObject[] targets;
    public float currentDistance;
    public float largestDistance;
    public float height = 5.0f;
    public float avgDistance;
    public float distance = 0.0f;                    // Default Distance 
    public int speed = 1;
    public float heightoffset;
    public float smooth = 0.15F;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        if (Camera.main)
            isOrthographic = Camera.main.orthographic;
    }

    void OnGUI()
    {
        GUILayout.Label("largest distance is = " + largestDistance.ToString());
        GUILayout.Label("height = " + height.ToString());
        GUILayout.Label("number of players = " + targets.Length.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {

        targets = GameObject.FindGameObjectsWithTag("Player");
        if (!GameObject.FindWithTag("Player"))
            return;

        Vector3 sum = new Vector3(0, 0, 0);
        for (int n = 0; n < targets.Length; n++)
        {
            sum += targets[n].transform.position;
        }
        Vector3 avgDistance = sum / targets.Length;

        //    Debug.Log(avgDistance);

        float largestDifference = returnLargestDifference();
        height = Mathf.Lerp(height + heightoffset, largestDifference, Time.deltaTime * speed);

        if (isOrthographic)
        {
            Camera.main.transform.position = new Vector3(avgDistance.x, 0.0f, height);
            Camera.main.orthographicSize = largestDifference;
            Camera.main.transform.LookAt(avgDistance);
        } else
        {
            Vector3 destination = new Vector3(avgDistance.x, height, -(avgDistance.z - distance + largestDifference));
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smooth);
            // Camera.main.transform.position = new Vector3(avgDistance.x, height, -(avgDistance.z - distance + largestDifference));
            // Camera.main.transform.LookAt(avgDistance);
        }

    }

    float returnLargestDifference()
    {
        currentDistance = 0.0f;
        largestDistance = 0.0f;
        for (int i = 0; i < targets.Length; i++)
        {
            for (int j = 0; j < targets.Length; j++)
            {
                currentDistance = Vector3.Distance(targets[i].transform.position, targets[j].transform.position);
                if (currentDistance > largestDistance)
                {
                    largestDistance = currentDistance;
                }
            }
        }
        return largestDistance;

    }

}


