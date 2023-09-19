using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UB
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [HideInInspector] public PlayerManager playerManager;

        [Header("Save/Load")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] private int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;        
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            } 

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CharacterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CharacterSlot_10";
                    break;
                default:
                    break;
            }

            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            #region check to see if we can create a new save file for all slots
            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            //check to see if we can create a new save file (check for other existing files first)
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                //if this profile slot is not taken, make save file here
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;

                currentCharacterData = new CharacterSaveData();

                NewGame();

                return;
            }

            #endregion

            //if there are no free slots, notify player
            TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();
        }

        private void NewGame()
        {
            //temporary code
            playerManager.playerNetworkManager.strength.Value = 10;
            playerManager.playerNetworkManager.agility.Value = 10;

            // save newly created char stats and items
            SaveGame();
            // load into world
            StartCoroutine(LoadWorldScene());
        }

        public void LoadGame()
        {
            //load a previous file with name depending on slot we are using
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            //this path below generally works on multiple machines
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            //save the current file under a specific file name depending on which slot is in use
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            //this path below generally works on multiple machines
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            // pass the players info from game to the save file
            playerManager.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            //write that info onto a json file saved to computer
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            saveFileDataWriter = new SaveFileDataWriter();
            //this path below generally works on multiple machines
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            //choose a file to delete based on name
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            saveFileDataWriter.DeleteSaveFile();
        }

        //load all character profiles on device when starting game
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();
        }

        public IEnumerator LoadWorldScene()
        {
            //if you just want 1 world scene use this
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            // if you want to use different scenes for level in your project  use this
            //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);

            playerManager.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

            yield return null;
        }      

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

