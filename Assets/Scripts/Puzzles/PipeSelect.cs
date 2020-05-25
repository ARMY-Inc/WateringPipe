using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSelect : MonoBehaviour
{
    public Hydrant hydrant; //acheck if the hydrant is connected to the flower pot via pipes.
    public PipeScript[] allpipes; //all the pipes in the scene.
    public FlowerPotAndWater flower; //check if the water has reached the flowerpot yet.
    public bool _levelIsFinished; // checks if we finished this level or not.
    public bool _startConfetti; //start the confetti and Winning functions. (also prevents rapid fire calls)
    public Animator flowerBloom; //the animator that lets the flower bloom.

    // Start is called before the first frame update
    void Start()
    {
        hydrant = FindObjectOfType<Hydrant>();
        flower = FindObjectOfType<FlowerPotAndWater>();
        allpipes = FindObjectsOfType<PipeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePipe();
        WinLoseChecker();
    }


    void RotatePipe()
    {
        if (Input.touchCount > 0 && GameManager._canPlayGame == true)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Vector3.zero; //where our finger is, in relation to the world.
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), Camera.main.ScreenPointToRay(touch.position).direction * 50, out hit, 200))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    hit.transform.SendMessage("MakePipeRotate", null, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    void WinLoseChecker()
    {
        _levelIsFinished = true;
        if (hydrant._connectedToFlower == false)
        {
            _levelIsFinished = false;
        }

        for (int i=0;i<allpipes.Length;i++)
        {
            if (allpipes[i]._connectedToFlower == false || allpipes[i]._connectedToHydrant == false)
            {
                _levelIsFinished = false;
            }
        }


        if (_startConfetti == false && _levelIsFinished == true && flower._spawnedWater == true)
        {
            _startConfetti = true;
            flowerBloom.enabled = true;
            StartCoroutine(GameManager.WeWonFunction());
        }
    }
}
