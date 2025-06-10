using DG.Tweening;
using UnityEngine;

/// <summary>
/// 動く床にプレイヤーの慣性が乗るようにするクラスです。
/// </summary>
public class MoveGround : MonoBehaviour
{
    [SerializeField]
    bool onPlayer;

    public bool OnPlayer { get => onPlayer; set => onPlayer = value; }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親を移動床にする
            other.transform.SetParent(transform);
            onPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 触れたobjの親をなくす
            other.transform.SetParent(null);
            onPlayer = false;
        }
    }
}
