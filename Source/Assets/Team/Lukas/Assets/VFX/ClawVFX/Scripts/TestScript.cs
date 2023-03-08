using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestScript : MonoBehaviour
{
    [SerializeField] VisualEffect _AttackVFX;

    private void Update()
        {
            if (Input.GetMouseButtonUp(0))
                PlayParticle(); 
        }

    void PlayParticle()
      {
        _AttackVFX.Play();
      }
}