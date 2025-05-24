using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CubeRotation : MonoBehaviour
{
    [SerializeField] private float RotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalRotate(new Vector3(18f, 45f, -100f), RotateSpeed).SetLoops(-1, LoopType.Incremental);
    }

    // Update is called once per frame
    void Update()
    {

        //RotateAngle += RotateSpeed * Time.deltaTime;

        //transform.eulerAngles = new Vector3(RotateAngle, 0, RotateAngle);
        //transform.localRotation = Quaternion.Euler(RotateAngle, RotateAngle, RotateAngle);
    }
}
