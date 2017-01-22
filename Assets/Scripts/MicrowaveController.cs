using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MicrowaveController : MonoBehaviour {

    [Header("Object References")]
    [SerializeField] Transform projectileSpawn;
    [SerializeField] GameObject foodPrefab;
    [SerializeField] Text cookTimeUI;
    [SerializeField] GameObject timer;
    [SerializeField] Image forceBar;
    [SerializeField] MeshRenderer[] meshes;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    [Header("Object Variables")]
    [SerializeField] float maxForce = 10;
    [SerializeField] float minForce = 2;
    [SerializeField] float stunTime = 3;

    float throwForce;
    float cookTime;
    float stunInterval = 0.2f;
    bool stunned = false;
    GameObject heldPickup;

    Material baseMat1;
    Material baseMat2;

    // Use this for initialization
    void Start () {
        throwForce = minForce;
        baseMat1 = meshes[0].material;
        baseMat2 = meshes[1].material;
    }

    void Update() {
        if (!stunned && meshes[0].material != baseMat1) {
            meshes[0].material = baseMat1;
            meshes[1].material = baseMat2;
            StopCoroutine("StunPlayer");
        }
    }

    void OnCollisionEnter(Collision other) {

        // If it is a pickup object, and it is being thrown and it has not been thrown by you...
        if (other.gameObject.CompareTag("Pickup") && other.gameObject.GetComponent<Pickup>().IsProjectile() && 
            other.gameObject.GetComponent<Pickup>().GetOwnerID() != this.GetComponent<MicrowaveInput>().GetPlayerID()) {
            audioSource.clip = audioClips[6];
            audioSource.Play();
            StartCoroutine("StunPlayer");
            throwForce = 10;
            ThrowObject();
            stunned = true;
            stunTime = 3;
        }
    }

    void DisplayTime() {
        if (!timer.gameObject.activeSelf) { timer.gameObject.SetActive(true); }
        int minutes = (int)(cookTime / 60);
        int seconds = (int)(cookTime - (minutes * 60));
        cookTimeUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void HideTime() {
        timer.gameObject.SetActive(false);
        heldPickup.GetComponent<Pickup>().SetCookTime(cookTime);
        cookTime = 0;
    }

    IEnumerator Countdown() {

        yield return new WaitForSeconds(1);
        if (cookTime >= 1) {
            cookTime--;
            DisplayTime();
            StartCoroutine("Countdown");
        } else {
            cookTimeUI.text = "Done!";
            audioSource.loop = false;
            audioSource.clip = audioClips[2];
            audioSource.Play();
        }
    }

    IEnumerator StunPlayer() {
        Debug.Log("Stunning Player");

        Material m = meshes[0].material;
        Color32 c = meshes[0].material.color;
        Material m2 = meshes[1].material;
        Color32 c2 = meshes[1].material.color;

        for (int i = 0; i < meshes.Length; i++)  {
            meshes[i].material = null;
            meshes[i].material.color = Color.white;
        }

        yield return new WaitForSeconds(stunInterval);

        meshes[0].material = m;
        meshes[0].material.color = c;
        meshes[1].material = m2;
        meshes[1].material.color = c2;

        stunTime -= stunInterval;

        yield return new WaitForSeconds(stunInterval);

        stunTime -= stunInterval;

        if (stunTime <= 0) {
            stunTime = 3.0f;
            stunned = false;
        } else { StartCoroutine("StunPlayer"); }

    }

    void HandleThrow() {
        heldPickup.GetComponent<Pickup>().SetProjectile(true);
        heldPickup.transform.localPosition = projectileSpawn.transform.localPosition;
        heldPickup.transform.rotation = projectileSpawn.rotation;
        heldPickup.transform.parent = null;
        heldPickup.transform.localScale = Vector3.one;
        heldPickup.gameObject.SetActive(true);
        heldPickup.GetComponent<Rigidbody>().AddForce(heldPickup.transform.forward * throwForce, ForceMode.Impulse);
        heldPickup = null;
    }

    void HandlePickup(GameObject newPickup) {
        heldPickup = newPickup;
        heldPickup.transform.parent = this.transform;
        heldPickup.gameObject.SetActive(false);
        heldPickup.transform.localPosition = Vector3.zero;
        heldPickup.GetComponent<Pickup>().SetOwnerID(this.GetComponent<MicrowaveInput>().GetPlayerID());
    }

    public void IncreaseForce() {
        if (!forceBar.gameObject.activeSelf) { forceBar.gameObject.SetActive(true); } 

        // Increasing Force
        if (throwForce < maxForce) {
            Debug.Log("Growing");
            throwForce += 0.3f;
        }

        // Find Percentage Filled
        forceBar.transform.GetChild(0).GetComponent<Image>().fillAmount = throwForce / maxForce;
    }

    public void PickupObject(GameObject newPickup) {
        
        if (heldPickup == null && !stunned) {
            // Debug.Log("Picking Up Object");
            // Start the audio
            audioSource.clip = audioClips[4];
            audioSource.loop = true;
            audioSource.Play();

            HandlePickup(newPickup);
            cookTime = heldPickup.GetComponent<Pickup>().GetCookTime();
            DisplayTime();
            StartCoroutine("Countdown");
        }
    }

    public void ThrowObject() {

        forceBar.gameObject.SetActive(false);
        forceBar.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        // throwForce = minForce;

        if (heldPickup != null) {
            // Debug.Log("Throwing Food");
            // Handle the timer
            audioSource.loop = false;
            audioSource.clip = audioClips[1];
            audioSource.Play();

            StopCoroutine("Countdown");
            HideTime();
            HandleThrow();
            throwForce = minForce;
        }
    }

    public bool IsStunned() {
        return stunned;
    }
}
