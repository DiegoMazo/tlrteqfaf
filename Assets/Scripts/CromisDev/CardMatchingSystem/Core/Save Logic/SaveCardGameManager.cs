

using System;
using CromisDev.SaveSystem;

namespace CromisDev.CardMatchingSystem
{
    public class SaveCardGameManager : SaveManager<GameSaveData>
    {
        public static async void SaveGame(string fileName)
        {
            GameSaveData data = CreateSaveData();
            await SaveAsync(data, fileName);
        }

        public static GameSaveData CreateSaveData()
        {
            GameSaveData data = new()
            {
                score = ScoreController.Score,
                shouldInteract = GameController.ShouldInteract,
                saveTime = DateTime.Now,
                GridWidth = BoardLayoutController.Width,
                GridHeight = BoardLayoutController.Height
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
    }
}
