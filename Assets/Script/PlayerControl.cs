﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float JumpNum;
    public float abilityNum;
    public int Maxjumpcount;
    public Image AbilityBar;

    float dtime;
    float ability;
    int jumpcount;
    bool isGrounded;
    bool isAbility;

    Rigidbody2D rigid;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpcount =  0;  ability = 0f;
        isGrounded = isAbility = false;
    }

    private void Update()
    {
        checkAbility();

        #region Normal_Jump
        if (Input.GetButtonDown("Jump"))
        {
            OnJump();
        }
        #endregion

        #region N_Jump
        //if (isGrounded)
        //{
        //    if (jumpcount > 0)
        //    {
        //        if (Input.GetButtonDown("Jump"))
        //        {
        //            OnJump();
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

    void checkAbility()
    {
        dtime += Time.deltaTime;

        if (isAbility)
        {
            if (dtime < 2f)
                return;

            ability -= Time.deltaTime * abilityNum;
            AbilityBar.fillAmount = ability / 100f;
            isAbility = (AbilityBar.fillAmount <= 0) ? false : true;
            if (!isAbility)
            {
                gameObject.transform.Find("Brace").gameObject.SetActive(false);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
                Invoke("OnPlayerMode", 2f);
                dtime = 0;
            }
        }
        else
        {
            if (dtime < 2f)
                return;

            ability += Time.deltaTime * abilityNum;
            AbilityBar.fillAmount = ability / 100f;
            isAbility = (AbilityBar.fillAmount == 1) ? true : false;
            if (isAbility)
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerAbility");
                gameObject.transform.Find("Brace").gameObject.SetActive(true);
                dtime = 0;
            }
        }
    }

    void OnPlayerMode()
    {
        GameManager.instance.OnPlayerMode();
    }

    public void OnJump()
    {
        if (!anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * JumpNum, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
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
                if (gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    GameManager.instance.OnDamage(10, collision.gameObject);
                }
                else
                {
                    GameManager.instance.ScoreAdd(100);
                    Destroy(collision.gameObject);
                }
                break;
            default:
                //gameObject.layer = LayerMask.NameToLayer("Player");
                break;
        }
    }
}
