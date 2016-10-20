using UnityEngine;
using System.Collections;

public class DroneController : MonoBehaviour {

	public float speed = 5.0f;

	Vector3 move;
	Rigidbody rb;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");
		move = new Vector3 (x, y, 0.0f);

	}

	void FixedUpdate() {

		Vector3 move_by = move * speed * Time.deltaTime;
		rb.AddForce (move_by, ForceMode.Force);
	
	}


}
