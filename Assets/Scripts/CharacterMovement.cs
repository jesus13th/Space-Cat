using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour {
    private static CharacterMovement instance;
    public static CharacterMovement Instance => instance ?? (instance = FindObjectOfType<CharacterMovement>());

    Rigidbody2D _rb;
    internal ParticleSystem _ps;

    private int health = 7;

    [SerializeField] VirtualJoystick vJoystick;
    [SerializeField] float rotateSpeed = 4;
    [SerializeField] float force = 4;

    [SerializeField] Transform spawnFire;
    [SerializeField] GameObject pBullet;
    [SerializeField] float fireForce;
    public AudioSource fireFX;

    [SerializeField] Sprite[] michis;

    void Start() {
        GetComponent<SpriteRenderer>().sprite = michis[PlayerPrefs.GetInt("MichiSelected")];
        _rb = GetComponent<Rigidbody2D>();
        _ps = GetComponentInChildren<ParticleSystem>();
        _ps.Stop();
    }
    private void LateUpdate() {
        Wrapping();
    }
    void FixedUpdate() {
        _rb.MoveRotation(_rb.rotation + -vJoystick.axis.x * rotateSpeed * Time.deltaTime * Time.timeScale);

        if (vJoystick.holdA) {
            _rb.AddForce(transform.up * force, ForceMode2D.Force);
        }
    }
    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Bullet")) {
            Destroy(coll.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Coin")) {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
            Destroy(coll.gameObject);
        }
        if (coll.CompareTag("Hearth")) {
            HealthManager(+1);
            Destroy(coll.gameObject);
        }
    }
    void Wrapping() {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

        Bounds bounds = GetComponent<SpriteRenderer>().bounds;
        Vector2 posStart = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
        Vector2 posEnd = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));

        Vector2 scale = new Vector2((int)(posEnd.x - posStart.x), (int)(posEnd.y - posStart.y));

        if (pos.x > Screen.width + (scale.x / 2)) {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0 - scale.x / 2, pos.y));
        } else if (pos.x < 0 - (scale.x / 2)) {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + scale.x / 2, pos.y));
        } else if (pos.y > Screen.height + (scale.y / 2)) {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, 0 - scale.y / 2));
        } else if (pos.y < 0 - (scale.y / 2)) {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, Screen.height + scale.y / 2));
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
    internal void AddTorque(float force) {
        _rb.AddTorque(force * rotateSpeed * -100 , ForceMode2D.Impulse);
        Debug.Log("force: " + force);
    }
    internal void Fire() {
        GameObject b = Instantiate(pBullet, spawnFire.position, transform.rotation);
        b.GetComponent<Rigidbody2D>().AddForce(b.transform.up * fireForce, ForceMode2D.Force);
        Destroy(b, 5.0f);
    }
    internal void HealthManager(int value) {
        if (value < 0) {

            GetComponent<AudioSource>().Play();
        }

        health += value;

        health = Mathf.Clamp(health, 0, 7);
        GameObject.Find("HealthText").GetComponent<Text>().text = health.ToString();

        if (health <= 0) {
            GameManager.Instance.Die();
        }

    }
}
