using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����L�[�A����{�^���̉摜���J�����̕����Ɍ�������N���X�ł��B
/// </summary>
public class DecisionCsrMove : MonoBehaviour //���̖��O�͂悭�Ȃ��B��������N���X���킩��ɂ����B
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
