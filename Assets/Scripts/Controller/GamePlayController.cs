using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour {

    public static GamePlayController instance;
    public static int countEnemyScore;
    private static int countSpider; 
    public static int lifeNum;
    public static int score;
    public static int boundCenter = -3;
    public static string levelName;
    
    public static bool ispaused = false;
    private static GameObject boss, spawner;
    private int highscore;
    private bool isActive;
    private float Timer;

    int count = 0;

    [SerializeField]
    private Text textScoreLast, textCurrentScore, textScoreBest;

    [SerializeField]
    private GameObject pauseButton, continueButton, GameOverPanel, ScorePanel;


   [SerializeField]
    private GameObject SpawnerObject,SpiderObject, SniperObject, PlayerObject;

    public static Vector3 currentPos;

    private AudioSource audioSource;

    [SerializeField]
    private AudioSource MainAudioSource;

    [SerializeField]
    private AudioClip click, gameoverClip, bgGameOverClip,bgGame, appearBossClip, bossDied;
  

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Awake()
    {
        MakeInstance();
        highscore = PlayerPrefs.GetInt("highscore", highscore);
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        ispaused = false;
        Timer = 0;
        ScorePanel.SetActive(true);
        isActive = true;
        countEnemyScore = 0;
        countSpider = 0;
        lifeNum = 5;
        score = 0;
        textCurrentScore.enabled = true;
        switch (LevelController.levelName)
        {
            case "BtnZen":
                levelName = "Zen";
                break;
            case "BtnClassic":
                levelName = "Classic";
                break;
            case "BtnInsane":
                levelName = "Insane";
                break;
            case "BtnTimeAttack":
                levelName = "TimeAttack";
                break;
            default:
                levelName = "Insane";
                break;
        }
        spawner = Instantiate(SpawnerObject, SpawnerObject.transform.position, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive == true)
        {
            Timer += Time.deltaTime;
            textCurrentScore.text = score + "";
            if (levelName != "TimeAttack")
            {
                //
                if (lifeNum <= 0 || (boss != null && ((boss.name == "SpiderBoss(Clone)" && boss.GetComponent<SpiderBoss>().IsKilled == true)
                    || (boss.name == "SniperBoss(Clone)" && boss.GetComponent<SniperBoss>().IsKilled == true)))) // nếu hết mạng hoặc boss chưa chết
                {
                    isActive = false;
                    Player.die = true;
                }

                if (levelName != "Classic")
                {
                    
                    if (countEnemyScore > 25) // cứ 25 con enemy
                    {
                        Spawner.isActive = false;
                        if (levelName == "Insane" && Timer >= 90) // nếu thời gian hơn 90s và là màn insane
                        {
                            boss = Instantiate(SniperObject, SniperObject.transform.position, Quaternion.identity);
                            gameObject.GetComponent<CountDownTimer>().enabled = true;
                            Timer = 0;
                        }
                        else
                            boss = Instantiate(SpiderObject, SpiderObject.transform.position, Quaternion.identity);

                        countEnemyScore = 0;
                        SpiderBoss.speed += 0.2f;
                        countSpider++;
                        if (MainAudioSource.clip != appearBossClip) // nếu boss xuất hiện thì đổinhạc
                        {
                            MainAudioSource.clip = appearBossClip;
                            MainAudioSource.Play();
                        }
                    }

                    if (boss != null && spawner != null && Spawner.EnemyList.Count == 0)
                    {
                        spawner = null;
                    }

                    if (boss == null && spawner == null)
                    {
                        Spawner.isActive = true;
                        spawner = Instantiate(SpawnerObject, SpawnerObject.transform.position, Quaternion.identity);
                        if (MainAudioSource.clip != bgGame) // set nhạc lại bình thường
                        {
                            audioSource.PlayOneShot(bossDied);
                            MainAudioSource.clip = bgGame;
                            MainAudioSource.Play();
                        }
                    }

                }

                
            }

            
        }
        
        if (isActive == false)
        {
            ispaused = true;
            spawner = null;
            boss = null;
            textCurrentScore.enabled = false;
            Enemy.speed = 2;
            SpiderBoss.speed = 1;
            StartCoroutine(waitAndShowScorePanel());
        }


    }


    public void pauseGame()
    {
        audioSource.PlayOneShot(click);
        MainAudioSource.Pause();
        ispaused = true;
        continueButton.SetActive(true);
        Time.timeScale = 0f;
    }

    public void continueGame()
    {
        audioSource.PlayOneShot(click);
        MainAudioSource.UnPause();
        ispaused = false;
        continueButton.SetActive(false);       
        Time.timeScale = 1f;
    }

    public void Home()
    {
        audioSource.PlayOneShot(click);
        Application.LoadLevel("MainMenu");
    }

    public void PlayGame()
    {
        audioSource.PlayOneShot(click);
        Application.LoadLevel("GamePlay");
    }

    public void showGameOverPanel()
    {
        if (MainAudioSource.isPlaying && MainAudioSource.clip != bgGameOverClip)
        {
            MainAudioSource.Stop();
            audioSource.PlayOneShot(gameoverClip);
            MainAudioSource.clip = bgGameOverClip;
            MainAudioSource.Play();
        }
                         
        //MainAudioSource.clip = backgroundGameOver;
        GameOverPanel.SetActive(true);
        textScoreLast.text = score.ToString();

        if (score > highscore)
            highscore = score;

        textScoreBest.text = highscore.ToString();

        PlayerPrefs.SetInt("highscore", highscore);

        pauseButton.SetActive(false);
        continueButton.SetActive(false);
        textCurrentScore.enabled = false ;
        ScorePanel.SetActive(false);
    }

    IEnumerator waitAndShowScorePanel()
    {
        yield return new WaitForSeconds(4f);
        showGameOverPanel();
    }

}
