using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float[] enemiesHealth;

    [Header("Axe Skeleton")]
    public float axeDamage;
    
    [Header("Bomber Skeleton")]
    public float bomberDamage;
    public float bomberKnockbackForce;
    public float bombRadius;

    [Header("ThrowingAxeSkeleton")]
    public float throwingAxeDamage;
    public GameObject throwingAxePrefab;

    [Header("Player Settings")]
    public Transform player;
    public float maxPlayerHealth;
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpingForce;
    public float mouseSensitivity;
    public float recoilSpeed;
    public float rocketDamage;
    public float rocketBlastRadius;
    public float rocketExplosionForce;

}

