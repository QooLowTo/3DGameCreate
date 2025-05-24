using UnityEngine;

//エディターから右クリックで作成できるようにするため
[CreateAssetMenu(menuName = "スクリプタブ/フラグ管理オブジェクト")]
public class FlagManagementData : ScriptableObject
{
    [SerializeField]
    private int killCount = 0;

    [SerializeField]
    private bool positionLoad = false;
    [SerializeField] 
    private bool rotateLoad = false;

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private bool tutorialClear = false;

    [SerializeField]
    private bool map1Clear = false;

    [SerializeField]
    private bool map2Clear = false;

    [SerializeField]
    private bool gameClear = false;

    [SerializeField]
    private bool data1Exist = false;
    [SerializeField]
    private bool data2Exist = false;
    [SerializeField]
    private bool data3Exist = false;


    public int KillCount { get => killCount; set => killCount = value; }
    public bool PositionLoad { get => positionLoad; set => positionLoad = value; }
    public bool RotateLoad { get => rotateLoad; set => rotateLoad = value; }
    public string SceneName { get => sceneName; set => sceneName = value; }
    public bool TutorialClear { get => tutorialClear; set => tutorialClear = value; }
    public bool Map1Clear { get => map1Clear; set => map1Clear = value; }
    public bool Map2Clear { get => map2Clear; set => map2Clear = value; }
    public bool GameClear { get => gameClear; set => gameClear = value; }
    public bool Data1Exist { get => data1Exist; set => data1Exist = value; }
    public bool Data2Exist { get => data2Exist; set => data2Exist = value; }
    public bool Data3Exist { get => data3Exist; set => data3Exist = value; }
}
