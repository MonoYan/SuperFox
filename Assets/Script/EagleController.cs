using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : Enemy
{
    public Rigidbody2D rb;
    private Collider2D coll;
    public Transform up,down;

    public bool goUp;
    private float upY, downY;
    public float speed;
    public float flyForce;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        upY = up.position.y;
        downY = down.position.y;
        Destroy(up.gameObject);
        Destroy(down.gameObject);
    }

 
    void Update()
    {
        Movement();
    }

    void Movement() {
        if (goUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > upY) {
                goUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x,-speed);
            if (transform.position.y < downY) {
                goUp = true;
            }
        }
    }
}
