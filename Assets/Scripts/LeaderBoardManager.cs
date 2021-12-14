using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour {
    public bool displayScores = true;

    private const string privateCode = "-Uk6Fnotuk2ifwKoYa6DAwFDj-PQjWz0628z2lMBdBbw";
    private const string publicCode = "5d783d50d1041303ec83ca2b";
    private const string webURL = "http://dreamlo.com/lb/";

    public HighScores[] highScoreList;


    [SerializeField] private Image iPlayerData;
    [SerializeField] private RectTransform content;

    [SerializeField] private Image iErrorConection;

    public GameObject mePanel;

    private void Start() {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.LogError("Error. Check internet connection!");
        }

        if (displayScores) {
            StartCoroutine(RefreshHighScores());
        }
    }
    public void Back() {
        SceneManager.LoadScene("Main Menu");
    }
    public void FindMe() {
        mePanel = GameObject.Find(PlayerPrefs.GetString("IDComplete"));
        if (mePanel == null) {
            Debug.LogError("there's register yet uwu.");
            return;
        }
        mePanel.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
        content.anchoredPosition =new Vector2(content.anchoredPosition.x, -mePanel.GetComponent<RectTransform>().anchoredPosition.y + Mathf.Abs(content.sizeDelta.y / 4));
    }
    void SpawnPlayers(HighScores[] highScores) {
        int size = highScoreList.Length;
        var hF = iPlayerData.GetComponent<RectTransform>().sizeDelta.y;
        content.sizeDelta = new Vector2(0, (size) * hF);
        for (int i = 0; i < size; i++) {
            Image g = Instantiate(iPlayerData, content);
            
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (((size * hF) / 2) - (hF / 2)) - (hF * i));
            g.GetComponent<SlotPlayerData>().SetPropierties(i + 1, highScores[i].slave, highScores[i].michi, highScores[i].idIcon, highScores[i].ID, highScores[i].score);
        }
    }
    public void OnHighScoresDownloaded(HighScores[] hs) {
        SpawnPlayers(hs);
    }
    IEnumerator RefreshHighScores() {
            DownloadHighScore();
            yield return new WaitForSeconds(30);
    }
    public void AddNewHighScore(string user, int score) {
        StartCoroutine(UploadNewHighScore(user, score));
    }

    public IEnumerator UploadNewHighScore(string user, int score) {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(user) + "/" + score);
        yield return www;
        if (string.IsNullOrEmpty(www.error)) {
            Debug.Log("upload successful");
        } else {
            Debug.LogError("Error uploading: " + www.error);
        }
    }
    public void DownloadHighScore() {
        StartCoroutine(DownloadNewHighScoreFromDB());
    }

    IEnumerator DownloadNewHighScoreFromDB() {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;
        if (string.IsNullOrEmpty(www.error)) {
            FormatHighScores(www.text);
            OnHighScoresDownloaded(highScoreList);  
        } else {
            Debug.LogError("Error uploading: " + www.error);
        }
    }
    void FormatHighScores(string textStream) {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highScoreList = new HighScores[entries.Length];

        for (int i = 0; i < entries.Length; i++) {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            int score = int.Parse(entryInfo[1]);

            string[] userComplete = entryInfo[0].Split(new char[] { '.' });

            highScoreList[i] = new HighScores(userComplete[0], userComplete[1], int.Parse(userComplete[2]), userComplete[3], score);
        }
    }
}

public struct HighScores {
    public string slave;
    public string michi;
    public int idIcon;
    public int score;
    public string ID;

    public HighScores(string _slave, string _michi,int _idIcon, string _ID, int _score) {
        slave = _slave;
        michi = _michi;
        idIcon = _idIcon;
        ID = _ID;
        score = _score;
    }
}
