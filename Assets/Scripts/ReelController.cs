
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReelController : MonoBehaviour
{
    [Header("Symbol Images - 5 images")]
    public Image[] symbols = new Image[5];

    [Header("Configuration")]
    public SlotMachineConfig config;

    [Header("Reel Settings")]
    public float cellHeight = 180f;
    public float spinSpeed = 1200f;
    public float stopDuration = 0.8f;

    private bool isSpinning;
    public bool IsSpinning => isSpinning;
    private float totalHeight;

    void Awake()
    {
        SetupReel();
    }

    void SetupReel()
    {
        if (symbols == null || symbols.Length != 5)
        {
            Debug.LogError("Assign exactly 5 reel images!");
            return;
        }

        totalHeight = symbols.Length * cellHeight;

        for (int i = 0; i < symbols.Length; i++)
        {
            RectTransform rect = symbols[i].rectTransform;

            rect.anchoredPosition = new Vector2(
                0f,
                (2 - i) * cellHeight
            );

            rect.sizeDelta = new Vector2(
                rect.sizeDelta.x,
                cellHeight
            );

            symbols[i].sprite = GetRandomSprite();
        }
    }

    public IEnumerator Spin(
        SlotSymbol finalTop,
        SlotSymbol finalMiddle,
        SlotSymbol finalBottom,
        float duration)
    {
        if (isSpinning)
            yield break;

        isSpinning = true;

        float timer = 0f;

        // Continuous downward spin
        while (timer < duration)
        {
            MoveReel(spinSpeed * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }

        // Select the next three symbols above the viewport.
        int topIndex = GetTopSymbolIndex();

        // Reserve the three consecutive symbols.
        int middleIndex = (topIndex + 1) % 5;
        int bottomIndex = (topIndex + 2) % 5;

        // Assign the exact winning result.
        symbols[topIndex].sprite =
            config.GetSprite(finalTop);

        symbols[middleIndex].sprite =
            config.GetSprite(finalMiddle);

        symbols[bottomIndex].sprite =
            config.GetSprite(finalBottom);

        // Calculate exact distance to top symbol's final position.
        float currentY =
            symbols[topIndex].rectTransform.anchoredPosition.y;

        float distance = currentY - cellHeight;

        // Smooth deceleration.
        float elapsed = 0f;

        while (elapsed < stopDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / stopDuration);

            float eased = 1f - Mathf.Pow(1f - t, 3f);

            float previous = elapsed - Time.deltaTime;

            float previousT =
                Mathf.Clamp01(previous / stopDuration);

            float previousEased =
                1f - Mathf.Pow(1f - previousT, 3f);

            float movement =
                distance * (eased - previousEased);

            MoveReel(movement);

            yield return null;
        }

        // Exact final alignment.
        float correction =
            symbols[topIndex].rectTransform.anchoredPosition.y
            - cellHeight;

        MoveReel(correction);

        // Keep final positions exactly aligned.
        SetY(topIndex, cellHeight);
        SetY(middleIndex, 0f);
        SetY(bottomIndex, -cellHeight);

        isSpinning = false;
    }

    void MoveReel(float distance)
    {
        for (int i = 0; i < symbols.Length; i++)
        {
            RectTransform rect = symbols[i].rectTransform;

            Vector2 pos = rect.anchoredPosition;

            pos.y -= distance;

            // Recycle at bottom to top.
            while (pos.y < -2f * cellHeight)
            {
                pos.y += totalHeight;
                symbols[i].sprite = GetRandomSprite();
            }

            rect.anchoredPosition = pos;
        }
    }

    int GetTopSymbolIndex()
    {
        int index = -1;
        float highestY = float.NegativeInfinity;

        for (int i = 0; i < symbols.Length; i++)
        {
            float y =
                symbols[i].rectTransform.anchoredPosition.y;

            if (y > cellHeight && y > highestY)
            {
                highestY = y;
                index = i;
            }
        }

        // Fallback: choose the highest symbol.
        if (index == -1)
        {
            for (int i = 0; i < symbols.Length; i++)
            {
                float y =
                    symbols[i].rectTransform.anchoredPosition.y;

                if (y > highestY)
                {
                    highestY = y;
                    index = i;
                }
            }
        }

        return index;
    }

    void SetY(int index, float y)
    {
        Vector2 pos =
            symbols[index].rectTransform.anchoredPosition;

        pos.y = y;

        symbols[index].rectTransform.anchoredPosition = pos;
    }

    Sprite GetRandomSprite()
    {
        int index = Random.Range(0, config.symbols.Length);

        return config.symbols[index].sprite;
    }
}
