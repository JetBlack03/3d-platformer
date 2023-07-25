using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightSystem : MonoBehaviour
{
    public Vector3[] waypoints;
    int waypointIndex;
    public float speed;
    public Color defaultColor;
    public Color alertColor;
    public bool caught;
    Light light;
    public GameObject[] barriers;
     
    // Start is called before the first frame update
    void Start()
    {
        caught = false;
        light = gameObject.GetComponent<Light>();
        waypointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!caught)
        {
            //move to waypoint
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex], speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, waypoints[waypointIndex]) < .5f)
            {
                waypointIndex++;
                if (waypointIndex >= waypoints.Length) waypointIndex = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name + " on alert");
        if (other.name == "Player")
        {
            light.color = alertColor;
            caught = true;
            if (barriers.Length > 0)
            {
                for(int i = 0; i < barriers.Length; i++)
                {
                    barriers[i].SetActive(true);
                }
            }
            StartCoroutine(Continue());
        }
    }
    IEnumerator Continue()
    {
        yield return new WaitForSeconds(3);
        caught = false;
        light.color = defaultColor;
    }
}
