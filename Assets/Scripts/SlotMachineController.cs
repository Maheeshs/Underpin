
using UnityEngine;
using TMPro;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    [Header("Configuration")]
    public SlotMachineConfig config;

    [Header("Reels - Left to Right")]
    public ReelController[] reels = new ReelController[3];

    [Header("UI")]
    public TMP_Text resultText;

    [Header("Spin Settings")]
    public float spinDuration = 2f;
    public float reelStopDelay = 0.3f;

    private SlotSymbol[] finalResults = new SlotSymbol[3];

    private bool isSpinning;

    public bool IsSpinning => isSpinning;

    [Header("References")]
    public PayoutManager payoutManager;

    private int currentBet;

    public void TrySpin(int bet)
    {
        if (isSpinning || config == null)
            return;

        currentBet = bet;

        StartCoroutine(SpinMachine());
    }
IEnumerator SpinMachine()
{
    isSpinning = true;

    resultText.text = "";

    // Generate final middle row for all 3 reels
    for (int i = 0; i < 3; i++)
    {
        finalResults[i] = GetRandomSymbol();
    }

    // Start all reels together
    for (int i = 0; i < 3; i++)
    {
        StartCoroutine(
            reels[i].Spin(
                GetRandomSymbol(),
                finalResults[i],
                GetRandomSymbol(),
                spinDuration + (i * reelStopDelay)
            )
        );
    }

    // Wait until all reels finish
    while (reels[0].IsSpinning ||
           reels[1].IsSpinning ||
           reels[2].IsSpinning)
    {
        yield return null;
    }

    CheckWin();

    isSpinning = false;
}

    SlotSymbol GetRandomSymbol()
    {
        int index = Random.Range(0, config.symbols.Length);

        return config.symbols[index].symbol;
    }

    void CheckWin()
    {
        var rule = config.CheckWin(finalResults);

        if (rule != null)
        {
            int reward = currentBet * rule.multiplier;

            resultText.text =
                "YOU WIN! " + rule.ruleName +
                "\nBet: " + currentBet +
                "\nMultiplier: " + rule.multiplier + "x" +
                "\nReward: " + reward;

            payoutManager.AddWinnings(reward);
        }
        else
        {
            resultText.text = "TRY AGAIN!";
        }
    }
}
