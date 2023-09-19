using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace UB
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        //before creating new save file, check to see if file already exists (max 10 char slots)
        public bool CheckToSeeIfFileExists()
        {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //used to delete character save files
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        //used to create a safefile
        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            //make a path to the save file (a location on your computer)
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                //create directory file will be written to if it doesnt exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE AT SAVE PATH: + " + savePath);

                //serialize the C# game data object into json
                string dataToStore = JsonUtility.ToJson(characterData, true);

                //write file to computer
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA< GAME NOT SAVED " + savePath + "\n" + ex);
            }
        }

        //used to load save file
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;

            //make path to load the file from computer
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    //deserialize data from json to a format that unity can use
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.LogError("COULD NOT LOAD FILE");
                }
            }

            return characterData;
        }
    }  
}

