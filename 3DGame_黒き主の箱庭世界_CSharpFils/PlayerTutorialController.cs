using DG.Tweening;
using Fungus;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

/// <summary>
/// チュートリアル中のプレイヤーの動きを制御するクラスです。
/// </summary>
public class PlayerTutorialController : PlayerBattleController
{

    private Player_Camera_Controller playerCameraCon;

    private Player_UI_Controller playerUICon;

    private TutorialManager tutorialManager;

    [SerializeField]
    private GameObject findTutorialManager;

    [SerializeField]
    private GameObject hpUI;

    [SerializeField]
    private GameObject lockon;

    private float animCount = 1f;

    private bool stopAttack = true;

    void Start()
    {
        StartCoroutine(Starting());

        playerCameraCon = gameObject.GetComponent<Player_Camera_Controller>();

        playerUICon = gameObject.GetComponent<Player_UI_Controller>();

        plasta = GetComponent<Player_Status_Controller>();

        plaIn = GetComponent<PlayerInput>();

        soundManager = findSoundManager.GetComponent<SoundManager>();

        tutorialManager = findTutorialManager.GetComponent<TutorialManager>();

        battleManager = findBattleManager.GetComponent<BattleManager>();

        charaCon = GetComponent<CharacterController>();

        animCon = GetComponent<Animator>();

        rig = GetComponent<RigBuilder>();

        StartPlayerSet();

        dashInput = plaIn.actions["Dash"];



        returnSpeed = playerSpeed;

        myWepon.SetActive(false);



    }

    void FixedUpdate()
    {
       
        if (gameOver || !gameStart || !charaCon.enabled) return;

        CheckGroundPlayerList();

        if (!attacking && !jumpAttackingFall && !isDown && !avoidancing && !isChanegeAction && !hitting)
        {
     
            PlayerMoveInputControl();
        }
        else
        {
            animCon.SetFloat("MoveInput", 0);
        }

        GravityLoad();

        if (avoidancing)
        {
            if (myWepon.GetComponent<BoxCollider>().enabled)
            {
                OffCollider();
            }

            if (weponEffect.GetComponent<TrailRenderer>().emitting)
            {
                OffEffect();
            }
        }

        if (isDown || hitting)
        {
            AllCancel();
        }


        if (!isChanegeAction && !attacking && !isDown && !hitting)
        {

            PlayerMoveControl();

            if (moveInputAbs != 0 && !isJumpping && groundPlayerList[1]/*階段だったら*/)
            {

                MovingLimit();

            }
        }


        PlayerGravityControl();


        var ispressed = dashInput.IsPressed();

        if (ispressed && moveInputAbs != 0)
        {

            OnDash();

        }
        else
        {

            OffDash();

        }

        if ((moveInputAbs != 0) && !stopAttack && !isChanegeAction && !attacking && !isJumpping && !avoidancing && !isDown && OnGroundLayer(true))
        {
            rig.enabled = true;
        }
        else
        {
            rig.enabled = false;
        }

        if (landing && OnGroundLayer(false))
        {

            if (starting || hitting) return;


            Fall();
        }


        if (!landing && OnGroundLayer(true))
        {

            landing = true;

            if (jumpAttackingFall)
            {

                AttackGrounded();

            }
            else
            {

                Grounded();
            }
        }

        if (!attacking && stopCombo)
        {

            stopCombo = false;

        }

        if (plasta.LivePlayerHP <= 0)
        {

            Die();
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tutorial")
        {
            if (tutorialManager.MoveTutorialing || tutorialManager.CameraTutorialing)
            {
                tutorialManager.TutorialObject[1].SetActive(false);

                tutorialManager.TutorialFlow.SendFungusMessage("JumpTutorial");

                Destroy(tutorialManager.ChangeTutorialColList[0], 0.1f);

                tutorialManager.MoveTutorialing = false;

                if (tutorialManager.CameraTutorialing)
                {
                    tutorialManager.CameraTutorialing = false;
                }
            }

            if (tutorialManager.JumpTutorialing)
            {
                tutorialManager.TutorialObject[2].SetActive(false);
                tutorialManager.TutorialFlow.SendFungusMessage("DashTutorial");
                Destroy(tutorialManager.ChangeTutorialColList[1], 0.1f);
                tutorialManager.JumpTutorialing = false;
            }

            if (tutorialManager.DashTutorialing)
            {
                tutorialManager.TutorialObject[3].SetActive(false);
                tutorialManager.TutorialFlow.SendFungusMessage("TutorialIvent");
                Destroy(tutorialManager.ChangeTutorialColList[2], 0.1f);
                tutorialManager.DashTutorialing = false;
            }

        }
    }

    public void TutorialAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || hitting || isDown || avoidancing || stopAttack || OnGroundLayer(false)) return;

      
            attacking = true;

            attackingLook = true;

           animCon.SetBool("Attacking", true);//AnySatateの制御用
        
            attackCombo++;

            animCon.SetInteger("AttackCombo", attackCombo);


            if (attackCombo <= 1)
            {
                animCon.SetTrigger("Attack");
            }


    }

   
   /// <summary>
   /// 説明書いて
   /// </summary>
    public void WeaponGet()
    {
        myWepon.SetActive(true);
    }

    public void AttackOn()
    { 
        stopAttack = false;

        playerCameraCon.enabled = true;

        rig.enabled = true;

        lockon.SetActive(true);

        hpUI.SetActive(true);

        animCon.SetFloat("idleChange",animCount);//待機モーションチェンジ用
    }


}
