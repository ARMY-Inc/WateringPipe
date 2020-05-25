using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : MonoBehaviour
{
    public bool _connectedToFlower; //are we connected to the flower
    public PipeScript[] allpipes; //check if all the pipes are connected to the flower pot.
    private static Hydrant instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        allpipes = FindObjectsOfType<PipeScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        _connectedToFlower = true;
        if (other.CompareTag("pipe") == true)
        {
            for (int i=0;i<allpipes.Length;i++)
            {
                if (allpipes[i]._connectedToFlower == false || allpipes[i]._connectedToHydrant == false)
                {
                    _connectedToFlower = false;
                }
            }
            if (_connectedToFlower == true)
            {
                other.GetComponent<PipeScript>()._turnedBlue = true; //show that we are doing the water.
                other.GetComponent<PipeScript>().rend.materials[1].SetColor("_BaseColor", Color.cyan);
                GameManager._canPlayGame = false;
            }
        }
    }
   public static void CheckConnections()
    {
        for (int i = 0; i < instance.allpipes.Length; i++)
        {
            instance.allpipes[i]._connectedToFlower = false; //sometimes it doesnt register leaving the pipe quick enough.
            instance.allpipes[i]._connectedToHydrant = false; //sometimes it doesnt register leaving the pipe quick enough.
        }
    }
}
