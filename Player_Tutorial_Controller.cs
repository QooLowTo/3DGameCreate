using DG.Tweening;
using Fungus;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class Player_Tutorial_Controller : MonoBehaviour
{
   
    private Player_Battle_Controller placon;

    private Player_Camera_Controller placam;

    private Player_UI_Controller placonUI;

    [SerializeField]
    private GameObject hpUI;

    [SerializeField]
    private GameObject lockon;

    private float animCount = 1f;

    private bool stopAttack = true;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placon = gameObject.GetComponent<Player_Battle_Controller>();
        placam = gameObject.GetComponent<Player_Camera_Controller>();
        placonUI = gameObject.GetComponent<Player_UI_Controller>();
       

        placon.MyWepon.SetActive(false);

       
    }

    // Update is called once per frame
    void Update()
    {
        if (stopAttack)
        { 
        placon.Rig.enabled = false;
        }

        if (stopAttack == false)
        {
           
        }
    }

    public void GameStart()//シグナルで制御
    {
        placon.GameStarting = true;
        placon.CameraAxis.enabled = true;
       
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (stopAttack) return;

        if (placon.Attacking == false && placon.Hitting == false && placon.IsDown == false && placon.Avoidancing == false && placon.GroundedPlayer)
        {
            placon.Attacking = true;

            placon.AttackingLook = true;

            placon.AnimCon.SetBool("Attacking", true);//AnySatateの制御用
        }

        if (placon.StopCombo == false && placon.Hitting == false && placon.IsDown == false && placon.Avoidancing == false && placon.GroundedPlayer)
        {
            placon.AttackCombo++;
            placon.StopCombo = true;

            if (placon.AttackCombo <= 1)
            {
                placon.AnimCon.SetTrigger("Attack");
            }
        }

    }

    public void Avoidance(InputAction.CallbackContext context)
    {
        if (stopAttack) return;

        if (placon.Avoidancing == false && placon.MoveInputAbs == 0 && placon.Hitting == false && placon.IsDown == false && placon.GroundedPlayer)
        {
            placon.AnimCon.SetBool("BackAvoidance", true);

            placon.OffAttacking();

            placon.GameManager.Player_Avoidance_Sound();

            placon.Avoidancing = true;

        }

        else if (placon.Attacking == true && placon.MoveInput.x >= 0.1f && placon.Avoidancing == false && placon.Hitting == false && placon.IsDown == false && placon.GroundedPlayer)
        {
            placon.AnimCon.SetBool("RightAvoidance", true);

            placon.OffAttacking();

            placon.GameManager.Player_Avoidance_Sound();

            placon.Avoidancing = true;

        }
        else if (placon.Attacking == true && placon.MoveInput.x <= -0.1f && placon.Avoidancing == false && placon.Hitting == false && placon.IsDown == false && placon.GroundedPlayer)
        {
            placon.AnimCon.SetBool("LeftAvoidance", true);

            placon.OffAttacking();

            placon.GameManager.Player_Avoidance_Sound();

            placon.Avoidancing = true;
        }
        else if (placon.Avoidancing == false && placon.MoveInputAbs >= 0.1f && placon.Hitting == false && placon.IsDown == false && placon.GroundedPlayer)
        {
            placon.AnimCon.SetBool("ForwardAvoidance", true);

            placon.OffAttacking();

            placon.GameManager.Player_Avoidance_Sound();

            placon.Avoidancing = true;
        }
    }
    public void WeaponGet()
    {
        placon.MyWepon.SetActive(true);
    }

    public void AttackOn()
    { 
        stopAttack = false;
        placam.enabled = true;
        placon.Rig.enabled = true;
        lockon.SetActive(true);
        hpUI.SetActive(true);
        placon.AnimCon.SetFloat("idleChange",animCount);//待機モーションチェンジ用
    }


}
