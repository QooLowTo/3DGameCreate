using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using MoonSharp.Interpreter;
using System.Collections.Generic;

/// <summary>
/// ボスのHPバーを制御するクラスです
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    
    [SerializeField]
    Slider slider; //何用スライダー？ 例：hpSlider
    // 例：HPバーのスライダー

    [SerializeField]
    Gradient gradient;
    //何用グラディエント？　名前もそういう意味にしましょう。
    // 例：HPバーの色を変えるためのグラデーション なら hpBarGradient


    [SerializeField]
    Image fill;
    //何用イメージ？ 例：HPバーの色を変えるためのイメージ


/// <summary>
/// 説明書いて
/// </summary>
/// <param name="health"></param>
    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    
    // <summary>
    /// スライダーの最大値と現在の値を最大HPに設定するメソッド
    /// </summary>
    /// <param name="maxHealth"></param>
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        fill.color = gradient.Evaluate(1f);

    }

    /// <summary>
    /// ボスのHPが0になったときに呼ばれるメソッド　→　ファティンが追加したが、合っていますか？
    /// </summary>
    public void HealthDeath()
    {
        fill.enabled = false;
    }
}
