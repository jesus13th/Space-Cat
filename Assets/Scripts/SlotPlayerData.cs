using UnityEngine;
using UnityEngine.UI;

public class SlotPlayerData : MonoBehaviour {
    public Sprite[] michis;

    [SerializeField] private Text position;
    [SerializeField] private Image image;
    [SerializeField] private Text slave;
    [SerializeField] private Text michi;
    [SerializeField] private Text score;
    public string ID;

    internal void SetPropierties(int _position, string _slave, string _michi, int _idIcon, string _ID, int _score) {
        position.text = _position.ToString();
        image.sprite = michis[_idIcon];
        slave.text = _slave;
        michi.text = _michi;
        gameObject.name = ID = $"{ _slave }.{ _michi }.{ _idIcon }.{ _ID }";
        score.text = _score.ToString();
    } 
}
