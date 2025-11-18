using UnityEngine;
using UnityEngine.UI;

public class CoinTrackerUI : MonoBehaviour
{
    public Text coinText;

    void Update()
    {
        var (actual, max, percent) = GameManager.Instance.GetCoinStats();
        coinText.text = $"Coins: {actual}/{max} ({percent}%)";
    }
}