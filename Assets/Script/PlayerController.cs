using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Collider2D coll;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public Text cherryNum;
    public int Cherry = 0;
    public int Gem = 0;
    [SerializeField]private bool isHurt = false;

    // Start is called before the first frame update    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt) 
        { 
        Movement();
        }
        HurtAnim();   
        SwitchAnim();
    }

    void Movement() 
    {
        float horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        
        //角色移动
        if (horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("running",Mathf.Abs(facedirection));
        }
        
        if(facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        //角色跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
            anim.SetBool("jumping", true);
        }
    }

    //切换动画效果
    void SwitchAnim()
    {
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground)) {

            anim.SetBool("falling", true);
        }



        if (anim.GetBool("jumping"))
        {
            anim.SetBool("idle", false);

            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
            else if (isHurt)
            {
                anim.SetBool("hurting", true);
                anim.SetFloat("running", 0);
                if (Mathf.Abs(rb.velocity.x) <= 0.1f)
                {
                    isHurt = false;
                    anim.SetBool("hurting", false);
                    anim.SetBool("idle", true);
                }

            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);

        }
    }
    //受伤后的移动修复及动画效果
    void HurtAnim(){
        if (isHurt)
        {
            anim.SetBool("hurting", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) <= 0.1f)
            {
                isHurt = false;
                anim.SetBool("hurting", false);
                anim.SetBool("idle", true);
            }

        }
    }

    //可收集的物品
    private void OnTriggerEnter2D(Collider2D colision) 
    {
        if (colision.tag == "Collection") {
            Destroy(colision.gameObject);
            Cherry += 1;
            cherryNum.text = Cherry.ToString();
        }

        if (colision.tag == "Item-Gem")
        {
            Destroy(colision.gameObject);
            Gem += 1;
        }
    }

    //敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.gameObject.tag == "Enemies")
            {
            if (anim.GetBool("falling"))
            {
                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-8, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x) 
            {
                rb.velocity = new Vector2(8, rb.velocity.y);
                isHurt = true;
            }

        }

    }
}
