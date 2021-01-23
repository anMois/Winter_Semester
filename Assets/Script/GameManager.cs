using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float hpreduceNum;   //체력이 다는 속도
    public float MaxHealth;     //최대 체력
    public int MaxmeetCount;    //최대로 먹을 수 있는 고기 조각
    public Image healthBar;     //체력바
    public Text scoreText;      //점수텍스트
    public Text PiceMeetText;   //먹은 고기 조각 텍스트
    public Button DashBtn;
    public GameObject stageText;//스테이지텍스트
    public GameObject startText;//시이이이자아아아아아아아아아아악 하겠습니다!!!!

    public ImageScroll Sky_Speed;       //배경
    public ImageScroll F_Speed;         //바닥
    public ImageScroll Mountain_Speed;  //산
    public ImagesScroll Build_Speed;     //뒷건물
    public ImagesScroll Cloude_Speed;   //구름
    public ObstacleScroll Obs_Speed;    //장애물
    public GameObject DashParticle;
    public GameObject Gameover;
    public GameObject Wintext;

    int score;          //획득 점수
    int picemeetCount;  //현재 먹은 고기 조각
    float health;       //현재 체력
    float savehpredNum; //저장된 체력이 다는 속도
    float windline;
    bool isRunnig;
    bool isStart;

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

        isRunnig = isStart = false;
        Player = GameObject.Find("Player").gameObject;
        windline = DashParticle.GetComponent<ParticleSystem>().startLifetime;
        health = MaxHealth;
        savehpredNum = hpreduceNum;

    }
    #endregion

    private void Start()
    {
        Gameover.SetActive(false);
        Wintext.SetActive(false);
        score = picemeetCount = 0;
        scoreText.text = score.ToString();
        PiceMeetText.text = picemeetCount.ToString() + " / " + MaxmeetCount.ToString();
    }

    private void Update()
    {
        if (isStart)
        {
            checkHealth();
            StageChange();
        }
        else
            LockKey();
    }

    #region Score
    //점수 획득
    public void ScoreAdd(int num)
    {
        score += num;
        scoreText.text = score.ToString();
    }

    void StageChange()
    {
        if (score >= 4000)
        {
            if (SceneManager.GetActiveScene().name == "PalaceScene")
                SceneManager.LoadScene("GardenScene");
            else
                return;
        }
        else if(score >= 7000)
        {
            Wintext.SetActive(true);
            SpeedReset();
        }
    }
    #endregion

    #region Health & Dath
    public void checkHealth()
    {
        //time.deltatime
        health -= Time.deltaTime * hpreduceNum;
        healthBar.fillAmount = health / 100f;
        Dath();
    }

    //고기조각 획득
    public void AddPiceMeet()
    {
        picemeetCount++;

        if (picemeetCount == 3)
        {
            //recovery
            AddHealth(8);
            ScoreAdd(125);
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
        }
    }

    //체력 0으로 죽음
    void Dath()
    {
        if (health < 0)
        {
            Gameover.SetActive(true);
            Player.SetActive(false);
            SpeedReset();
        }
    }

    //다시하기
    public void Retrybtn()
    {
        SceneManager.LoadScene("PalaceScene");
    }
    
    //나가기
    public void ExitBtn()
    {
        Application.Quit();
    }
    #endregion

    #region Dash
    //대쉬버튼을 눌렀을 때
    public void OnDashBtn()
    {
        if (isRunnig)
            return;

        Obs_Speed.speed *= 2;
        F_Speed.speed *= 2;
        Sky_Speed.speed *= 2;
        Mountain_Speed.speed *= 2;
        Cloude_Speed.speed *= 2;
        Build_Speed.speed *= 2;
        DashParticle.GetComponent<ParticleSystem>().startLifetime *= 2.0f;
        isRunnig = true;

        Invoke("OffDashBtn", 5f);
        DashBtn.interactable = false;
    }

    //대쉬 시간이 지난후
    void OffDashBtn()
    {
        Obs_Speed.speed /= 2;
        F_Speed.speed /= 2;
        Sky_Speed.speed /= 2;
        Mountain_Speed.speed /= 2;
        Cloude_Speed.speed /= 2;
        Build_Speed.speed /= 2;
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

    #region Key Lock&UnLock
    public void LockKey()
    {
        SpeedReset();

        Invoke("LookStarttext", 3.0f);
    }

    void SpeedReset()
    {
        Sky_Speed.speed = F_Speed.speed = Mountain_Speed.speed = Build_Speed.speed = Cloude_Speed.speed = Obs_Speed.speed = 0;
        Player.GetComponent<PlayerControl>().abilityNum = DashParticle.GetComponent<ParticleSystem>().startLifetime = 0;
    }

    public void LookStarttext()
    {
        stageText.SetActive(false);
        startText.SetActive(true);

        if(Time.time > 5.0f)
            startText.SetActive(false);

        Invoke("UnLockKey", 2.0f);
    }

    void UnLockKey()
    {
        if(SceneManager.GetActiveScene().name == "GardenScene")
        {
            Sky_Speed.speed = Cloude_Speed.speed = 0.5f;
            Mountain_Speed.speed = 1f;
            F_Speed.speed = Obs_Speed.speed = 4f;
            Build_Speed.speed = 2f;
            Player.GetComponent<PlayerControl>().abilityNum = 20f;
            DashParticle.GetComponent<ParticleSystem>().startLifetime = 2f;
        }
        else if (SceneManager.GetActiveScene().name == "PalaceScene")
        {
            Sky_Speed.speed = Cloude_Speed.speed = 0.375f;
            Mountain_Speed.speed = 0.725f;
            F_Speed.speed = Obs_Speed.speed = 2.5f;
            Build_Speed.speed = 1.25f;
            Player.GetComponent<PlayerControl>().abilityNum = 20f;
            DashParticle.GetComponent<ParticleSystem>().startLifetime = 2f;
        }

        isStart = true;
    }
    #endregion
}
