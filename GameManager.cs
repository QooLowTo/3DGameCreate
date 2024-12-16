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
    private Player_Battle_Controller plaBcon; //これはマシな方の命名規則。他の変数もこれに統一しましょう。何なら playerBCon とかにしてもいいかもしれない

    [SerializeField] 
    private Player_Rest_Controller plaRcon; //これはマシな方の命名規則。他の変数もこれに統一しましょう。何なら playerRCon とかにしてもいいかもしれない

    [SerializeField] 
    private Player_Status_Controller plaScon; //これはマシな方の命名規則。他の変数もこれに統一しましょう。何なら playerSCon とかにしてもいいかもしれない

    [SerializeField] 
    private GameObject playerObj; //説明書きましょう。何のオブジェクトかわかりにくいです。

    [SerializeField]
    private GameObject levelUPEffect;

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
    private GameObject gameOver; //説明書きましょう。何のオブジェクトかわかりにくいです。

    private Gamepad gamepad; //説明書きましょう。何のオブジェクトかわかりにくいです。

    [SerializeField]
    private GameObject musicManager;

    [SerializeField]
    private SettingData settingData;

    [SerializeField] 
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip audioClip;

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
            audioSource.PlayOneShot(UI_Sounds[7]);
        }

        plaBcon = playerObj.GetComponent<Player_Battle_Controller>();
        plaRcon = playerObj.GetComponent <Player_Rest_Controller>();
        plaScon = playerObj.GetComponent<Player_Status_Controller>();

        damageText = damageUI.GetComponentInChildren<TextMeshProUGUI>();
        playerDamageText = playerDamageUI.GetComponentInChildren<TextMeshProUGUI>();
        expText = expUI.GetComponentInChildren<TextMeshProUGUI>();
        levelUpText = levelUpUI.GetComponentInChildren<TextMeshProUGUI>();

        gamepad = Gamepad.current;

        audioSource.GetComponent<AudioSource>();
        
    }
   
    /// <summary>
    /// プレイヤーが敵に攻撃を与える又は、敵から攻撃を受けた際のダメージ計算のメソッドです。引数は対象の「防御力」と「攻撃力」を代入してください。
    /// </summary>
    /// <param name="Defence"></param>
    public int DamageCalculation(int Defence,int AttackPower)//ダメージ計算
    {

        //以下のマジックナンバーすべて変数化するか、コメント書いてください。なぜ２？なぜ４？
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

        int resultDamage = baseDamage + Random.Range((baseDamage/16) - 1, (baseDamage / 16) + 1);

        if (resultDamage < 0)
        {
            resultDamage = 0;
        }
        
        return resultDamage;
    }

    /// <summary>
    /// プレイヤーが敵に与えたダメージ又は、プレイヤーが受けたダメージを表示させるメソッドです。引数は対象の「接触コライダー」と「受けたダメージ」を代入してください。
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="damage"></param>
    public void DamageText(Collider collider, int damage,float UIpos)
    {

        if (collider == collider.GetComponent<CharacterController>())
        {
            Instantiate<GameObject>(playerDamageUI, collider.bounds.center - Camera.main.transform.forward * UIpos, Quaternion.identity);
            playerDamageText.text = damage.ToString();
        }
        else
        {
            Instantiate<GameObject>(damageUI, collider.bounds.center - Camera.main.transform.forward * UIpos, Quaternion.identity);
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
        //0.2マジックナンバー！変数化かコメント書いてください。
    }

    public void Player_LevelUp_EffectAndSound(Collider collider,GameObject target)
    {
        audioSource.PlayOneShot(player_Sounds[9]); //レベルアップ音を再生する
       var obj = Instantiate<GameObject>(levelUpUI, collider.bounds.center - Camera.main.transform.up * 1.0f, Quaternion.identity);
        obj.transform.SetParent(collider.transform);

       var obj2 = Instantiate(levelUPEffect, target.transform.position, Quaternion.identity);
        obj2.transform.SetParent(target.transform);
        Invoke("LevUPEffectDes", 5f); //5秒後にレベルアップエフェクトを削除する
    }

    /// <summary>
    /// レベルアップエフェクトを削除するメソッドです。
    /// </summary>
    private void LevUPEffectDes()
    { 
        Destroy(levelUPEffect);
    }

    /// <summary>
    /// プレイヤーの移動音を再生します。
    /// </summary>
    public void Player_Swing_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[0]);
    }

