using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    public float lifetime = 1.0f;
    public int damage = 3;
    public Vector3 endingPoint;
    public float speed = 1;
    
    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Wall")
        {
            // TODO: particle effect for explosion
            Destroy(gameObject);
        } 
    }

}