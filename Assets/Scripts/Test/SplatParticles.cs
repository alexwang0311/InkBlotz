using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatParticles : MonoBehaviour {
    public ParticleSystem splatParticles;
    public GameObject[] splatPrefabs;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(splatParticles,other, collisionEvents);

        int count = collisionEvents.Count;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, splatPrefabs.Length);
            GameObject splat = Instantiate(splatPrefabs[randomIndex], collisionEvents[i].intersection, Quaternion.identity) as GameObject;
            Vector3 scaler = splat.transform.localScale;
            scaler.x *= 50;
            scaler.y *= 5;
            splat.transform.localScale = scaler;
        }
    }
}
