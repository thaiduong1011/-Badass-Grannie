using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public static string levelName;

    void Update()
    {
        var currentEventSystem = EventSystem.current;
        if (currentEventSystem == null) { return; }

        var currentSelectedGameObject = currentEventSystem.currentSelectedGameObject;
        if (currentSelectedGameObject == null) { return; }

        levelName = currentSelectedGameObject.name;
        Application.LoadLevel("GamePlay");
    }
}
