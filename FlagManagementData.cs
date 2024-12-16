using UnityEngine;

/// <summary>
/// �t���O�Ǘ����s���X�N���v�^�u���I�u�W�F�N�g�ł��B
/// </summary>
//�G�f�B�^�[����E�N���b�N�ō쐬�ł���悤�ɂ��邽��
[CreateAssetMenu(menuName = "�t���O�Ǘ�/�t���O�Ǘ��I�u�W�F�N�g")]
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
