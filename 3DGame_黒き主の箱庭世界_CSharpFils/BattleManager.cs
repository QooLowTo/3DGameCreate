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
    private GameObject levelUPEffect; //説明は？？

    [SerializeField]
    private GameObject enemySummonEffect;　//説明は？？

    [SerializeField]
    private GameObject enemyHitEffect;　//説明は？？

    [SerializeField]
    private GameObject enemyDieEffect;　//説明は？？

    [SerializeField]
    private GameObject damageUI;　//説明は？？
    private TextMeshProUGUI damageText;　//説明は？？

    [SerializeField]
    private GameObject playerDamageUI;　//説明は？？
    private TextMeshProUGUI playerDamageText;　//説明は？？

    [SerializeField]
    private GameObject expUI;　//説明は？？
    private TextMeshProUGUI expText;　//説明は？？
　
    [SerializeField]
    private GameObject levelUpUI;　//説明は？？
    private TextMeshProUGUI levelUpText;　//説明は？？

    private SoundManager soundManager;　//説明は？？
    [SerializeField]
    private GameObject findSoundManager;　//説明は？？

    [SerializeField]
    private GameObject gameOverObject; //説明は？？


    //以下の部分必要ないなら消してください。残すならなぜ残す必要があるか説明書いてください！

    //[SerializeField]
    //protected List<AudioClip> enemy_Sounds = new List<AudioClip>();

    //[SerializeField]
    //protected List<AudioClip> enemy_Attack_Sounds = new List<AudioClip>();

    //[SerializeField]
    //protected List<AudioClip> boss_Sounds = new List<AudioClip>();

    //public bool GameOver { get => gameOver; set => gameOver = value; }

    public bool BossBattle { get => bossBattle; set => bossBattle = value; }

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
            // この７はマジックナンバーになります。変数化するかもしくはなぜ７なのか書いてください。
        }

        if (debugMode)
        {

            StartCoroutine(DebugGameStart());
           
        }
    }

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

        //ダメージ計算の式
        //攻撃力が防御力の4/7倍以上ならtrueDamage＜＝＝こういう説明を書くか４と７と２の変数化をしてください
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

        //マジックナンバーを使わないようにするために、変数化するか、なぜこの数字を使うのか説明を書いてください。
        int resultDamage = baseDamage + Random.Range((baseDamage / 16) - 1, (baseDamage / 16) + 1);

        if (resultDamage <= 0)
        {
            resultDamage = 1;
        }

        return resultDamage;
    }

    /// <summary>
    /// プレイヤーが敵に与えたダメージ又は、プレイヤーが受けたダメージを表示させるメソッドです。引数は対象の「接触コライダー」と「受けたダメージ」を代入してください。
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="damage"></param>
    public void DamageText(Collider collider, int damage, float UIpos)
    {
        var uiPotion = Camera.main.transform.forward * UIpos;

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

        //0.2fはマジックナンバーです。変数化するか、なぜ0.2fなのか説明を書いてください。
        
        Instantiate<GameObject>(expUI, collider.bounds.center - Camera.main.transform.forward * 0.2f, Quaternion.identity);

        expText.text = "Exp+" + getExp.ToString();

        plaSta.PlayerExp += getExp;

        //99レベルまでの経験値を取得した場合、レベルアップの処理を行います。この説明でいいのか？
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

        var obj2 = Instantiate(levelUPEffect, target.transform.position, Quaternion.identity);

        obj2.transform.SetParent(target.transform);

        //5秒後オブジェクト削除
        Destroy(obj2, 5f);
    }

    /// <summary>
    /// 敵が召喚された時のエフェクトと効果音を発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Summon_Effect(GameObject target)
    {
        //以下の90fはマジックナンバーです。変数化するか、なぜ90fなのか説明を書いてください。
        Instantiate(enemySummonEffect, target.transform.position, Quaternion.Euler(-90f, 0f, 0f));
       
    }

    public void Enemy_Hit_Effect(GameObject target)
    {
        Instantiate(enemyHitEffect, target.transform.position += Vector3.up, Quaternion.identity);
    }

    public void Enemy_Hit_Vibration()
    {
        if (settingData.VibrationON == false) return;

        if (gamepad != null)
        {
            //変数化してください
            StartCoroutine(GamePadVibration(0, 0.5f, 0.1f));
        }
    }

    /// <summary>
    /// 敵が倒された時のエフェクトを発生させます。引数には「敵のオブジェクト」を代入してください。
    /// </summary>
    /// <param name="target"></param>
    public void Enemy_Die_Effect(GameObject target)
    {
        //以下の90fはマジックナンバーです。変数化するか、なぜ90fなのか説明を書いてください。
        Instantiate(enemyDieEffect, target.transform.position, Quaternion.Euler(-90f, 0f, 0f));      
    }


    public void Boss_Die_Vibration()
    {
        if (settingData.VibrationON == false) return;

        if (gamepad != null)
        {
            //変数化してください
            StartCoroutine(Boss_Kill_GamePadVibration(0.8f, 0.8f, 3f));
        }
    }
    public void OnGameOver()
    {
        gameOverObject.SetActive(true);
    }
}
