using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {
    private static AdsManager instance;
    public static AdsManager Instance => instance ?? (instance = FindObjectOfType<AdsManager>());
    private string gameId = "3289362";
    private bool testMode = false;

    void Start() => Advertisement.Initialize(gameId, testMode);

    public void ShowAd() => Advertisement.Show();
}
