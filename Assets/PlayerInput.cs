using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

    private float move_x = 0.0f;
    private DroneController dc;

    // Use this for initialization
    void Start()
    {

        dc = GetComponent<DroneController>();

    }

    // Update is called once per frame
    void Update()
    {
        // moving
        move_x = Input.GetAxis("Horizontal");
        dc.Move(move_x);

        //jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dc.Jump();
        }

        // shooting
        if (Input.GetButtonDown("Fire1"))
        {
            dc.FireWeapon();
        }
    }
}
