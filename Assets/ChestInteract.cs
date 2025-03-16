using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestInteract : MonoBehaviour {
    GameManager gameManager;
    BoxCollider col;
    Animator animator;
    [SerializeField] TextMeshPro code;
    [SerializeField] GameObject wall_code;

    private void Awake() {
        col = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other) {
        animator.SetTrigger("playerNearby");
        StartCoroutine(ShowCode());
    }

    IEnumerator ShowCode() {
        yield return new WaitForSeconds(0.65f);
        code.gameObject.SetActive(true);

        //yield return new WaitForSeconds(0.5f);
        //wall_code.gameObject.SetActive(true);
    }
}
