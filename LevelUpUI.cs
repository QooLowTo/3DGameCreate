using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// ���x���A�b�v���ɉ��o����e�L�X�g�𐧌䂷��N���X�ł��B
/// </summary>
public class LevelUpUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI popText;

    private float alphaColor = 1f;

    private float fadeOutStartTime = 0f;

    [SerializeField]
    private float fadeOutSpeed = 1f;//�t�F�[�h�A�E�g����X�s�[�h

    [SerializeField]
    private float moveSpeed = 0.4f;//�ړ��l

    void Start()
    {
        popText = GetComponentInChildren<TextMeshProUGUI>();

        //�ǂ��������������Ă��邩�����������Ă�������
        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(1f, 1f))
            .Append(transform.DOScale(2.0f, 0.3f))
            .Append(transform.DOLocalMoveY(1.2f, 2f))
            .Append(transform.DOLocalMoveY(10f, 1f));

            //��L�̏����Ɏg����}�W�b�N�i���o�[��ϐ������邩�A�萔�����Ă�������
            

    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if (fadeOutStartTime <= 3f) //�}�W�b�N�i���o�[�����I�ϐ����@OR�@�萔���@OR�@�R�����g�c���Ă��Ă�������
        { 
            fadeOutStartTime += Time.deltaTime;
        }
       
        if (fadeOutStartTime < 3f) return;�@//�}�W�b�N�i���o�[�����I�ϐ����@OR�@�萔���@OR�@�R�����g�c���Ă��Ă�������

        alphaColor -= fadeOutSpeed * Time.deltaTime;
        popText.color = new Color(popText.color.r, popText.color.g, popText.color.b, alphaColor);
        if (popText.color.a <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
