using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());
    int score = 0;

    internal bool isPause = false;
    [SerializeField] private Image panelPause;
    [SerializeField] private Image panelDie;
    public static int timesPlayed;

    [SerializeField] private AudioClip explosion;
    public Transform _canvas;

    void Start() {

    }

    internal void AddScore(int value) {
        score += value;
        GameObject.Find("Score").GetComponent<Text>().text = score.ToString();
    }
    public void Pause() {
        if (isPause) {
            return;
        }
        isPause = true;
        Time.timeScale = 0;
        Instantiate(panelPause, _canvas, false);
    }
    internal void Die() {
        Time.timeScale = 0;

        if (PlayerPrefs.HasKey("HighScore")) {
            if (score > PlayerPrefs.GetInt("HighScore")) {
                PlayerPrefs.SetInt("HighScore", score);
            }
        } else {
            PlayerPrefs.SetInt("HighScore", score);
        }
        Debug.Log("ID" + PlayerPrefs.GetString("IDComplete"));
        GetComponent<LeaderBoardManager>().AddNewHighScore(PlayerPrefs.GetString("IDComplete"), score);
        Image pd = Instantiate(panelDie, _canvas, false);
        pd.GetComponent<PanelPause>().SetScores(score, PlayerPrefs.GetInt("HighScore"));
    }
    internal void PlayExplosion() {
        GetComponent<AudioSource>().PlayOneShot(explosion);
    }
}
