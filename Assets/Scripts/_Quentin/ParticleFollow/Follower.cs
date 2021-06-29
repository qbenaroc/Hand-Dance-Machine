//		Path following particle system script
//		Set up 6 empty objects for the particles to follow
//     Copyright (c) Vincent DeLuca 2014.  All rights reserved.
//

using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{

    //setting up your 6 targets
    public Transform target1;
    public Transform target2;
    public Transform target3;
    public Transform target4;
    public Transform target5;
    public Transform target6;

    void Start()
    {

    }

    void Update()
    {
        Trail();
    }

    void Trail()
    {
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount + 1];
        int l = GetComponent<ParticleSystem>().GetParticles(p);

        Vector3 D1 = target1.position - transform.position;
        Vector3 D2 = target2.position - target1.position;
        Vector3 D3 = target3.position - target2.position;
        Vector3 D4 = target4.position - target3.position;
        Vector3 D5 = target5.position - target4.position;
        Vector3 D6 = target6.position - target5.position;

        int i = 0;
        while (i < l)
        {
            //setting the velocity of each particle from target to target
            if (p[i].remainingLifetime < (p[i].startLifetime / 12))
            {
                p[i].velocity = 6f / p[i].startLifetime * D6;
            }
            else if (p[i].remainingLifetime < ((3 * p[i].startLifetime) / 12))
            {
                float t = ((p[i].startLifetime / 6) - (p[i].remainingLifetime - (p[i].startLifetime / 12))) / (p[i].startLifetime / 6);
                p[i].velocity = 6f / p[i].startLifetime * Bezier(D5, D6, t);
            }
            else if (p[i].remainingLifetime < ((5 * p[i].startLifetime) / 12))
            {
                float t = ((p[i].startLifetime / 6) - (p[i].remainingLifetime - ((3 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                p[i].velocity = 6f / p[i].startLifetime * Bezier(D4, D5, t);
            }
            else if (p[i].remainingLifetime < ((7 * p[i].startLifetime) / 12))
            {
                float t = ((p[i].startLifetime / 6) - (p[i].remainingLifetime - ((5 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                p[i].velocity = 6f / p[i].startLifetime * Bezier(D3, D4, t);
            }
            else if (p[i].remainingLifetime < ((9 * p[i].startLifetime) / 12))
            {
                float t = ((p[i].startLifetime / 6) - (p[i].remainingLifetime - ((7 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                p[i].velocity = 6f / p[i].startLifetime * Bezier(D2, D3, t);
            }
            else if (p[i].remainingLifetime < ((11 * p[i].startLifetime) / 12))
            {
                float t = ((p[i].startLifetime / 6) - (p[i].remainingLifetime - ((9 * p[i].startLifetime) / 12))) / (p[i].startLifetime / 6);
                p[i].velocity = 6f / p[i].startLifetime * Bezier(D1, D2, t);
            }
            else
            {
                p[i].velocity = 6f / p[i].startLifetime * D1;
            }
            i++;
        }

        GetComponent<ParticleSystem>().SetParticles(p, l);
    }

    //this is the math to smooth out the path, known as bezier curves
    private Vector3 Bezier(Vector3 P0, Vector3 P2, float t)
    {
        Vector3 P1 = (P0 + P2) / 2f;
        Vector3 B;
        B = (1f - t) * ((1f - t) * P0 + t * P1) + t * ((1f - t) * P1 + t * P2);
        return B;
    }
}