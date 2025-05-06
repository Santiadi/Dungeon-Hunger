using TMPro;
using UnityEngine;

public class HUDCoins : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    void Start()
    {
        UpdateCoins(GameManager.Instance.coins);
    }

    public void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }
}
