using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class MicrowaveInput : MonoBehaviour {
    
    [SerializeField] int playerID;
    Player player;

    Pickup heldPickup;

	// Use this for initialization
	void Start () {
        player = ReInput.players.GetPlayer(playerID);
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

    void ThrowObject() {
        // Collect Forces the longer the A button is being held
    }

    public void PickupObject(Pickup newObject) {
        heldPickup = newObject;
    }
}
