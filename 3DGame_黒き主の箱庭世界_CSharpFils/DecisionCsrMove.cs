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

    // Start is called before the first frame update
    void Start()
    {
        //transform.DOLocalMove(Vector3.down * Time.deltaTime, moveSpeed * Time.deltaTime).SetLoops(-1,LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
         transform.rotation = Camera.main.transform.rotation;
    }
}
