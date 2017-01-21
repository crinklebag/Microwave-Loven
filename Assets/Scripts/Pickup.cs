using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public enum PickupType { FOOD, ROCK }
    [SerializeField] PickupType type;
    [SerializeField] float cookTime;

    bool projectile = false;

    void OnCollisionEnter(Collision other) {
        if (projectile && other.gameObject.CompareTag("Player")) {
            // Attack Player

        } else if (other.gameObject.CompareTag("Player")) {
            // Enter Player
            Destroy(gameObject);
        }
    }
}
