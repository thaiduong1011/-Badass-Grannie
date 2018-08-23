using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private Text txtStart;

    private AudioSource MainAudioSource;

    [SerializeField]
    private AudioClip click;

    private void Awake()
    {
        MainAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        if (txtStart.enabled)
            txtStart.enabled = false;
        else
            txtStart.enabled = true;

        StartCoroutine(wait());
    }

    public void StartButton()
    {
        MainAudioSource.PlayOneShot(click);
        Application.LoadLevel("LevelMenu");
    }

    public void Exit()
    {
        MainAudioSource.PlayOneShot(click);
        Application.Quit();
    }


}
