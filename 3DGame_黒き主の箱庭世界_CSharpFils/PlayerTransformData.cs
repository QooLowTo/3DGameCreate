using UnityEngine;

/// <summary>
/// プレイヤーのセーブした時点でのトランスフォームを記録するクラスです。
/// </summary>
[CreateAssetMenu(menuName = "スクリプタブ/トランスフォーム管理オブジェクト")]
public class PlayerTransformData : ScriptableObject
{
   

    [SerializeField]
    private Vector3 loadTransform;


    [SerializeField]
    private Quaternion loadRotate;

    public Vector3 LoadTransform { get => loadTransform; set => loadTransform = value; }
    public Quaternion LoadRotate { get => loadRotate; set => loadRotate = value; }
}
