using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public static bool canModifyData = false;
    public static bool mute = true;
    public Sprite[] audioSprites;
    public GameObject AudioButton;

    void Awake() {
        AudioButton.GetComponent<Image>().sprite = audioSprites[(mute ? 0 : 1)];
    }

    public void Play() =>
        SceneManager.LoadScene("Game");
    public void PlayerData() {
        canModifyData = true;
        SceneManager.LoadScene("PlayerData");
    }
    public void LeaderBoard() => SceneManager.LoadScene("LeaderBoard");
    public void About() => SceneManager.LoadScene("About");
    public void Mute(GameObject g) {
        mute = !mute;

        AudioListener.volume = (mute ? 1 : 0);
        g.GetComponent<Image>().sprite = audioSprites[(mute ? 0 : 1)];
    }
    public void Exit() => Application.Quit();
}
