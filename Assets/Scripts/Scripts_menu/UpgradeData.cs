using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Información general")]
    public string upgradeName;
    [TextArea(2, 4)]
    public string description;

    [Header("Visual")]
    public Sprite icon;
    public Sprite frame;

    [Header("Progreso")]
    public int maxLevel = 3;
    [HideInInspector]
    public int currentLevel = 0;

    [Header("Coste en oro por nivel")]
    public int[] costPerLevel;

    [Header("Valores por nivel")]
    public float[] bonusPerLevel;
}