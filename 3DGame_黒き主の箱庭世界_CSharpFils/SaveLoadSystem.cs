using Mono.Data.Sqlite; // 1
using System.Collections.Generic;
using System.Data; // 1
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 繧ｻ繝ｼ繝悶ｄ繝ｭ繝ｼ繝峨↑縺ｩ縺ｮ繧ｻ繝ｼ繝悶ョ繝ｼ繧ｿ縺ｫ髢｢縺吶ｋ繝｡繧ｽ繝・ラ繧堤ｮ｡逅・☆繧九せ繧ｯ繝ｪ繝励ヨ縺ｧ縺吶・
/// </summary>
public class SaveLoadSystem : MonoBehaviour
{
    // Resources: //蜿り・
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

    //繝・・繧ｿ繝吶・繧ｹ繧剃ｽ懈・縲√い繧ｯ繧ｻ繧ｹ縺吶ｋ
    private IDbConnection CreateAndOpenDatabase() // 3
    {
        //繝代せ菴懈・縲竍偵縲勲ySaveData.sqlite縲阪′繝代せ縲√勲ySaveData縲阪′繝輔ぃ繧､繝ｫ蜷阪↓縺ｪ繧・
        //縺薙・蝣ｴ蜷医・繝ｭ繧ｸ繧ｧ繧ｯ繝医・繝ｫ繝ｼ繝医ョ繧｣繝ｬ繧ｯ繝医Μ縺ｫ菴懈・縺輔ｌ繧・
        string dbUri = fileName; // 4
        //繝代せ繧剃ｽｿ縺｣縺ｦ繧｢繧ｯ繧ｻ繧ｹ縺吶ｋ
        IDbConnection dbConnection = new SqliteConnection(dbUri); // 5
        //繝・・繧ｿ繝吶・繧ｹ繧帝幕縺・
        dbConnection.Open(); // 6

        //繧ゅ＠縺ｾ縺繝・・繧ｿ縺悟ｭ伜惠縺励※縺・↑縺・↑繧峨∽ｽ懈・縺吶ｋ
        //陦ｨ菴懈・縺ｮ繧ｳ繝槭Φ繝我ｽ懈・
        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand(); // 6
        //繧ｳ繝槭Φ繝峨ユ繧ｭ繧ｹ繝井ｽ懈・
        //CREATE TABLE IF NOT EXISTS 竍偵陦ｨ縺悟ｭ伜惠縺励※縺・↑縺・↑繧我ｽ懈・縺吶ｋ
        //SaveData縺瑚｡ｨ縺ｮ蜷榊燕
        //()縺ｮ荳ｭ縺ｯ菫晏ｭ倥☆繧九ョ繝ｼ繧ｿ縲√％縺ｮ蝣ｴ蜷医景d縲阪→縲敬its縲阪ゅ☆縺舌→縺ｪ繧翫′縺ｩ縺・＞縺・梛縺ｧ菫晏ｭ倥☆繧九°縲INTEGER縺ｪ縺ｮ縺ｧ謨ｰ蟄励〒菫晏ｭ倥☆繧・
        dbCommandCreateTable.CommandText =
            "CREATE TABLE IF NOT EXISTS SaveData (" +
             "id INTEGER PRIMARY KEY, " +
             "positionX REAL, " +         // 菴咲ｽｮ諠・ｱVector3 縺ｮ縲X 謌仙・縲
             "positionY REAL, " +         // 菴咲ｽｮ諠・ｱVector3 縺ｮ縲Y 謌仙・
             "positionZ REAL, " +         // 菴咲ｽｮ諠・ｱVector3 縺ｮ縲Z 謌仙・
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

        //繧ｳ繝槭Φ繝牙ｮ溯｡・
        dbCommandCreateTable.ExecuteReader(); // 8

        //縺薙・繧ｳ繝阪け繧ｷ繝ｧ繝ｳ繧定ｿ斐☆
        return dbConnection;
    }

