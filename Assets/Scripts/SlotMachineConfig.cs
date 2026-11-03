using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SlotMachineConfig",
    menuName = "Slot Machine/Configuration")]
public class SlotMachineConfig : ScriptableObject
{
    [Serializable]
    public class SymbolData
    {
        public SlotSymbol symbol;
        public Sprite sprite;
    }

    [Serializable]
    public class WinningRule
    {
        public string ruleName;

        // Three symbols, in left-to-right order
        public SlotSymbol[] combination = new SlotSymbol[3];

        [Header("Reward Multiplier")]
        public int multiplier = 1;
    }

    [Header("Symbols")]
    public SymbolData[] symbols;

    [Header("Winning Combinations")]
    public WinningRule[] winningRules;

    public Sprite GetSprite(SlotSymbol symbol)
    {
        foreach (var item in symbols)
        {
            if (item.symbol == symbol)
                return item.sprite;
        }

        return null;
    }

    public WinningRule CheckWin(SlotSymbol[] result)
    {
        foreach (var rule in winningRules)
        {
            if (rule.combination.Length != 3)
                continue;

            if (rule.combination[0] == result[0] &&
                rule.combination[1] == result[1] &&
                rule.combination[2] == result[2])
            {
                return rule;
            }
        }

        return null;
    }
}