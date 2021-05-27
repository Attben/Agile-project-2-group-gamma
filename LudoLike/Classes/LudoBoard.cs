using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LudoLike
{

    public class LudoBoard
    {
        public float BoardWidth = 800;
        public float BoardHeight = 800;

        public Rect MainBoard;
        public Rect RedNest;
        public Rect BlueNest;
        public Rect YellowNest;
        public Rect GreenNest;

        public Dictionary<string, List<Vector2>> NestTiles = new Dictionary<string, List<Vector2>>() 
        {
            { "Red", new List<Vector2>() { new Vector2(0,0), new Vector2(0,1), new Vector2(1,0), new Vector2(1,1) } },
            { "Blue", new List<Vector2>() { new Vector2(10, 0), new Vector2(10, 1), new Vector2(9, 0), new Vector2(9, 1) } },
            { "Yellow", new List<Vector2>() { new Vector2(10, 10), new Vector2(10, 9), new Vector2(9, 10), new Vector2(9, 9) } },
            { "Green",  new List<Vector2>() { new Vector2(0, 10), new Vector2(0, 9), new Vector2(1, 10), new Vector2(1, 9) } }
        };

        public Dictionary<Vector2, Rect> TileGrid = new Dictionary<Vector2, Rect>();

        public List<Vector2> DynamicTiles = new List<Vector2>()
        {
            new Vector2(1, 4), new Vector2(2, 4), new Vector2(3, 4), new Vector2(4, 4),
            new Vector2(4, 3), new Vector2(4, 2), new Vector2(4, 1), new Vector2(4, 0), new Vector2(5, 0),
            new Vector2(6, 1), new Vector2(6, 2), new Vector2(6, 3), new Vector2(6, 4),
            new Vector2(7, 4), new Vector2(8, 4), new Vector2(9, 4), new Vector2(10, 4), new Vector2(10, 5),
            new Vector2(9, 6), new Vector2(8, 6), new Vector2(7, 6), new Vector2(6, 6),
            new Vector2(6, 7), new Vector2(6, 8), new Vector2(6, 9), new Vector2(6, 10), new Vector2(5, 10),
            new Vector2(4, 9), new Vector2(4, 8), new Vector2(4, 7), new Vector2(4, 6),
            new Vector2(3, 6), new Vector2(2, 6), new Vector2(1, 6), new Vector2(0, 6), new Vector2(0, 5)
        };

        public Dictionary<string, List<Vector2>> StaticTiles = new Dictionary<string, List<Vector2>>()
        {
            { "Red", new List<Vector2>() { new Vector2(0, 4), new Vector2(1, 5), new Vector2(2, 5), new Vector2(3, 5), new Vector2(4, 5) } },
            { "Blue", new List<Vector2>() { new Vector2(6, 0), new Vector2(5, 1), new Vector2(5, 2), new Vector2(5, 3), new Vector2(5, 4) } },
            { "Yellow", new List<Vector2>() { new Vector2(10, 6), new Vector2(9, 5), new Vector2(8, 5), new Vector2(7, 5), new Vector2(6, 5) } },
            { "Green", new List<Vector2>() { new Vector2(4, 10), new Vector2(5, 9), new Vector2(5, 8), new Vector2(5, 7), new Vector2(5, 6) } },
            { "Middle", new List<Vector2>() { new Vector2(5, 5) } }
        };

        public static readonly List<Vector2> RedPath = new List<Vector2>() 
        {
            new Vector2(0, 4), new Vector2(1, 4), new Vector2(2, 4), new Vector2(3, 4), new Vector2(4, 4),
            new Vector2(4, 3), new Vector2(4, 2), new Vector2(4, 1), new Vector2(4, 0), new Vector2(5, 0),
            new Vector2(6, 0), new Vector2(6, 1), new Vector2(6, 2), new Vector2(6, 3), new Vector2(6, 4),
            new Vector2(7, 4), new Vector2(8, 4), new Vector2(9, 4), new Vector2(10, 4), new Vector2(10, 5),
            new Vector2(10, 6), new Vector2(9, 6), new Vector2(8, 6), new Vector2(7, 6), new Vector2(6, 6),
            new Vector2(6, 7), new Vector2(6, 8), new Vector2(6, 9), new Vector2(6, 10), new Vector2(5, 10),
            new Vector2(4, 10), new Vector2(4, 9), new Vector2(4, 8), new Vector2(4, 7), new Vector2(4, 6),
            new Vector2(3, 6), new Vector2(2, 6), new Vector2(1, 6), new Vector2(0, 6), new Vector2(0, 5),
            new Vector2(1, 5), new Vector2(2, 5), new Vector2(3, 5), new Vector2(4, 5), new Vector2(5, 5)
        };

        public static readonly List<Vector2> BluePath = new List<Vector2>()
        {
            new Vector2(6, 0), new Vector2(6, 1), new Vector2(6, 2), new Vector2(6, 3), new Vector2(6, 4),
            new Vector2(7, 4), new Vector2(8, 4), new Vector2(9, 4), new Vector2(10, 4), new Vector2(10, 5),
            new Vector2(10, 6), new Vector2(9, 6), new Vector2(8, 6), new Vector2(7, 6), new Vector2(6, 6),
            new Vector2(6, 7), new Vector2(6, 8), new Vector2(6, 9), new Vector2(6, 10), new Vector2(5, 10),
            new Vector2(4, 10), new Vector2(4, 9), new Vector2(4, 8), new Vector2(4, 7), new Vector2(4, 6),
            new Vector2(3, 6), new Vector2(2, 6), new Vector2(1, 6), new Vector2(0, 6), new Vector2(0, 5),
            new Vector2(0, 4), new Vector2(1, 4), new Vector2(2, 4), new Vector2(3, 4), new Vector2(4, 4),
            new Vector2(4, 3), new Vector2(4, 2), new Vector2(4, 1), new Vector2(4, 0), new Vector2(5, 0),
            new Vector2(5, 1), new Vector2(5, 2), new Vector2(5, 3), new Vector2(5, 4), new Vector2(5, 5)
        };

        public static readonly List<Vector2> YellowPath = new List<Vector2>()
        {
            new Vector2(10, 6), new Vector2(9, 6), new Vector2(8, 6), new Vector2(7, 6), new Vector2(6, 6),
            new Vector2(6, 7), new Vector2(6, 8), new Vector2(6, 9), new Vector2(6, 10), new Vector2(5, 10),
            new Vector2(4, 10), new Vector2(4, 9), new Vector2(4, 8), new Vector2(4, 7), new Vector2(4, 6),
            new Vector2(3, 6), new Vector2(2, 6), new Vector2(1, 6), new Vector2(0, 6), new Vector2(0, 5),
            new Vector2(0, 4), new Vector2(1, 4), new Vector2(2, 4), new Vector2(3, 4), new Vector2(4, 4),
            new Vector2(4, 3), new Vector2(4, 2), new Vector2(4, 1), new Vector2(4, 0), new Vector2(5, 0),
            new Vector2(6, 0), new Vector2(6, 1), new Vector2(6, 2), new Vector2(6, 3), new Vector2(6, 4),
            new Vector2(7, 4), new Vector2(8, 4), new Vector2(9, 4), new Vector2(10, 4), new Vector2(10, 5),
            new Vector2(9, 5), new Vector2(8, 5), new Vector2(7, 5), new Vector2(6, 5), new Vector2(5, 5)
        };

        public static readonly List<Vector2> GreenPath = new List<Vector2>()
        {
            new Vector2(4, 10), new Vector2(4, 9), new Vector2(4, 8), new Vector2(4, 7), new Vector2(4, 6),
            new Vector2(3, 6), new Vector2(2, 6), new Vector2(1, 6), new Vector2(0, 6), new Vector2(0, 5),
            new Vector2(0, 4), new Vector2(1, 4), new Vector2(2, 4), new Vector2(3, 4), new Vector2(4, 4),
            new Vector2(4, 3), new Vector2(4, 2), new Vector2(4, 1), new Vector2(4, 0), new Vector2(5, 0),
            new Vector2(6, 0), new Vector2(6, 1), new Vector2(6, 2), new Vector2(6, 3), new Vector2(6, 4),
            new Vector2(7, 4), new Vector2(8, 4), new Vector2(9, 4), new Vector2(10, 4), new Vector2(10, 5),
            new Vector2(10, 6), new Vector2(9, 6), new Vector2(8, 6), new Vector2(7, 6), new Vector2(6, 6),
            new Vector2(6, 7), new Vector2(6, 8), new Vector2(6, 9), new Vector2(6, 10), new Vector2(5, 10),
            new Vector2(5, 9), new Vector2(5, 8), new Vector2(5, 7), new Vector2(5, 6), new Vector2(5, 5)
        };

        public static readonly List<List<Vector2>> PlayerPaths = new List<List<Vector2>>()
        {
            RedPath, BluePath, YellowPath, GreenPath
        };

        public LudoBoard()
        {
            MainBoard = new Rect(Scaling.Xpos((float)(Scaling.bWidth / 2 - BoardWidth / 2)),
                                 Scaling.Ypos((float)(Scaling.bHeight / 2 - BoardHeight / 2)),
                                 Scaling.Xpos(BoardWidth),
                                 Scaling.Ypos(BoardHeight));

            RedNest = new Rect(MainBoard.Left, MainBoard.Top, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            BlueNest = new Rect(MainBoard.Left, MainBoard.Bottom - MainBoard.Height / 11 * 4, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            YellowNest = new Rect(MainBoard.Right - MainBoard.Width / 11 * 4, MainBoard.Bottom - MainBoard.Height / 11 * 4, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            GreenNest = new Rect(MainBoard.Right - MainBoard.Width / 11 * 4, MainBoard.Top, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            CreateTileGrid();
        }

        /// <summary>
        /// Takes a canvas and draws a Ludo game board.
        /// </summary>
        /// <param name="args"></param>
        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            MainBoard = new Rect(Scaling.bWidth / 2 - Scaling.Xpos(BoardWidth / 2),
                            Scaling.bHeight / 2 - Scaling.Ypos(BoardHeight / 2),
                            Scaling.Xpos(BoardWidth),
                            Scaling.Ypos(BoardHeight));
            args.DrawingSession.FillRectangle(MainBoard, Windows.UI.Colors.White);
            DrawNests(args);
            
        }

        public void DrawNests(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.FillRectangle(RedNest, Windows.UI.Colors.Red);
            args.DrawingSession.FillRectangle(YellowNest, Windows.UI.Colors.Yellow);
            args.DrawingSession.FillRectangle(GreenNest, Windows.UI.Colors.LawnGreen);
            args.DrawingSession.FillRectangle(BlueNest, Windows.UI.Colors.Blue);
            RedNest = new Rect(MainBoard.Left, MainBoard.Top, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            BlueNest = new Rect(MainBoard.Left, MainBoard.Bottom - MainBoard.Height / 11 * 4, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            YellowNest = new Rect(MainBoard.Right - MainBoard.Width / 11 * 4, MainBoard.Bottom - MainBoard.Height / 11 * 4, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            GreenNest = new Rect(MainBoard.Right - MainBoard.Width / 11 * 4, MainBoard.Top, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);

        }
        
        
        /// <summary>
        /// This was for testing that we target the right squares
        /// </summary>
        /// <param name="args"></param>
        public void DrawStaticTiles(CanvasAnimatedDrawEventArgs args)
        {
            foreach (KeyValuePair<string, List<Vector2>> staticTile in StaticTiles)
            {
                foreach (Vector2 vector in staticTile.Value)
                {
                    switch (staticTile.Key)
                    {
                        case "Red":
                            args.DrawingSession.FillRectangle(TileGrid[vector], Windows.UI.Colors.Red);
                            break;
                        case "Blue":
                            args.DrawingSession.FillRectangle(TileGrid[vector], Windows.UI.Colors.Blue);
                            break;
                        case "Yellow":
                            args.DrawingSession.FillRectangle(TileGrid[vector], Windows.UI.Colors.Yellow);
                            break;
                        case "Green":
                            args.DrawingSession.FillRectangle(TileGrid[vector], Windows.UI.Colors.LawnGreen);
                            break;
                        case "Middle":
                            args.DrawingSession.FillRectangle(TileGrid[vector], Windows.UI.Colors.Pink);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void CreateTileGrid() 
        {
            for (int row = 0; row < 11; row++)
            {
                for (int column = 0; column < 11; column++)
                {
                    TileGrid[new Vector2(row, column)] = new Rect(MainBoard.X + (MainBoard.Width / 11 * column) + 2.5, 
                                                                    MainBoard.Y + (MainBoard.Height / 11 * row) + 2.5, 
                                                                    MainBoard.Width / 11 - 5, 
                                                                    MainBoard.Height / 11 - 5);
                }
            }
        }
    }
}
