using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bodyGo;
    private Rigidbody2D rb;
    private Vector2 velocityBeforePhysicsUpdate;
    private bool isPlayers = false; // true if fired by player

    // Start is called before the first frame update
    void Start() {
      rb = GetComponent<Rigidbody2D>();

      var sprite = bodyGo.GetComponent<SpriteRenderer>() as SpriteRenderer;
      // if player's, color = #419FDD blue; else #E74C3C red
      sprite.color = isPlayers ? new Color(.25f,.62f,.87f) : new Color(.9f,.3f,.2f);
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

    #region PUBLIC METHODS

      public void SetIsPlayers () {
        isPlayers = true;
      }

      public bool IsPlayers () {
        return isPlayers;
      }

    #endregion
}
