using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isVisible : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public Renderer player;
    public Transform shot;
    public float shotdelay;
    public bool visible;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(player.isVisible)
        {
            Debug.Log("Player spotted");
        }
        else
        {
            Debug.Log("Player lost");
        }*/
        

        Vector3 viewPos = cam.WorldToViewportPoint(target.position);
        if(viewPos.x > .5F)
        {
            Debug.Log("Player seen");
            visible = true;
        }
        else
        {
            Debug.Log("Player lost");
        }
            
    }
    void Shoot()
    {
        Instantiate(shot, transform.position, transform.rotation);
    }
}
