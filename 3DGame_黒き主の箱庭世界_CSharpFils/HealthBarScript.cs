using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのHP、レベルをUIに反映させるクラスです。
/// </summary>
public class HealthBarScript : MonoBehaviour
{
    Player_Status_Controller plaSta;

    [SerializeField]
    Slider hpSlider;

    [SerializeField]
    TextMeshProUGUI hpText;

    [SerializeField]
    TextMeshProUGUI lvlText;

    [SerializeField]
    Gradient sliderGradient;

    [SerializeField]
    Image fill;

    private GameObject findPlayerObj;

    private void Start()
    {
        findPlayerObj = GameObject.FindWithTag("Player");

        plaSta = findPlayerObj.GetComponent<Player_Status_Controller>();

        gameObject.SetActive(false);
    }

    /// <summary>
    /// スライダーの値を引数の値に変更するメソッド
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health) { 
    
        hpSlider.value = health;

        hpText.text = "<size=50>■H</size>P:<size=50>" + plaSta.LivePlayerHP + "■";

        lvlText.text = "□<size=60>L</size>v.<size=60>" + plaSta.PlayerLevel + "</size>□";

        hpText.color = sliderGradient.Evaluate(hpSlider.normalizedValue);

        lvlText.color = sliderGradient.Evaluate(hpSlider.normalizedValue);

        fill.color = sliderGradient.Evaluate(hpSlider.normalizedValue);

    }
    /// <summary>
    /// スライダーの最大値と現在の値を最大HPに設定するメソッド
    /// </summary>
    /// <param name="maxHealth"></param>
    public void SetMaxHealth(int maxHealth) { 
    
        hpSlider.maxValue = maxHealth;

        hpSlider.value = maxHealth;

        hpText.color = sliderGradient.Evaluate(1f);

        fill.color = sliderGradient.Evaluate(1f);
    }

    public void HealthDeath()
    { 
        fill.enabled = false;
    }
}
