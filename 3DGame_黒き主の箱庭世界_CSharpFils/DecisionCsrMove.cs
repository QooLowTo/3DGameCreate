using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����L�[�A����{�^���̉摜���J�����̕����Ɍ�������N���X�ł��B
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
