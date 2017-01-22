using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    GameController gameController;

    [SerializeField] Image fillUI;
    [SerializeField] int teamNum;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winAudio;

    float fillAmount = 0;

    void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Pickup") && other.gameObject.GetComponent<Pickup>().IsCooked() && other.gameObject.GetComponent<Pickup>().IsProjectile()) {
            Debug.Log("Hit Target");
            fillAmount += other.gameObject.GetComponent<Pickup>().GetFillValue();
            fillUI.fillAmount = fillAmount;
            Destroy(other.gameObject);
            gameController.DecreaseObjectCount();

            audioSource.Play();

            if (fillAmount >= 1) {
                gameController.EndGame(teamNum);
                audioSource.clip = winAudio;
                audioSource.Play();
            }
        }
    }
}