    //蟾ｦ繧ｯ繝ｪ繝・け縺梧款縺輔ｌ縺溷ｴ蜷・
    public void Save()
    {
        // Insert hits into the table.
        //繝・・繧ｿ繝吶・繧ｹ縺ｫ繧｢繧ｯ繧ｻ繧ｹ縺吶ｋ
        IDbConnection dbConnection = CreateAndOpenDatabase(); // 2
                                                              //蜈･蜉帷畑縺ｮ繧ｳ繝槭Φ繝我ｽ懈・
        IDbCommand dbCommandInsertValue = dbConnection.CreateCommand(); // 9

        //SQL縺悟ｯｾ蠢懊〒縺阪ｋ繝・・繧ｿ縺ｸ螟画鋤逕ｨ螟画焚
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

        
        //繧ｳ繝槭Φ繝峨ユ繧ｭ繧ｹ繝井ｽ懈・ 
        //INSERT縲竍偵蜈･蜉帙・謖ｿ蜈･
        //REPLACE縲竍偵荳頑嶌縺・
        //INTO 竍偵縺ｮ荳ｭ縺ｸ
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

        //繝代Λ繝｡繝ｼ繧ｿ縺ｧ繝・・繧ｿ菫晏ｭ・
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

        //繧ｳ繝槭Φ繝牙ｮ溯｡・
        dbCommandInsertValue.ExecuteNonQuery(); // 11

        // Remember to always close the connection at the end.
        //逕ｨ縺梧ｸ医ｓ縺繧峨ョ繝ｼ繧ｿ繝吶・繧ｹ縺ｸ縺ｮ繧ｳ繝阪け繧ｷ繝ｧ繝ｳ繧貞・繧・
        dbConnection.Close(); // 12

        

        Debug.Log("save");
    }
   
