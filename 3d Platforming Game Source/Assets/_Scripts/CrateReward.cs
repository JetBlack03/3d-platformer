using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateReward : MonoBehaviour
{
    public Transform enemySection;
    public GameObject goldBar;
    public int goldReward = 2;

    public Transform central;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemySection != null && enemySection.childCount == 0)
        {
            GetComponent<Animator>().SetBool("open", true);
        }

    }

    public void Opened()
    {
        for (int i = 0; i < goldReward; i++)
        {
            GameObject bar = Instantiate(goldBar, central.position, transform.rotation);
            float j = Random.Range(-35, 35);
            bar.transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(15, j, 0));
            bar.GetComponent<Rigidbody>().AddForce(bar.transform.forward * 350);
            bar.transform.rotation = transform.rotation;
        }
    }
}
