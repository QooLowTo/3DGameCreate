using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャンバスの参照解像度や子のImage等のRectTransformをプレイする機種の画面比率に応じて動的に制御するクラスです。
/// </summary>
public class ChangeCanvasSize : MonoBehaviour
{
    [SerializeField, Header("キャンバスのCanvasScaler")]
    private CanvasScaler canvasScaler;

    //[SerializeField, Header("背景画像のRectTransform")]
    //private RectTransform backGroundRectTransform;

    [SerializeField, Header("背景画像のRectTransformList")]
    private List<RectTransform> rectTransformList = new List<RectTransform>();

    void Start()
    {
        var width = Screen.width;
        var height = Screen.height;

        var newArea = new Vector2(width, height);

        SetReferenceResolution(newArea);

        SetRectTransform(newArea);
    }

    /// <summary>
    /// キャンバスの新しい参照解像度を設定するメソッドです。
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
            Debug.LogWarning("CanvasScalerがアタッチされていません。");
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
            Debug.LogWarning("CanvasScalerがアタッチされていません。");
        }
    }
}
