/* NOTE TO SELF: SETTING UP BATTLESHIPS
 * 1) Create all battleships (array?)
 * 2) Assign battleship to board via board.placeShip(ship)
 * 3) Ships will automaticaly gain Tile's corrdinates
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Battle_Ship_474
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int NUM_SHIPS = 5;

        const int NUM = 8;
        const int SIZE = 10;

        static int MISS = 0;
        static int HIT = 1;
        static int ALREADY_HIT = 2;

        KeyboardState keyState;
        GraphicsDeviceManager graphics;

        //Test Stuffs
        SpriteBatch spriteBatch;
        SpriteFont font;
        Vector2 pos = new Vector2(30.0f, 0.0f);
        String testString;
        String XString;
        String YString;
        Ship testShip;
        Ship testShip2;
        int status;

        //Input stuffs
        int X;
        int Y;
        
        //Game board stuffs
        Tile[,] playerPBoard;
        TrackTile[,] playerTBoard;

        Tile[,] enemyPBoard;
        TrackTile[,] enemyTBoard;

        //Ship Stuffs
        Ship[] playerShips;
        Ship[] enemyShips;

        Ship pPatrolShip;
        Ship pDestroyerShip;
        Ship pSubShip;
        Ship pBattleShip;
        Ship pCarrierShip;

        Ship ePatrolShip;
        Ship eDestroyerShip;
        Ship eSubShip;
        Ship eBattleShip;
        Ship eCarrierShip;

        //AI stuffs
        AI enemy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            status = -1;
            X = 0;
            Y = 0;

            playerPBoard = new Tile[SIZE, SIZE];
            playerTBoard = new TrackTile[SIZE, SIZE];

            enemyPBoard = new Tile[SIZE, SIZE];
            enemyTBoard = new TrackTile[SIZE, SIZE];

            playerShips = new Ship[NUM_SHIPS];
            enemyShips = new Ship[NUM_SHIPS];

            pPatrolShip = new Ship("Patrol Boat");
            playerShips[0] = pPatrolShip;
            pDestroyerShip = new Ship("Destroyer");
            playerShips[1] = pDestroyerShip;
            pSubShip = new Ship("Submarine");
            playerShips[2] = pSubShip;
            pBattleShip = new Ship("Battleship");
            playerShips[3] = pBattleShip;
            pCarrierShip = new Ship("Aircraft Carrier");
            playerShips[4] = pCarrierShip;

            ePatrolShip = new Ship("Patrol Boat");
            enemyShips[0] = new Ship("Patrol Boat");
            eDestroyerShip = new Ship("Destroyer");
            enemyShips[1] = new Ship("Destroyer");
            eSubShip = new Ship("Submarine");
            enemyShips[2] = new Ship("Submarine");
            eBattleShip = new Ship("Battleship");
            enemyShips[3] = new Ship("Battleship");
            eCarrierShip = new Ship("Aircraft Carrier");
            enemyShips[4] = new Ship("Aircraft Carrier");

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    playerTBoard[i, j] = new TrackTile();
                    enemyTBoard[i, j] = new TrackTile();
                }
            }

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    char let = (char)((int)'A' + j);
                    int num = i + 1;
                    playerPBoard[i, j] = new Tile(let, num);
                    enemyPBoard[i, j] = new Tile(let, num);
                }
            }

            enemy = new AI(ref playerPBoard, ref enemyPBoard, ref enemyTBoard, ref enemyShips);
            enemy.createRandPBoard(NUM_SHIPS);

            //Set up a static board
            /*
            enemyPBoard[1, 3].placeShip(ePatrolShip);
            enemyPBoard[2, 3].placeShip(ePatrolShip);
            
            enemyPBoard[4, 6].placeShip(eDestroyerShip);
            enemyPBoard[5, 6].placeShip(eDestroyerShip);
            enemyPBoard[6, 6].placeShip(eDestroyerShip);

            enemyPBoard[8, 1].placeShip(eSubShip);
            enemyPBoard[8, 2].placeShip(eSubShip);
            enemyPBoard[8, 3].placeShip(eSubShip);

            enemyPBoard[4, 1].placeShip(eBattleShip);
            enemyPBoard[5, 1].placeShip(eBattleShip);
            enemyPBoard[6, 1].placeShip(eBattleShip);
            enemyPBoard[7, 1].placeShip(eBattleShip);

            enemyPBoard[0, 5].placeShip(eCarrierShip);
            enemyPBoard[0, 6].placeShip(eCarrierShip);
            enemyPBoard[0, 7].placeShip(eCarrierShip);
            enemyPBoard[0, 8].placeShip(eCarrierShip);
            enemyPBoard[0, 9].placeShip(eCarrierShip);
            */

            testString = enemyShips[0].getSize() + "";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A))
                Y = 0;
            if (keyState.IsKeyDown(Keys.B))
                Y = 1;
            if (keyState.IsKeyDown(Keys.C))
                Y = 2;
            if (keyState.IsKeyDown(Keys.D)) 
                Y = 3;
            if (keyState.IsKeyDown(Keys.E))
                Y = 4;
            if (keyState.IsKeyDown(Keys.F))
                Y = 5;
            if (keyState.IsKeyDown(Keys.G))
                Y = 6;
            if (keyState.IsKeyDown(Keys.H))
                Y = 7;
            if (keyState.IsKeyDown(Keys.I))
                Y = 8;
            if (keyState.IsKeyDown(Keys.J))
                Y = 9;
            if (keyState.IsKeyDown(Keys.Z))
                testString = enemyShips[0].getName();

            if (keyState.IsKeyDown(Keys.D0))
                X = 9;
            if (keyState.IsKeyDown(Keys.D1))
                X = 0;
            if (keyState.IsKeyDown(Keys.D2))
                X = 1;
            if (keyState.IsKeyDown(Keys.D3))
                X = 2;
            if (keyState.IsKeyDown(Keys.D4))
                X = 3;
            if (keyState.IsKeyDown(Keys.D5))
                X = 4;
            if (keyState.IsKeyDown(Keys.D6))
                X = 5;
            if (keyState.IsKeyDown(Keys.D7))
                X = 6;
            if (keyState.IsKeyDown(Keys.D8))
                X = 7;
            if (keyState.IsKeyDown(Keys.D9))
                X = 8;

            XString = (X + 1) + "";
            YString = (char)((char)Y + 'A') + "";

            if (keyState.IsKeyDown(Keys.Enter))
            {
                status = enemyPBoard[X, Y].hitShip();

                if (status == HIT)
                {
                    playerTBoard[X, Y].hitEm();
                    testString = "HIT!";
                    if (enemyPBoard[X, Y].getShip().isSunk())
                    {
                        testString = "You sunk the enemy's " + enemyPBoard[X, Y].getShip().getName() + "!";
                    }
                }
                else if (status == MISS)
                {
                    playerTBoard[X, Y].missEm();
                    testString = "MISS!";
                }
                else if (status == ALREADY_HIT)
                {
                    testString = "ALREADY HIT!";
                    if (enemyPBoard[X, Y].getShip().isSunk())
                    {
                        testString = "You sunk the enemy's " + enemyPBoard[X, Y].getShip().getName() + "!";
                    }
                }
                else if (status == -1)
                {
                    testString = "WTF?!";
                }

                /*if (ePatrolShip.isSunk() && eBattleShip.isSunk() &&
                    eSubShip.isSunk() && eDestroyerShip.isSunk() &&
                    eCarrierShip.isSunk())*/

                if (enemyShips[0].isSunk() && enemyShips[1].isSunk() &&
                    enemyShips[2].isSunk() && enemyShips[3].isSunk() &&
                    enemyShips[4].isSunk())
                {
                    testString = "YOU WIN!";
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.DrawString(font, testString, pos, Color.OrangeRed);

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    spriteBatch.DrawString(font, playerTBoard[i, j].getStatus() + "", new Vector2((float)i * 85, (float)j * 50), Color.DeepPink); 
                }
            }

            spriteBatch.DrawString(font, XString, new Vector2(60, 35), Color.BurlyWood);
            spriteBatch.DrawString(font, YString, new Vector2(50, 35), Color.BurlyWood);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
