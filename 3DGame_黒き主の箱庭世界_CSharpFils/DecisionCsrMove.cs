using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 決定キー、決定ボタンの画像をカメラの方向に向かせるクラスです。
/// </summary>
public class DecisionCsrMove : MonoBehaviour
{
    

    [SerializeField]
    private float moveSpeed;


    void Update()
    {
         transform.rotation = Camera.main.transform.rotation;
    }
}
