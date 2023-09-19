using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace UB
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance;

        [Header("Menu Objects")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;


        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkButton;
        [SerializeField] GameObject deleteCharacterSlotPopUp;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;

        [Header("Save Slots")]
        public CharacterSlot currentSelectedCharacterSlot = CharacterSlot.No_Slot;

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

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            //close main menu
            titleScreenMainMenu.SetActive(false);

            //open load menu
            titleScreenLoadMenu.SetActive(true);

            // Select the return button first
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            //close load menu
            titleScreenLoadMenu.SetActive(false);

            //open main menu
            titleScreenMainMenu.SetActive(true);

            // Select the return button first
            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }

        //character slots
        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedCharacterSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedCharacterSlot = CharacterSlot.No_Slot;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedCharacterSlot != CharacterSlot.No_Slot)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }           
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentSelectedCharacterSlot);

            //disable and enable to refresh chararacter slots after deletion
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();           
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }
}

