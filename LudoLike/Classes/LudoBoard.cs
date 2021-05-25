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
        // Not used yet
        public List<Vector2> RedPath = new List<Vector2>() 
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

        public List<Vector2> BluePath = new List<Vector2>()
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

        public List<Vector2> YellowPath = new List<Vector2>()
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

        public List<Vector2> GreenPath = new List<Vector2>()
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
            CreateTileGrid();
            DrawNests(args);
            DrawStaticTiles(args);
            
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
                if( row < 4 || row > 6)
                {
                    for (int column = 4; column < 7; column++)
                    {
                        TileGrid[new Vector2(row, column)] = new Rect(MainBoard.X + (MainBoard.Width / 11 * column), 
                                                                      MainBoard.Y + (MainBoard.Height / 11 * row), 
                                                                      MainBoard.Width / 11, 
                                                                      MainBoard.Height / 11);
                    }
                }
                else
                {
                    for (int column = 0; column < 11; column++)
                    {
                        TileGrid[new Vector2(row, column)] = new Rect(MainBoard.X + (MainBoard.Width / 11 * column), 
                                                                      MainBoard.Y + (MainBoard.Height / 11 * row), 
                                                                      MainBoard.Width / 11, 
                                                                      MainBoard.Height / 11);
                    }
                }
            }
            
            
            //int x = 1;
            //for (int row = 1; row <= 11; row++)
            //{
            //    if (row <= 4 || row > 7 )
            //    {
            //        for (int column = 5; column <= 7; column++)
            //        {
            //            TileGrid.Add(x, new Rect(BoardWidth / 11 * column, BoardHeight / 11 * row, Scaling.Xpos((float)MainBoard.Width / 11), Scaling.Ypos((float)MainBoard.Height / 11)));
            //        x++;
            //        }
            //    }
            //    else
            //    {
            //        for (int column = 1; column <= 11; column++)
            //        {
            //            TileGrid.Add(x, new Rect(BoardWidth / 11 * column, BoardHeight / 11 * row, Scaling.Xpos((float)MainBoard.Width / 11), Scaling.Ypos((float)MainBoard.Height / 11)));
            //            x++;
            //        }
            //    }
            //    x++;
            //}
        }


        /// <summary>
        /// May be used later. Not working for now,
        /// </summary>
        public void UpdateBoard()
        {
            UpdateRectangle(MainBoard,
                            Scaling.Xpos((float)(Scaling.bWidth / 2 - BoardWidth / 2)),
                            Scaling.Ypos((float)(Scaling.bHeight / 2 - BoardHeight / 2)),
                            Scaling.Xpos(BoardWidth),
                            Scaling.Ypos(BoardHeight));

            UpdateRectangle(RedNest, (float)MainBoard.Left, (float)MainBoard.Top, (float)(MainBoard.Width / 3), (float)MainBoard.Height / 3);
            UpdateRectangle(BlueNest, (float)MainBoard.Left, (float)(MainBoard.Bottom - MainBoard.Height / 3), (float)(MainBoard.Width / 3), (float)(MainBoard.Height / 3));
            UpdateRectangle(YellowNest, (float)(MainBoard.Right - MainBoard.Width / 3), (float)(MainBoard.Bottom - MainBoard.Height / 3), (float)(MainBoard.Width / 3), (float)(MainBoard.Height / 3));
            UpdateRectangle(GreenNest, (float)(MainBoard.Right - MainBoard.Width / 3), (float)MainBoard.Top, (float)(MainBoard.Width / 3), (float)(MainBoard.Height / 3));

        }
        /// <summary>
        /// May be used later. Not Working for now.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        public void UpdateRectangle(Rect rectangle, float width, float height, float xpos, float ypos)
        {
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.X = xpos;
            rectangle.Y = ypos;
        }

    }
}
