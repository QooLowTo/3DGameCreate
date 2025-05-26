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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    
    //}

    //// Update is called once per frame
    //void Update()
    //{
       
    //}

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
