using UnityEngine;
using TMPro;

public class PayoutManager : MonoBehaviour
{
    [Header("Betting Settings")]
    public int startingBalance = 1000;

    [Header("Betting Panel")]
    public GameObject bettingPanel;
    public TMP_Text balanceText;
    public TMP_Text messageText;
    public TMP_Text selectedBetText;

    [Header("References")]
    public SlotLever slotLever;

    private int balance;
    private int selectedBet = 10;

    private void Start()
    {
        balance = startingBalance;
        bettingPanel.SetActive(false);
        UpdateUI();
    }

    public void ShowBetPanel()
    {
        bettingPanel.SetActive(true);
        messageText.text = "";
        selectedBet = 10;
        UpdateBetUI();
    }

    public void SelectBet(int amount)
    {
        selectedBet = amount;
        UpdateBetUI();
    }

    void UpdateBetUI()
    {
        selectedBetText.text = "Selected Bet: " + selectedBet;
    }

    public void ConfirmBet()
    {
        if (balance < selectedBet)
        {
            messageText.text = "Insufficient Balance!";
            return;
        }

        balance -= selectedBet;
        UpdateUI();

        bettingPanel.SetActive(false);

        slotLever.StartSpinAfterBet(selectedBet);
    }

    public void AddWinnings(int amount)
    {
        balance += amount;

        messageText.text = "You won " + amount + " coins!";

        UpdateUI();
    }

    void UpdateUI()
    {
        balanceText.text = "Balance: " + balance;
    }
}