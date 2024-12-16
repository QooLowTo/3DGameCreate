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
    private Boss_1 boss_1; //�����������O�͂悭�Ȃ��A�P���ĉ��H�ƂȂ邩������Ƌ�̓I�ȃ{�X�̓���������ׂ�

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

/// <summary>
/// �{�X��HP��0�ɂȂ�������HP�o�[���\���ɂ��郁�\�b�h
/// </summary>
    public void HealthDeath()
    {
        fill.enabled = false;
    }
}
