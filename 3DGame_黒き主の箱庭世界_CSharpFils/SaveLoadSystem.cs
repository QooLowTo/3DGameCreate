using Mono.Data.Sqlite; // 1
using System.Collections.Generic;
using System.Data; // 1
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// セーブやロードなどのセーブデータに関するメソッドを管理するスクリプトです。
/// </summary>
public class SaveLoadSystem : MonoBehaviour
{
    // Resources: //参考
    // https://www.mono-project.com/docs/database-access/providers/sqlite/

    private string fileName;

    int elementNum;

    [SerializeField]
    private List<int> playerStatus = new List<int>();

    [SerializeField]
    private List<float> settingValues = new List<float>();

    [SerializeField] 
    private int frameRate;
    [SerializeField]
    private bool vibrationOn;

    [SerializeField]
    private List<float> playerPositions = new List<float>();

    [SerializeField]
    private List<float> playerRotations = new List<float>();

    [SerializeField]
    private List<bool> flagmentBools = new List<bool>();

    private Vector3 playerPosition;
    private Quaternion playerRotation;

    [SerializeField]
    private StatusDate status;

    [SerializeField]
    private PlayerTransformData playerTransformData;

    [SerializeField]
    private FlagManagementData flagmentData;

    [SerializeField]
    private SettingData settingData;
    [SerializeField]
    private MemorizeLevelAndSceneName memorizeLevelAndSceneName;

    [SerializeField] 
    private Transform playerTransform;

    public string FileName { get => fileName; set => fileName = value; }

    //データベースを作成、アクセスする
    private IDbConnection CreateAndOpenDatabase() // 3
    {
        //パス作成　⇒　「MySaveData.sqlite」がパス、「MySaveData」がファイル名になる
        //この場合プロジェクトのルートディレクトリに作成される
        string dbUri = fileName; // 4
        //パスを使ってアクセスする
        IDbConnection dbConnection = new SqliteConnection(dbUri); // 5
        //データベースを開く
        dbConnection.Open(); // 6

        //もしまだデータが存在していないなら、作成する
        //表作成のコマンド作成
        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand(); // 6
        //コマンドテキスト作成
        //CREATE TABLE IF NOT EXISTS ⇒　表が存在していないなら作成する
        //SaveDataが表の名前
        //()の中は保存するデータ、この場合「id」と「hits」。すぐとなりがどういう型で保存するか　INTEGERなので数字で保存する
        dbCommandCreateTable.CommandText =
            "CREATE TABLE IF NOT EXISTS SaveData (" +
             "id INTEGER PRIMARY KEY, " +
             "positionX REAL, " +         // 位置情報Vector3 の　X 成分　
             "positionY REAL, " +         // 位置情報Vector3 の　Y 成分
             "positionZ REAL, " +         // 位置情報Vector3 の　Z 成分
             "quaternionX REAL," +
             "quaternionY REAL," +
             "quaternionZ REAL," +
             "quaternionW REAL," +
             "playerHP INTEGER," +
             "livePlayerHP INTEGER," +
             "playerAttackPower INTEGER," +
             "playerDefance INTEGER," +
             "playerLevel INTEGER," +
             "playerExp INTEGER," +
             "playerExElement INTEGER," +
             "bgmVolume REAL," +
             "soundVolume REAL," +
             "frameRate INTEGER," +
             "vibrationOn INTEGER," +
             "sceneNameNum INTEGER," +
             "tutorialClear INTEGER," +
             "map1Clear INTEGER," +
             "map2Clear INTEGER," +
             "gameClear INTEGER," +
             "Data1Exist INTEGER," +
             "Data2Exist INTEGER," +
             "Data3Exist INTEGER)";

        //コマンド実行
        dbCommandCreateTable.ExecuteReader(); // 8

        //このコネクションを返す
        return dbConnection;
    }

