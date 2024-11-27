using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム中のエフェクト、サウンドなどの演出を制御するクラスです。
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private Player_Battle_Controller plaBcon;

    [SerializeField] 
    private Player_Rest_Controller plaRcon;

    [SerializeField] 
    private Player_Status_Controller plaScon;

    [SerializeField] 
    private GameObject placon;

    [SerializeField]
    private GameObject levelUPEffct;

    [SerializeField] 
    private GameObject enemySummonEffect;

    [SerializeField]
    private GameObject enemyHitEffect;

    [SerializeField] 
    private GameObject enemyDieEffect;

    [SerializeField]
    private GameObject startUI;

    [SerializeField]
    private GameObject damageUI;
    private TextMeshProUGUI damageText;

    [SerializeField]
    private GameObject playerDamageUI;
    private TextMeshProUGUI playerDamageText;

    [SerializeField]
    private GameObject expUI;
    private TextMeshProUGUI expText;

    [SerializeField]
    private GameObject levelUpUI;
    private TextMeshProUGUI levelUpText;

    [SerializeField]
    private GameObject gameOver;

    private Gamepad gamepad;

    [SerializeField]
    private GameObject musicManager;

    [SerializeField]
    private SettingData settingData;

    [SerializeField] 
    private AudioSource adios;

    [SerializeField]
    private AudioClip move;

    [SerializeField]
    private List<AudioClip> player_Sounds = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> enemy_Sounds = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> enemy_Attack_Sounds = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> boss_Sounds = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> UI_Sounds = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> other_Sounds = new List<AudioClip>();

    public GameObject MusicManager { get => musicManager; set => musicManager = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeScale != 1.0f)
        { 
        Time.timeScale = 1.0f;
        }

        musicManager.GetComponent<AudioSource>().volume = settingData.BgmVolume;

        gameObject.GetComponent<AudioSource>().volume = settingData.SoundVolum;  
        
        Application.targetFrameRate = settingData.FrameRate;

        if (startUI != null)
        { 
        startUI.SetActive(true);
            //adios.PlayOneShot(UI_Sounds[6]);
            adios.PlayOneShot(UI_Sounds[7]);
        }

        plaBcon = placon.GetComponent<Player_Battle_Controller>();
        plaRcon = placon.GetComponent <Player_Rest_Controller>();
        plaScon = placon.GetComponent<Player_Status_Controller>();

        damageText = damageUI.GetComponentInChildren<TextMeshProUGUI>();
        playerDamageText = playerDamageUI.GetComponentInChildren<TextMeshProUGUI>();
        expText = expUI.GetComponentInChildren<TextMeshProUGUI>();
        levelUpText = levelUpUI.GetComponentInChildren<TextMeshProUGUI>();

        gamepad = Gamepad.current;

        adios.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
   
    /// <summary>
    /// プレイヤーが敵に攻撃を与える又は、敵から攻撃を受けた際のダメージ計算のメソッドです。引数は対象の「防御力」と「攻撃力」を代入してください。
    /// </summary>
    /// <param name="Defence"></param>
    public int DamegeCalculation(int Defence,int AttackPower)//ダメージ計算
    {

        int trueDamage = (AttackPower / 2 - Defence / 4);

        int subDamage = Random.Range(0, AttackPower / 16);

        int fewDamage = Random.Range(0, 1);

        int baseDamage = 0;

        if (AttackPower > Defence * 4 / 7)
        {
            baseDamage = trueDamage;
        }
        else if (Defence * 4 / 7 > AttackPower && AttackPower > Defence * 1 / 2)
        {
            baseDamage = subDamage;
        }
        else if (Defence * 1 / 2 > AttackPower)
        { 
            baseDamage = fewDamage;
        }

        int resultDamege = baseDamage + Random.Range((baseDamage/16) - 1, (baseDamage / 16) + 1);

        if (resultDamege < 0)
        {
            resultDamege = 0;
        }
        
        return resultDamege;
    }

    /// <summary>
    /// プレイヤーが敵に与えたダメージ又は、プレイヤーが受けたダメージを表示させるメソッドです。引数は対象の「接触コライダー」と「受けたダメージ」を代入してください。
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="damage"></param>
    public void DamageText(Collider collider, int damage,float uipos)
    {

        if (collider == collider.GetComponent<CharacterController>())
        {
            Instantiate<GameObject>(playerDamageUI, collider.bounds.center - Camera.main.transform.forward * uipos, Quaternion.identity);
            playerDamageText.text = damage.ToString();
        }
        else
        {
            Instantiate<GameObject>(damageUI, collider.bounds.center - Camera.main.transform.forward * uipos, Quaternion.identity);
            damageText.text = damage.ToString();
        }

      
    }

    /// <summary>
    /// 獲得した経験値を表示するメソッドです。引数は「接触コライダー」と「獲得経験値数」を代入してください。
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="getExp"></param>
    public void GetExpText(Collider collider,int getExp)
    {
        Instantiate<GameObject>(expUI, collider.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);
        expText.text = "Exp+" + getExp.ToString();
    }

    public void Player_LevelUp_EffectAndSound(Collider collider,GameObject target)
    {
        adios.PlayOneShot(player_Sounds[9]);
       var obj = Instantiate<GameObject>(levelUpUI, collider.bounds.center - Camera.main.transform.up * 1.0f, Quaternion.identity);
        obj.transform.SetParent(collider.transform);

       var obj2 = Instantiate(levelUPEffct, target.transform.position, Quaternion.identity);
        obj2.transform.SetParent(target.transform);
        Invoke("LevUPEffectDes", 5f);
    }
    private void LevUPEffectDes()
    { 
    Destroy(levelUPEffct);
    }
    public void Player_Swing_Sound()
    {
        adios.PlayOneShot(player_Sounds[0]);
    }

    public void Player_DashStart_Sound()
    {
        adios.PlayOneShot(player_Sounds[1]);
    }
    public void Player_Jump_Sound()
    {
        adios.PlayOneShot(player_Sounds[2]);
    }
    public void Player_DoubleJump_Sound()
    {
        adios.PlayOneShot(player_Sounds[3]);
    }
    public void Player_Landing_Sound()
    {
        adios.PlayOneShot(player_Sounds[4]);
    }
    public void Player_Avoidance_Sound()
    {
        adios.PlayOneShot(player_Sounds[5]);
    }

    public void Player_Damage_Sound()
    {
        adios.PlayOneShot(player_Sounds[6]);
    }

    public void Player_KnockBack_Sound()
    {
        adios.PlayOneShot(player_Sounds[7]);
    }

    public void Player_LockOn_Sound()
    {
        adios.PlayOneShot(player_Sounds[10]);
    }

    private void Player_GameOver_Sound()
    {
        adios.PlayOneShot(player_Sounds[11]);
   
    }

    public void Player_Move_Sound_1()
    {
        adios.PlayOneShot(player_Sounds[12]);
    }

    public void Player_Move_Sound_2()
    {
        adios.PlayOneShot(player_Sounds[13]);
    }
    public void Decision_Sound()
    {
        adios.PlayOneShot(UI_Sounds[4]);
    }
    public void Cancel_Sound()
    {
        adios.PlayOneShot(UI_Sounds[5]);
    }
    /// <summary>
    /// 敵が召喚された時のエフェクトと効果音を発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Summon_EffectAndSound(GameObject target)
    {
        Instantiate(enemySummonEffect,target.transform.position,Quaternion.Euler(-90f, 0f, 0f));
        adios.PlayOneShot(enemy_Sounds[0]);
    }

    public void Enemy_Attack_Sound(int attackNum)
    {
        adios.PlayOneShot(enemy_Attack_Sounds[attackNum]);
    }

    public void Enemy_Hit_EffectAndSound(GameObject target)
    {
        Instantiate(enemyHitEffect, target.transform.position += Vector3.up, Quaternion.identity);
        adios.PlayOneShot(player_Sounds[8]);

        if (gamepad != null)
        {
            StartCoroutine(gamePadVibration(0,0.5f,0.1f));
        }
      
    }

   
   

    /// <summary>
    /// 敵が倒された時のエフェクトと効果音を発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Die_EffectAndSound(GameObject target)
    {
        Instantiate(enemyDieEffect,target.transform.position,Quaternion.Euler(-90f, 0f, 0f));
        adios.PlayOneShot(enemy_Sounds[1]);
    }

    public void Boss_Action_Sound(int soundNumber)
    {
        adios.PlayOneShot(boss_Sounds[soundNumber]);
    }

    public void Menu_Open_Sound()
    {
        adios.PlayOneShot(UI_Sounds[0]);
    }

    public void Select_Sound()
    {
        adios.PlayOneShot(UI_Sounds[2]);
    }

    public void Select_Ceiling_Sound()
    {
        adios.PlayOneShot(UI_Sounds[3]);
    }

    public void UI_Slide_Sound()
    {
        adios.PlayOneShot(UI_Sounds[1]);
    }

    public void LoadingStart_Sound()
    {
        adios.PlayOneShot(UI_Sounds[6]);
        adios.PlayOneShot(UI_Sounds[7]);
    }

    public void Get_Weapon_Sound()
    {
        adios.PlayOneShot(other_Sounds[0]);
    }

    public void Portal_Appearance_Sound()
    {
        adios.PlayOneShot(other_Sounds[1]);
    }

    public void BGMStop()
    { 
    musicManager.GetComponent<AudioSource>().Stop();
    }

    public void GameOver()
    {
        musicManager.GetComponent<AudioSource>().Stop();
        Invoke("Player_GameOver_Sound", 1f);
        gameOver.SetActive(true);

    } 

    /// <summary>
    /// ゲームパッドを振動させます。引数には「低周波(左)」、「高周波(右)」、「時間」を代入してください。
    /// </summary>
    /// <param name="low"></param>
    /// <param name="high"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator gamePadVibration(float low,float high,float time)
    { 
        gamepad.SetMotorSpeeds(low,high);
        yield return new WaitForSeconds(time);
        gamepad.SetMotorSpeeds(0.0f, 0.0f);//振動停止
    }
}
