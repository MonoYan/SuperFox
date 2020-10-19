using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Collider2D coll;
    public Collider2D disColl;
    public Transform cellingCheck,groundCheck;
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public Text cherryNum;
    public int cherry = 0;
    public int gem = 0;
    public bool isHurt;
    public bool isGround, isJump;
    bool jumpPressed;
    int jumpCount;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (!isHurt) 
        {
            GroundMovement();
            Jump();
            Crouch();
        }

        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        HurtAnim();   
        SwitchAnim();

    }


    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        if (horizontalMove != 0)

        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    //跳跃以及二段跳
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.instance.JumpAudio();
            jumpCount--;
            jumpPressed = false;
        }
    }

    //动画切换
    void SwitchAnim()

    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }

        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }

        //修复突然从上面落到地面动画不能切换问题
        if (anim.GetBool("jumping") && isGround)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("idle", true);
        }
    }



    /*
    void Movement() //移动
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        
        //角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running",Mathf.Abs(facedirection));
        }
        
        if(facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        //角色跳跃
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            jumpAudio.Play();
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
    */

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

    //trigger
    private void OnTriggerEnter2D(Collider2D colision) 
    {
        //可收集的物品
        if (colision.tag == "Collection")
        {
            AudioManager.instance.CollectAudio();
            Destroy(colision.gameObject);
            cherry += 1;
            cherryNum.text = cherry.ToString();
        }

        if (colision.tag == "Item-Gem")
        {
            AudioManager.instance.CollectAudio();
            Destroy(colision.gameObject);
            gem += 1;
        }

        if (colision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("Restart",1f);
        }

    }

    //敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.gameObject.tag == "Enemy")
            {//消灭敌人
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("jumping", true);
                jumpCount++;
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                Debug.Log("this is a test");
                rb.velocity = new Vector2(-12, rb.velocity.y);
                AudioManager.instance.HurtAudio();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x) 
            {
                Debug.Log("this is a test");
                rb.velocity = new Vector2(12, rb.velocity.y);
                AudioManager.instance.HurtAudio();
                isHurt = true;
            }

        }

    }
    //下蹲
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))
        {
            if (Input.GetButtonDown("Crouch") && coll.IsTouchingLayers(ground))
            {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
            }
        }
    }

    void Restart()
    {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

















}

