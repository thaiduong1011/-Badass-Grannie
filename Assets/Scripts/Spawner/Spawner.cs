using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public static List<GameObject> EnemyList;
    public static bool isActive;

    [SerializeField]
    private GameObject enemy;
    private BoxCollider2D box;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip increaseScore;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        isActive = true;
        StartCoroutine(spawnerEnemy());
        EnemyList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyBoard();
        checkPosition();
    }

    IEnumerator spawnerEnemy()
    {
        if (GamePlayController.score > 25) // nếu trên 25d, rút ngắn thời gian tạo ra enemy
            yield return new WaitForSeconds(Random.Range(0.2f, 1.3f));
        else
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        float minX = -box.bounds.size.x / 2f;
        float maxX = box.bounds.size.x / 2f;

        Vector3 temp = transform.position;
        temp.x = Random.Range(minX, maxX);

        //Instantiate(enemy, temp, Quaternion.identity);
        if (isActive == true)
        {
            EnemyList.Add(Instantiate(enemy, temp, Quaternion.identity));
            StartCoroutine(spawnerEnemy());
        }
            
    }

    void checkPosition()
    {
        if (EnemyList.Count > 0)
        {
            Enemy enemy = EnemyList[0].GetComponent<Enemy>();
            GamePlayController.currentPos.x = enemy.transform.position.x;
            enemy.Item.GetComponent<Item>().Circle.GetComponent<CircleLight>().Active = true;
            enemy.Item.GetComponent<Item>().Circle.GetComponent<CircleLight>().firstTimeTrue = true;
            Vector2 pos = enemy.transform.position;
           if (pos.y < GamePlayController.boundCenter)
            {
                enemy.Item.GetComponent<Item>().Circle.GetComponent<CircleLight>().Active = false;
                enemy.Item.GetComponent<Item>().Circle.GetComponent<CircleLight>().firstTimeTrue = false;
                EnemyList.RemoveAt(0);                
            }
                
        }
        
    }

    void GetKeyBoard()
    {
        if (EnemyList.Count > 0 && GamePlayController.ispaused == false)
        {
            float h = Input.GetAxisRaw("Horizontal"); //GetAxisRaw nghia la khi nhan A or mui ten qua trai thi biên thanh con so -1 0 1
            float v = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown("left") || Input.GetKeyDown("up") || Input.GetKeyDown("right") || Input.GetKeyDown("down"))
            {
                Enemy enemy = EnemyList[0].GetComponent<Enemy>();
                string EnemyName = enemy.ItemName1;
               

                if ((h == -1 && v == 0 && EnemyName == "Left") ||
                    (h == 1 && v == 0 && EnemyName == "Right") ||
                    (h == 0 && v == 1 && EnemyName == "Up") ||
                    (h == 0 && v == -1 && EnemyName == "Down") ||
                    (h == -1 && v == 1 && EnemyName == "Left_Up") ||
                    (h == -1 && v == -1 && EnemyName == "Left_Down") ||
                    (h == 1 && v == 1 && EnemyName == "Right_Up") ||
                    (h == 1 && v == -1 && EnemyName == "Right_Down"))
                {
                    EnemyList[0].GetComponent<Enemy>().IsKilled = true;
                    audioSource.PlayOneShot(increaseScore);                    
                }
            }

           
        }
    }
}
