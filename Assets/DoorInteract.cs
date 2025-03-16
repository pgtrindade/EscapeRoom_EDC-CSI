using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour {
    GameManager gameManager;
    BoxCollider col;
    GameObject promptUI;

    bool isPlayerInTrigger = false;
    [SerializeField] bool correctDoor;

    private void Awake() {
        col = GetComponent<BoxCollider>();
    }

    void Start() {
        gameManager = FindObjectOfType<GameManager>();

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null) {
            promptUI = canvas.transform.Find("PromptText").gameObject;
            if (promptUI != null) {
                promptUI.SetActive(false); // Ensure it's hidden initially
            } else {
                Debug.LogWarning("Prompt UI Text not found in the Canvas!");
            }
        } else {
            Debug.LogWarning("Canvas not found in the scene!");
        }
    }

    void Update() {
        if (isPlayerInTrigger && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)))
            OpenDoor();
    }

    void OpenDoor() {
        if (!correctDoor) {
            // TODO: Level Down Sound
            gameManager.Respawn(); 
            return;
        }

        gameManager.NextLevel();
        // TODO: Level Up Sound
    }

    private void OnTriggerEnter(Collider other) {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player")) {
            isPlayerInTrigger = true;
            promptUI.SetActive(true); // Show the UI prompt
        }
    }

    private void OnTriggerExit(Collider other) {
        // Check if the player exits the trigger zone
        if (other.CompareTag("Player")) {
            isPlayerInTrigger = false;
            promptUI.SetActive(false); // Hide the UI prompt
        }
    }
}
