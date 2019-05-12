﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private CameraController camera;
    [SerializeField] private float fallDeath = -10;
    [SerializeField] private float holdRespawnDelay = 0.25f;

    private Timer timer;
    private ScoreMenu scoreMenu;
    private ScreenShakeTest screnshake;
    private Vector3 spawnPos;

    void Start()
    {
        timer = GameObject.FindObjectOfType<Timer>();
        spawnPos = player.transform.position;
        screnshake = Camera.main.GetComponent<ScreenShakeTest>();
        scoreMenu = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreMenu>();
        scoreMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RestartTimer());
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            StopAllCoroutines();//StopCoroutine(RestartTimer());
        }
    }

    void FixedUpdate()
    {
        if (player.transform.position.y < fallDeath && player.GetMovementState())
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {

        foreach (MovingObject mo in GameObject.FindObjectsOfType<MovingObject>())
        {
            mo.Reset();
        }
        player.transform.position = spawnPos;
        player.StopMovement();
        player.DestroyRope();
        player.ClearLine();
        camera.ResetCamera();
        timer.ResetTime();

        screnshake.AddShake(Vector2.up, 0.5f);
        GameObject.FindGameObjectWithTag("GameplayManager").GetComponent<ParticleManager>().playerRespawn();

    }

    public void LevelCleared()
    {
        scoreMenu.gameObject.SetActive(true);
        scoreMenu.ShowScreen();
        scoreMenu.crosshair.enabled = false;
        camera.UnlockMouse();
        timer.StopTimer();
    }

    private IEnumerator RestartTimer()
    {
        yield return new WaitForSeconds(holdRespawnDelay);
        RespawnPlayer();
    }
}