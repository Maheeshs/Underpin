# Underpin

This Slot Machine game is a 2D interactive casino-style game developed using Unity. It features three spinning reels with multiple symbols, configurable winning combinations, and a betting system that allows players to choose between 10, 50, or 100 coins before each spin. All three reels spin simultaneously and stop sequentially, creating an engaging slot machine experience. Players can win rewards based on predefined combinations, with multipliers ranging from 20x to 100x depending on the winning result. The game also includes a coin balance system, dynamic reward calculation, and an interactive UI for betting and gameplay.

to run the use this link: https://maheesh-s.itch.io/unity-slot-game-assignment


### My Thought Process

For this assignment I identified the key elements of how the slot machine would work and broke down what needed to be done into smaller systems within the game. In terms of code structure I wanted to keep everything as modular and flexible as possible. This meant separating the systems into different scripts such as the reel spinner, game flow, betting system, payout and winning combinations. The symbols and winning rules were set up in a ScriptableObject for ease of configuration. During game play the reels spin at the same time but stop one after the other. There's also a betting system whereby the player can choose how much they want to bet. The rewards are calculated using a multiplier. Overall I was trying to keep my code clean with re-usable components, smooth game play and an easy to expand system.