using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coins = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveGame();
    }

    public void SaveGame()
    {
        PlayerData data = new PlayerData(coins);
        SaveSystem.SavePlayer(data);
    }

    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        coins = data.coins;
    }
}
