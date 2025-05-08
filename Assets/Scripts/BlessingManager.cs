using System.Collections.Generic;
using UnityEngine;

public class BlessingManager : MonoBehaviour
{
    public List<Blessing> allBlessings;

    [Header("Sprites de fondo por rareza")]
    public Sprite commonCardSprite;
    public Sprite rareCardSprite;
    public Sprite epicCardSprite;

    public List<Blessing> GetRandomBlessings(int count)
    {
        List<Blessing> selected = new List<Blessing>();

        for (int i = 0; i < count; i++)
        {
            float roll = Random.Range(0f, 1f);
            BlessingRarity rarity = roll switch
            {
                < 0.05f => BlessingRarity.Epic,
                < 0.15f => BlessingRarity.Rare,
                _ => BlessingRarity.Common
            };

            List<Blessing> candidates = allBlessings.FindAll(bl =>
                bl.rarity == rarity &&
                (bl.blessingName != "Extra Slot" || !slot2AlreadyUnlocked()) // solo si no se ha usado
            );

            if (candidates.Count == 0)
            {
                i--;
                continue;
            }

            Blessing chosen = candidates[Random.Range(0, candidates.Count)];
            if (!selected.Contains(chosen))
                selected.Add(chosen);
            else
                i--;
        }

        return selected;
    }

    private bool slot2AlreadyUnlocked()
    {
        return FindObjectOfType<InventoryManager>()?.CanUnlockSlot2() == false;
    }

    public Sprite GetCardSprite(BlessingRarity rarity)
    {
        return rarity switch
        {
            BlessingRarity.Common => commonCardSprite,
            BlessingRarity.Rare => rareCardSprite,
            BlessingRarity.Epic => epicCardSprite,
            _ => null
        };
    }
}
