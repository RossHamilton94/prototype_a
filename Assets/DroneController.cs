using UnityEngine;
using System.Collections;

public class DroneController : MonoBehaviour
{

    public float speed = 8.5f;
    public float jump_strength = 5.0f;
    public float bullet_force = 15.0f;

    public int ammo = 0;
    public int base_ammo_count = 30;
    public int health = 20;

    private Rigidbody rb;
    private int facing = 1; // 1 is right -1 is left

    private bool is_jumping = false;
    private bool double_jumped = false;

    // weapons
    private GameObject bullet;

    // Use this for initialization
    void Start()
    {
        //setup rigidbody and camera
        rb = GetComponent<Rigidbody>();
        Camera.main.transform.GetComponent<SmoothFollow>().target = this.transform;

        // setup weapons
        bullet = Resources.Load("Prefabs/Bullet") as GameObject;
        ammo = base_ammo_count;

    }

    // Update is called once per frame
    void Update()
    {



    }

    void FixedUpdate()
    {

    }

    public void Move(float move_x)
    {
        //use Translate to move
        transform.Translate(new Vector3(0.0f, 0.0f, speed * -move_x * Time.deltaTime));
        facing = move_x > 0 ? 1 : -1;
    }

    //checks whether the drone can jump
    public void Jump()
    {
        //check if double jump is allowed
        if (is_jumping && !double_jumped)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rb.velocity.y < 0)
                {
                    double_jumped = true;
                    JumpAction();
                }
            }
        }
        //check if first jump is allowed
        else if (!is_jumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                is_jumping = true;
                JumpAction();
            }
        }
        //check if the drone is not jumping
        if (rb.velocity.magnitude <= float.Epsilon)
        {
            is_jumping = false;
            double_jumped = false;
        }

    }

    private void JumpAction()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        rb.AddForce(new Vector2(0, 1 * jump_strength), ForceMode.Impulse);
    }

    public void FireWeapon()
    {
        //fire bullet object
        GameObject bullet_fired = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        bullet_fired.GetComponent<Rigidbody>().AddForce(new Vector2(bullet_force * 1.0f * facing, 0.0f), ForceMode.Impulse);
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
