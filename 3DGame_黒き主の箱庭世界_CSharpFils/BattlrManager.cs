using Fungus;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleManager : GameManager
{
    //private bool gameOver = false;//�Q�[���I�[�o�[����

    private bool bossBattle = false;//�{�X�Ɛ퓬������


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

            soundManager.OneShot_UI_Sound(7);//���[�h������
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
    /// �v���C���[���G�ɍU����^���閔�́A�G����U�����󂯂��ۂ̃_���[�W�v�Z�̃��\�b�h�ł��B�����͑Ώۂ́u�h��́v�Ɓu�U���́v�������Ă��������B
    /// </summary>
    /// <param name="Defence"></param>
    public int DamegeCalculation(int Defence, int AttackPower)//�_���[�W�v�Z
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
    /// �v���C���[���G�ɗ^�����_���[�W���́A�v���C���[���󂯂��_���[�W��\�������郁�\�b�h�ł��B�����͑Ώۂ́u�ڐG�R���C�_�[�v�Ɓu�󂯂��_���[�W�v�������Ă��������B
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
    /// �l�������o���l�̕\���ƃ��x���A�b�v�̏������s�����\�b�h�ł��B�����́u�ڐG�R���C�_�[�v�Ɓu�l���o���l���v�������Ă��������B
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
    /// ���x���A�b�v���̃e�L�X�g�ƃG�t�F�N�g���Ăяo�����\�b�h�ł��B�����ɂ̓v���C���[�̃R���C�_�[��
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
    /// �G���������ꂽ���̃G�t�F�N�g�ƌ��ʉ��𔭐������܂��B�����ɂ́u�G�̃I�u�W�F�N�g�v�������Ă��������B
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
    /// �G���|���ꂽ���̃G�t�F�N�g�𔭐������܂��B�����ɂ́u�G�̃I�u�W�F�N�g�v�������Ă��������B
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
