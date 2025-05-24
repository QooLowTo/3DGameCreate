using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using MoonSharp.Interpreter;
using System.Collections.Generic;

/// <summary>
/// �{�X��HP�o�[�𐧌䂷��N���X�ł�
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
        /// �X���C�_�[�̍ő�l�ƌ��݂̒l���ő�HP�ɐݒ肷�郁�\�b�h
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
