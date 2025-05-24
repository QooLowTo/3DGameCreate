using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCanvasSize : MonoBehaviour
{
    [SerializeField, Header("�L�����o�X��CanvasScaler")]
    private CanvasScaler canvasScaler;

    //[SerializeField, Header("�w�i�摜��RectTransform")]
    //private RectTransform backGroundRectTransform;

    [SerializeField, Header("�w�i�摜��RectTransformList")]
    private List<RectTransform> rectTransformList = new List<RectTransform>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var width = Screen.width;
        var height = Screen.height;

        var newArea = new Vector2(width, height);

        SetReferenceResolution(newArea);

        SetRectTransform(newArea);
    }

    // Update is called once per frame
    /// <summary>
    /// �L�����o�X�̐V�����Q�Ɖ𑜓x��ݒ肷�郁�\�b�h�ł��B
    /// </summary>
    /// <param name="newResolution"></param>
    public void SetReferenceResolution(Vector2 newResolution)
    {
        if (canvasScaler != null)
        {
            canvasScaler.referenceResolution = newResolution;
        }
        else
        {
            Debug.LogWarning("CanvasScaler���A�^�b�`����Ă��܂���B");
        }
    }

    private void SetRectTransform(Vector2 newRectTransform)
    {
        if (rectTransformList != null)
        {
            for (int i = 0; i < rectTransformList.Count; i++)
            {
                rectTransformList[i].sizeDelta = newRectTransform;
            }
        }
        else
        {
            Debug.LogWarning("CanvasScaler���A�^�b�`����Ă��܂���B");
        }
    }
}
