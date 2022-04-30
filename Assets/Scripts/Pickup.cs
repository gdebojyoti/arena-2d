using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    ArenaController arenaController;

    void Start() {
        arenaController = GameObject.Find("Arena").GetComponent<ArenaController>();
    }

    void Spawn() {
        Debug.Log("spawned");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // get tag
        string tag = other.gameObject.tag;

        // if tag is "Bullet", trigger collection
        if (tag == "Bullet") {
            OnCollect();
        }
    }

    void OnCollect () {
        Destroy(gameObject);
        arenaController.OnPickupCollect();
    }
}
