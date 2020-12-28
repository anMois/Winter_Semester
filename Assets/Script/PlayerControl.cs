using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float Speed;
    public float JumpNum;
    public int Maxjumpcount;
    public int Pice_Meet;
    public Text scoreText;

    int score;
    int jumpcount;
    bool isGrounded;

    Rigidbody2D rigid;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpcount = Pice_Meet = score = 0;
        scoreText.text = score.ToString();
        isGrounded = false;
    }

    private void Update()
    {
        #region Normal_Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * JumpNum, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }
        #endregion

        #region N_Jump
        //if (isGrounded)
        //{
        //    if (jumpcount > 0)
        //    {
        //        if (Input.GetButtonDown("Jump"))
        //        {
        //            rigid.AddForce(Vector2.up * JumpNum, ForceMode2D.Impulse);
        //            anim.SetBool("isJump", true);
        //            jumpcount--;
        //        }
        //    }
        //}
        #endregion

        if (Pice_Meet == 3)
        {
            //recovery
            Debug.Log("고기조각을 3개 먹었습니다.");
            ScoreAdd(200);
            Pice_Meet = 0;
        }
    }

    private void FixedUpdate()
    {
        //Jump
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 0.5f, LayerMask.GetMask("Floor"));
            if (rayhit.collider != null)
            {
                jumpcount = Maxjumpcount;
                isGrounded = true;
                anim.SetBool("isJump", false);
            }
        }
    }

    void ScoreAdd(int num)
    {
        score += num;
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
        {
            collision.isTrigger = false;
        }

        if (collision.gameObject.tag == "PiceMeet")
        {
            Pice_Meet++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "BigFood")
        {
            //big Recovery
            Debug.Log("큰 음식을 먹었다!");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "BadFood")
        {
            //Damage
            Debug.Log("안좋은 음식을 먹었다!");
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Jelly")
        {
            ScoreAdd(100);
            Destroy(collision.gameObject);
        }
    }
}
