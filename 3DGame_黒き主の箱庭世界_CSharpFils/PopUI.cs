using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ダメージテキストの動きを制御するクラスです。
/// </summary>
public class PopUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI popText;

    private float alphaColor = 1f;

    [SerializeField]
    private float fadeOutSpeed = 1f;//フェードアウトするスピード
    
    [SerializeField]
    private float moveSpeed = 0.4f;//移動値

    void Start()
    {    
        popText = GetComponentInChildren<TextMeshProUGUI>();
    
    }

    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        alphaColor -= fadeOutSpeed * Time.deltaTime;
        popText.color = new Color(popText.color.r, popText.color.g, popText.color.b, alphaColor);
        if (popText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
    }
