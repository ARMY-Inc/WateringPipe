using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMY.MasterData;

public class PipeScript : MonoBehaviour
{
    private Quaternion quaternion;
    [HideInInspector] public List<PipeScript> connectedPipes; //the pipes we are connected to. (used to detect if we are connected to the water or not.)
    private Transform StartPoint; //detects if this pipe is connect to the hydrant. (prevents _connectedToHydrant becoming false)
    private Transform EndPoint; //detects if this pipe is connect to the flower pot. (prevents _connectedToFlower becoming false)
    [Tooltip("Does this pipe connect to the flower pot?")]
    public bool _IsEndPipe; //does this pipe connect to the flowerpot? (all _IsEndPipes must be marked as true to win.)
    [HideInInspector] public bool _connectedToHydrant; //are we connected to the hydrant
    [HideInInspector] public bool _connectedToFlower; //are we connected to the flower
    [HideInInspector] public bool _turnedBlue; //turns blue to show that water is going through it.
    private bool _canTurn = true; //have we finished turning yet?
    [HideInInspector] public Renderer rend; //we need to access the renderer.material and make it turn blue.

    //This is used for EndPoints
    [Header("These are used for End Point pipes")]
    public GameObject waterPrefab; //spawns the water particle system.
    private GameObject curWater; //the water that we see currently.
    public float distanceAdjust = 2.0f; // we need to adjust the distance between the water and the pipe
    public float heightAdjust = 2.0f; // we need to adjust the height between the water and the pipe
    [HideInInspector] public bool _spawnedWater; //prevents rapid fire spawning of water.


    // Start is called before the first frame update
    void Start()
    {
        quaternion = this.transform.rotation;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RotateThePipe();
        CheckIfConnectedToHydrant();
        CheckIfConnectedToWater();
        SpawnWaterPrefab();
    }

    private void RotateThePipe()
    {
        if (this.transform.rotation != quaternion)
        {
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, quaternion, Time.deltaTime * MasterDataManager.Instance.Data.pipeRotateSpeed);
        }
        else
        {
            _canTurn = true;
        }
        quaternion = Quaternion.Euler(this.transform.eulerAngles.x, Mathf.Abs(quaternion.eulerAngles.y), this.transform.eulerAngles.z);
    }

    public void MakePipeRotate()
    {
        if (_canTurn == true)
        {
            _canTurn = false;
            quaternion = Quaternion.Euler(this.transform.eulerAngles.x, Mathf.Abs(quaternion.eulerAngles.y + 90), this.transform.eulerAngles.z);
        }
    }

    void CheckIfConnectedToHydrant()
    {
        if (StartPoint == null)
        {
            _connectedToHydrant = false;
        }
        for (int i = 0; i < connectedPipes.Count; i++)
        {
            if (connectedPipes[i]._connectedToHydrant == true)
            {
                _connectedToHydrant = true;
            }
            if (connectedPipes[i]._turnedBlue == true)
            {
                StartCoroutine(TurnPipeBlue());
            }
        }
    }

    void CheckIfConnectedToWater()
    {
        if (EndPoint == null)
        {
            _connectedToFlower = false;
        }
        for (int i = 0; i < connectedPipes.Count; i++)
        {
            if (connectedPipes[i]._connectedToFlower == true)
            {
                _connectedToFlower = true;
            }
        }
    }

    IEnumerator TurnPipeBlue()
    {
        if (_turnedBlue == false)
        {
            yield return new WaitForSeconds(MasterDataManager.Instance.Data.pipeWaterSpeed);
            _turnedBlue = true;
            rend.materials[1].SetColor("_BaseColor", Color.cyan);
        }
    }

    public void SpawnWaterPrefab()
    {
        if (_IsEndPipe == true)
        {
            if (_spawnedWater == false && _turnedBlue == true)
            {
                _spawnedWater = true;
                curWater = Instantiate<GameObject>(waterPrefab, this.transform);
            }
            if (curWater != null)
            {
                Vector3 newPos = (this.transform.position + EndPoint.position) / distanceAdjust;
                newPos.y = heightAdjust;
                curWater.transform.position = newPos;
                curWater.transform.LookAt(EndPoint.position);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("pipe") == true)
        {
            if (connectedPipes.Contains(other.GetComponent<PipeScript>()) == false)
            {
                if (other.GetComponent<PipeScript>()._connectedToHydrant == true)
                {
                    _connectedToHydrant = true;
                    connectedPipes.Add(other.GetComponent<PipeScript>());
                }
                if (other.GetComponent<PipeScript>()._connectedToFlower == true && _IsEndPipe == false)
                {
                    _connectedToFlower = true;
                    connectedPipes.Add(other.GetComponent<PipeScript>());
                }
            }
        }
        else if (other.CompareTag("hydrant") == true)
        {
            _connectedToHydrant = true;
            StartPoint = other.transform;
        }
        else if (other.CompareTag("flower") == true && _IsEndPipe == true)
        {
            _connectedToFlower = true;
            EndPoint = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Hydrant.CheckConnections(); //sometimes it doesnt register leaving the pipe quick enough. This prevents that.
        if (other.CompareTag("pipe") == true)
        {
            if (connectedPipes.Contains(other.GetComponent<PipeScript>()) == true)
            {
                if (other.GetComponent<PipeScript>()._connectedToHydrant == true)
                {
                    _connectedToHydrant = false;
                }
                if (other.GetComponent<PipeScript>()._connectedToFlower == true)
                {
                    _connectedToFlower = false;
                }
                connectedPipes.Remove(other.GetComponent<PipeScript>());
            }
        }
        else if (other.CompareTag("hydrant") == true)
        {
            _connectedToHydrant = false;
            StartPoint = null;
        }
        else if (other.CompareTag("flower") == true)
        {
            _connectedToFlower = false;
            EndPoint = null;
        }
    }
}
