using System;
using System.Threading.Tasks;
using CromisDev.SaveSystem;

namespace CromisDev.CardMatchingSystem
{
    public class SaveCardGameManager : SaveManager<GameSaveData>
    {
        public static string SAVE_FILE_NAME = "save_data";
        public static async void SaveGame()
        {
            GameSaveData data = CreateSaveData();
            await SaveAsync(data, SAVE_FILE_NAME);
        }

        public static GameSaveData CreateSaveData()
        {
            GameSaveData data = new()
            {
                score = GameController.ScoreController.Score,
                shouldInteract = GameController.ShouldInteract,
                saveTime = DateTime.Now,
                GridWidth = BoardLayoutController.Width,
                GridHeight = BoardLayoutController.Height,
                ComboCount = GameController.ComboTracker.CurrentCombo
            };

            foreach (Card card in BoardLayoutController.Cards)
            {
                data.cards.Add(new SerializableCardData
                {
                    cardId = card.CardId,
                    position = card.transform.position,
                    isMatched = card.IsMatched
                });
            }

            return data;
        }

        public static async Task<GameSaveData> LoadAsync() => await LoadAsync(SAVE_FILE_NAME);

        public static bool Exists() => Exists(SAVE_FILE_NAME);
        public static void Delete() => Delete(SAVE_FILE_NAME);

    }
}
