﻿using UnityEngine;
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
    Slider slider;

    [SerializeField]
    Gradient gradient;


    [SerializeField]
    Image fill;


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

    public void HealthDeath()
    {
        fill.enabled = false;
    }
}
