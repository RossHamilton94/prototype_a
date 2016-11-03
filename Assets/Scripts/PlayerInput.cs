using UnityEngine;
using System.Collections;

public class PlayerInput : Entity
{
    private float move_x = 0.0f;
    private float move_y = 0.0f;
    private DroneController dc;

    public float arcFraction = 0.35f;
    public float time = 1.0f;

    bool traveling = false;

    // Use this for initialization
    void Start()
    {
        dc = GetComponent<DroneController>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        // moving
        move_x = Input.GetAxis("Horizontal");
        move_y = Input.GetAxis("Vertical");

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
        base.Update();

        // Debug depth switch
        if (Input.GetKeyDown(KeyCode.J))
        {
            Vector3 start = this.transform.position;
            Vector3 shootraydownfrom = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 4);

            Ray ray = new Ray(shootraydownfrom, Vector3.down);
            RaycastHit hit;
            Vector3 end = Vector3.zero;
            bool didCollide = Physics.Raycast(ray, out hit);
            if (didCollide) {
                end = hit.point;
            } 
            StartCoroutine(MoveTowards(start, end));
        }
    }

    public IEnumerator MoveTowards(Vector3 posA, Vector3 posB)
    {
        Vector3 targetPos = posB;
        traveling = true;
        Vector3 startPos = transform.position;
        float arcDist = (targetPos - startPos).magnitude * arcFraction;
        float timer = 0.0f;
        while (timer < time) {
            Vector3 pos = Vector3.Lerp(startPos, targetPos, timer / time);
            pos.y += Mathf.Sin(Mathf.PI * timer / time) * arcDist;
           
            Debug.DrawLine(transform.position, pos, Color.red, 1.0f);
            transform.position = pos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        traveling = false; 

        // Second step, wait
        yield return new WaitForSeconds(0.0f);

    }


    void ArcToTarget(Vector3 targetPos, float arcFraction, float time)
    {
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 startPosition, Vector3 endPosition, Vector3 controlPoint)
    {
        float u = 1 - t;
        float uu = u * u;

        Vector3 point = uu * startPosition;
        point += 2 * u * t * controlPoint;
        point += t * t * endPosition;

        return point;
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
        dc.Move(move_x, move_y);
    }
}
