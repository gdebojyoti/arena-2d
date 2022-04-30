using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
  ArenaController arenaController;

  void Start() {
    arenaController = GameObject.Find("Arena").GetComponent<ArenaController>();
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    GameObject collidingGo = collision.gameObject;

    switch (collidingGo.tag) {
        case "Bullet": {
          var bullet = collidingGo.GetComponent<Bullet>() as Bullet;
          Debug.Log("pick?" + bullet.IsPlayers());
          if (bullet.IsPlayers()) {
            OnCollect();
          }
          break;
        }
    }
  }

  void OnCollect () {
    Destroy(gameObject);
    arenaController.OnPickupCollect();
  }
}
