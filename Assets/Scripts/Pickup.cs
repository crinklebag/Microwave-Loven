using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour {

    public enum PickupType { FOOD, ROCK, FOIL }

    [Header("Object References")]
    [SerializeField] Text cookTimeUI;
    [SerializeField] AudioSource audioSource;

    [Header("Object Variables")]
    [SerializeField] PickupType type;
    [SerializeField] float cookTime;
    [SerializeField] float fillValue;

    bool projectile = false;
    bool cooked = false;
    int ownerID = 3;

    void OnEnable() {
        //cookTimeUI.text = cookTime.ToString();
        DisplayTime();
    }

    void Update() {
        if (cookTime <= 1.0f) {
            cooked = true;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") && projectile == false) {
            // Enter Player
            // Debug.Log("Hitting Player");
            other.gameObject.GetComponent<MicrowaveController>().PickupObject(this.gameObject);
        } else if (other.gameObject.CompareTag("Floor")) {
            // Nobody owns it
            ownerID = 3;
            if (projectile) {
                projectile = false;
            }
        } else if (other.gameObject.CompareTag("Player") && projectile == true)  {
            audioSource.Play();
        }
    }

    void DisplayTime() {
        int minutes = (int)(cookTime / 60);
        int seconds = (int)(cookTime - (minutes * 60));
        cookTimeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetFillValue(float newFillValue) {
        fillValue = newFillValue;
    }

    public void SetCookTime(float newTime) {
        cookTime = newTime;
        DisplayTime();
    }

    public float GetCookTime() {
        return cookTime;
    }

    public void SetProjectile(bool state) {
        projectile = state;
    }

    public void SetOwnerID(int newID) {
        ownerID = newID;
    }

    public int GetOwnerID() {
        return ownerID;
    }

    public float GetFillValue() {
        return fillValue;
    }

    public bool IsCooked() {
        return cooked;
    }

    public bool IsProjectile() {
        return projectile;
    }
}
