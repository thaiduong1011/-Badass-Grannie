using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveForce = 20f;
    public float maxVelocity = 4f;// van toc

    private Rigidbody2D myBody;
    private Animator anim;

    private bool moveLeft, moveRight;
    public static bool attack;
    public static bool die;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip atack, dieclip;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        die = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(transform.position.x - GamePlayController.currentPos.x) < 1)
        {
            StopMoving();
        }
        else if (transform.position.x < GamePlayController.currentPos.x)
        {
            SetMoveLeft(false);
        }           
        else
        {
            SetMoveLeft(true);
        }
          
        PlayerWalk();
        checkPosition();
    }

    void checkPosition()
    {
        Vector2 currentPos = transform.position;
        Vector2 pos = Camera.main.WorldToViewportPoint(currentPos);
        
    }

    public void SetMoveLeft(bool moveLeft)
    {
        this.moveLeft = moveLeft;
        this.moveRight = !moveLeft;
    }

    public void StopMoving()
    {
        this.moveLeft = false;
        this.moveRight = false;
    }

    public void Died()
    {
        StopMoving();
        attack = false;
    }
   
    void PlayerWalk()
    {
        float forceX = 0f;
        float vel = Mathf.Abs(myBody.velocity.x);

        if (die == true) // nếu chết
        {
            Died();
            anim.SetBool("Die", true);
        }

        if (attack == true)
        {
            anim.SetBool("Attack", true);
        }
        else  if (moveRight)
        {
            if (vel < maxVelocity)
            {
                    forceX = moveForce;
            }

            //mat nhan vat se huong ve ben phai
            Vector3 scale = transform.localScale;
            scale.x = 0.6f;
            transform.localScale = scale;

            anim.SetBool("Run", true);// cho nhan vat di chuyen
        }
        else if (moveLeft)
        {
            if (vel < maxVelocity)
            {
               forceX = -moveForce;
                
            }

            //mat nhan vat se huong ve ben phai
            Vector3 scale = transform.localScale;
            scale.x = -0.6f;
            transform.localScale = scale;

            anim.SetBool("Run", true);// cho nhan vat di chuyen
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Run", false);
        }

        myBody.AddForce(new Vector2(forceX, 0));
        if (attack == true)
            StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        if (anim.GetBool("Attack") == true)
        {
            anim.SetBool("Attack", false);
            attack = false;
        }
            
    }

}
