using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticleRotation : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private Dictionary<int, Vector3> lockedRotations = new Dictionary<int, Vector3>();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void OnParticleCollision(GameObject other)
    {
        int numAlive = ps.GetParticles(particles);

        for (int i = 0; i < numAlive; i++)
        {
            int id = i;

            if (!lockedRotations.ContainsKey(id))
            {
                lockedRotations[id] = particles[i].rotation3D;
            }

            particles[i].rotation3D = lockedRotations[id];
            particles[i].angularVelocity3D = Vector3.zero;
        }

        ps.SetParticles(particles, numAlive);
    }
}
