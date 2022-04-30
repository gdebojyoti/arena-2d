using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
  public int fireInterval = 2;
  [SerializeField] GameObject bulletPrefab;
  [SerializeField] GameObject bulletSpawnPoint; // location from which bullet is spawned
  [SerializeField] float bulletSpeed = 6f;

  private ArenaController arenaController;
  private GameObject playerGo;
  private Vector2 lastLocation;
  private GameObject lastBulletGo;

  #region MONOBEHAVIOUR METHODS

    // Start is called before the first frame update
    private void Start() {
      arenaController = GameObject.Find("Arena").GetComponent<ArenaController>();
      playerGo = GameObject.Find("Player").gameObject; // TODO: fix error ("Object reference not set to an instance of an object") - triggered when enemy spawns after player dies
      StartCoroutine(Fire());
    }

    // Update is called once per frame
    private void Update() {
      if (playerGo) {
        lastLocation = playerGo.transform.position - transform.position;
      }
      transform.up = lastLocation;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      GameObject collidingGo = collision.gameObject;
      
      // TODO: find a better solution for this
      // for handling bullets that are just fired (ignore collisions with them)
      if (lastBulletGo == collidingGo) {
        lastBulletGo = null;
        return;
      }
      switch (collidingGo.tag) {
        case "Bullet": {
          // detroy bullet
          Destroy(collidingGo);
          var bullet = collidingGo.GetComponent<Bullet>() as Bullet;
          Debug.Log("pick?" + bullet.IsPlayers());
          if (bullet.IsPlayers()) {
            // get destroyed if bullet was fired by player
            Destroy(gameObject);
            arenaController.OnEnemyDestroy();
          }
          break;
        }
      }
    }

  #endregion

  #region PRIVATE METHODS

    IEnumerator Fire () {
      while (true) {
        yield return new WaitForSeconds(fireInterval);
        
        // instantiate bullet at spawn point
        lastBulletGo = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, Quaternion.identity) as GameObject;
        
        // apply initial velocity to bullet
        lastBulletGo.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
      }
    }

  #endregion
}