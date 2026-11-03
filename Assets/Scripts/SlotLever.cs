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

    [Header("Animation")]
    public float leverDownTime = 0.2f;

    private bool isPulling;

    public void OnLeverClicked()
    {
        if (isPulling || slotMachine == null ||
            slotMachine.IsSpinning)
            return;

        StartCoroutine(PullLever());
    }

    IEnumerator PullLever()
    {
        isPulling = true;

        // Lever goes down
        leverImage.sprite = leverDown;

        slotMachine.TrySpin();

        yield return new WaitForSeconds(leverDownTime);

        // Lever returns up
        leverImage.sprite = leverUp;

        isPulling = false;
    }
}