using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLight : MonoBehaviour {

    private SpriteRenderer sprite;
    private bool active;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    // Use this for initialization
    void Start () {
        active = false;
        StartCoroutine(setColor());
	}

    // Update is called once per frame
    void Update () {
        
    }

    static Color color;
    static bool increaseA = false;

    public bool Active
    {
        get
        {
            return active;
        }

        set
        {
            active = value;
        }
    }

    public bool firstTimeTrue = false;
    IEnumerator setColor()
    {
        color = sprite.color;
        yield return new WaitForSeconds(0.05f);
        if (active == true)
        {
            if (firstTimeTrue == true) // lần đầu thì set lại là 1
                color.a = 1;

            if (!increaseA)
            {
                color.a -= 0.1f;
                sprite.color = color;
                if (color.a <= 0)
                    increaseA = true;
            }
            else
            {
                color.a += 0.1f;
                sprite.color = color;
                if (color.a >= 1)
                    increaseA = false;
            }

        }
        else
        {
            color.a = 0;
            sprite.color = color;
        }
        StartCoroutine(setColor());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "border")
        {
            Destroy(gameObject);
        }
    }
}
