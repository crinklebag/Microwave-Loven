using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MicrowaveController))]
public class MicrowaveInput : MonoBehaviour {

    GameController gameController;

    [SerializeField] int playerID;
    Player player;

	// Use this for initialization
	void Start () {
        player = ReInput.players.GetPlayer(playerID);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveDirection = Vector3.zero;

        // Left/Right
        if (player.GetAxisRaw("Horizontal") > 0) {
            
            moveDirection += new Vector3(0.1f, 0, 0);
        }
        else if (player.GetAxisRaw("Horizontal") < 0) {
            
            moveDirection += new Vector3(-0.1f, 0, 0);
        }

        // Foreward/Backward
        if (player.GetAxisRaw("Vertical") > 0) {
            
            moveDirection += new Vector3(0, 0, 0.1f);
        }
        else if (player.GetAxisRaw("Vertical") < 0) {
            
            moveDirection += new Vector3(0, 0, -0.1f);
        }

        // Throw
        if (player.GetButton("A Button")) {
            Debug.Log("Holding Button");
            this.GetComponent<MicrowaveController>().IncreaseForce();
        }

        if (player.GetButtonUp("A Button")) {
            Debug.Log("Release Button");
            this.GetComponent<MicrowaveController>().ThrowObject();
        }

        if (player.GetButtonDown("A Button") && gameController.IsOver()) {
            SceneManager.LoadScene(0);
        }

        if (moveDirection != Vector3.zero) {

            this.GetComponent<Rigidbody>().MovePosition(this.transform.position + moveDirection);
        }

        RotateMicrowave();
	}

    void RotateMicrowave()  {
        float horz = player.GetAxisRaw("Horizontal"); //grabs the float value for the joystick on its x axis
        float vert = player.GetAxisRaw("Vertical"); //grabs the float value for the joystick on its x axis
        float tarAngle = Mathf.Atan2(vert, horz) * Mathf.Rad2Deg; //converting the joystick vectors into a rotational angle around the z axis

        if (player.GetAxisRaw("Vertical") != 0 || player.GetAxisRaw("Horizontal") != 0) {
            
            this.transform.rotation = Quaternion.Euler(0, 270 - tarAngle, 0); //changes the z rotation of the pointer to the z rotation of the joystick. (-90) to sync unity rotaion values and controller values
        }
    }

    public int GetPlayerID() {
        return playerID;
    }
}
