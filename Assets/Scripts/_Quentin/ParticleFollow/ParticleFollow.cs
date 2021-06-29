using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public float speed = 0.1f;

    ParticleSystem _ps;
    ParticleSystem.Particle[] particles;

    private float[] dstTravelled;

    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[_ps.main.maxParticles];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int numParticleAlive = _ps.GetParticles(particles);

        dstTravelled = new float[numParticleAlive];

        for (int i = 0; i < numParticleAlive; i++)
		{
            ParticleSystem.Particle p = particles[i];
            if (pathCreator != null)
			{
                dstTravelled[i] += Time.deltaTime * speed;
                p.position = pathCreator.path.GetDirectionAtDistance(dstTravelled[i], end);
            }
            particles[i] = p;
		}

        _ps.SetParticles(particles, particles.Length);
    }
}