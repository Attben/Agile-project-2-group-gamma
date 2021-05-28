using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoLike
{
    class GameStateManager
    {
        public GameState CurrentGameState = GameState.NewGame;

        public void GameStateInit()
        {
        }

        public void Update()
        {
            if(CurrentGameState == GameState.NewGame)
            {

            }
            if (CurrentGameState == GameState.Playing)
            {

            }
            if (CurrentGameState == GameState.Paused)
            {

            }
            if (CurrentGameState == GameState.GameOver)
            {

            }
        }

        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            if (CurrentGameState == GameState.NewGame)
            {
                args.DrawingSession.DrawImage(Scaling.TransformImage(GameBoard.BackGround));
                args.DrawingSession.DrawText(CurrentGameState.ToString(), 20, 20, Windows.UI.Colors.White);
            }
            if (CurrentGameState == GameState.Playing)
            {
                args.DrawingSession.DrawImage(Scaling.TransformImage(GameBoard.BackGround));
                args.DrawingSession.DrawText(CurrentGameState.ToString(), 20, 20, Windows.UI.Colors.White);
            }
            if (CurrentGameState == GameState.Paused)
            {

            }
            if (CurrentGameState == GameState.GameOver)
            {

            }
        }
    }

    public enum GameState
    {
        NewGame,
        Playing,
        Paused,
        GameOver
    }
}
