using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Battle_Ship_474
{
    class AI
    {
        Tile[,] playerPBoard;
        Tile[,] enemyPBoard;
        TrackTile[,] enemyTBoard;
        Ship[] enemyShips;

        int lastXHit;
        int lastYHit;

        bool hitLast;

        Random RNG;

        public AI(ref Tile[,] playerPBoard, ref Tile[,] enemyPBoard, ref TrackTile[,] enemyTBoard, ref Ship[] enemyShips)
        {
            this.playerPBoard = playerPBoard;
            this.enemyPBoard = enemyPBoard;
            this.enemyTBoard = enemyTBoard;
            this.enemyShips = enemyShips;

            this.lastXHit = 0;
            this.lastYHit = 0;

            this.hitLast = false;

            this.RNG = new Random();
        }

        private bool createHelper(int i, int shipSize)
        {
            int randX, randY;
            int XY = RNG.Next(0, 2);

            bool go = false;

            if (XY == 0)
            {
                randX = RNG.Next(0, 5);
                randY = RNG.Next(0, 10);
                while (!go)
                {
                    randX = RNG.Next(0, 5);
                    randY = RNG.Next(0, 10);

                    int j = 0;
                    while (this.enemyPBoard[randX + j, randY].getShip().getSize() == 0 && j < shipSize)
                    {
                        j++;
                    }
                    if (j == shipSize)
                    {
                        go = true;
                    }
                }

                for (int j = 0; j < shipSize; j++)
                {
                    this.enemyPBoard[randX + j, randY].placeShip(enemyShips[i]);
                }
            }
            else
            {
                randX = RNG.Next(0, 10);
                randY = RNG.Next(0, 5);
                while (!go)
                {
                    randX = RNG.Next(0, 10);
                    randY = RNG.Next(0, 5);

                    int j = 0;
                    while (this.enemyPBoard[randX, randY + j].getShip().getSize() == 0 && j < shipSize)
                    {
                        j++;
                    }
                    if (j == shipSize)
                    {
                        go = true;
                    }
                }

                for (int j = 0; j < shipSize; j++)
                {
                    this.enemyPBoard[randX, randY + j].placeShip(enemyShips[i]);
                }
            }

            return true;
        }

        public bool createRandPBoard(int numShips)
        {
            createHelper(0, 2);
            createHelper(1, 3);
            createHelper(2, 3);
            createHelper(3, 4);
            createHelper(4, 5);

            return true;
        }

        public int attack()
        {
            return 0;
        }
    }
}
