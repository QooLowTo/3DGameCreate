using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "�X�N���v�^�u/�Z�[�u�X���b�g�f�[�^�L�^�I�u�W�F�N�g")]
public class MemorizeLevelAndSceneName : ScriptableObject
{
    [SerializeField]
    private List<string> savedPlayerLevelList = new List<string>();

    [SerializeField]
    private List<string> savedSceneNameList = new List<string>();

    public List<string> SavedPlayerLevelList { get => savedPlayerLevelList; set => savedPlayerLevelList = value; }
    public List<string> SavedSceneNameList { get => savedSceneNameList; set => savedSceneNameList = value; }
}
