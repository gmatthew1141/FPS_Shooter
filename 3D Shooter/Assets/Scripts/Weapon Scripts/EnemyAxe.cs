using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    private float axeDamage;
    private GameSetting gameSetting;

    private void Awake() {
        gameSetting = FindObjectOfType<GameSetting>();

        axeDamage = gameSetting.axeDamage;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            Debug.Log("Player hit");
            var status = other.GetComponent<Status>();

            status.TakeDamage(axeDamage, 0f);
        }
    }
}

