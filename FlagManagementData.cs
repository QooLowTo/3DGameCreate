using UnityEngine;

/// <summary>
/// フラグ管理を行うスクリプタブルオブジェクトです。
/// </summary>
//エディターから右クリックで作成できるようにするため
[CreateAssetMenu(menuName = "フラグ管理/フラグ管理オブジェクト")]
public class FlagManagementData : ScriptableObject
{
    [SerializeField]
    private int killCount = 0;

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private bool tutorialClear = false;

    public int KillCount { get => killCount; set => killCount = value; }
    public string SceneName { get => sceneName; set => sceneName = value; }
    public bool TutorialClear { get => tutorialClear; set => tutorialClear = value; }
}
