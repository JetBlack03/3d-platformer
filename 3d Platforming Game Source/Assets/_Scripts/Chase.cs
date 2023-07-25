using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public GameObject sword;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = FindObjectOfType<PlayerController>().transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (Vector3.Distance(player.position, transform.position) < 12 && angle < 90)
        {
            
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), .1f);

            anim.SetBool("IsIdle", false); 
            if(direction.magnitude > 2.5f)
            {
                anim.SetBool("IsWalking", true);
                anim.SetBool("IsAttacking", false);
                transform.Translate(0, 0, 0.05f);
                sword.GetComponent<Collider>().enabled = false;
            }
            else
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsAttacking", true);
            }
        }
        else
        {
            anim.SetBool("IsIdle", true);
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsAttacking", false);
            sword.GetComponent<Collider>().enabled = false;
        }
    }
    public void Sword()
    {
        print("sword");
        sword.GetComponent<Collider>().enabled = true;
    }
}
