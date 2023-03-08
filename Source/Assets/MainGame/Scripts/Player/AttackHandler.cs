using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] float attackDelay = 0.5f;

    private bool attackBlocked;

    Vector2 pointerInput;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    PlayerWeapon weapon;
}
