using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentGold;
    public Text goldText;
    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + gameObject.GetComponent<HealthManager>().currentHealth;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        goldText.text = "Gold: " + currentGold;
    }
}
