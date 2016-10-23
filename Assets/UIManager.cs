using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Transform[] players;

    void Awake()
    {

    }

    void OnEnable()
    {
        PlayerInput.OnDamage += SetHealth;
        BossController.OnDamage += SetHealth;
    }

    void OnDisable()
    {
        PlayerInput.OnDamage -= SetHealth;
        BossController.OnDamage += SetHealth;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 0 indexed player identifier, damage amount taken
    void SetHealth(int player_id, float max_health, float amount)
    {
        Debug.Log("The " + ((player_id == 3) ? "boss:" : "player: " + player_id) + " just took " + amount + " damage.");
        SetFillAmount(players[player_id].GetChild(0).GetComponent<Image>(), (amount / max_health), player_id);
    }

    void SetFillAmount(Image image, float amount, int player_id)
    {
        if (image.fillAmount - amount <= 0.0f)
        {
            Debug.Log("Player: " + player_id + " has reached 0 health, you ded son.");
            image.fillAmount = 0.0f;
        } else
        {
            image.fillAmount -= amount;
        }
    }
}