/// <summary>
/// プレイヤーのダッシュ開始音を再生します。
/// </summary>
    public void Player_DashStart_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[1]);
    }
    /// <summary>
    /// プレイヤーのジャンプ音を再生します。
    /// </summary>
    public void Player_Jump_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[2]);
    }
    /// <summary>
    /// プレイヤーのダブルジャンプ音を再生します。
    /// </summary>
    public void Player_DoubleJump_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[3]);
    }
    /// <summary>
    /// プレイヤーの着地音を再生します。
    /// </summary>
    public void Player_Landing_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[4]);
    }
    /// <summary>
    /// プレイヤーの回避音を再生します。
    /// </summary>
    public void Player_Avoidance_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[5]);
    }

/// <summary>
/// プレイヤーのダメージ音を再生します。
/// </summary>
    public void Player_Damage_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[6]);
    }

/// <summary>
/// プレイヤーのノックバック音を再生します。
/// </summary>
    public void Player_KnockBack_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[7]);
    }

/// <summary>
/// プレイヤーの攻撃音を再生します。
/// </summary>
    public void Player_LockOn_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[10]);
    }

/// <summary>
/// プレイヤーのゲームオーバー音を再生します。
/// </summary>
    private void Player_GameOver_Sound()
    {
        audioSource.PlayOneShot(player_Sounds[11]);
   
    }

/// <summary>
///     プレイヤーの移動音を再生します。
/// </summary>
    public void Player_Move_Sound_1()
    {
        audioSource.PlayOneShot(player_Sounds[12]);
    }

/// <summary>
/// プレイヤーの移動音を再生します。
/// </summary>
    public void Player_Move_Sound_2()
    {
        audioSource.PlayOneShot(player_Sounds[13]);
    }
    /// <summary>
    /// プレイヤーの移動音を再生します。
    /// </summary>
    public void Decision_Sound()
    {
        audioSource.PlayOneShot(UI_Sounds[4]);
    }
    /// <summary>
    /// プレイヤーの移動音を再生します。
    /// </summary>
    public void Cancel_Sound()
    {
        audioSource.PlayOneShot(UI_Sounds[5]);
    }
    /// <summary>
    /// 敵が召喚された時のエフェクトと効果音を発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Summon_EffectAndSound(GameObject target)
    {
        Instantiate(enemySummonEffect,target.transform.position,Quaternion.Euler(-90f, 0f, 0f));
        //説明書いてください。
        audioSource.PlayOneShot(enemy_Sounds[0]);
    }

/// <summary>
/// 敵の攻撃音を再生します。引数には「攻撃番号」を代入してください。
/// </summary>
/// <param name="attackNum"></param>
    public void Enemy_Attack_Sound(int attackNum)
    {
        audioSource.PlayOneShot(enemy_Attack_Sounds[attackNum]);
    }

/// <summary>
/// 敵が攻撃を受けた時のエフェクトと効果音を発生させます。引数には「敵のオブジェクト」を代入してください。
/// </summary>
/// <param name="target"></param>
    public void Enemy_Hit_EffectAndSound(GameObject target)
    {
        Instantiate(enemyHitEffect, target.transform.position += Vector3.up, Quaternion.identity);
        audioSource.PlayOneShot(player_Sounds[8]);

        if (gamepad != null)
        {
            StartCoroutine(gamePadVibration(0,0.5f,0.1f));
            //説明書いてください。
        }
      
    }

   
   

    /// <summary>
    /// 敵が倒された時のエフェクトと効果音を発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Die_EffectAndSound(GameObject target)
    {
        Instantiate(enemyDieEffect,target.transform.position,Quaternion.Euler(-90f, 0f, 0f));
        audioSource.PlayOneShot(enemy_Sounds[1]);
    }

    public void Boss_Action_Sound(int soundNumber)
    {
        audioSource.PlayOneShot(boss_Sounds[soundNumber]);
    }

    public void Menu_Open_Sound()
    {
        audioSource.PlayOneShot(UI_Sounds[0]);
    }

    public void Select_Sound()
    {
        audioSource.PlayOneShot(UI_Sounds[2]);
    }

    public void Select_Ceiling_Sound()
    {
        audioSource.PlayOneShot(UI_Sounds[3]);
    }

    public void UI_Slide_Sound()
    {
        audioSource.PlayOneShot(UI_Sounds[1]);
    }

    public void LoadingStart_Sound()
    {
        audioSource.PlayOneShot(UI_Sounds[6]);
        audioSource.PlayOneShot(UI_Sounds[7]);
    }

    public void Get_Weapon_Sound()
    {
        audioSource.PlayOneShot(other_Sounds[0]);
    }

    public void Portal_Appearance_Sound()
    {
        audioSource.PlayOneShot(other_Sounds[1]);
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
