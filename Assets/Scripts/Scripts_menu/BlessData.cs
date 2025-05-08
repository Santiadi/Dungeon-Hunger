using System;
using System.Collections.Generic;

[System.Serializable]
public class BlessData
{
    public List<string> name;  // Usamos listas para facilitar la adición de nuevos elementos
    public List<int> level;
    public List<float> bonusPerLevel = new List<float>(); // Lista para los valores por nivel

    public BlessData()
    {
        name = new List<string>();  // Inicializar las listas
        level = new List<int>();
        bonusPerLevel = new List<float>();
        UnityEngine.Debug.Log("BlessData creado con listas vacías");
    }

    // Constructor para inicializar con valores
    public BlessData(List<string> name, List<int> level, List<float> bonusPerLevel)
    {
        this.name = name;
        this.level = level;
        this.bonusPerLevel = bonusPerLevel;
        UnityEngine.Debug.Log("BlessData creado con datos: " + string.Join(",", level));
    }

    // Método para agregar nuevos datos
    public void AddBlessing(string blessingName, int blessingLevel, float bonusValue)
    {
        UnityEngine.Debug.Log($"AddBlessing: {blessingName}, nivel={blessingLevel}, bonus={bonusValue}");
        if(name.Contains(blessingName))
        {
            int index = name.IndexOf(blessingName);
            level[index] = blessingLevel;  // Actualiza el nivel si ya existe
            bonusPerLevel[index] = bonusValue; // Actualiza el valor por nivel si ya existe
            return;
        }
        name.Add(blessingName);
        level.Add(blessingLevel);
        bonusPerLevel.Add(bonusValue); // Agrega el nuevo valor por nivel
    }

}
