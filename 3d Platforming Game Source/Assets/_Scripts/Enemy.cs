using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody rbody;
    public int health = 3;
    public GameObject goldBar;
    public int goldReward = 3;
    public Transform central;

    // Start is called before the first frame update
    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody>();
        if (central == null) central = transform;
    }

    // Update is called once per frame
    void Update()
    {


        
    }

    

    void Death()
    {
        for (int i = 0; i < goldReward; i++) {
            GameObject bar = Instantiate(goldBar, central.position, transform.rotation);
            float j = Random.Range(0, 360);
            bar.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(15, j, 0));
            bar.GetComponent<Rigidbody>().AddForce(bar.transform.forward * 350);
            bar.transform.rotation = transform.rotation;
        }

        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "playerAttack")
        {
            print("lost health");
            PlayerController player = other.transform.root.GetComponent<PlayerController>();
            health -= 1;
            rbody.AddForce(player.transform.forward * 500);
            if (health <= 0)
            {
                Death();
            }
        }if(other.tag== "Fallout")
        {
            Death();
        }
    }
}
