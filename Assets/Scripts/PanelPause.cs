using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelPause : MonoBehaviour
{
    [Header("HighScore")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    public void Resume() {
        GameManager.Instance.isPause = false;
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    public void Restart() {
        Time.timeScale = 1;

        if (++GameManager.timesPlayed >=3) {
            AdsManager.Instance.ShowAd();
            GameManager.timesPlayed = 0;
        }
        SceneManager.LoadScene("Game");
    }
    public void Exit() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    internal void SetScores(int score, int highScore) {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }
    public void CloseThis() {
        Destroy(gameObject);
    }
}
