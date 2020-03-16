using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public Transform leftPoint, rightPoint;
    public LayerMask ground;
    private bool faceLeft = true;
    public float speed;
    private float leftX, rightX;
    public float jumpForce;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();


        transform.DetachChildren();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

 
    void Update()
    {
        //Movement();    
        SwitchAnim();
    }

    void Movement()
    {
        if (faceLeft)//左
        {
            if (coll.IsTouchingLayers(ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
                if (transform.position.x < leftX)
                {
                    rb.velocity = new Vector2(0, 0);
                    transform.localScale = new Vector3(-1, 1, 1);
                    faceLeft = false;
                }
            }
        }
        else
        {//右
            if (coll.IsTouchingLayers(ground))
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
                if (transform.position.x > rightX)
                {
                    rb.velocity = new Vector2(0, 0);
                    transform.localScale = new Vector3(1, 1, 1);
                    faceLeft = true;
                }
            }
        }
     
    }

    void SwitchAnim()
    {
        if (Anim.GetBool("jumping")) {
            if (rb.velocity.y < 0.1) {
                Anim.SetBool("jumping", false);
                Anim.SetBool("falling", true);
            }
        }
        
        if (coll.IsTouchingLayers(ground) && Anim.GetBool("falling")) {
            Anim.SetBool("idle", true);
        }
    }


}

