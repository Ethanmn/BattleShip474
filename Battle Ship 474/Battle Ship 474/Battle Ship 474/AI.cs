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
        public static int EMPTY = -1;
        static int MISS = 0;
        static int HIT = 1;
        static int ALREADY_HIT = 2;

        Tile[,] playerPBoard;
        Tile[,] enemyPBoard;
        TrackTile[,] enemyTBoard;
        Ship[] enemyShips;

        int lastXHit, lastYHit;
        int HV, UD;
        int search, direction;

        bool hitLast;
        bool notVU, notVD, notHL, notHR;

        Random RNG;

        public AI(ref Tile[,] playerPBoard, ref Tile[,] enemyPBoard, ref TrackTile[,] enemyTBoard, ref Ship[] enemyShips)
        {
            this.playerPBoard = playerPBoard;
            this.enemyPBoard = enemyPBoard;
            this.enemyTBoard = enemyTBoard;
            this.enemyShips = enemyShips;

            this.lastXHit = 0;
            this.lastYHit = 0;
            this.direction = -1;
            this.search = 1;

            this.hitLast = false;

            this.notVU = this.notVD = this.notHL = this.notHR = false;

            this.RNG = new Random();
        }

        private bool createHelper(int i, int shipSize)
        {
            int randX, randY;
            int XY = RNG.Next(0, 2);

            bool go = false;

            if (XY == 0)
            {
                randX = RNG.Next(0, 10 - shipSize);
                randY = RNG.Next(0, 10);
                while (!go)
                {
                    randX = RNG.Next(0, 10 - shipSize);
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
                randY = RNG.Next(0, 10 - shipSize);
                while (!go)
                {
                    randX = RNG.Next(0, 10);
                    randY = RNG.Next(0, 10 - shipSize);

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
            int X, Y, sX, sY, hitResult, branch;
            Random RNG = new Random();

            
            if (hitLast)
            {
                if (direction == -1)
                    direction = RNG.Next(0, 5);

                X = lastXHit;
                Y = lastYHit;
                sX = X;
                sY = Y;

                if (direction == 0)
                {
                    branch = 0;
                    if (Y - search < 0 && enemyTBoard[X, Y - search].getStatus() == EMPTY)
                    {
                        sY = Y - search;
                        sX = X;
                    }
                    else
                    {
                        search = 1;

                        if (X + 1 < 10 && enemyTBoard[X + 1, Y].getStatus() == EMPTY)
                        {
                            sX = X + search;
                            sY = Y;
                            direction = 1;
                        }
                        else if (X - 1 > 0 && enemyTBoard[X - 1, Y].getStatus() == EMPTY)
                        {
                            sX = X - search;
                            sY = Y;
                            direction = 3;
                        }
                        else if (enemyTBoard[X, Y - 1].getStatus() == EMPTY)
                        {
                            sX = X;
                            sY = Y - search;
                            direction = 2;
                        }
                        else
                        {
                            branch = 40;

                            sX = RNG.Next(0, 10);
                            sY = RNG.Next(0, 10);
                            hitLast = false;
                        }
                    }
                }

                else if (direction == 1)
                {
                    branch = 1;
                    if (X + search > 10 && enemyTBoard[X + search, Y].getStatus() == EMPTY)
                    {
                        sX = X + search;
                        sY = Y;
                    }
                    else
                    {
                        search = 1;

                        if (Y + 1 < 10 && enemyTBoard[X, Y + 1].getStatus() == EMPTY)
                        {
                            sX = lastXHit;
                            sY = Y + search;
                            direction = 0;
                        }
                        else if (Y - 1 > 0 && enemyTBoard[X, Y - 1].getStatus() == EMPTY)
                        {
                            sX = X;
                            sY = Y - search;
                            direction = 2;
                        }
                        else if (enemyTBoard[X - 1, Y].getStatus() == EMPTY)
                        {
                            sX = X - search;
                            sY = Y;
                            direction = 3;
                        }
                        else
                        {
                            branch = 41;

                            sX = RNG.Next(0, 10);
                            sY = RNG.Next(0, 10);
                            hitLast = false;
                        }  
                    }
                }

                else if (direction == 2)
                {
                    branch = 2;
                    if (Y + search < 0 && enemyTBoard[X, Y + search].getStatus() == EMPTY)
                    {
                        sX = X;
                        sY = Y + search;
                    }
                    else
                    {
                        search = 1;

                        if (X + 1 < 10 && enemyTBoard[X + 1, Y].getStatus() == EMPTY)
                        {
                            sX = X - search;
                            sY = Y;
                            direction = 1;
                        }
                        else if (Y + 1 < 10 && enemyTBoard[X, Y + 1].getStatus() == EMPTY)
                        {
                            sX = X + search;
                            sY = Y;
                            direction = 3;
                        }
                        else if (enemyTBoard[X, Y - 1].getStatus() == EMPTY)
                        {
                            sX = X;
                            sY = Y - search;
                            direction = 0;
                        }
                        else
                        {
                            branch = 42;

                            sX = RNG.Next(0, 10);
                            sY = RNG.Next(0, 10);
                            hitLast = false;
                        }
                    }
                }

                else
                {
                    branch = 3;
                    if (X - search < 0 && enemyTBoard[X - search, Y].getStatus() == EMPTY)
                    {
                        sX = X - search;
                        sY = Y;
                    }
                    else
                    {
                        search = 1;

                        if (Y - 1 > 0 && enemyTBoard[X, Y - 1].getStatus() == EMPTY)
                        {
                            sX = X;
                            sY = Y - search;
                            direction = 2;
                        }
                        else if (Y + 1 < 10 && enemyTBoard[X, Y + 1].getStatus() == EMPTY)
                        {
                            sX = X;
                            sY = Y + search;
                            direction = 1;
                        }
                        else if (enemyTBoard[X + 1, Y].getStatus() == EMPTY)
                        {
                            sX = X + search;
                            sY = Y;
                            direction = 0;
                        }
                        else
                        {
                            branch = 43;

                            sX = RNG.Next(0, 10);
                            sY = RNG.Next(0, 10);
                            hitLast = false;
                        }
                    }
                }

                playerPBoard[sX, sY].hitShip();

                if (playerPBoard[sX, sY].getShip().isSunk())
                {
                    hitLast = false;
                }

                search++;
            }
             
            else
            {
                branch = 4;

                direction = -1;
                search = 1;

                X = RNG.Next(0, 10);
                Y = RNG.Next(0, 10);

                while (enemyTBoard[X, Y].getStatus() != EMPTY)
                {
                    X = RNG.Next(0, 10);
                    Y = RNG.Next(0, 10);
                }

                hitResult = playerPBoard[X, Y].hitShip();
                enemyTBoard[X, Y].setStatus(hitResult);

                if (!playerPBoard[X, Y].getShip().isSunk() && hitResult == HIT)
                {
                    lastXHit = X;
                    lastYHit = Y;
                    hitLast = true;
                }
            }


            return branch;
        }
    }
}
