using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainingDummy : MonoBehaviour
{
    [SerializeField] Sprite deadSprite;

    public void Dead()
    {
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
