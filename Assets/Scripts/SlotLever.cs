using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotLever : MonoBehaviour
{
    [Header("Lever Sprites")]
    public Image leverImage;
    public Sprite leverUp;
    public Sprite leverDown;

    [Header("References")]
    public SlotMachineController slotMachine;
    public PayoutManager payoutManager;

    [Header("Animation")]
    public float leverDownTime = 0.2f;

    private bool isPulling;

    public void OnLeverClicked()
    {
        if (isPulling || slotMachine == null ||
            slotMachine.IsSpinning)
            return;

        payoutManager.ShowBetPanel();
    }

    public void StartSpinAfterBet(int bet)
    {
        if (isPulling || slotMachine.IsSpinning)
            return;

        StartCoroutine(PullLevers(bet));
    }

    IEnumerator PullLevers(int bet)
    {
        isPulling = true;

        leverImage.sprite = leverDown;

        slotMachine.TrySpin(bet);

        yield return new WaitForSeconds(leverDownTime);

        leverImage.sprite = leverUp;

        isPulling = false;
    }
}