using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {

    public static float speed = 2;
    public static int countEnemyScore = 0;
    private String ItemName;

    private bool isKilled;   
    private Rigidbody2D myBody;
    private BoxCollider2D myBox;

    [SerializeField]
    private GameObject itemObject;
    private GameObject item;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBox = GetComponent<BoxCollider2D>();      
    }

    private void FixedUpdate()
    {
        myBody.velocity = new Vector2(0f, -speed);
    }

    // Use this for initialization
    void Start ()
    {
        isKilled = false;
        Vector3 temp = transform.position;
        temp.y += 1;
        item = Instantiate(itemObject, temp, Quaternion.identity);
        item.GetComponent<Item>().Speed = speed;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (isKilled == true)
        {
            
            GamePlayController.score++;
            countEnemyScore++;
            GamePlayController.countEnemyScore++;          
            Spawner.EnemyList.RemoveAt(0);            
            Destroy(gameObject);
            Destroy(item);
            Destroy(item.GetComponent<Item>().Circle);
            Player.attack = true;
        }
        else
        {
            if (ItemName == null)
            {
                ItemName = item.GetComponent<Item>().GetName();
            }

            if (GamePlayController.score != 0 && countEnemyScore > 10) // cứ 10d thì tăng tốc độ
            {
                speed += 0.5f;
                countEnemyScore = 0;
            }

            if (speed >= 8)
            {
                speed = 3;
            }

            if (speed != item.GetComponent<Item>().Speed) // cập nhật lại tốc độ item và enemy cho giống nhau
            {
                item.GetComponent<Item>().Speed = speed;
            }
        }

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            GamePlayController.lifeNum--;
        }

        if (collision.tag == "border")
        {
           
            Destroy(gameObject);
        }
    }


    public bool IsKilled
    {
        get
        {
            return isKilled;
        }

        set
        {
            isKilled = value;
        }
    }

    public string ItemName1
    {
        get
        {
            return ItemName;
        }

        set
        {
            ItemName = value;
        }
    }

    public GameObject Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }
}
