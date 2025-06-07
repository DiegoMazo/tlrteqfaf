# Memory Card Matching Game

## Unity Version
Developed using **Unity 2021.3.45f1** (LTS). This is the latest LTS version of 2021 at the time of starting the test, so it is recommended to run the project in this version to avoid problems.

### The code has almost no comments as I am using a self-documenting code approach, adding comments only when they are truly necessary. For example, the comment added in the GameSettingsData.

## For this test, I used the standVard deck.
- Numbers (1-10/J/Q/K)
- Colors (red/black)
- Suits (♠ Spades, ♥ Hearts, ♦ Diamonds, ♣ Clubs)

### Unique Pair Generation
- Each session creates **completely unique random pairs** from the deck
- While some cards may appear similar due to shared suits/colors, **every matched pair is distinct**
- The system guarantees no duplicate pairs exist in the same game session

## The main menu will include a "Load Game" option, allowing players to continue from their most recent save. This option will only be available if a saved game exists.
## Players can save their current progress by accessing the in-game menu and selecting the save option.
## If you complete a level, the saved data will be cleared. This is done on purpose.v

## Features Review
- Dynamic grid size with size selection in the game menu, ranging from 2x2 to 20x2
- Card MatchingA
- Sounds
- Combo System
- Save/Load System
- Responsive Camera System that Adjusts to Any Layout Size
- Scoring
- Main Menu
- Pause Menu
- Audio System
- Basic Panel system to manage UI panel transitions

## Save System
### Robust Two-Layer Architecture
1. **Core Save Framework**
   Reusable base system handling:
   - Data serialization (JSON)
   - File operations
   - Error recovery

2. **Game-Specific Implementation**
   - Optimized to store only essential data:
   - Player score
   - Matched card pairs (tracked by unique identifiers)
   - Current board state (card positions)
   - Current Combo

By just saving this essential data we ensure:
- Reliable save/load operations
- Small save file sizes
- Prevention of save corruption issues

### Continuous Flip Implementation

#### Exact implementation of the required specification:
"The system should allow continuous card flipping without requiring users to wait for card comparisons to finish before selecting additional cards."

#### How I did it:
- I only compare the TWO MOST RECENTLY revealed cards
- Player can keep flipping cards without waiting
- Animations and comparisons happen in the background
- No blocking - immediate interaction

## Architecture Focused on Reusability and Modification

The game is designed with a highly modular architecture that leverages Unity's ScriptableObjects to facilitate reusability and easy modifications. This approach allows for efficient management of game assets and settings, making it easier to update and maintain the game.

### Audio System
The `AudioLibrarySO.cs` ScriptableObject is used to manage audio clips efficiently. It allows for easy access and retrieval of audio clips by their names, ensuring that sound management is both efficient and straightforward.

### Game Settings
The `GameSettingsSO.cs` ScriptableObject provides a centralized way to manage game settings, such as revealing cards at the start of each game for a specified duration. This allows for easy adjustments to game behavior without modifying the core game logic.

``` csharp

using UnityEngine;

namespace CromisDev.CardMatchingSystem
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Card Game/Game Settings")]
    public class GameSettingsSO : ScriptableObject
    {
        [SerializeField] private GameSettingsData gameSettingsData;

        public GameSettingsData Data => gameSettingsData;
    }

    [System.Serializable]
    public class GameSettingsData
    {
        public bool InitialRevealCards = true; // Optionally show cards to player at the beginning for memorizing
        public float RevealTime;
        public uint PointsPerCardMatch;
        public float MaxComboInterval = 3;
        public uint ComboPoints = 5;
    }
}
```

### Card Deck Data
The `CardDeckDataSO.so` ScriptableObject is used to specify the set of art used in the game. It manages card backs and fronts, providing methods to retrieve random card backs and unique card fronts for game sessions.
