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
    // Start is called before the first frame update
    void Start()
    {
        popText = GetComponentInChildren<TextMeshProUGUI>();

        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(1f, 1f))
            .Append(transform.DOScale(2.0f, 0.3f))
            .Append(transform.DOLocalMoveY(1.2f, 2f))
            .Append(transform.DOLocalMoveY(10f, 1f));

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if (fadeOutStartTime <= 3f)
        { 
         fadeOutStartTime += Time.deltaTime;
        }
       
        if (fadeOutStartTime < 3f) return;

        alphaColor -= fadeOutSpeed * Time.deltaTime;
        popText.color = new Color(popText.color.r, popText.color.g, popText.color.b, alphaColor);
        if (popText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
