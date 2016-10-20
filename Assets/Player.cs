using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float speed = 8.5f;
    public float jump_strength = 5.0f;
    public float bullet_force = 15.0f;

    public int ammo = 0;
    public int base_ammo_count = 30;
    public int health = 20;

    private Rigidbody rb;
    private float move_x = 0.0f;
    private bool is_jumping = false;
    private bool double_jumped = false;
    private int facing = 1; // 1 is right -1 is left

    // weapons
    private GameObject bullet;

    // Use this for initialization
    void Start()
    {
        // setup player objects
        rb = GetComponent<Rigidbody>();
        // Camera.main.transform.GetComponent<SmoothFollow>().target = this.transform;

        // setup weapons
        bullet = Resources.Load("rocket") as GameObject;
        ammo = base_ammo_count;
    }

    // Update is called once per frame
    void Update()
    {

        // moving
        move_x = Input.GetAxis("Horizontal");

        if (move_x > 0)
        {
            facing = 1;
        }
        else if (move_x < 0)
        {
            facing = -1;
        }
        else if (move_x == 0)
        {
            if (facing == 1)
            {
                facing = 1;
            }
            else if (facing == -1)
            {
                facing = -1;
            }
        }

        // facing = move_x > 0 ? 1 : -1;

        // jumping
        if (is_jumping && !double_jumped)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rb.velocity.y < 0)
                {
                    double_jumped = true;
                    Jump();
                }
            }
        }
        else if (!is_jumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                is_jumping = true;
                Jump();
            }
        }
        if (rb.velocity.magnitude <= float.Epsilon)
        {
            is_jumping = false;
            double_jumped = false;
        }

        // shooting
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet_fired = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            bullet_fired.transform.rotation = Quaternion.Euler(new Vector3(0.0f, facing == 1 ? 0.0f : 180.0f, 0.0f));
            bullet_fired.SetActive(true);
            bullet_fired.GetComponent<Rigidbody>().AddForce(new Vector2(bullet_force * 1.0f * facing, 0.0f), ForceMode.Impulse);
        }

    }

    void OnGUI()
    {
        GUI.TextArea(new Rect(new Vector2(20.0f, 20.0f), new Vector2(150.0f, 150.0f)),
            "Facing: " + (facing == 1 ? "Right" : "Left") + "\n" +
            "Last Facing: " + (facing == 1 ? "Right" : "Left") + "\n" +
            "Is Jumping: " + (is_jumping == true ? "Yes" : "No") + "\n"
           );
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        rb.AddForce(new Vector2(0, 1 * jump_strength), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        rb.transform.Translate(new Vector3(0.0f, 0.0f, speed * ((facing == 1) ? -move_x : move_x) * Time.deltaTime));
        if (move_x != 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, facing == -1 ? 90.0f : 270.0f, 0.0f));
        }
    }

    void AddHealth(int amount)
    {

        if (health + amount <= 0)
        {
            Debug.Log("You're dead.");
        }
        else
        {
            health += amount;
        }
    }

    void AddAmmo(int amount)
    {

        if (ammo + amount <= 0)
        {
            Debug.Log("You're out of ammo, please reload.");
        }
        else
        {
            ammo += amount;
        }
    }
}
