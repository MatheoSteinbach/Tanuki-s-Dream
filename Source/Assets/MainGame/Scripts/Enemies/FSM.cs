using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    protected Transform playerTransform;
    protected Vector3 destinationPos;
    [SerializeField] protected GameObject[] wanderPoints;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        FSMUpdate();
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void FSMUpdate()
    {

    }


}
