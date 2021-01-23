using System;
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
    Animator braceAnim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        braceAnim = gameObject.transform.Find("Brace").GetComponent<Animator>();
        jumpcount =  0;  ability = 0f;
        isGrounded = isAbility = false;
    }

    private void Update()
    {
        checkAbility();
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        #region Jump&Dash
        //Jump
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

        //Dash
        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.instance.OnDashBtn();
        }
        #endregion
        
    }

    private void FixedUpdate()
    {
        //Jump
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 4f, LayerMask.GetMask("Floor"));
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
                braceAnim.SetBool("isBrace", false);
                anim.SetBool("isAttack", false);
                gameObject.transform.Find("Brace").gameObject.SetActive(false);
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 110/255f);

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
                anim.SetBool("isAttack", true);
                gameObject.transform.Find("Brace").gameObject.SetActive(true);
                braceAnim.SetBool("isBrace", true);

                dtime = 0;
            }
        }
    }

    void OnPlayerMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
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
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                break;
            case "BigFood":
                GameManager.instance.AddHealth(20);
                GameManager.instance.ScoreAdd(250);
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                break;
            case "Jelly":
                GameManager.instance.ScoreAdd(75);
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                break;
            case "BadFood":
                //Damage
                Debug.Log("안좋은 음식을 먹었다!");
                GameManager.instance.OnDamage(15, collision.gameObject);
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                break;
            case "Enemy":
                if (gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    GameManager.instance.OnDamage(10, collision.gameObject);
                }
                else
                {
                    GameManager.instance.ScoreAdd(500);
                    //Destroy(collision.gameObject);
                    collision.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
