using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PositionCorrectClimb()
    {
        print("correct the positions" + transform.GetChild(1).position + " " + transform.parent.position);

        transform.parent.position = transform.GetChild(1).position;
        transform.parent.rotation = Quaternion.Euler( new Vector3(0, transform.parent.eulerAngles.y, 0));
    }
    public void Unfreeze()
    {
        transform.parent.GetComponent<PlayerController>().frozen  = false;
    }
    public void JumpAttack()
    {
        transform.parent.GetComponent<PlayerController>().jumpAttacking = true;
    }
    public void Idle()
    {
        if(transform.parent.GetComponent<PlayerController>()!=null)
        transform.parent.GetComponent<PlayerController>().speed = 10;
    }
}
