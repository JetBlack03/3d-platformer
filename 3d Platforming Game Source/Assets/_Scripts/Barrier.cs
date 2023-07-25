using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{

    public Transform enemySection;
    public SpotlightSystem spotlight;
    public bool needed;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!needed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySection!=null && enemySection.childCount == 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(needed);
            }
        }
        if(spotlight!= null && spotlight.caught)
        {
            gameObject.SetActive(true);
        }
    }
}
