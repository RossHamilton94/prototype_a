using UnityEngine;
using System.Collections;

public class DroneController : EntityController
{
    public float speed = 8.5f;
    public float jump_strength = 5.0f;
    public float bullet_force = 15.0f;
    
    private bool is_jumping = false;
    private bool double_jumped = false;

    // weapons
    private GameObject bullet;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Camera.main.transform.GetComponent<DualFollowCamera>().player_target = this.transform;
    }

    // Use this for initialization
    void Start()
    {        
        // setup weapons
        bullet = Resources.Load("Prefabs/rocket") as GameObject;
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
        rb.transform.Translate(new Vector3(0.0f, 0.0f, speed * ((facing == 1) ? -move_x : move_x) * Time.deltaTime));

        if (move_x != 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, facing == -1 ? 90.0f : 270.0f, 0.0f));
        }

        if (move_x > 0)
        {
            facing = 1;
        } else if (move_x < 0)
        {
            facing = -1;
        } else if (move_x == 0)
        {
            if (facing == 1)
            {
                facing = 1;
            } else if (facing == -1)
            {
                facing = -1;
            }
        }
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
        bullet_fired.transform.rotation = Quaternion.Euler(new Vector3(0.0f, facing == 1 ? 0.0f : 180.0f, 0.0f));
        bullet_fired.SetActive(true);
        bullet_fired.GetComponent<Rigidbody>().AddForce(new Vector2(bullet_force * 1.0f * facing, 0.0f), ForceMode.Impulse);

        Physics.IgnoreCollision(bullet_fired.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
