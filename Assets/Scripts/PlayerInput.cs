using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    private float move_x = 0.0f;
    private DroneController dc;

    public int health = 20;
    public int base_ammo_count = 30;
    public int ammo = 0;

    public delegate void DamageAction(int pid, float health, float amount);
    public static event DamageAction OnDamage;

    public int playerid;

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

        // Debug health testing
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (OnDamage != null)
            {
                OnDamage(playerid, health, Mathf.RoundToInt(Random.Range(0.0f, 5.0f)));
            }
        }
    }

    void FixedUpdate()
    {
        dc.Move(move_x);
    }
}
