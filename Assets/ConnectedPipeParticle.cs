using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedPipeParticle : MonoBehaviour
{
    //plays the sparks animation whenver we connect pipes.
    public ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sparks"))
        {
            ps.Play();
        }
    }
}
