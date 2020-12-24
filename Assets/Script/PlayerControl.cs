using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float Speed;
    public float JumpNum;
    public int Maxjumpcount;

    public int Pice_Meet;
    int jumpcount;
    bool isGrounded;

    Rigidbody2D rigid;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpcount = Pice_Meet = 0;
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

        //#region N_Jump
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
        //#endregion
    }

    private void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(Speed, rigid.velocity.y);

        //Jump
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 0.5f, LayerMask.GetMask("Floor"));
            if (rayhit.collider != null)
            {
                anim.SetBool("isJump", false);
                isGrounded = true;
                jumpcount = Maxjumpcount;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PiceMeet")
        {
            Pice_Meet++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "BigFood")
        {
            Debug.Log("큰 음식을 먹었다!");
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "BadFood")
        {
            Debug.Log("안좋은 음식을 먹었다!");
            Destroy(collision.gameObject);
        }
    }
}
