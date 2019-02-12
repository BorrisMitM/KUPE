using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
//This Compontent gets started in a preload scene and persists through all scenes
//handles levels, and input mappings
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public static List<InputInformation> inputInformation;
    public static bool useKeyboard;
    public static int keyBoardPlayersAmount;
    public static bool paused;
    [SerializeField]
    private static Characters characters;
    [SerializeField]
    public static Levels levels;
    public static int levelToLoad;
    public static bool inBetweenLevels;
    public static bool doingRun;
    public static string previousScene;
    public static int unlockedLevel;
    public static int UnlockedLevel { get { return unlockedLevel >= 16 ? 18 : unlockedLevel; }}
    public static int playerAmount;
    public static Cutscenes cutscenes;
    private void Awake()
    {
        if (instance == null) instance = this;  //there should only be one gameManager at all times
        else if (instance != this)
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);      //persist through all scenes
        characters = (Characters)Resources.Load("Characters");
        levels = (Levels)Resources.Load("Levels");
        cutscenes = (Cutscenes)Resources.Load("Cutscenes");
        useKeyboard = true;
        keyBoardPlayersAmount = PlayerPrefs.GetInt("KeyBoardPlayersAmount", 2);
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log(unlockedLevel + " level unlocked.");
        playerAmount = 4;
    }

    public static void SaveLevel()
    {
        if(unlockedLevel < levelToLoad + 2)
            unlockedLevel = levelToLoad + 2;
        PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
    }
    public static void Initialize() //initializes in MainMenu
    {
        Debug.Log("Initializing GameManager");
        inputInformation = new List<InputInformation>();
        paused = false;
        PlayerChoser.playerNr = 0;
        inBetweenLevels = false;
        doingRun = false;
        Death.playerDeaths = new Dictionary<PlayerType, int>();
    }
    public static string GetLevelString()   //returns the right level string for the amount of players. 
    {
        string add = "";
        if (playerAmount == 1) add = "K";
        else if (playerAmount == 2) add = "PE";
        else if (playerAmount == 3) add = "KUP";
        string cutsceneName = cutscenes.GetCutsceneName(levels.levels[levelToLoad]);
        if (cutsceneName != null && SceneManager.GetActiveScene().name != cutsceneName && !doingRun)
            return cutsceneName;
        else 
            return levels.levels[levelToLoad] + add;
    }
    public static string GetCleanLevelString()  //returns general level name
    {
        return levels.levels[levelToLoad];
    }
    public static void SetLevelToLoad(string name)      
    {
        levelToLoad = levels.levels.IndexOf(name);
    }
    public static void AddInputInformation(int cursorNr,int inputNr)        //adds a player/character combination to the list
    {
        inputInformation.Add(new InputInformation(characters.charPrefabs[cursorNr],inputNr));
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "1_MainMenu")
        {
            Initialize();
        }
        else if(scene.name == "CharacterSelection")
        {
            PlayerChoser.playerNr = 0;
        }
    }

}

[System.Serializable]
public class InputInformation   //small class to hold the input couples
{
    public GameObject characterPrefab;
    public int inputNr;

    public InputInformation(GameObject _characterPrefab,int _inputNr)
    {
        characterPrefab = _characterPrefab;
        inputNr = _inputNr;
    }
}