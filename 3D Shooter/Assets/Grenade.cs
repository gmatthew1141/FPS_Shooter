using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float delay = 3f;

    float countdown;
    bool hasExploded;

    public GameObject explosionEffect;
    public float blastRadius = 5f;
    public float force = 700f;

    // Start is called before the first frame update
    void Start()
    {
        hasExploded = false;
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {

        countdown -= Time.deltaTime;

        if (countdown <= 0 && !hasExploded) {
            Explode();
            hasExploded = true;
        }
        
    }

    private void Explode() {
        Debug.Log("Explode");
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in colliders) {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (rb != null) {
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }
        }

        Destroy(gameObject);
    }
}
