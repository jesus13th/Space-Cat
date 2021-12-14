using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] GameObject[] asteroids;
    [SerializeField] float timeSpawn;
    float timer;

    void Update() {
        timer += Time.deltaTime * Time.timeScale;

        if (timer > Random.Range(3, 6)) {
            GameObject a = null;
            int r = Random.Range(0,10);

            if (r < 3) {
                a = Instantiate(asteroids[0]);
            } else if (r >= 3 && r <= 8) {
                a = Instantiate(asteroids[1]);
            } else {
                a = Instantiate(asteroids[2]);
            }

            if (Random.Range(0, 3) == 0) {
                a.transform.SetPositionAndRotation(PositionRandom(a), Rotation(a));
            } else {
                a.transform.position = PositionRandom(a, false);
                a.transform.rotation = (Random.Range(0, 4) == 0) ? Rotation(a) : Quaternion.Euler(0, 0, 180);
            }
            a.transform.SetParent(transform);
            timer = 0;
        }
    }
    Quaternion Rotation(GameObject a) {
        Vector2 direction = new Vector2(a.transform.position.x, a.transform.position.y) - new Vector2(player.transform.position.x, player.transform.position.y);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, angle) * Quaternion.Euler(0, 0, 90);
    }
    Vector2 PositionRandom(GameObject a, bool random = true) {
        Vector2 result = transform.position;

        Bounds bounds = a.GetComponent<SpriteRenderer>().bounds;
        Vector2 posStart = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
        Vector2 posEnd = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));

        Vector2 scale = new Vector2((int)(posEnd.x - posStart.x), (int)(posEnd.y - posStart.y));

        if (random) {
            int side = Random.Range(0, 2);
            result = Camera.main.ScreenToWorldPoint(new Vector2(side * Screen.width + (side == 1 ? scale.x : -scale.x), Random.Range(Screen.width / 2, Screen.width)));
        } else {
            result = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0 + scale.x / 2, Screen.width - scale.x / 2), Screen.height + scale.y / 2));
        }

        return result;
    }

}
