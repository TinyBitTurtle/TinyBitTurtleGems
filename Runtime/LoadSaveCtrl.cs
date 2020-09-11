using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Text;
using TinyBitTurtle.Core;

namespace TinyBitTurtle.Gems
{
    public partial class loadSaveCtrl : SingletonMonoBehaviour<loadSaveCtrl>
    {
        private string permanentPath;
        private BinaryFormatter bf = new BinaryFormatter();

        protected GameSaveData gameData;

        private void Awake()
        {
            permanentPath = Application.persistentDataPath + "/game.sav";
        }

        //
        public virtual void SaveGameData()
        {
            // get the save string from the serialized class
            string stream = JsonUtility.ToJson(permanentPath);

            // 64 based encode the string
            byte[] encodedBytes = Encoding.UTF8.GetBytes(stream);
            string encodedString = Convert.ToBase64String(encodedBytes);

            // write out the encoded string
            File.WriteAllText(permanentPath, encodedString);
        }

        //
        public void LoadGameData()
        {
            // do we have a save file?
            if (File.Exists(permanentPath))
            {
                // read in the encoded string
                string stream = File.ReadAllText(permanentPath);

                // decode it
                byte[] decryptedBytes = Convert.FromBase64String(stream);
                string decryptedString = Encoding.UTF8.GetString(decryptedBytes);

                // set values into save class
                gameData = JsonUtility.FromJson<GameSaveData>(decryptedString);
                //currentLevel = gameData.currentLevel;
            }
           
        }

        public GameSaveData LoadGameInit()
        {
            //gameData = new loadSaveCtrl.GameData();

            //TextAsset json = Resources.Load<TextAsset>("GameInit");

            //return JsonUtility.FromJson<GameData>(json.text);
            return null;

        }
    }
}