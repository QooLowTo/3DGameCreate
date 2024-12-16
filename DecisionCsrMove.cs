using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 決定キー、決定ボタンの画像をカメラの方向に向かせるクラスです。
/// </summary>
public class DecisionCsrMove : MonoBehaviour //この名前はよくない。何をするクラスかわかりにくい。
{
    
    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        //transform.DOLocalMove(Vector3.down * Time.deltaTime, moveSpeed * Time.deltaTime).SetLoops(-1,LoopType.Yoyo);
    }

    void Update()
    {
         transform.rotation = Camera.main.transform.rotation;
    }
}
