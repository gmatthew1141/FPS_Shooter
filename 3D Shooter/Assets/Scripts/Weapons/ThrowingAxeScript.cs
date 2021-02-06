using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAxeScript : MonoBehaviour {

    private Rigidbody axeRB;
    private float axeDamage = 0f;
    private float knockbackForce = 0f;

    [SerializeField] private float destroyAfterTime = 20f;
    [SerializeField] private float bulletForce = 20f;
    
    // Start is called before the first frame update
    void Awake() {
        axeRB = GetComponent<Rigidbody>();
    }

    private void Start() {
        // automatically destroy bullets after certain time passed
        StartCoroutine(DestroyAfter());
    }

    public void ThrowForward(Quaternion rotation) {
        // Calculate the direction of the force to be applied to the bullet 
        Quaternion startingAngle = Quaternion.AngleAxis(0f, Vector3.up);
        var angle = rotation * startingAngle;
        var direction = angle * Vector3.forward;

        axeRB.AddForce(direction * bulletForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.transform.tag == "Player") {
            var status = other.transform.GetComponent<Status>();
            status.TakeDamage(axeDamage, knockbackForce);

            // destroy object after hit
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfter() {
        //Wait for set amount of time
        yield return new WaitForSeconds(destroyAfterTime);
        //Destroy bullet object
        Destroy(gameObject);
    }

    public void SetAxedamage(float damage) {
        axeDamage = damage;
    }

    public void SetKnockbackForce(float knockback) {
        knockbackForce = knockback;
    }
}
