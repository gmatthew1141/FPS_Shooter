using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{

    [SerializeField] protected float health;
    protected bool dead;

    public abstract void TakeDamage(float damage, float knockbackForce);

    public abstract void Dead();

    public bool IsDead() {
        return dead;
    }
}
