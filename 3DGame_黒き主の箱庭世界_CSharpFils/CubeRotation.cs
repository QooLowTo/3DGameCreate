using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// キューブのオブジェクトを回転させるクラスです。
/// </summary>
public class CubeRotation : MonoBehaviour
{
    [SerializeField] private float RotateSpeed;

    void Start()
    {
        transform.DOLocalRotate(new Vector3(18f, 45f, -100f), RotateSpeed).SetLoops(-1, LoopType.Incremental);
    }

}
