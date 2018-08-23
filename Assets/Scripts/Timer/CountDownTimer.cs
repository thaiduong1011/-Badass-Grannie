using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {

    public static float timerRemaining;
    private bool isDone;
    private bool firstTime; // sử dụng để gọi change color 1 lần

    [SerializeField]
    private GameObject timePanel;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private GameObject bgObject;

    

    // Use this for initialization
    void Start () {
        timePanel.SetActive(true);
        timerText.enabled = true;
        firstTime = false;
        isDone = false;
        timerRemaining = 20; // trong 20s phải giết dc enemy
        timerText.text = (int) timerRemaining + "";
	}
	
	// Update is called once per frame
	void Update () {
        timerRemaining -= Time.deltaTime;
        timerText.text = (int)timerRemaining + "";
        if (isDone == true)
        {
            timePanel.active = false;
            timerText.enabled = false;
            bgObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.GetComponent<CountDownTimer>().enabled = false;
        }
        else if (timerRemaining <= 5 && firstTime == false)
        {            
            StartCoroutine(wait());
            firstTime = true;
        }


        if (timerRemaining > 0 && SniperBoss.ItemList.Count == 0)
        {
            bgObject.GetComponent<SpriteRenderer>().color = Color.white;
            isDone = true;
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        if (bgObject.GetComponent<SpriteRenderer>().color == Color.white)
        {
            Color red = new Color(1, 0.25f, 0.25f, 1);
            bgObject.GetComponent<SpriteRenderer>().color = red;
        }
        else
            bgObject.GetComponent<SpriteRenderer>().color = Color.white;

        if (timerRemaining <= 0)
        {
            
            bgObject.GetComponent<SpriteRenderer>().color = Color.white;
            isDone = true;  
        }
            
        else
            StartCoroutine(wait());
    }

    public float TimerRemaining
    {
        get
        {
            return timerRemaining;
        }

        set
        {
            timerRemaining = value;
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
}
