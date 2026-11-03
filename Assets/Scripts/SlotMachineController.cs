
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

    public void TrySpin()
    {
        if (isSpinning || config == null)
            return;

        StartCoroutine(SpinMachine());
    }

    IEnumerator SpinMachine()
    {
        isSpinning = true;

        resultText.text = "";

        // Generate final middle row.
        for (int i = 0; i < 3; i++)
        {
            finalResults[i] = GetRandomSymbol();
        }

        // Spin from left to right.
        for (int i = 0; i < 3; i++)
        {
            SlotSymbol top = GetRandomSymbol();
            SlotSymbol bottom = GetRandomSymbol();

            yield return StartCoroutine(
                reels[i].Spin(
                    top,
                    finalResults[i],
                    bottom,
                    spinDuration
                )
            );

            yield return new WaitForSeconds(reelStopDelay);
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
            resultText.text =
                "YOU WIN! " + rule.ruleName +
                " Reward: " + rule.reward;
        }
        else
        {
            resultText.text = "TRY AGAIN!";
        }
    }
}
