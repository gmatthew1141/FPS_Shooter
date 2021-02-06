using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class RocketScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPart;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start() {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Collider collider = other.GetComponent<Collider>();
        
        for (int i = 0; i < numCollisionEvents; i++) {
            PlayExplosionParticles(collisionEvents[i]);
        }

    }

    private void PlayExplosionParticles(ParticleCollisionEvent particleCollisionEvent) {
        CameraShaker.Instance.ShakeOnce(10f, 10f, .2f, 2f);
        var hitPos = particleCollisionEvent.intersection;
        Vector3 pos = new Vector3(hitPos.x, hitPos.y, hitPos.z - 1f);
        var explosion = Instantiate(explosionPart, hitPos, Quaternion.identity);
        explosion.Play();
        DestroyAfter(explosion);
    }

    private void DestroyAfter(ParticleSystem obj) {
        Destroy(gameObject);
        Destroy(obj, 2f);
    }
}
