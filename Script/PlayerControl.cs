using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float Speed;
    public float JumpNum;
    public int Maxjumpcount;

    int jumpcount;
    bool isGrounded = false;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        jumpcount = 0;
    }

    private void Update()
    {
        //Move
        rigid.velocity = new Vector2(Speed, rigid.velocity.y);

        //Jump
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Floor"));
        if (rayhit.collider != null)
        {
            isGrounded = true;
            jumpcount = Maxjumpcount;
        }

        if (isGrounded)
        {
            if (jumpcount > 0)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rigid.AddForce(Vector2.up * JumpNum, ForceMode2D.Impulse);
                    jumpcount--;
                }
            }
            isGrounded = false;
        }
    }
}
