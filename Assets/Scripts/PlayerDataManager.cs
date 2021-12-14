using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour {
    [SerializeField] private InputField slaveInput;
    [SerializeField] private InputField michiInput;
    [SerializeField] private string ID;
    private char[] alpha = new char[] {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
    };

    [SerializeField] private MichiCharacter[] michis;
    [SerializeField] private Text textCost;
    [SerializeField] private Text currentCoins;
    [SerializeField] private int counter;
    [SerializeField] private Image iMichi;
    [SerializeField] private Image iError;
    [SerializeField] private Button bBack;

    void Start() {
        currentCoins.text = PlayerPrefs.GetInt("Coins").ToString();

        slaveInput.text = (PlayerPrefs.HasKey("User") ? PlayerPrefs.GetString("User") : "");
        michiInput.text = (PlayerPrefs.HasKey("Michi") ? PlayerPrefs.GetString("Michi") : "");
        counter = (PlayerPrefs.HasKey("MichiSelected") ? PlayerPrefs.GetInt("MichiSelected") : 0);
        iMichi.sprite =  michis[counter].iMichi;

        for (int i = 1; i < michis.Length; i++) {
            if (PlayerPrefs.HasKey("Michi" + i)) {
                michis[i].unlock = true;
            }
        }

        if (PlayerPrefs.HasKey("ID")) {
            if (!MainMenu.canModifyData) {
                SceneManager.LoadScene("Main Menu");
            } else {
            }
        } else {
            for (int i = 0; i < 10; i++) {
                ID += alpha[Random.Range(0, alpha.Length)];
            }
            bBack.gameObject.SetActive(false);
            PlayerPrefs.SetString("ID", ID);
        }

        textCost.gameObject.SetActive(false);
    }
    public void SaveData() {
        try {
            Destroy(GameObject.Find("MessageError(Clone)"));
        } catch { }

        if (string.IsNullOrEmpty(slaveInput.text)) {
            Instantiate(iError, GameManager.Instance._canvas, false).GetComponentInChildren<Text>().text = "Enter Slave name";
            return;
        }
        if (string.IsNullOrEmpty(michiInput.text)) {
            Instantiate(iError, GameManager.Instance._canvas, false).GetComponentInChildren<Text>().text = "Enter Michi name";
            return;
        }

        slaveInput.text = slaveInput.text.ToLower();
        michiInput.text = michiInput.text.ToLower();

        foreach (char c in slaveInput.text) {
            if (!alpha.Contains(c)) {
                Instantiate(iError, GameManager.Instance._canvas, false).GetComponentInChildren<Text>().text = "Only use characters and numbers";
                return;
            }
        }
        foreach (char c in michiInput.text) {
            if (!alpha.Contains(c)) {
                Instantiate(iError, GameManager.Instance._canvas, false).GetComponentInChildren<Text>().text = "Only use characters and numbers";
                return;
            }
        }

        if (!michis[counter].unlock) {
            Instantiate(iError, GameManager.Instance._canvas, false).GetComponentInChildren<Text>().text = "The michi selected is not available";
            return;
        }

        var IdComplete = $"{slaveInput.text}.{ michiInput.text}.{counter}.{PlayerPrefs.GetString("ID")}";


        PlayerPrefs.SetInt ("MichiSelected", counter);
        PlayerPrefs.SetString("User", slaveInput.text);
        PlayerPrefs.SetString("Michi", michiInput.text);
        PlayerPrefs.SetString("IDComplete", IdComplete);
        Debug.Log(IdComplete);
        MainMenu.canModifyData = false;
        SceneManager.LoadScene("Main Menu");
    }
    public void Previous() {
        if (counter == 0) {
            return;
        }
        counter--;
        loadMichi();
    }
    public void Next() {
        if (counter >= michis.Length - 1) {
            return;
        }
        counter++;
        loadMichi();
    }
    public void BoughtMichi() {
        if (PlayerPrefs.GetInt("Coins") >= michis[counter].cost) {
            michis[counter].unlock = true;
            loadMichi();
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - michis[counter].cost);
            PlayerPrefs.SetString("Michi" + counter, "Unlock");
            currentCoins.text = PlayerPrefs.GetInt("Coins").ToString();
        }
    }
    public void Back() {
        MainMenu.canModifyData = false;
        SceneManager.LoadScene("Main Menu");
    }
    void loadMichi() {
        iMichi.sprite = michis[counter].iMichi;
        if (!michis[counter].unlock) {
            iMichi.color = Color.black;
            textCost.gameObject.SetActive(true);
            textCost.text = michis[counter].cost.ToString();
        } else {
            textCost.gameObject.SetActive(false);
            iMichi.color = Color.white;
        }
    }
}
[System.Serializable]
public class MichiCharacter {
    public Sprite iMichi;
    public bool unlock;
    public int cost;
}