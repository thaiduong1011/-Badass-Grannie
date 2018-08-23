using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{

    public static float speed = 0.4f;

    private Rigidbody2D myBody;
    private BoxCollider2D myBox;
    private SpriteRenderer sprite;

    [SerializeField]
    private GameObject itemObject;

    public static List<GameObject> ItemList;
    private int countScore = 0;
    private bool isKilled;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip died;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBox = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        myBody.velocity = new Vector2(0f, -speed);
    }

    // Use this for initialization
    void Start()
    {
        GamePlayController.currentPos.x = 0; // khi boss xuat hien thi cho player đứng giữa
        isKilled = false;
        ItemList = new List<GameObject>();

        if (speed >= 3f) // nếu tốc độ >= 3 thì set lại speed cho item và boss
        {
            speed = 1;
        }

        Vector2 BossSize = myBox.size;
        Vector3 temp = transform.position;
        Vector2 ItemSize = new Vector2(1.5f / 2, 1.5f / 2);
        int rows, cols;
            rows = 2;
            cols = 4;
        
        Vector2 BeginPos;

        BeginPos.x = temp.x - (0.5f * ItemSize.x + (cols / 2 - 1) * ItemSize.x);
        BeginPos.y = temp.y + BossSize.y / 2 + (0.5f * ItemSize.y) + (rows - 1) * ItemSize.y;

        for (float y = BeginPos.y; y >= BeginPos.y - (rows - 1) * ItemSize.y; y = y - ItemSize.y)
        {
            for (float x = BeginPos.x; x <= BeginPos.x + cols * ItemSize.x; x = x + ItemSize.x)
            {
                temp.x = x;
                temp.y = y;
                GameObject item = Instantiate(itemObject, temp, Quaternion.identity);
                item.GetComponent<Item>().Speed = speed;
                ItemList.Add(item);
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyBoard();
        checkPosition();

        if (ItemList.Count == 0)
        {
            StartCoroutine(increaseScore());
        }
    }

    IEnumerator increaseScore()
    {
        yield return new WaitForSeconds(0.06f);
        GamePlayController.score++;
        countScore++;
        if (countScore >= 10) // diệt được boss thì tăng lên 10d thì destroy gameobject
        {
            countScore = 0;         
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile" && ItemList.Count > 0)
        {
            isKilled = true;
        }

        if (collision.tag == "border")
        {
            Destroy(gameObject);  
        }


    }

    void checkPosition()
    {
        if (ItemList.Count > 0)
        {
            Item item = ItemList[0].GetComponent<Item>();
            item.Circle.GetComponent<CircleLight>().Active = true;
            item.Circle.GetComponent<CircleLight>().firstTimeTrue = true;
        }
    }

    void GetKeyBoard()
    {
        if (ItemList.Count > 0 && GamePlayController.ispaused == false)
        {
            float h = Input.GetAxisRaw("Horizontal"); //GetAxisRaw nghia la khi nhan A or mui ten qua trai thi biên thanh con so -1 0 1
            float v = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown("left") || Input.GetKeyDown("up") || Input.GetKeyDown("right") || Input.GetKeyDown("down"))
            {
                Item enemy = ItemList[0].GetComponent<Item>();
                string EnemyName = enemy.GetName();

                if ((h == -1 && v == 0 && EnemyName == "Left") ||
                    (h == 1 && v == 0 && EnemyName == "Right") ||
                    (h == 0 && v == 1 && EnemyName == "Up") ||
                    (h == 0 && v == -1 && EnemyName == "Down") ||
                    (h == -1 && v == 1 && EnemyName == "Left_Up") ||
                    (h == -1 && v == -1 && EnemyName == "Left_Down") ||
                    (h == 1 && v == 1 && EnemyName == "Right_Up") ||
                    (h == 1 && v == -1 && EnemyName == "Right_Down"))
                {
                    ItemList[0].GetComponent<Item>().IsDone = true;
                    ItemList.RemoveAt(0);
                    audioSource.PlayOneShot(died); // av64n chưa dc
                    Player.attack = true;
                }
            }


        }
    }
}
