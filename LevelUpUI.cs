using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// レベルアップ時に演出するテキストを制御するクラスです。
/// </summary>
public class LevelUpUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI popText;

    private float alphaColor = 1f;

    private float fadeOutStartTime = 0f;

    [SerializeField]
    private float fadeOutSpeed = 1f;//フェードアウトするスピード

    [SerializeField]
    private float moveSpeed = 0.4f;//移動値

    void Start()
    {
        popText = GetComponentInChildren<TextMeshProUGUI>();

        //どういう処理をしているか説明を書いてください
        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(1f, 1f))
            .Append(transform.DOScale(2.0f, 0.3f))
            .Append(transform.DOLocalMoveY(1.2f, 2f))
            .Append(transform.DOLocalMoveY(10f, 1f));

            //上記の処理に使われるマジックナンバーを変数化するか、定数化してください
            

    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if (fadeOutStartTime <= 3f) //マジックナンバー発見！変数化　OR　定数化　OR　コメント残してしてください
        { 
            fadeOutStartTime += Time.deltaTime;
        }
       
        if (fadeOutStartTime < 3f) return;　//マジックナンバー発見！変数化　OR　定数化　OR　コメント残してしてください

        alphaColor -= fadeOutSpeed * Time.deltaTime;
        popText.color = new Color(popText.color.r, popText.color.g, popText.color.b, alphaColor);
        if (popText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
