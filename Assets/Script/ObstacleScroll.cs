using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScroll : MonoBehaviour
{
    public float speed;
    public float Find_x;

    GameObject lastObj;
    GameObject ObsObj;
    GameObject FoodObj;

    private void Awake()
    {
        lastObj = transform.Find("Obstacles/Obstacle/LastObstacle").gameObject;
        ObsObj = transform.Find("Obstacles/Obstacle").gameObject;
        FoodObj = transform.Find("Obstacles/Foods").gameObject;
    }

    private void Start()
    {
        Debug.Log(lastObj.name);
        Debug.Log(ObsObj.name);
        Debug.Log(FoodObj.name);
    }

    private void Update()
    {
        //move
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (lastObj.transform.position.x < Find_x*(-1))
        {
            //장애물 스폰
            transform.localPosition = new Vector3(12.0f, 0, 0);
            gameObject.SetActive(true);
            for (int i = 0; i < ObsObj.transform.childCount; i++)
            {
                if (ObsObj.transform.GetChild(i).gameObject.activeInHierarchy == false)
                {
                    ObsObj.transform.GetChild(i).gameObject.SetActive(true);
                    ObsObj.transform.GetChild(i).gameObject.GetComponent<Collider2D>().isTrigger = true;
                }
            }
            for (int i = 0; i < FoodObj.transform.childCount; i++)
            {
                if (FoodObj.transform.GetChild(i).gameObject.activeInHierarchy == false)
                {
                    FoodObj.transform.GetChild(i).gameObject.SetActive(true);
                    FoodObj.transform.GetChild(i).gameObject.GetComponent<Collider2D>().isTrigger = true;
                }
            }
        }
    }
}
