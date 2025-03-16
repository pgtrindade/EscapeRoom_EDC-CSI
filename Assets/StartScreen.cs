using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

    [SerializeField] string gameScene;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (gameScene != null) {
                SceneManager.LoadScene(gameScene);
            }
        }
    }
}
