using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [Header("Object References")]
    [SerializeField] GameObject team1Target;
    [SerializeField] GameObject team2Target;
    [SerializeField] GameObject foodPrefab;
    [SerializeField] GameObject EndPanel;
    [SerializeField] Text playerWinText;
    [SerializeField] Text playerLoseText;
    [SerializeField] GameObject[] credits;
    [SerializeField] GameObject logo;

    [Header("Object Variables")]
    [SerializeField] float minCookTime;
    [SerializeField] float maxCookTime;
    [SerializeField] int maxObjects;
    [SerializeField] float maxX;
    [SerializeField] float minX;
    [SerializeField] float maxZ;
    [SerializeField] float minZ;

    int objectCount = 0;
    int creditCounter = 0;
    int creditWaitTime = 0;
    float waitTime = 0;
    bool gameOver = false;
    bool gameStarted = false;

	// Use this for initialization
	void Start () {
        StartCoroutine("SpawnObjects");
        StartCoroutine("RollCredits");
	}
	
    void CreateFoodItem() {
        // Choose random values for the fill and cook time for the new item
        float randCookTime = Random.Range(minCookTime, maxCookTime);
        float randFillValue = randCookTime * 0.05f;
        // Choose a random location
        Vector3 newLocation = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
        GameObject newFoodItem = Instantiate(foodPrefab, newLocation, Quaternion.identity) as GameObject;
        newFoodItem.GetComponent<Pickup>().SetFillValue(randFillValue);
        newFoodItem.GetComponent<Pickup>().SetCookTime(randCookTime);
        objectCount++;
    }

    IEnumerator SpawnObjects() {
        waitTime = Random.Range(1, 5);

        yield return new WaitForSeconds(waitTime);

        // If you have room
        if (objectCount < maxObjects) {
            CreateFoodItem();
        }

        StartCoroutine("SpawnObjects");
    }

    IEnumerator RollCredits() {
        yield return new WaitForSeconds(creditWaitTime);

        // Set credit wait time to 3 seconds
        creditWaitTime = 2;
        // Turn on the instruction
        credits[creditCounter].SetActive(true);
        if (creditCounter != 0) { credits[creditCounter - 1].SetActive(false); }
        // If there are more to be shown - show the next one and hide the last
        if (creditCounter < credits.Length - 1) {
            creditCounter++;
            StartCoroutine("RollCredits");
        }
        else {
            credits[creditCounter].SetActive(false);
            logo.SetActive(true);
        }
    }

    public void DecreaseObjectCount() {
        objectCount--;
    }

    public void EndGame(int winningPlayer) {
        if (!gameOver) {
            gameOver = true;
            EndPanel.SetActive(true);
            logo.SetActive(false);

            if (winningPlayer == 0) {
                playerWinText.text = "Player 1";
                playerLoseText.text = "Player 2";
            } else {
                playerWinText.text = "Player 2";
                playerLoseText.text = "Player 1";
            }
        }
    }

    public bool IsOver() {
        return gameOver;
    }
}
