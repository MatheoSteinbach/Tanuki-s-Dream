using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatArena : MonoBehaviour
{
    [SerializeField] AudioSource combatMusic;
    [SerializeField] AudioSource normalMusic;
    private List<AIController> enemies = new List<AIController>();
    private CombatDoor[] doors;
    private bool doorsAreClosed = false;
    private BoxCollider2D boxCollider;
    private CameraShake camShake;
    private void Awake()
    {
        if (GetComponentsInChildren<CombatArena>() != null)
        {
            var enemiesArray = GetComponentsInChildren<AIController>();
            enemies.AddRange(enemiesArray);
        }
        if (GetComponentsInChildren<CombatDoor>() != null)
        {
            doors = GetComponentsInChildren<CombatDoor>();
        }
        boxCollider = GetComponent<BoxCollider2D>();
            
    }

    private void Start()
    {
        var player = FindObjectOfType<PlayerMovement2D>().gameObject;
        camShake = player.GetComponentInChildren<CameraShake>();
        //camShake = playerCam.GetComponent<CameraShake>();
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        var enemiesToRemove = new List<AIController>();
        if(enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.IsDead)
                {
                    enemiesToRemove.Add(enemy);
                }
            }
        }
        else if(enemies.Count == 0 && doorsAreClosed)
        {
            combatMusic.Stop();
            normalMusic.Play();
            foreach (var door in doors)
            {
                door.Open();
            }
            doorsAreClosed = false;
        }
        foreach (var enemy in enemiesToRemove)
        {
            enemies.Remove(enemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            camShake.ShakeCamera(5, 0.4f);
            combatMusic.Play();
            normalMusic.Stop();
            foreach (var enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
                enemy.Spawn();
            }
            foreach (var door in doors)
            {
                door.Close();
                doorsAreClosed = true;
            }
            boxCollider.enabled = false;
        }
    }
}
