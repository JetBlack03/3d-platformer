using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public float attackRate = 1f;
    public float attackDelay = 1f;
    public int attackDamage = 1;
    public float range = 100f;
    public Transform shot;
    //public GameManager gm;
    public WaitForSeconds shotDelay;
    public bool isShot = false;

    Animator anim;
    GameObject player;
    bool inRange;
    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    LineRenderer gunLine;

    isVisible inGunRange;
    HealthManager health;
    Enemy enemy;
    PlayerController pc;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //health = gm.GetComponent<HealthManager>();
        anim = GetComponent<Animator>();
        shootableMask = LayerMask.GetMask("Player");
        gunLine = GetComponent<LineRenderer>();
        enemy = GetComponent<Enemy>();
        inGunRange = GetComponent<isVisible>();
        pc = player.GetComponent<PlayerController>();

        shotDelay = new WaitForSeconds(attackDelay);
        isShot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            inRange = false;
        }
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= attackRate && inRange)
        {
            Attack();
        }
        if(inGunRange.visible == true && timer >= attackRate && Time.timeScale != 0)
        {
            StartCoroutine(shootProj());
            Invoke("Shoot", attackDelay);
        }
    }

    void Attack()
    {
        timer = 0f;
        FindObjectOfType<HealthManager>().HurtPlayer(attackDamage);
    }

    void Shoot()
    {
        /*shootRay.origin = inGunRange.cam.transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            health = shootHit.collider.GetComponent<HealthManager>();
            FindObjectOfType<HealthManager>().HurtPlayer(attackDamage);
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }*/
        if (isShot == true)
        {
            Instantiate(shot, transform.position, transform.rotation);
        }
    }

    private IEnumerator shootProj()
    {

        yield return shotDelay;
        Shoot();
        Invoke("Shoot", attackDelay);
    }
}
