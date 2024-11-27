using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// このHPバーの持ち主に「Damageable」スクリプトをアサインし、
/// InspectorでこのHPバーの持ち主のDamageableの「HealthBar」の箱に
/// このスクリプトを渡す
/// HUDに適切
/// </summary>
public class HealthBarScript : MonoBehaviour
{
    [SerializeField]
    Player_Status_Controller plasta;

    [SerializeField]
    Slider slider;

    [SerializeField]
    TextMeshProUGUI hpText;

    [SerializeField]
    TextMeshProUGUI lveText;

    [SerializeField]
    Gradient gradient;

    [SerializeField]
    Image fill;

    /// <summary>
    /// スライダーの値を引数の値に変更するメソッド
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health) { 
    
        slider.value = health;

        hpText.text = "<u>HP:" + plasta.PlayerHP + "/" + plasta.LivePlayerHP;

        lveText.text = "<u><scale=0.8>Lv."+ "</scale>" + plasta.PlayerLevel;

        hpText.color = gradient.Evaluate(slider.normalizedValue);

        lveText.color = gradient.Evaluate(slider.normalizedValue);

        fill.color = gradient.Evaluate(slider.normalizedValue);

    }
    /// <summary>
    /// スライダーの最大値と現在の値を最大HPに設定するメソッド
    /// </summary>
    /// <param name="maxHealth"></param>
    public void SetMaxHealth(int maxHealth) { 
    
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        hpText.color = gradient.Evaluate(1f);

        fill.color = gradient.Evaluate(1f);
    }

    public void HealthDeath()
    { 
    fill.enabled = false;
    }
}
