using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator Anim;
    private Collider2D coll;
    public Transform leftPoint, rightPoint;
    public LayerMask ground;
    private bool faceLeft = true;
    public float speed;
    private float leftX, rightX;
    public float jumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();


        transform.DetachChildren();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();    
        SwitchAnim();
    }

    void Movement() {
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

    void SwitchAnim() {
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

