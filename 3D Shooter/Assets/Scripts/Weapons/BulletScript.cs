using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private ParticleSystem particle;
    private Rigidbody bulletRB;
    private float bulletDamage = 0f;
    private float knockbackForce = 0f;

    [SerializeField] private float destroyAfterTime = 20f;
    [SerializeField] private float bulletForce = 20f;



    // Start is called before the first frame update
    void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
        particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        //particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() {
        // automatically destroy bullets after certain time passed
        StartCoroutine(DestroyAfter());
    }

    public void ShootForward(Quaternion rotation) {
        // Calculate the direction of the force to be applied to the bullet 
        Quaternion startingAngle = Quaternion.AngleAxis(0f, Vector3.up);
        var angle = rotation * startingAngle;
        var direction = angle * Vector3.forward;

        bulletRB.AddForce(direction * bulletForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {

        float damageMultiplier = 1f;
        particle.Play();

        if (other.transform.tag == "Enemy") {
            // damage enemy
            var status = other.transform.GetComponent<Status>();
            // extra damage if player shoot heads
            if (other.transform.name == "MetalJaw") {
                damageMultiplier = 1.5f;
            }
            status.TakeDamage(bulletDamage * damageMultiplier, knockbackForce);

            // destroy object after hit
            Destroy(gameObject, 1f);
        } else {
            // play hit animation/hit effect
            // destroy bullet
        }
    }

    private IEnumerator DestroyAfter() {
        //Wait for set amount of time
        yield return new WaitForSeconds(destroyAfterTime);
        //Destroy bullet object
        Destroy(gameObject);
    }

    public void SetBulletDamage(float damage) {
        bulletDamage = damage;
    }

    public void SetKnockbackForce(float knockback) {
        knockbackForce = knockback;
    }
}
