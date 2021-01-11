using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float speed;         //장애물이 움직이는 속도
    public float hpreduceNum;   //체력이 다는 속도
    public float MaxHealth;     //최대 체력
    public int MaxmeetCount;    //최대로 먹을 수 있는 고기 조각
    public Image healthBar;     //체력바
    public Text scoreText;      //점수텍스트
    public Text PiceMeetText;   //먹은 고기 조각 텍스트
    public Button DashBtn;

    public ImageScroll Sky_Speed;       //배경
    public ImageScroll F_Speed;         //바닥
    public ImageScroll Mountain_Speed;  //산
    public ImagesScroll Cloude_Speed;   //구름
    public ImagesScroll Pillar_Speed;   //기둥
    public GameObject DashParticle;

    int score;
    int picemeetCount;  //현재 먹은 고기 조각
    float health;       //현재 체력
    float savehpredNum; //저장된 체력이 다는 속도
    float windline;
    bool isRunnig;

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

        isRunnig = false;
        Player = GameObject.Find("Player").gameObject;
        windline = DashParticle.GetComponent<ParticleSystem>().startLifetime;
        health = MaxHealth;
        savehpredNum = hpreduceNum;

    }
    #endregion

    private void Start()
    {
        score = picemeetCount = 0;
        scoreText.text = score.ToString();
        PiceMeetText.text = picemeetCount.ToString() + " / " + MaxmeetCount.ToString();
    }

    private void Update()
    {
        //move
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        checkHealth();
        Debug.Log(windline);
    }

    //점수 획득
    public void ScoreAdd(int num)
    {
        score += num;
        scoreText.text = score.ToString();
    }

    #region Health
    public void checkHealth()
    {
        //time.deltatime
        health -= Time.deltaTime * hpreduceNum;
        healthBar.fillAmount = health / 100f;
    }

    //고기조각 획득
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

    //체력 회복
    public void AddHealth(int index)
    {
        health += index;

        if (health >= MaxHealth)
        {
            if (health >= (MaxHealth + 20))
                health = (MaxHealth + 20);

            DashBtn.interactable = true;
            Debug.Log(health);
        }
    }
    #endregion

    #region Dash
    //대쉬버튼을 눌렀을 때
    public void OnDashBtn()
    {
        if (isRunnig)
            return;

        speed *= 2;
        F_Speed.speed *= 2;
        Sky_Speed.speed *= 2;
        Mountain_Speed.speed *= 2;
        Cloude_Speed.speed *= 2;
        Pillar_Speed.speed *= 2;
        DashParticle.GetComponent<ParticleSystem>().startLifetime *= 2.0f;
        isRunnig = true;

        Invoke("OffDashBtn", 5f);
        DashBtn.interactable = false;
    }

    //대쉬 시간이 지난후
    void OffDashBtn()
    {
        speed /= 2;
        F_Speed.speed /= 2;
        Sky_Speed.speed /= 2;
        Mountain_Speed.speed /= 2;
        Cloude_Speed.speed /= 2;
        Pillar_Speed.speed /= 2;
        //DashParticle.SetActive(false);
        DashParticle.GetComponent<ParticleSystem>().startLifetime = windline;
        isRunnig = false;
    }
    #endregion

    #region Damage
    //충돌
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

    //독고기조각을 부딪쳤을 때
    void OnPoison()
    {
        hpreduceNum *= 2;
        healthBar.color = new Color(100 / 255f, 255 / 255f, 255 / 255f);

        Invoke("OffPoison", 5.0f);
    }
    //끝나고 원상복귀
    void OffPoison()
    {
        hpreduceNum = savehpredNum;
        healthBar.color = new Color(1, 1, 1);
    } 
    #endregion
}
