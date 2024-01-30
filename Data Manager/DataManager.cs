using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System;
namespace DywFunctions {
    namespace DataHandle {
        public static class DataManager
        {
            public static void JSONSaveData<T>(T data, string fileName) {
                string fullpath = Application.persistentDataPath + "/Data";
                bool fileExists = Directory.Exists(fullpath);

                if (!fileExists) {
                    Directory.CreateDirectory(fullpath);
                }

                string json = JsonUtility.ToJson(data);
                File.WriteAllText(fullpath + fileName, json);
            }

            public static T JSONLoadData<T>(string fileName) {
                string fullpath = Application.persistentDataPath + "/Data" + fileName;

                Debug.Log(fullpath);

                if (File.Exists(fullpath)) {
                    string textjson = File.ReadAllText(fullpath);

                    T obj = default;
                    try {
                        obj = JsonUtility.FromJson<T>(textjson);
                    } catch (Exception ex) {
                        Debug.Log(ex.Message);
                    }
                    return obj;    
                } 
                else {
                    return default;
                }
            }

            public static void BINSaveData<T>(T objectToSave, string fileName) {
                try {
                    string filepath = FilePath(fileName);
                    string name = FileName(fileName);

                    DirectoryInfo dir = Directory.CreateDirectory(filepath);
                    string dataPath = dir.FullName + "/" + name;

                    FileStream fileStream = new FileStream(dataPath, FileMode.Create);
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    binaryFormatter.Serialize(fileStream, objectToSave);
                    fileStream.Close();

                    // Debug.Log("Data saved in " + dataSPath);
                } catch (Exception ex) {
                    Debug.LogError(ex.Message);
                }

            }

            public static T BINLoadData<T>(string fileName) {
                string dataPath = fileName;
                T objectLoaded = default; 

                try {
                    if (File.Exists(dataPath)) {
                        FileStream fileStream = new FileStream(dataPath, FileMode.Open);
                        BinaryFormatter binaryFormatter = new BinaryFormatter();

                        objectLoaded = (T)binaryFormatter.Deserialize(fileStream);
                        fileStream.Close();
                        //Debug.Log("Data loaded");
                    }
                    else {
                        throw new Exception("No existe archivo");
                    } 
                } catch (Exception ex) {
                    ex.GetBaseException();

                    throw new Exception("Data corrupted or doesn't exists!");
                }

                return objectLoaded;
            }


            public static string FilePath(string filepath) {
                string path = "";
                string[] fileRoutes = filepath.Split('/');

                foreach (var r in fileRoutes)
                {
                    if (r.Split('.').Length > 1) break;
                    else path += path != "" ? "/" + r : r;
                }

                return path;
            }
            public static string FileName(string filepath)
            {
                string filename = "";
                string[] fileRoutes = filepath.Split('/');

                foreach (var r in fileRoutes)
                {
                    if (r.Split('.').Length > 1) { filename = r; break; }
                }

                return filename;
            }

        }
    }
}
