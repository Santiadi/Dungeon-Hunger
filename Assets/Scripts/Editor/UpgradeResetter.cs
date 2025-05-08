using UnityEngine;
using UnityEditor;

public class UpgradeResetter
{
    [MenuItem("Herramientas/Resetear upgrades (currentLevel = 0)")]
    public static void ResetUpgrades()
    {
        string[] guids = AssetDatabase.FindAssets("t:UpgradeData");
        int contador = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            UpgradeData upgrade = AssetDatabase.LoadAssetAtPath<UpgradeData>(path);

            if (upgrade != null && upgrade.currentLevel != 0)
            {
                upgrade.currentLevel = 0;
                EditorUtility.SetDirty(upgrade);
                contador++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Se han reiniciado {contador} upgrades a nivel 0.");
    }
}
