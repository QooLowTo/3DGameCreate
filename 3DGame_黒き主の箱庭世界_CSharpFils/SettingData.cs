using UnityEngine;

//エディターから右クリックで作成できるようにするため
[CreateAssetMenu(menuName = "スクリプタブ/設定管理オブジェクト")]

public class SettingData : ScriptableObject
{
    [SerializeField]
    private float bgmVolume;

    [SerializeField]
    private float soundVolum;

    [SerializeField]
    private int frameRate = 60;

    [SerializeField] 
    private float cameraRotateSpeed;

    [SerializeField]
    private bool vibrationON = true;

    public float BgmVolume { get => bgmVolume; set => bgmVolume = value; }
    public float SoundVolum { get => soundVolum; set => soundVolum = value; }
    public int FrameRate { get => frameRate; set => frameRate = value; }
    public float CameraRotateSpeed { get => cameraRotateSpeed; set => cameraRotateSpeed = value; }
    public bool VibrationON { get => vibrationON; set => vibrationON = value; }
    
}