    public void Load() // 13
    {
        //繝・・繧ｿ繝吶・繧ｹ繧剃ｽ懈・縺励√い繧ｯ繧ｻ繧ｹ縺吶ｋ縲荳九↓縺ゅｋ繝｡繧ｽ繝・ラ蜿り・
        IDbConnection dbConnection = CreateAndOpenDatabase(); // 14
        //隱ｭ縺ｿ霎ｼ縺ｿ逕ｨ縺ｮ繧ｳ繝槭Φ繝我ｽ懈・
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        //繝・・繧ｿ繝吶・繧ｹ蜀・・繝・・繧ｿ繧貞・驛ｨ隱ｭ縺ｿ霎ｼ繧
        //SELECT 竍偵驕ｸ謚・
        //縲鯉ｼ翫坂∫縲ALL縺ｮ諢丞袖縺､縺ｾ繧翫後☆縺ｹ縺ｦ縲・
        //FROM縲竍偵縺九ｉ
        dbCommandReadValues.CommandText = "SELECT * FROM SaveData"; // 16
        //螳滄圀縺ｫ繧ｳ繝槭Φ繝牙ｮ溯｡・
        IDataReader dataReader = dbCommandReadValues.ExecuteReader(); // 17

        //隱ｭ縺ｿ霎ｼ繧繝・・繧ｿ縺後∪縺蟄伜惠縺吶ｋ蝣ｴ蜷医∬ｪｭ繧薙〒縺・￥
        while (dataReader.Read())
        {
            //繝偵ャ繝医き繧ｦ繝ｳ繝医・繧､繝ｳ繝・ャ繧ｯ繧ｹ縺ｯ・・
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

        //繝・・繧ｿ隱ｭ縺ｿ蜿悶▲縺溘ｉ譛蠕後↓蠢・★繧｢繧ｯ繧ｻ繧ｹ繧帝哩縺倥ｋ縺薙→・・
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
        //繝・・繧ｿ繝吶・繧ｹ繧剃ｽ懈・縺励√い繧ｯ繧ｻ繧ｹ縺吶ｋ縲荳九↓縺ゅｋ繝｡繧ｽ繝・ラ蜿り・
        IDbConnection dbConnection = CreateAndOpenDatabase(); // 14
        //隱ｭ縺ｿ霎ｼ縺ｿ逕ｨ縺ｮ繧ｳ繝槭Φ繝我ｽ懈・
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand(); // 15
        //繝・・繧ｿ繝吶・繧ｹ蜀・・繝・・繧ｿ繧貞・驛ｨ隱ｭ縺ｿ霎ｼ繧
        //SELECT 竍偵驕ｸ謚・
        //縲鯉ｼ翫坂∫縲ALL縺ｮ諢丞袖縺､縺ｾ繧翫後☆縺ｹ縺ｦ縲・
        //FROM縲竍偵縺九ｉ
        dbCommandReadValues.CommandText = "SELECT * FROM SaveData"; // 16
        //螳滄圀縺ｫ繧ｳ繝槭Φ繝牙ｮ溯｡・
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
            //繝偵ャ繝医き繧ｦ繝ｳ繝医・繧､繝ｳ繝・ャ繧ｯ繧ｹ縺ｯ・・
          
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
    //    //繝・・繧ｿ繝吶・繧ｹ縺ｫ繧｢繧ｯ繧ｻ繧ｹ縺吶ｋ
    //    IDbConnection dbConnection = CreateAndOpenDatabase(); // 2
    //                                                          //蜈･蜉帷畑縺ｮ繧ｳ繝槭Φ繝我ｽ懈・
    //    IDbCommand dbCommandInsertValue = dbConnection.CreateCommand(); // 9

    //    //繧ｳ繝槭Φ繝峨ユ繧ｭ繧ｹ繝井ｽ懈・ 
    //    //INSERT縲竍偵蜈･蜉帙・謖ｿ蜈･
    //    //REPLACE縲竍偵荳頑嶌縺・
    //    //INTO 竍偵縺ｮ荳ｭ縺ｸ
    //    dbCommandInsertValue.CommandText =
    //        "INSERT OR REPLACE INTO SaveData (id, clickCount, isPoisoned, positionX, positionY, positionZ, quaternionX, quaternionY, quaternionZ, quaternionW, usePotion, herbCount) VALUES (0, @isPoisoned, @positionX, @positionY, @positionZ, @quaternionX, @quaternionY, @quaternionZ, @quaternionW, @herbCount)";

    //    //繝代Λ繝｡繝ｼ繧ｿ縺ｧ繝・・繧ｿ菫晏ｭ・
    //    //繧ｷ繝ｼ繝ｳ髢句ｧ区凾縺ｮposition縺ｨrotation縺ｮ蛟､縺後☆縺ｹ縺ｦ・舌・蝣ｴ蜷・
    //    //譛ｬ譚･縺ｯ繝励Ξ繧､繝､繝ｼ縺ｮ繧ｹ繧ｿ繝ｼ繝亥慍轤ｹ縺ｮ菴咲ｽｮ縺ｫ繝ｪ繧ｻ繝・ヨ縺吶∋縺・
        
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@isPoisoned", false));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionX", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionY", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@positionZ", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionX", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionY", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionZ", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@quaternionW", 0));
    //    dbCommandInsertValue.Parameters.Add(new SqliteParameter("@playerHP", 0));

    //    //繧ｳ繝槭Φ繝牙ｮ溯｡・
    //    dbCommandInsertValue.ExecuteNonQuery(); // 11

    //    //逕ｨ縺梧ｸ医ｓ縺繧峨ョ繝ｼ繧ｿ繝吶・繧ｹ縺ｸ縺ｮ繧ｳ繝阪け繧ｷ繝ｧ繝ｳ繧貞・繧・
    //    dbConnection.Close(); // 12

    //    Debug.Log("reset");
    //}

}
