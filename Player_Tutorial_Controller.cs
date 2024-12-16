using DG.Tweening;
using Fungus;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSyste
/// <summary>
/// チュートリアルのプレイヤーを制御するクラスです。
/// </summary>
public class Player_Tutorial_Controller : MonoBehaviour
{
   
    private Player_Battle_Controller playerBCon;

    private Player_Camera_Controller playerCamCon;

    private Player_UI_Controller playerUICon;

    [SerializeField]
    private GameObject hpUI;

    [SerializeField]
    private GameObject lockon;

    private float animCount = 1f;

    private bool stopAttack = true;

   
    void Start()
    {
        playerBCon = gameObject.GetComponent<Player_Battle_Controller>();
        playerCamCon = gameObject.GetComponent<Player_Camera_Controller>();
        playerUICon = gameObject.GetComponent<Player_UI_Controller>();

        playerBCon.MyWepon.SetActive(false); //武器の綴り MyWeaponです。修正してください。

    }

    void Update()
    {
        if (stopAttack)
        { 
            playerBCon.Rig.enabled = false;
        }

        if (stopAttack == false)
        {
           
        }
    }

    public void GameStart()//シグナルで制御
    {
        playerBCon.GameStarting = true;
        playerBCon.CameraAxis.enabled = true;
       
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (stopAttack) return;

        if (playerBCon.Attacking == false && playerBCon.Hitting == false && playerBCon.IsDown == false && playerBCon.Avoidancing == false && playerBCon.GroundedPlayer)
        {
            playerBCon.Attacking = true;

            playerBCon.AttackingLook = true;

            playerBCon.AnimCon.SetBool("Attacking", true);//AnySatateの制御用
        }

        if (playerBCon.StopCombo == false && playerBCon.Hitting == false && playerBCon.IsDown == false && playerBCon.Avoidancing == false && playerBCon.GroundedPlayer)
        {
            playerBCon.AttackCombo++;
            playerBCon.StopCombo = true;

            if (playerBCon.AttackCombo <= 1)
            {
                playerBCon.AnimCon.SetTrigger("Attack");
            }
        }

    }

    public void Avoidance(InputAction.CallbackContext context)
    {
        if (stopAttack) return;

        if (playerBCon.Avoidancing == false && playerBCon.MoveInputAbs == 0 && playerBCon.Hitting == false && playerBCon.IsDown == false && playerBCon.GroundedPlayer)
        {
            playerBCon.AnimCon.SetBool("BackAvoidance", true);

            playerBCon.OffAttacking();

            playerBCon.GameManager.Player_Avoidance_Sound();

            playerBCon.Avoidancing = true;

        }

        else if (playerBCon.Attacking == true && playerBCon.MoveInput.x >= 0.1f && playerBCon.Avoidancing == false && playerBCon.Hitting == false && playerBCon.IsDown == false && playerBCon.GroundedPlayer)
        {
            playerBCon.AnimCon.SetBool("RightAvoidance", true);

            playerBCon.OffAttacking();

            playerBCon.GameManager.Player_Avoidance_Sound();

            playerBCon.Avoidancing = true;

        }
        else if (playerBCon.Attacking == true && playerBCon.MoveInput.x <= -0.1f && playerBCon.Avoidancing == false && playerBCon.Hitting == false && playerBCon.IsDown == false && playerBCon.GroundedPlayer)
        {
            playerBCon.AnimCon.SetBool("LeftAvoidance", true);

            playerBCon.OffAttacking();

            playerBCon.GameManager.Player_Avoidance_Sound();

            playerBCon.Avoidancing = true;
        }
        else if (playerBCon.Avoidancing == false && playerBCon.MoveInputAbs >= 0.1f && playerBCon.Hitting == false && playerBCon.IsDown == false && playerBCon.GroundedPlayer)
        {
            playerBCon.AnimCon.SetBool("ForwardAvoidance", true);

            playerBCon.OffAttacking();

            playerBCon.GameManager.Player_Avoidance_Sound();

            playerBCon.Avoidancing = true;
        }
    }
    public void WeaponGet()
    {
        playerBCon.MyWepon.SetActive(true); //Weaponの綴り修正してね
    }

    public void AttackOn()
    { 
        stopAttack = false;
        playerCamCon.enabled = true;
        playerBCon.Rig.enabled = true;
        lockon.SetActive(true);
        hpUI.SetActive(true);
        playerBCon.AnimCon.SetFloat("idleChange",animCount);//待機モーションチェンジ用
    }


}
