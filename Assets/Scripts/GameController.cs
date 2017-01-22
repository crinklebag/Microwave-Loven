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

    [Header("Object Variables")]
    [SerializeField] float minCookTime;
    [SerializeField] float maxCookTime;
    [SerializeField] int maxObjects;
    [SerializeField] float maxX;
    [SerializeField] float minX;
    [SerializeField] float maxZ;
    [SerializeField] float minZ;

    int objectCount = 0;
    float waitTime = 0;
    bool gameOver = false;
    bool gameStarted = false;

	// Use this for initialization
	void Start () {
        StartCoroutine("SpawnObjects");
	}
	
    void CreateFoodItem() {
        // Choose random values for the fill and cook time for the new item
        float randCookTime = Random.Range(minCookTime, maxCookTime);
        float randFillValue = randCookTime * 0.1f;
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

    public void DecreaseObjectCount() {
        objectCount--;
    }

    public void EndGame(int winningPlayer) {
        if (!gameOver) {
            gameOver = true;
            EndPanel.SetActive(true);

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
