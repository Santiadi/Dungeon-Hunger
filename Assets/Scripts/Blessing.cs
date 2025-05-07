// Upgrade.cs
using UnityEngine;

public enum BlessingRarity { Common, Rare, Epic }

[CreateAssetMenu(fileName = "NewBlessing", menuName = "Blessings")]
public class Blessing: ScriptableObject
{
    public string blessingName;
    public string description;
    public BlessingRarity rarity;
    public Sprite icon; // Icono individual de la mejora
}
