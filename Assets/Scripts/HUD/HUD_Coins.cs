using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDCoins : MonoBehaviour
{
    public Text coinsText;

    void Start()
    {
        UpdateCoins(GameManager.Instance.coins);
    }

    public void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }
}
