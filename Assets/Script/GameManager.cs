using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float speed;
    public float timenum;
    public float MaxHealth;
    public int picemeetCount;
    public Image healthBar;
    public Text scoreText;
    public Text PiceMeetText;


    int MaxmeetCount;
    int score;
    float health;

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
        health = MaxHealth;
        score = picemeetCount = 0;  MaxmeetCount = 3;
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
        #region time.deltatime reduction
        //time.deltatime
        health -= Time.deltaTime * timenum;
        healthBar.fillAmount = health / 100f;
        #endregion
    }

    public void AddHealth(int index)
    {
        if (health == MaxHealth)
            return;
        else
            health += index;
    }

    public void DamagePoision(int index)
    {
        if (health < 0)
            return;

       health -= index;
       float idx = timenum;
       timenum *= 3;
       healthBar.color = new Color(100, 0, 255);
       //if (Time.time > 3.0f)
       //{
       //     timenum = idx;
       //     healthBar.color = new Color(255,255,255);
       //}
    }
}
