using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cats3.UI;

namespace Cats3.Data
{
    [Serializable]
    public class SaveData
    {

        public List<bool> _isActive = new (30);
        public List<int> _highScores = new(30);
        public List<int> _stars = new(30);
    }

    public class GameData : MonoBehaviour
    {
        public static GameData _gameData;
        public SaveData _saveData;

        private void Awake()
        {
            if(_gameData == null)
            {
                DontDestroyOnLoad(this.gameObject);
                _gameData = this;
            }
            else 
            {
                Destroy(this.gameObject);
            }
            Load();
        }

        private void Start()
        {
           
        }

        public void Save()
        {
            //сделать бинарное форматирование, чтобы читать файлы
            BinaryFormatter formatter = new BinaryFormatter();
            //Направим из программы в файл
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);
            //Создадим копию данных для сохранения
            SaveData data = new SaveData();
            data = _saveData;
            //Актуальные данные для сохрана
            formatter.Serialize(file, data);
            file.Close();
            Debug.Log("Сохранено");
        }

        public void Load()
        {
            //Проверим наличие файла сохранения
            if(File.Exists(Application.persistentDataPath + "/player.dat"))
            {
                //сделаем двоичный формат
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
                _saveData = formatter.Deserialize(file) as SaveData;
                file.Close();
                Debug.Log("Загрузка");
            }
            else 
            {
                for (int i = 0; i < 28; i++)
                {
                    _saveData._isActive.Add(false);
                    _saveData._stars.Add(0);
                    _saveData._highScores.Add(0);
                }
                _saveData._isActive[0] = true;
                _saveData._isActive[1] = true;
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }


        private void OnDisable()
        {
            Save();
        }
    }
}
