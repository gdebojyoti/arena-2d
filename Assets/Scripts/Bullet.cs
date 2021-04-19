using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 velocityBeforePhysicsUpdate;

    // Start is called before the first frame update
    void Start() {
        // Debug.Log("Bullet Fired");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        velocityBeforePhysicsUpdate = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidingGo = collision.gameObject;

        switch (collidingGo.tag) {
            case "Arena":
                // bounce when colliding against arena walls
                Bounce(collision);
                break;
            case "Bullet": {
                // get destroyed on colliding with another bullet
                Destroy(gameObject);
                break;
            }
            case "Player": {
                // not needed; check PlayerController.cs
                break;
            }
        }
        
    }

    void Bounce (Collision2D collision) {
        Vector2 inDirection = velocityBeforePhysicsUpdate;
        Vector2 normal = collision.contacts[0].normal;
        Vector2 newVelocity = Vector2.Reflect(inDirection, normal);
        rb.velocity = newVelocity;
    }
}
