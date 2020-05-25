using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPotAndWater : MonoBehaviour
{
    public List<PipeScript> thepipes; //the pipes that are currently connected to the waterspot.
    public bool _spawnedWater; //prevents rapid fire spawning of water.


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("pipe") == true)
        {
            if (thepipes.Contains(other.GetComponent<PipeScript>()) == false && _spawnedWater == false)
            {
                if (other.GetComponent<PipeScript>()._turnedBlue == true )
                {
                    _spawnedWater = true;
                }
            }
        }
    }

}
