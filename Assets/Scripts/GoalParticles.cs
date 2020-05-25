using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticles : MonoBehaviour
{
    public ParticleSystem[] ps;
    private bool _particlesPlayed;

    // Update is called once per frame
    void LateUpdate()
    {
        if (_particlesPlayed == false && GameManager._weWon == true)
        {
            _particlesPlayed = true;
            foreach (ParticleSystem child in ps)
            {
                child.Play();
            }
          //  ps[0].Play();
         //   ps[1].Play();
        }
    }
}
