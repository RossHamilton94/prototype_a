using UnityEngine;
using System.Collections;

public class DestroyOnCollide : MonoBehaviour {

	public float lifetime = 1.0f;

	// Use this for initialization
	void Start () {
	
		Destroy (this.gameObject, lifetime);

	}
	
	// Update is called once per frame
	void Update () {


	
	}

	void OnCollisionEnter(Collision col) {
	
		if (col.transform.tag == "Wall") {
			Destroy(this.gameObject);
		}
	}

}
