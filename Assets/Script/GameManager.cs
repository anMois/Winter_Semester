using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float speed;         //장애물이 움직이는 속도
    public float F_Speed;       //바닥 움직이는 속도
    public float hpreduceNum;   //체력이 다는 속도
    public float MaxHealth;     //최대 체력
    public int MaxmeetCount;    //최대로 먹을 수 있는 고기 조각
    public Image healthBar;     //체력바
    public Text scoreText;      //점수텍스트
    public Text PiceMeetText;   //먹은 고기 조각 텍스트

    int score;
    int picemeetCount;  //현재 먹은 고기 조각
    float health;       //현재 체력
    float savehpredNum; //저장된 체력이 다는 속도

    GameObject Player;

    #region SingleTon
    public static GameManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Player = GameObject.Find("Player").gameObject;
        health = MaxHealth;
        savehpredNum = hpreduceNum;
        score = picemeetCount = 0;
        scoreText.text = score.ToString();
        PiceMeetText.text = picemeetCount.ToString() + " / " + MaxmeetCount.ToString();
    }
    #endregion

    private void Update()
    {
        //move
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        checkHealth();
    }

    public void AddPiceMeet()
    {
        picemeetCount++;

        if (picemeetCount == 3)
        {
            //recovery
            Debug.Log("고기조각을 3개 먹었습니다.");
            AddHealth(3);
            ScoreAdd(200);
            picemeetCount = 0;
        }

        PiceMeetText.text = picemeetCount.ToString() + " / " + MaxmeetCount.ToString();
    }

    public void ScoreAdd(int num)
    {
        score += num;
        scoreText.text = score.ToString();
    }

    public void checkHealth()
    {
        //time.deltatime
        health -= Time.deltaTime * hpreduceNum;
        healthBar.fillAmount = health / 100f;
    }

    public void AddHealth(int index)
    {
        if (health == MaxHealth)
            return;
        else
            health += index;
    }

    #region Damage
    public void OnDamage(int index, GameObject obj)
    {
        if (health < 0)
            return;

        Player.layer = LayerMask.NameToLayer("PlayerDamaged");
        Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
        health -= index;

        if (obj.tag == "BadFood")
            OnPoison();

        Invoke("OnPlayerMode", 2.0f);
    }

    public void OnPlayerMode()
    {
        Player.layer = LayerMask.NameToLayer("Player");
        Player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    void OnPoison()
    {
        hpreduceNum *= 2;
        healthBar.color = new Color(100 / 255f, 255 / 255f, 255 / 255f);

        Invoke("OffPoison", 5.0f);
    }

    void OffPoison()
    {
        hpreduceNum = savehpredNum;
        healthBar.color = new Color(1, 1, 1);
    } 
    #endregion
}
