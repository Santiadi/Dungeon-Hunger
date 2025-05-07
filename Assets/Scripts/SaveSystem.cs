using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerdata.json";
    private static string path = Application.persistentDataPath + "/blessings.json";

    public static void SavePlayer(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public static void SaveUpgrades(BlessData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    // CARGAR POR UPGRADE
    public static BlessData LoadUpgrade(UpgradeData upgradeData)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (string.IsNullOrEmpty(json))  // Verificar si el archivo está vacío
            {
                Debug.LogError("El archivo de guardado está vacío.");
                return new BlessData();  // Retorna una instancia vacía si el archivo está vacío
            }

            BlessData loadedData = JsonUtility.FromJson<BlessData>(json);

            // Si cargamos datos, aseguramos que se apilen bien
            return loadedData;
        }
        else
        {
            Debug.LogError("El archivo de guardado no existe.");
            return new BlessData();  // Retorna una instancia vacía si el archivo no existe
        }
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

    // Obtener el nivel de un upgrade específico por nombre
    public static int GetUpgradeLevel(string upgradeName)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(json))
            {
                BlessData data = JsonUtility.FromJson<BlessData>(json);
                int index = data.name.IndexOf(upgradeName);
                if (index >= 0 && index < data.level.Count)
                {
                    return data.level[index];
                }
            }
        }
        return 0; // Nivel por defecto si no existe
    }

    public static float GetUpgradeBonus(string upgradeName)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(json))
            {
                BlessData data = JsonUtility.FromJson<BlessData>(json);
                int index = data.name.IndexOf(upgradeName);
                if (index >= 0 && index < data.bonusPerLevel.Count)
                {
                    return data.bonusPerLevel[index];
                }
            }
        }
        return 0; // Bonus por defecto si no existe
    }
}
