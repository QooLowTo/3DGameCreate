using DG.Tweening;
using Fungus;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
/// <summary>
/// チュートリアル中のプレイヤーの動きを制御するクラスです。
/// </summary>
public class Player_Tutorial_Controller : Player_Battle_Controller
{

    //private Player_Battle_Controller placon;

    private Player_Camera_Controller placam;

    private Player_UI_Controller placonUI;

    private TutorialManager tutorialManager;
    [SerializeField]
    private GameObject findTutorialManager;

    //[SerializeField, Header("プレイヤーの武器")]
    //private GameObject myWepon;//プレイヤーの武器

    [SerializeField]
    private GameObject hpUI;

    [SerializeField]
    private GameObject lockon;

    private float animCount = 1f;

    private bool stopAttack = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Starting());

        //placon = gameObject.GetComponent<Player_Battle_Controller>();
        placam = gameObject.GetComponent<Player_Camera_Controller>();

        placonUI = gameObject.GetComponent<Player_UI_Controller>();

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

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (gameOver || !gameStart || !charaCon.enabled) return;

        CheckGroundPlayerList();

        if (!attacking && !jumpAttackingFall && !isDown && !avoidancing && !isChanegeAction && !hitting)
        {
            //animCon.SetFloat("MoveInput", moveInputAbs);

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

   

    //public void GameStart()//シグナルで制御
    //{
    //    battleManager.GameStart  = true;
    //    battleManager.CameraAxis.enabled = true;

    //}

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
        

        //if (!placon.StopCombo)
        //{
            attackCombo++;

            animCon.SetInteger("AttackCombo", attackCombo);
        //placon.StopCombo = true;

            if (attackCombo <= 1)
            {
                animCon.SetTrigger("Attack");
            }
        //}

    }

    //public void Avoidance(InputAction.CallbackContext context)
    //{
    //    if (!context.performed || stopAttack || avoidancing  || hitting || isDown || OnGroundLayer(false)) return;

    //    if (moveInputAbs == 0){
    //        animCon.SetBool("BackAvoidance", true);
    //    }else if (attacking && moveInput.x >= 0.1f ){
    //        animCon.SetBool("RightAvoidance", true);
    //    }else if (placon.Attacking && placon.MoveInput.x <= -0.1f){
    //        animCon.SetBool("LeftAvoidance", true);
    //    }else if (placon.MoveInputAbs >= 0.1f){
    //        animCon.SetBool("ForwardAvoidance", true);
        
    //        placon.ForwardAvoidancing = true;
    //    }

    //    placon.OffAttacking();

    //    placon.SoundManager.OneShotPlayerSound(5);

    //    placon.Avoidancing = true;
    //}
    public void WeaponGet()
    {
        myWepon.SetActive(true);
    }

    public void AttackOn()
    { 
        stopAttack = false;

        placam.enabled = true;

        rig.enabled = true;

        lockon.SetActive(true);

        hpUI.SetActive(true);

        animCon.SetFloat("idleChange",animCount);//待機モーションチェンジ用
    }


}
