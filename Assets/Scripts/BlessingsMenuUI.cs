using System.Collections.Generic;
using UnityEngine;

public class BlessingsMenuUI : MonoBehaviour
{
    public List<BlessingCard> cards; // Asigna las 3 cartas desde el Inspector

    public void Setup(List<Blessing> blessings)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Setup(blessings[i]);
        }
    }
}
