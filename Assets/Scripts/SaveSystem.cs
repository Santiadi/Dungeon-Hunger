using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerdata.json";

    public static void SavePlayer(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public static PlayerData LoadPlayer()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            if (string.IsNullOrEmpty(json))  // Verificar si el archivo está vacío
            {
                Debug.LogError("El archivo de guardado está vacío.");
                return new PlayerData(0); // Retorna un valor por defecto si el archivo está vacío
            }

            return JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Debug.LogError("El archivo de guardado no existe.");
            return new PlayerData(0); // Retorna un valor por defecto si el archivo no existe
        }
    }
}
