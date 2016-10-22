using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{

    public delegate void DamageAction(int pid, float health, float amount);
    public static event DamageAction OnDamage;

    public float base_health = 100.0f;
    public float attack_stength = 2.0f;

    public int base_shield_amount = 100;
    public int shield_multipler = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {

    }

    void Damage(int amount)
    {
        if (base_health - amount <= 0.0f)
        {
            Debug.Log("The boss has been slain.");
        } 
        else
        {
            base_health -= amount;
            if (OnDamage != null)
            {
                OnDamage(3, base_health, amount);    // Always 3 because our boss is the 4th player
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Rocket")
        {
            Damage(col.GetComponent<Rocket>().damage);
            Destroy(col.gameObject);
        }
    }

}
