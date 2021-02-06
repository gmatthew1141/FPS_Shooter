using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    private GameSetting gameSetting;
    private float blastRadius;
    private float force;

    // Start is called before the first frame update
    void Start()
    {
        gameSetting = FindObjectOfType<GameSetting>();
        blastRadius = gameSetting.rocketBlastRadius;
        force = gameSetting.rocketExplosionForce;

        Explode();
    }

    private void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders) {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            Status status = nearbyObject.GetComponent<Status>();

            if (rb != null) {
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }

            if (status != null) {
                status.TakeDamage(gameSetting.rocketDamage, 0f);
            }
        }
        
    }
}
