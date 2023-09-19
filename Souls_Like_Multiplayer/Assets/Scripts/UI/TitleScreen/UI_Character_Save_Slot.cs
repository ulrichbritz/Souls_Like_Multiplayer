using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UB
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            switch (characterSlot)
            {
                //save slot 01
                case CharacterSlot.CharacterSlot_01:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 02
                case CharacterSlot.CharacterSlot_02:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 03
                case CharacterSlot.CharacterSlot_03:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 04
                case CharacterSlot.CharacterSlot_04:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 05
                case CharacterSlot.CharacterSlot_05:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 06
                case CharacterSlot.CharacterSlot_06:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 07
                case CharacterSlot.CharacterSlot_07:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 08
                case CharacterSlot.CharacterSlot_08:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 09
                case CharacterSlot.CharacterSlot_09:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                //save slot 10
                case CharacterSlot.CharacterSlot_10:
                    saveFileDataWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                    //if file exists get info from it
                    if (saveFileDataWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                    }
                    //else disable gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                default:
                    break;
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
        }
    }
}

