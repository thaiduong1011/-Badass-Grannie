using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    private float speed;
    private string name;
    private Rigidbody2D myBody;
    private SpriteRenderer sprite;
    public static Vector2 Size;
    public static System.Random r = new System.Random();
    private bool isKilled;
    private bool isDone;

    [SerializeField]
    private Sprite left, right, left_up, left_down, right_up, right_down, up, down;

    [SerializeField]
    private GameObject circle;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip itemDown;

    private float velocity = 0.0f;
    private Vector3 v3Pos;
    private float g = 9.8f;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
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
        circle = Instantiate(circle, transform.position, Quaternion.identity);
        name = SetName();
        IsKilled = false;
        IsDone = false;
        v3Pos = transform.position;

        switch (name)
        {
            case "Left":
                sprite.sprite = left;
                break;
            case "Right":
                sprite.sprite = right;
                break;
            case "Left_Up":
                sprite.sprite = left_up;
                break;
            case "Left_Down":
                sprite.sprite = left_down;
                break;
            case "Right_Up":
                sprite.sprite = right_up;
                break;
            case "Right_Down":
                sprite.sprite = right_down;
                break;
            case "Up":
                sprite.sprite = up;
                break;
            case "Down":
                sprite.sprite = down;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isKilled)
        {
            sprite.color = Color.black;
            Destroy(circle);
        }

        if (isDone)
        {

            float up = 0.0f;
            float time = Time.deltaTime;
            float delta = velocity * time + (up - g) * time * time*9.8f;    // quãng đường dịch chuyển trong khoảng tg t tính theo v, deltaY = v *t + a*t^2        
            velocity = velocity + (up - g) * time; // công thức v = v + a*t
            v3Pos.y += delta;
            if (v3Pos.y < -4)
            {
                v3Pos.y = 0.0f;
                velocity = -velocity * 0.8f;
                audioSource.PlayOneShot(itemDown);
            }
            transform.position = v3Pos;         
            
            sprite.color = Color.black;
            Destroy(circle);
            Destroy(gameObject, 2.0f);
        }

        if (circle != null)
            circle.transform.position = transform.position;
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

    public bool IsDone
    {
        get
        {
            return isDone;
        }

        set
        {
            isDone = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public GameObject Circle
    {
        get
        {
            return circle;
        }

        set
        {
            circle = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "border")
        {
            Destroy(gameObject);
        }
    }

    string SetName()
    {
        string[] chars = { "Left", "Right", "Up", "Down", "Left_Up", "Left_Down", "Right_Up", "Right_Down" };
        int i;
        if (GamePlayController.levelName == "Classic" && GamePlayController.score <= 10) // chỉ cho random 4 nút lên xuống trái phải khi chế độ classic và đei63m <= 10
        {
            i = r.Next(4);
        }
        else
            i = r.Next(chars.Length);

        return chars[i];
    }

    public string GetName()
    {
        return this.name;
    }
}
