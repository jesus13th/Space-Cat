using UnityEngine;

public class Asteroid : MonoBehaviour {
    private Rigidbody2D _rb;
    [SerializeField] private float force;
    [SerializeField] private float torque;
    [SerializeField] private int damage;
    public int score;
    public int health;
    
    [SerializeField] private GameObject pCoin;
    [SerializeField] private GameObject pHealth;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(transform.up * force * _rb.mass, ForceMode2D.Force);
        _rb.AddTorque(torque * _rb.mass, ForceMode2D.Force);
    }
    
    void Update() {
        Wrapping();
    }
    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
            GameManager.Instance.PlayExplosion();
            coll.gameObject.GetComponent<CharacterMovement>().HealthManager(-damage);
        }
        if (coll.gameObject.CompareTag("Bullet")) {
            DamageManager(1);
            Destroy(coll.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Coin") || coll.gameObject.CompareTag("Hearth")) {
            Destroy(coll.gameObject);
        }
    }
    internal void DamageManager(int damage) {
        health -= damage;
        if (health <= 0) {
            Explote();
        }
    }
    void Explote() {
        GameManager.Instance.PlayExplosion();
        if (Random.Range(0, 11) < 4) {
            if (Random.Range(0, 6) > 1) {
                Instantiate(pCoin, transform.position, Quaternion.identity);
            } else {
                Instantiate(pHealth, transform.position, Quaternion.identity);
            }
        }
        GameManager.Instance.AddScore(score);
        Destroy(gameObject);
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
        }

        if (pos.y < 0 - scale.y / 2 || pos.y > Screen.height + scale.y / 2) {
            Destroy(gameObject);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

}
