using Fungus;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// バトル中のゲームの進行等を管理するクラスです。
/// </summary>
public class BattleManager : GameManager
{
    //private bool gameOver = false;//ゲームオーバー判定

    private bool bossBattle = false;//ボスと戦闘中判定


    [SerializeField]
    private GameObject levelUPEffct;

    [SerializeField]
    private GameObject enemySummonEffect;

    [SerializeField]
    private GameObject enemyHitEffect;

    [SerializeField]
    private GameObject enemyDieEffect;

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

    private SoundManager soundManager;
    [SerializeField]
    private GameObject findSoundManager;

    [SerializeField]
    private GameObject gameOverObject;

    //[SerializeField]
    //protected List<AudioClip> enemy_Sounds = new List<AudioClip>();

    //[SerializeField]
    //protected List<AudioClip> enemy_Attack_Sounds = new List<AudioClip>();

    //[SerializeField]
    //protected List<AudioClip> boss_Sounds = new List<AudioClip>();

    //public bool GameOver { get => gameOver; set => gameOver = value; }
    public bool BossBattle { get => bossBattle; set => bossBattle = value; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartGameSetting();

        StartGetPlayerStatus();

        damageText = damageUI.GetComponentInChildren<TextMeshProUGUI>();

        playerDamageText = playerDamageUI.GetComponentInChildren<TextMeshProUGUI>();

        expText = expUI.GetComponentInChildren<TextMeshProUGUI>();

        levelUpText = levelUpUI.GetComponentInChildren<TextMeshProUGUI>();

        soundManager = findSoundManager.GetComponent<SoundManager>();


        if (startUI != null && !debugMode)
        {

            startUI.SetActive(true);

            soundManager.OneShot_UI_Sound(7);//ロード完了音
        }

        if (debugMode)
        {

            StartCoroutine(DebugGameStart());
           
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    /// <summary>
    /// プレイヤーが敵に攻撃を与える又は、敵から攻撃を受けた際のダメージ計算のメソッドです。引数は対象の「防御力」と「攻撃力」を代入してください。
    /// </summary>
    /// <param name="Defence"></param>
    public int DamegeCalculation(int Defence, int AttackPower)//ダメージ計算
    {

        int trueDamage = (AttackPower / 2 - Defence / 4);

        int subDamage = Random.Range(0, AttackPower / 16);

        int fewDamage = 1;

        int baseDamage = 1;

        if (AttackPower > Defence * (4 / 7))
        {
            baseDamage = trueDamage;
        }
        else if (Defence * (4 / 7) > AttackPower && AttackPower > Defence * (1 / 2))
        {
            baseDamage = subDamage;
        }
        else if (Defence * (1 / 2) > AttackPower)
        {
            baseDamage = fewDamage;
        }

        int resultDamege = baseDamage + Random.Range((baseDamage / 16) - 1, (baseDamage / 16) + 1);

        if (resultDamege <= 0)
        {
            resultDamege = 1;
        }

        return resultDamege;
    }

    /// <summary>
    /// プレイヤーが敵に与えたダメージ又は、プレイヤーが受けたダメージを表示させるメソッドです。引数は対象の「接触コライダー」と「受けたダメージ」を代入してください。
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="damage"></param>
    public void DamageText(Collider collider, int damage, float uipos)
    {
        var uiPotion = Camera.main.transform.forward * uipos;

        if (collider == collider.GetComponent<CharacterController>())
        {

            Instantiate<GameObject>(playerDamageUI, collider.bounds.center - uiPotion, Quaternion.identity);

            playerDamageText.text = damage.ToString();

        }
        else
        {

            Instantiate<GameObject>(damageUI, collider.bounds.center - uiPotion, Quaternion.identity);

            damageText.text = damage.ToString();

        }
    }

    /// <summary>
    /// 獲得した経験値の表示とレベルアップの処理を行うメソッドです。引数は「接触コライダー」と「獲得経験値数」を代入してください。
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="getExp"></param>
    public void GetExp(Collider collider, int getExp)
    {
      

        Instantiate<GameObject>(expUI, collider.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);

        expText.text = "Exp+" + getExp.ToString();

        plaSta.PlayerExp += getExp;

        if (plaSta.PlayerLevel < 99)
        { 
            while (plaSta.PlayerExp >= expManager.ExpTablesList[statusDate.D_ExpListElement])
            {
                plaExp.LevelUp();
            }
        }    
    }
    /// <summary>
    /// レベルアップ時のテキストとエフェクトを呼び出すメソッドです。引数にはプレイヤーのコライダーと
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="target"></param>
    public void Player_LevelUp_EffectAndSound(Collider collider, GameObject target)
    {
        soundManager.OneShot_Player_Sound(9);

        var obj = Instantiate<GameObject>(levelUpUI, collider.bounds.center - Camera.main.transform.up * 1.0f, Quaternion.identity);

        obj.transform.SetParent(collider.transform);

        var obj2 = Instantiate(levelUPEffct, target.transform.position, Quaternion.identity);

        obj2.transform.SetParent(target.transform);

        Destroy(obj2, 5f);
    }

    /// <summary>
    /// 敵が召喚された時のエフェクトと効果音を発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Summon_Effect(GameObject target)
    {
        Instantiate(enemySummonEffect, target.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        //adios.PlayOneShot(enemy_Sounds[0]);
    }

    //public void Enemy_Attack_Sound(int attackNum)
    //{
    //    adios.PlayOneShot(enemy_Attack_Sounds[attackNum]);
    //}

    public void Enemy_Hit_Effect(GameObject target)
    {
        Instantiate(enemyHitEffect, target.transform.position += Vector3.up, Quaternion.identity);
        //soundManager.OneShot_Player_Sound(8);
    }

    public void Enemy_Hit_Vibration()
    {
        if (settingData.VibrationON == false) return;

        if (gamepad != null)
        {
            StartCoroutine(GamePadVibration(0, 0.5f, 0.1f));
        }
    }

    /// <summary>
    /// 敵が倒された時のエフェクトを発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Die_Effect(GameObject target)
    {
        Instantiate(enemyDieEffect, target.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        //adios.PlayOneShot(enemy_Sounds[1]);
    }

    //public void Boss_Action_Sound(int soundNumber)
    //{
    //    adios.PlayOneShot(boss_Sounds[soundNumber]);
    //}

    public void Boss_Die_Vibration()
    {
        if (settingData.VibrationON == false) return;

        if (gamepad != null)
        {
            StartCoroutine(Boss_Kill_GamePadVibration(0.8f, 0.8f, 3f));
        }
    }
    public void OnGameOver()
    {
        gameOverObject.SetActive(true);
    }
}
