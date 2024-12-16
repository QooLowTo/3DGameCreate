using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �_���[�W�e�L�X�g�̓����𐧌䂷��N���X�ł��B
/// </summary>
public class PopUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI popText;

    private float alphaColor = 1f;

    [SerializeField]
    private float fadeOutSpeed = 1f;//�t�F�[�h�A�E�g����X�s�[�h
    
    [SerializeField]
    private float moveSpeed = 0.4f;//�ړ��l

    void Start()
    {    

        popText = GetComponentInChildren<TextMeshProUGUI>();
    
    }

    /// <summary>
    /// �_���[�W�e�L�X�g���t�F�[�h�A�E�g�����鏈���ł��B
    /// </summary>
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