    //左クリックが押された場合
    public void Save()
    {
        // Insert hits into the table.
        //データベースにアクセスする
        IDbConnection dbConnection = CreateAndOpenDatabase(); // 2
                                                              //入力用のコマンド作成
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand(); // 9

        //SQLが対応できるデータへ変換用変数
        playerPosition = playerTransform.position;
        playerRotation = playerTransform.rotation;

        playerPositions[0] = playerPosition.x;
        playerPositions[1] = playerPosition.y;
        playerPositions[2] = playerPosition.z;

        playerRotations[0] = playerRotation.x;
        playerRotations[1] = playerRotation.y;
        playerRotations[2] = playerRotation.z;
        playerRotations[3] = playerRotation.w;

        playerStatus[0] = status.D_PlayerHP;
        playerStatus[1] = status.D_LivePlayerHP;
        playerStatus[2] = status.D_PlayerAttackPower;
        playerStatus[3] = status.D_PlayerDefance;
        playerStatus[4] = status.D_PlayerLevel;
        playerStatus[5] = status.D_PlayerExp;
        playerStatus[6] = status.D_ExpListElement;

        settingValues[0] = settingData.BgmVolume;
        settingValues[1] = settingData.SoundVolum;
        frameRate = settingData.FrameRate;
        vibrationOn = settingData.VibrationON;

        flagmentBools[0] = flagmentData.TutorialClear;
        flagmentBools[1] = flagmentData.Map1Clear;
        flagmentBools[2] = flagmentData.Map2Clear;
        flagmentBools[3] = flagmentData.GameClear;
        flagmentBools[4] = flagmentData.Data1Exist;
        flagmentBools[5] = flagmentData.Data2Exist;
        flagmentBools[6] = flagmentData.Data3Exist;



        int SQL_isVibrationOn = -1;

         SQL_isVibrationOn = vibrationOn ? 1 : 0;

        int SQL_tutorialClear = -1; 
        
         SQL_tutorialClear = flagmentBools[0] ? 1 : 0;

        int SQL_Map1Clear = -1;

         SQL_Map1Clear = flagmentBools[1] ? 1 : 0;

        int SQL_Map2Clear = -1;

        SQL_Map2Clear = flagmentBools[2] ? 1 : 0;

        int SQL_gameClear = -1;

        SQL_gameClear = flagmentBools[3] ? 1 : 0;

        int SQL_Data1Exist = -1;

        SQL_Data1Exist = flagmentBools[4] ? 1 : 0;

        int SQL_Data2Exist = -1;

        SQL_Data2Exist = flagmentBools[5] ? 1 : 0;

        int SQL_Data3Exist = -1;

        SQL_Data3Exist = flagmentBools[6] ? 1 : 0;

        int SQL_sceneNameNum = 0;

        switch (flagmentData.SceneName)
        {
            case "HomeMap":

                SQL_sceneNameNum = 1;

            break;

            case "Map1":

                SQL_sceneNameNum = 2;

            break;

            case "Map2":

                SQL_sceneNameNum = 3;

            break;

            case "Map3":

                SQL_sceneNameNum = 4;

            break;

        }

        
        //コマンドテキスト作成 
        //INSERT　⇒　入力・挿入
        //REPLACE　⇒　上書き
        //INTO ⇒　の中へ
        dbCommandInsertValue.CommandText =
            "INSERT OR REPLACE INTO SaveData " +
            "(id, " +
            "positionX, " +
            "positionY, " +
            "positionZ, " +
            "quaternionX, " +
            "quaternionY, " +
            "quaternionZ, " +
            "quaternionW," +
            "playerHP," +
            "livePlayerHP," +
            "playerAttackPower," +
            "playerDefance," +
            "playerLevel," +
            "playerExp," +
            "playerExElement," +
            "bgmVolume," +
            "soundVolume," +
            "frameRate," +
            "vibrationOn," +
            "sceneNameNum," +
            "tutorialClear," +
            "map1Clear," +
            "map2Clear," +
            "gameClear," +
            "Data1Exist," +
            "Data2Exist," +
            "Data3Exist)" +
            "VALUES " +
            "(0, " +
            "@positionX, " +
            "@positionY," +
            "@positionZ, " +
            "@quaternionX," +
            "@quaternionY," +
            "@quaternionZ," +
            "@quaternionW, " +
            "@playerHP," +
            "@livePlayerHP," +
            "@playerAttackPower," +
            "@playerDefance," +
            "@playerLevel," +
            "@playerExp," +
            "@playerExElement," +
            "@bgmVolume," +
            "@soundVolume," +
            "@frameRate," +
            "@vibrationOn," +
            "@sceneNameNum," +
            "@tutorialClear," +
            "@map1Clear," +
            "@map2Clear," +
            "@gameClear," +
            "@Data1Exist," +
            "@Data2Exist," +
            "@Data3Exist)";

        //パラメータでデータ保存
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionX", playerPositions[0]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionY", playerPositions[1]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionZ", playerPositions[2]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionX", playerRotations[0]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionY", playerRotations[1]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionZ", playerRotations[2]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionW", playerRotations[3]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerHP", playerStatus[0]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@livePlayerHP", playerStatus[1]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerAttackPower", playerStatus[2]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerDefance", playerStatus[3]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerLevel", playerStatus[4]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerExp", playerStatus[5]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerExElement", playerStatus[6]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@bgmVolume", settingValues[0]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@soundVolume", settingValues[1]));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@frameRate", frameRate));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@vibrationOn", SQL_isVibrationOn));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@sceneNameNum", SQL_sceneNameNum));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@tutorialClear", SQL_tutorialClear));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@map1Clear", SQL_Map1Clear));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@map2Clear", SQL_Map2Clear));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@gameClear", SQL_gameClear));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@Data1Exist", SQL_Data1Exist));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@Data2Exist", SQL_Data2Exist));
        dbCommandInsertValue.Parameters.Add(new SqliteParameter("@Data3Exist", SQL_Data3Exist));

        //コマンド実行
        dbCommandInsertValue.ExecuteNonQuery(); // 11

        // Remember to always close the connection at the end.
        //用が済んだらデータベースへのコネクションを切る
        dbConnection.Close(); // 12

        

        Debug.Log("save");
    }
   
    public void Load() // 13
    {
        //データベースを作成し、アクセスする　下にあるメソッド参考
        IDbConnection dbConnection = CreateAndOpenDatabase(); // 14
        //読み込み用のコマンド作成
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        //データベース内のデータを全部読み込む
        //SELECT ⇒　選択 
        //「＊」⇒　ALLの意味つまり「すべて」
        //FROM　⇒　から
        dbCommandReadValues.CommandText = "SELECT * FROM SaveData"; // 16
        //実際にコマンド実行
        IDataReader dataReader = dbCommandReadValues.ExecuteReader(); // 17

        //読み込むデータがまだ存在する場合、読んでいく
        while (dataReader.Read())
        {
            //ヒットカウントのインデックスは１
            playerPositions[0] = dataReader.GetFloat(1);
            playerPositions[1] = dataReader.GetFloat(2);
            playerPositions[2] = dataReader.GetFloat(3);
            playerRotations[0] = dataReader.GetFloat(4);
            playerRotations[1] = dataReader.GetFloat(5);
            playerRotations[2] = dataReader.GetFloat(6);
            playerRotations[3] = dataReader.GetFloat(7);
            playerStatus[0] = dataReader.GetInt32(8);
            playerStatus[1] = dataReader.GetInt32(9);
            playerStatus[2] = dataReader.GetInt32(10);
            playerStatus[3] = dataReader.GetInt32(11);
            playerStatus[4] = dataReader.GetInt32(12);
            playerStatus[5] = dataReader.GetInt32(13);
            playerStatus[6] = dataReader.GetInt32(14);
            settingValues[0] = dataReader.GetFloat(15);
            settingValues[1] = dataReader.GetFloat(16);
            frameRate = dataReader.GetInt32(17);
            int SQL_isVibrationOn = dataReader.GetInt32(18);
            int SQL_sceneNameNum = dataReader.GetInt32(19);
            int SQL_tutorialClear = dataReader.GetInt32(20);
            int SQL_map1Clear = dataReader.GetInt32(21);
            int SQL_map2Clear = dataReader.GetInt32(22);
            int SQL_gameClear = dataReader.GetInt32(23);
            //int SQL_Data1Exist = dataReader.GetInt32(24);
            //int SQL_Data2Exist = dataReader.GetInt32(25);
            //int SQL_Data3Exist = dataReader.GetInt32(26);

            if (SQL_isVibrationOn == 1)
            {
                vibrationOn = true;
            }
            else 
            {
               vibrationOn = false;
            }

            if (SQL_tutorialClear == 1)
            {
                flagmentBools[0] = true;
            }
            else
            {
                flagmentBools[0] = false;
            }

            if (SQL_map1Clear == 1)
            {
                flagmentBools[1] = true;
            }
            else
            {
                flagmentBools[1] = false;
            }

            if (SQL_map2Clear == 1)
            {
                flagmentBools[2] = true;
            }
            else
            {
                flagmentBools[2] = false;
            }

            if (SQL_gameClear == 1)
            {
                flagmentBools[3] = true;
            }
            else
            {
                flagmentBools[3] = false;
            }

            //if (SQL_Data1Exist == 1)
            //{
            //    flagmentBools[4] = true;
            //}
            //else
            //{
            //    flagmentBools[4] = false;
            //}

            //if (SQL_Data2Exist == 1)
            //{
            //    flagmentBools[5] = true;
            //}
            //else
            //{
            //    flagmentBools[5] = false;
            //}

            //if (SQL_Data3Exist == 1)
            //{
            //    flagmentBools[6] = true;
            //}
            //else
            //{
            //    flagmentBools[6] = false;
            //}


            playerTransformData.LoadTransform = new Vector3(playerPositions[0], playerPositions[1], playerPositions[2]);

            playerTransformData.LoadRotate = new Quaternion(playerRotations[0], playerRotations[1], playerRotations[2], playerRotations[3]);

            flagmentData.PositionLoad = true;
            flagmentData.RotateLoad = true;

            InputStatus();

            InputSettings();

            InputSceneName(SQL_sceneNameNum);

            InputFlagments();

           

        }

        //データ読み取ったら最後に必ずアクセスを閉じること！
        dbConnection.Close(); // 20
        Debug.Log("load");
    }

    public void InputStatus()
    { 
        status.D_PlayerHP = playerStatus[0];
        status.D_LivePlayerHP = playerStatus[1];
        status.D_PlayerAttackPower = playerStatus[2];
        status.D_PlayerDefance = playerStatus[3];
        status.D_PlayerLevel = playerStatus[4];
        status.D_PlayerExp = playerStatus[5];
        status.D_ExpListElement = playerStatus[6];

        //memorizeLevelAndSceneName.SavedPlayerLevelList[0]
    }

    private void InputSettings()
    { 
        settingData.BgmVolume = settingValues[0];
        settingData.SoundVolum = settingValues[1];
        settingData.FrameRate = frameRate;
    }

    private void InputFlagments()
    {
        flagmentData.TutorialClear = flagmentBools[0];
        flagmentData.Map1Clear = flagmentBools[1];
        flagmentData.Map2Clear = flagmentBools[2];
        flagmentData.GameClear = flagmentBools[3];
    }

    private void InputSceneName(int sceneNameNum)
    {

        switch (sceneNameNum)
        { 
          case 1:

                flagmentData.SceneName = "HomeMap";

          break;

            case 2:

                flagmentData.SceneName = "Map1";

                break;

            case 3:

                flagmentData.SceneName = "Map2";

                break;

            case 4:

                flagmentData.SceneName = "Map3";

                break;

        }

    }



    private void InputSavedStatus(int elementNum)
    {
        memorizeLevelAndSceneName.SavedPlayerLevelList[elementNum] = playerStatus[4].ToString();
    }

    private void InputSavedSceneName(int sceneNameNum,int elementNum)
    {

        switch (sceneNameNum)
        {
            case 1:

                memorizeLevelAndSceneName.SavedSceneNameList[elementNum] = "HomeMap";

                break;

            case 2:

                memorizeLevelAndSceneName.SavedSceneNameList[elementNum] = "Map1";

                break;

            case 3:

                memorizeLevelAndSceneName.SavedSceneNameList[elementNum] = "Map2";

                break;

            case 4:

                memorizeLevelAndSceneName.SavedSceneNameList[elementNum] = "Map3";

                break;

        }

    }

    private void InputSavedFlagments()
    {
        flagmentData.Data1Exist = flagmentBools[4];
        flagmentData.Data2Exist = flagmentBools[5];
        flagmentData.Data3Exist = flagmentBools[6];
    }



    public void FirstLoad()
    {
        //データベースを作成し、アクセスする　下にあるメソッド参考
        IDbConnection dbConnection = CreateAndOpenDatabase(); // 14
        //読み込み用のコマンド作成
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        //データベース内のデータを全部読み込む
        //SELECT ⇒　選択 
        //「＊」⇒　ALLの意味つまり「すべて」
        //FROM　⇒　から
        dbCommandReadValues.CommandText = "SELECT * FROM SaveData"; // 16
        //実際にコマンド実行
        IDataReader dataReader = dbCommandReadValues.ExecuteReader(); // 17

        switch (fileName)
        {

            case "URI=file:HakoniwaSekaiSaveData1.sqlite":
                elementNum = 0;
            break;

            case "URI=file:HakoniwaSekaiSaveData2.sqlite":
                elementNum = 1;
                break;

            case "URI=file:HakoniwaSekaiSaveData3.sqlite":
                elementNum = 2;
                break;
        }

        while (dataReader.Read())
        {
            //ヒットカウントのインデックスは１
          
            playerStatus[4] = dataReader.GetInt32(12);
           
            int SQL_sceneNameNum = dataReader.GetInt32(19);

            int SQL_Data1Exist = dataReader.GetInt32(24);
            int SQL_Data2Exist = dataReader.GetInt32(25);
            int SQL_Data3Exist = dataReader.GetInt32(26);

            if (SQL_Data1Exist == 1)
            {
                flagmentBools[4] = true;
            }
            else
            {
                flagmentBools[4] = false;
            }

            if (SQL_Data2Exist == 1)
            {
                flagmentBools[5] = true;
            }
            else
            {
                flagmentBools[5] = false;
            }

            if (SQL_Data3Exist == 1)
            {
                flagmentBools[6] = true;
            }
            else
            {
                flagmentBools[6] = false;
            }

            InputSavedStatus(elementNum);
          
            InputSavedSceneName(SQL_sceneNameNum,elementNum);

            InputSavedFlagments();

        }

        dbConnection.Close(); // 20
    }

    //public void ResetData()
    //{
    //    //データベースにアクセスする
    //    IDbConnection dbConnection = CreateAndOpenDatabase(); // 2
    //                                                          //入力用のコマンド作成
    //    IDbCommand dbCommandInsertValue = dbConnection.CreateCommand(); // 9

    //    //コマンドテキスト作成 
    //    //INSERT　⇒　入力・挿入
    //    //REPLACE　⇒　上書き
    //    //INTO ⇒　の中へ
    //    dbCommandInsertValue.CommandText =
    //        "INSERT OR REPLACE INTO SaveData (id, clickCount, isPoisoned, positionX, positionY, positionZ, quaternionX, quaternionY, quaternionZ, quaternionW, usePotion, herbCount) VALUES (0, @isPoisoned, @positionX, @positionY, @positionZ, @quaternionX, @quaternionY, @quaternionZ, @quaternionW, @herbCount)";

    //    //パラメータでデータ保存
    //    //シーン開始時のpositionとrotationの値がすべて０の場合
    //    //本来はプレイヤーのスタート地点の位置にリセットすべき
        
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@isPoisoned", false));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionX", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionY", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionZ", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionX", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionY", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionZ", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionW", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerHP", 0));

    //    //コマンド実行
    //    dbCommandInsertValue.ExecuteNonQuery(); // 11

    //    //用が済んだらデータベースへのコネクションを切る
    //    dbConnection.Close(); // 12

    //    Debug.Log("reset");
    //}

}