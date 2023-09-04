using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed = 5;
    public CharacterController controller;
    public float jumpForce = 5;
    public Vector3 moveDirection;
    public float gravityScale = 1f;
    public Animator anim;
    public GameObject playerModel;
    public Transform pivot;
    public float rotateSpeed;
    public float climbLength;
    public GameObject climbingWall;
    public bool doubleJump;
    public Transform respawn;
    public bool jumpAttacking;
    public bool frozen = false;
    float jumpAttackForce;
    public GameObject finishText;
    

	// Use this for initialization
	void Start ()
    {
        finishText.SetActive(false);
    }

    // Update is called once per frame
    void Update() {


        if (climbLength == 0)
        {
            StandardMove();
        }
        else if (climbLength < 1)
        {
            
            anim.SetFloat("ClimbingLength", climbLength);
            anim.SetBool("Climbing", true);
            if (Input.GetKey(KeyCode.W)) controller.Move(new Vector3(0, 3.35f * Time.deltaTime, 0));
            else if(Input.GetKey(KeyCode.S)) controller.Move(new Vector3(0, -3.35f * Time.deltaTime, 0));
            else
            {
                anim.SetBool("Climbing", false);
            }


            float newRotY = Mathf.LerpAngle(transform.eulerAngles.y, climbingWall.transform.eulerAngles.y, 7 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(-21, newRotY, 0);

            float minY = climbingWall.GetComponent<Collider>().bounds.min.y;
            float maxY = climbingWall.GetComponent<Collider>().bounds.max.y;
            climbLength = (transform.position.y - minY) / (maxY - minY);

            if (maxY - transform.position.y < .45f)
            {
                print("ready");
                climbLength = 1.1f;

            }
            if (controller.isGrounded)
            {
                anim.SetFloat("ClimbingLength", 0);
                anim.SetFloat("Speed", 0);
                climbLength = 0;
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);

            anim.SetFloat("ClimbingLength", climbLength);
            if (transform.eulerAngles.x == 0)
            {
                climbLength = 0;
                anim.SetFloat("ClimbingLength", 0);

            }
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Climbing Wall"))
        {
            float minY = hit.collider.bounds.min.y;
            float maxY = hit.collider.bounds.max.y;
            climbLength = (transform.position.y - minY) / (maxY - minY);
            climbingWall = hit.gameObject;
            anim.SetFloat("Speed", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fallout")
        {
           transform.position = respawn.position;
           FindObjectOfType<HealthManager>().HurtPlayer(1);

        }

        if (other.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (other.tag == "EndGame")
        {
         finishText.SetActive(true);
        }
        if(other.tag == "respawn")
        {
            respawn = other.transform;
        }
    }

    public void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void StandardMove()
    {
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * speed;

        moveDirection.y = yStore;

        if (controller.isGrounded)
        {
            anim.SetBool("DoubleJump", false);
            doubleJump = false;
            moveDirection.y = 0; //when grounded, gravity will not accumulate.
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }else if (!doubleJump && Input.GetButtonDown("Jump"))
        {
            doubleJump = true;
            moveDirection.y = jumpForce*1.1f;
            anim.SetBool("DoubleJump", true);
        }
        if (jumpAttacking)
        {
            doubleJump = true;
            moveDirection.y = jumpAttackForce;
            jumpAttacking = false;
        }
        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        Vector3 oldmove = moveDirection;
        if (frozen)
        {
            moveDirection.x = 0;
            moveDirection.z = 0;
        }
        controller.Move(moveDirection * Time.deltaTime);
        
        if (!frozen && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(oldmove.x, 0f, oldmove.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        anim.SetBool("IsGrounded", controller.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MeleeGroundAttack());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(JumpAttack());
        }
    }

    IEnumerator MeleeGroundAttack()
    {
        if (controller.isGrounded)
        {
            print("speed " + speed);
            if (anim.GetFloat("Speed") < .5f)
            {

                transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
                playerModel.transform.localRotation = Quaternion.identity;

                anim.SetBool("IsKicking", true);
                float oldSpeed = speed;
                speed = speed * .25f;
                yield return new WaitForSeconds(1.25f);
                speed = oldSpeed;
                anim.SetBool("IsKicking", false);
            }
            else
            {
                anim.SetBool("IsHitting", true);
                float oldSpeed = speed;
                speed = speed * .5f;
                yield return new WaitForSeconds(.925f);
                speed = 10;
                anim.SetBool("IsHitting", false);
            }
        }
    }
    IEnumerator JumpAttack()
    {
        if (controller.isGrounded)
        {
            if (anim.GetFloat("Speed") < .1f)
            {
                transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
                playerModel.transform.localRotation = Quaternion.identity;
                jumpAttackForce = 14;
                frozen = true;
                anim.SetBool("IsSlamming", true);
                yield return new WaitForSeconds(2.1f);
                anim.SetBool("IsSlamming", false);
            }
            else
            {
                anim.SetBool("ComboAttacking", true);
                jumpAttacking = true;
                jumpAttackForce = 7;
                float oldSpeed = speed;
                speed = speed*.8f;
                yield return new WaitForSeconds(0.7f);
                speed = oldSpeed;
                frozen = true;
                anim.SetBool("ComboAttacking", false);
            }
        }
        else
        {
            anim.SetBool("IsSlamming", true);
            yield return new WaitForSeconds(2.1f);
            anim.SetBool("IsSlamming", false);
        }
    }

}
