using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Rect> TileGrid;

        public LudoBoard()
        {
            MainBoard = new Rect(Scaling.Xpos((float)(Scaling.bWidth / 2 - BoardWidth / 2)),
                                 Scaling.Ypos((float)(Scaling.bHeight / 2 - BoardHeight / 2)),
                                 Scaling.Xpos(BoardWidth),
                                 Scaling.Ypos(BoardHeight));

            RedNest = new Rect(MainBoard.Left, MainBoard.Top, MainBoard.Width / 3, MainBoard.Height / 3);
            BlueNest = new Rect(MainBoard.Left, MainBoard.Bottom - MainBoard.Height / 3, MainBoard.Width / 3, MainBoard.Height / 3);
            YellowNest = new Rect(MainBoard.Right - MainBoard.Width / 3, MainBoard.Bottom - MainBoard.Height / 3, MainBoard.Width / 3, MainBoard.Height / 3);
            GreenNest = new Rect(MainBoard.Right - MainBoard.Width / 3, MainBoard.Top, MainBoard.Width / 3, MainBoard.Height / 3);
            

        }

        /// <summary>
        /// Takes a canvas and draws a Ludo game board
        /// </summary>
        /// <param name="args"></param>
        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            MainBoard = new Rect(Scaling.bWidth / 2 - Scaling.Xpos(BoardWidth / 2),
                            Scaling.bHeight / 2 - Scaling.Ypos(BoardHeight / 2),
                            Scaling.Xpos(BoardWidth),
                            Scaling.Ypos(BoardHeight));
            RedNest = new Rect(MainBoard.Left, MainBoard.Top, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            BlueNest = new Rect(MainBoard.Left, MainBoard.Bottom - MainBoard.Height / 11 * 4, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            YellowNest = new Rect(MainBoard.Right - MainBoard.Width / 11 * 4, MainBoard.Bottom - MainBoard.Height / 11 * 4, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            GreenNest = new Rect(MainBoard.Right - MainBoard.Width / 11 * 4, MainBoard.Top, MainBoard.Width / 11 * 4, MainBoard.Height / 11 * 4);
            CreateTileGrid();
            args.DrawingSession.FillRectangle(MainBoard, Windows.UI.Colors.White);
            args.DrawingSession.FillRectangle(RedNest, Windows.UI.Colors.Red);
            args.DrawingSession.FillRectangle(YellowNest, Windows.UI.Colors.Yellow);
            args.DrawingSession.FillRectangle(GreenNest, Windows.UI.Colors.LawnGreen);
            args.DrawingSession.FillRectangle(BlueNest, Windows.UI.Colors.Blue);
            
            //foreach (KeyValuePair<int, Rect> TileHolder in TileGrid)
            //{
            //    args.DrawingSession.FillRectangle(TileHolder.Value, Windows.UI.Colors.Red);
            //}
        }

        private void CreateTileGrid() 
        {
            TileGrid = new List<Rect>();
            for (int row = 0; row < 11; row++)
            {
                if( row < 4 || row > 6)
                {
                    for (int column = 4; column < 7; column++)
                    {
                        TileGrid.Add(new Rect(MainBoard.X + (MainBoard.Width / 11 * column), MainBoard.Y + (MainBoard.Height / 11 * row), MainBoard.Width / 11, MainBoard.Height / 11));
                    }
                }
                else
                {
                    for (int column = 0; column < 11; column++)
                    {
                        TileGrid.Add(new Rect(MainBoard.X + (MainBoard.Width / 11 * column), MainBoard.Y + (MainBoard.Height / 11 * row), MainBoard.Width / 11, MainBoard.Height / 11));
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
