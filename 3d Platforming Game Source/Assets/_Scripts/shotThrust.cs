using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotThrust : MonoBehaviour
{
    public float thrust = 150f;
    public int damage = 1;
    GameObject player;
    HealthManager hm;
    public float fadeTime = 2f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hm = player.GetComponent<HealthManager>();
        GetComponent<Rigidbody>().AddForce(transform.forward * thrust, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            FindObjectOfType<HealthManager>().HurtPlayer(damage);
            Invoke("Fade", fadeTime);
            Debug.Log("Player tagged");
        }
    }
    void Fade()
    {
        GameObject.Destroy(gameObject);
    }
}
