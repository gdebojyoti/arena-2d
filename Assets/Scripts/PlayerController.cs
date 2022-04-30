using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float speed = 10f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] int fireRate = 6; // bullets fired per second
    [SerializeField] GameObject bulletSpawnPoint; // location from which bullet is spawned
    [SerializeField] bool isGodMode = false;

    ArenaController arenaController;
    Vector2 mousePosition;
    Camera cam;
    Rigidbody2D rb;
    Coroutine fireCr;
    GameObject lastBulletGo;

    // Start is called before the first frame update
    void Start() {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        arenaController = GameObject.Find("Arena").GetComponent<ArenaController>();
    }

    // Update is called once per frame
    void Update() {
        UpdateMousePosition();
        CheckForInputs();
    }

    void FixedUpdate() {
        // get user input values
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Move(inputX, inputY);

        Vector2 lookDirection = rb.position - mousePosition;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 90f;
        rb.rotation = angle;
    }

    void OnCollisionEnter2D(Collision2D collision) {
      GameObject otherGo = collision.gameObject; // gameobject with which player collided
      
      // TODO: find a better solution for this
      // for handling bullets that are just fired (ignore collisions with them)
      if (lastBulletGo == otherGo) {
        lastBulletGo = null;
        return;
      }

      switch (otherGo.tag) {
          case "Bullet": {
            Destroy(otherGo); // destroy the bullet
            Die();
            break;
          }
          // on collision with enemy (mine)
          case "Enemy": {
            Die();
            break;
          }
      }
    }

    void Move(float inputX, float inputY) {
        // speed to be applied in current frame
        float speedPerFrame = speed * Time.deltaTime;

        // move rigidbody
        rb.velocity = new Vector2(speed * inputX, speed * inputY);
    }

    void UpdateMousePosition () {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void CheckForInputs () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (fireCr != null) {
                StopCoroutine(fireCr); // stop existing coroutines; this might come in handy in case there are multiple ways to fire
            }
            fireCr = StartCoroutine(Attack());
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            // NOTE: null check required when the space bar is spammed (rapidly pressed) for devices with lower frame rates
            if (fireCr != null) {
                StopCoroutine(fireCr);
                fireCr = null;
            }
        }
    }

    IEnumerator Attack () {
        while (true) {
            // instantiate bullet at spawn point
            lastBulletGo = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity) as GameObject;
            
            // apply initial velocity to bullet
            lastBulletGo.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
            lastBulletGo.GetComponent<Bullet>().SetIsPlayers();

            // wait for (1 / fireRate) seconds
            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    void Die () {
      // ignore if in god mode
      if (isGodMode) {
        return;
      }

      // destroy player
      Destroy(gameObject);

      arenaController.OnGameEnd();
    }
}
