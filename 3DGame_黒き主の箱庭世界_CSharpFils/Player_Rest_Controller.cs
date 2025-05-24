using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Player_Rest_Controller : Player
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

        //charaCon.enabled = false;

        //if (flagmentData.PositionLoad)
        //{
        //    transform.position = playerTransformData.LoadTransform;
        //    flagmentData.PositionLoad = false;
        //}
        //else
        //{
        //    transform.position = startPos;
        //}

        //if (flagmentData.RotateLoad)
        //{
        //    transform.rotation = playerTransformData.LoadRotate;
        //    flagmentData.RotateLoad = false;
        //}
        //else
        //{
        //    transform.rotation = startRotate;
        //}





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
