using UnityEngine;
using System.Collections;
using System;

public class MeatboyController : MonoBehaviour
{
    [Serializable]
    public class GroundState
    {
        public GameObject player;
        public float width;
        public float height;
        public float length;

        //GroundState constructor.  Sets offsets for raycasting.
        public GroundState(GameObject playerRef)
        {
            player = playerRef;
            width = player.GetComponentInChildren<Collider>().bounds.extents.x - 0.1f;
            height = player.GetComponentInChildren<Collider>().bounds.extents.y - 0.2f;
            length = 1.0f;
        }

        //Returns whether or not player is touching wall.
        public bool isWall()
        {
            bool left = Physics.Raycast(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, length);
            bool right = Physics.Raycast(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, length);

            Debug.DrawRay(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, Color.red, 0.1f);
            Debug.DrawRay(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, Color.red, 0.1f);

            if (left || right)
                return true;
            else
                return false;
        }

        //Returns whether or not player is touching ground.
        public bool isGround()
        {
            bool bottom1 = Physics.Raycast(new Vector2(player.transform.position.x, player.transform.position.y - height), -Vector2.up, length);
            bool bottom2 = Physics.Raycast(new Vector2(player.transform.position.x + (width - 0.2f), player.transform.position.y - height), -Vector2.up, length);
            bool bottom3 = Physics.Raycast(new Vector2(player.transform.position.x - (width - 0.2f), player.transform.position.y - height), -Vector2.up, length);

            Debug.DrawRay(new Vector2(player.transform.position.x, player.transform.position.y - height), -Vector2.up, Color.green, 0.1f);
            Debug.DrawRay(new Vector2(player.transform.position.x + (width - 0.2f), player.transform.position.y - height), -Vector2.up, Color.green, 0.1f);
            Debug.DrawRay(new Vector2(player.transform.position.x - (width - 0.2f), player.transform.position.y - height), -Vector2.up, Color.green, 0.1f);

            if (bottom1 || bottom2 || bottom3)
                return true;
            else
                return false;
        }

        //Returns whether or not player is touching wall or ground.
        public bool isTouching()
        {
            if (isGround() || isWall())
                return true;
            else
                return false;
        }

        //Returns direction of wall.
        public int wallDirection()
        {
            RaycastHit left_hi, right_hi;
            bool left, right;
            Ray left_ray = new Ray(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right);
            Ray right_ray = new Ray(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right);

            Debug.DrawRay(left_ray.origin, left_ray.direction, Color.blue, 0.2f);
            Debug.DrawRay(right_ray.origin, right_ray.direction, Color.blue, 0.2f);

            bool test_a = Physics.Raycast(left_ray.origin, left_ray.direction, out left_hi, length);
            bool test_b = Physics.Raycast(right_ray.origin, right_ray.direction, out right_hi, length);

            if (left_hi.transform != null)
            {
                left = true;
            } else
            {
                left = false;
            }

            if (right_hi.transform != null)
            {
                right = true;
            } else
            {
                right = false;
            }

            // bool left = Physics.Raycast(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, length);
            // bool right = Physics.Raycast(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, length);

            if (left)
                return -1;
            else if (right)
                return 1;
            else
                return 0;
        }
    }

    private float speed;
    private float accel;
    private float airAccel;
    private float jump;
    private float wallPush;

    public float base_speed = 12.0f;
    public float base_accel = 6.0f;
    public float base_air_accel = 3.0f;
    public float base_jump = 8.0f;
    public float base_wall_push = 2.5f;
    public float shift_mod_factor = 1.5f;

    private Rigidbody rb;

    [SerializeField]
    public GroundState groundState;

    void Start()
    {
        // Create an object to check if player is grounded or touching wall
        groundState = new GroundState(transform.gameObject);
        rb = GetComponent<Rigidbody>();
        speed = base_speed;
        accel = base_accel;
        airAccel = base_air_accel;
        jump = base_jump;
        wallPush = base_wall_push;
    }

    private Vector2 input;

    void OnGUI()
    {
        GUILayout.Label("Grounded: " + groundState.isGround());
        GUILayout.Label("Touching: " + groundState.isTouching());
        GUILayout.Label("Wall: " + groundState.isWall());
        GUILayout.Label("Wall Direction: " + groundState.wallDirection());
    }

    void Update()
    {
        // Handle input
        if (Input.GetKey(KeyCode.A)) {
            input.x = -1;
        } else if (Input.GetKey(KeyCode.D)) {
            input.x = 1;
        } else {
            input.x = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
            input.y = 1;

        // Speed up
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = base_speed * shift_mod_factor;
            jump = base_jump * shift_mod_factor;
            airAccel = base_air_accel * shift_mod_factor;
            accel = base_accel * shift_mod_factor;
        }

        // Slow down
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = base_speed;
            jump = base_jump;
            airAccel = base_air_accel;
            accel = base_accel;
        }

        //Reverse player if going different direction
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (input.x == 0) ? transform.localEulerAngles.y : (input.x + 1) * 90, transform.localEulerAngles.z);
    }



    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(
            new Vector2(
                ((input.x * speed) - GetComponent<Rigidbody>().velocity.x) * (groundState.isGround() ? accel : airAccel),
                0)); //Move player.

        GetComponent<Rigidbody>().velocity
            = new Vector2(
                (input.x == 0 && groundState.isGround()) ? 0 : GetComponent<Rigidbody>().velocity.x,
                (input.y == 1 && groundState.isTouching()) ? jump : GetComponent<Rigidbody>().velocity.y); //Stop player if input.x is 0 (and grounded) and jump if input.y is 1

        if (groundState.isWall() && !groundState.isGround() && input.y == 1)
            GetComponent<Rigidbody>().velocity = new Vector2(
                -groundState.wallDirection() * speed * wallPush,
                GetComponent<Rigidbody>().velocity.y); //Add force negative to wall direction (with speed reduction)

        input.y = 0;
    }
}
