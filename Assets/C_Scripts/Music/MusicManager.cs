using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    private AudioSource audioSource;
    private int sceneIndex;
    private string sceneName;

    void Start() {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        AudioClip manuBGM = Resources.Load("C_Music/上海アリス幻樂団 - 神々が恋した幻想郷") as AudioClip;
        audioSource.clip = manuBGM;
        audioSource.loop = true;
        audioSource.Play();
        ReqSetBGMName(manuBGM.name);
    }

    void GetSceneInfos() {
        sceneName = SceneManager.GetActiveScene().name;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnLevelWasLoaded(int level) {
        if(level == 0) {
            AudioClip manuBGM = Resources.Load("C_Music/上海アリス幻樂団 - 神々が恋した幻想郷") as AudioClip;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = manuBGM;
            audioSource.loop = true;
            audioSource.Play();
            ReqSetBGMName(manuBGM.name);
        } else if(level == 1) {
            AudioClip gameStartBGM = Resources.Load("C_Music/ジャージと愉快な仲間たち - 今日ぞハレの日、われらが祭り") as AudioClip;
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = gameStartBGM;
            audioSource.loop = true;
            audioSource.Play();
            ReqSetBGMName(gameStartBGM.name);
        }
    }

    private void PlayGameOverBGM() {
        AudioClip gameOverBGM = Resources.Load("C_Music/グーシャンダグー - 幽境奏楽") as AudioClip;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameOverBGM;
        audioSource.loop = true;
        audioSource.Play();
        ReqSetBGMName(gameOverBGM.name);
    }

    private void ReqSetBGMName(string name) {
        GameObject uiGameManager = GameObject.Find("UIGameManager");
        uiGameManager.SendMessage("SetBGMName", name);
    }
}
