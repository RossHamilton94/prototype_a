using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    private float move_x = 0.0f;
    private DroneController dc;

    public int base_health = 20;
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
            int damage = Mathf.RoundToInt(Random.Range(0.0f, 5.0f));

            // Update the UI
            if (OnDamage != null)
            {
                OnDamage(playerid, base_health, damage);
            }

            Damage(damage);

        }
    }

    void Damage(int amount)
    {
        if (health - amount <= 0)
        {
            // We ded
            Debug.Log("I am player: " + playerid + " and i just died");
            health = 0;

        } else
        {
            health -= amount;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Pickup")
        {
            col.GetComponent<Pickup>().Use(this);
        }
        Debug.Log(col.transform.name);
    }

    void FixedUpdate()
    {
        dc.Move(move_x);
    }
}
