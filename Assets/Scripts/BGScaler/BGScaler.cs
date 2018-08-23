using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGScaler : MonoBehaviour {

    [SerializeField]
    private Sprite StartBackground, Classic, Zen, Insane, TimeAttack;

    // Use this for initialization
    void Start () {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        Scene currentScene = SceneManager.GetActiveScene(); // check current scene to set background
        string sceneName = currentScene.name;

        if (sceneName == "LevelMenu")
            spriteRenderer.sprite = Classic;
        else if (sceneName == "MainMenu")
            spriteRenderer.sprite = StartBackground;
        else
        {
            switch (LevelController.levelName)
            {
                case "BtnClassic":
                    spriteRenderer.sprite = Classic;
                    break;
                case "BtnZen":
                    spriteRenderer.sprite = Zen;
                    break;
                case "BtnInsane":
                    spriteRenderer.sprite = Insane;
                    break;
                case "BtnTimeAttack":
                    spriteRenderer.sprite = TimeAttack;
                    break;
            }
        }
        

        Vector3 tempScale = transform.localScale;

        float height = spriteRenderer.bounds.size.y;
        float width = spriteRenderer.bounds.size.x;

        float worldHeight = Camera.main.orthographicSize * 2f;
        float worldWidth = worldHeight * Screen.width / Screen.height;

        tempScale.y = worldHeight / height;
        tempScale.x = worldWidth / width;
        transform.localScale = tempScale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
