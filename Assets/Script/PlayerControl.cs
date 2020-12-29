using System;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
        {
            collision.isTrigger = false;
        }

        switch (collision.gameObject.tag)
        {
            case "PiceMeet":
                GameManager.instance.AddPiceMeet();
                Destroy(collision.gameObject);
                break;
            case "BigFood":
                GameManager.instance.AddHealth(10);
                Destroy(collision.gameObject);
                break;
            case "Jelly":
                GameManager.instance.ScoreAdd(100);
                Destroy(collision.gameObject);
                break;
            case "BadFood":
                //Damage
                Debug.Log("안좋은 음식을 먹었다!");
                GameManager.instance.OnDamage(15, collision.gameObject);
                Destroy(collision.gameObject);
                break;
            case "Enemy":
                GameManager.instance.OnDamage(10, collision.gameObject);
                break;
            default:
                break;
        }
    }
}
