using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����HP�o�[�̎�����ɁuDamageable�v�X�N���v�g���A�T�C�����A
/// Inspector�ł���HP�o�[�̎������Damageable�́uHealthBar�v�̔���
/// ���̃X�N���v�g��n��
/// HUD�ɓK��
/// </summary>
public class HealthBarScript : MonoBehaviour
{
    [SerializeField]
    Player_Status_Controller playerSCon;  // �v���C���[�̃X�e�[�^�X�R���g���[���[   

    [SerializeField]
    Slider slider;

    [SerializeField]
    TextMeshProUGUI hpText;

    [SerializeField]
    TextMeshProUGUI lveText; //??? ���������Ă�������

    [SerializeField]
    Gradient gradient;

    [SerializeField]
    Image fill;

    /// <summary>
    /// �X���C�_�[�̒l�������̒l�ɕύX���郁�\�b�h
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health) { 
    
        slider.value = health;

        hpText.text = "<u>HP:" + playerSCon.PlayerHP + "/" + playerSCon.LivePlayerHP;

        lveText.text = "<u><scale=0.8>Lv."+ "</scale>" + playerSCon.PlayerLevel;

        hpText.color = gradient.Evaluate(slider.normalizedValue);

        lveText.color = gradient.Evaluate(slider.normalizedValue);

        fill.color = gradient.Evaluate(slider.normalizedValue);

    }
    /// <summary>
    /// �X���C�_�[�̍ő�l�ƌ��݂̒l���ő�HP�ɐݒ肷�郁�\�b�h
    /// </summary>
    /// <param name="maxHealth"></param>
    public void SetMaxHealth(int maxHealth) { 
    
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        hpText.color = gradient.Evaluate(1f);

        fill.color = gradient.Evaluate(1f);
    }

/// <summary>
/// �v���C���[��HP��0�ɂȂ�������HP�o�[���\���ɂ��郁�\�b�h
/// </summary>
    public void HealthDeath()
    { 
        fill.enabled = false;
    }
}
