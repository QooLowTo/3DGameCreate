using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

/// <summary>
/// バトル外でのプレイヤーの動きを制御するクラスです。
/// </summary>
public class PlayerRestController : Player
{
    private HomeMapManager homeMapManager;
    [SerializeField]
    private GameObject findHomeMapManager;

    private void Start()
    {
        plaIn = GetComponent<PlayerInput>();

        plasta = GetComponent<Player_Status_Controller>();

        charaCon = GetComponent<CharacterController>();

        animCon = GetComponent<Animator>();

        homeMapManager = findHomeMapManager.GetComponent<HomeMapManager>();

        soundManager = findSoundManager.GetComponent<SoundManager>();

        StartPlayerSet();

        StartCoroutine(Starting());   

    }

    

    private void FixedUpdate()
    {
        if (!gameStart) return;

        CheckGroundPlayerList();

        PlayerMoveInputControl();

        GravityLoad();

        PlayerMoveControl();

        PlayerGravityControl();


        if (landing && OnGroundLayer(false))
        {
            if (starting) return;

            Fall();

        }

        if (!landing && OnGroundLayer(true))
        {

            landing = true;

            Grounded();

        }

    }
}
