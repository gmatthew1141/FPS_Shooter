using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status
{
    [SerializeField] private HealthBar healthBar;
    private GameSetting gameSetting;
    private GameManager gameManager; 
    private PlayerState playerState = PlayerState.ALIVE;
    private int activeGunIndex;

    private void Start() {
        gameSetting = FindObjectOfType<GameSetting>();
        gameManager = FindObjectOfType<GameManager>();
        healthBar.SetMaxHealth(gameSetting.maxPlayerHealth);
        activeGunIndex = 0;
    }

    private void Update() {
        if (health <= 0) {
            Dead();
        }
    }

    public override void TakeDamage(float damage, float knockbackForce) {
        // play take damage effect
        health -= damage;
        // decrease health bar
        healthBar.SetHealth(health);
    }

    public override void Dead() {
        // show dead UI
        Debug.Log("Player is dead");
        playerState = PlayerState.DEAD;
        gameManager.PlayerDead();
    }

    public void ResetStatus() {
        health = gameSetting.maxPlayerHealth;
        playerState = PlayerState.ALIVE;
        healthBar.SetMaxHealth(gameSetting.maxPlayerHealth);
    }

    public PlayerState GetPlayerState() {
        return playerState;
    }

    public int GetCurrGunIndex() {
        return activeGunIndex;
    }

    public void UpdateGunIndex(int index) {
        activeGunIndex = index;
    }
}
