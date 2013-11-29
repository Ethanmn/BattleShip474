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
        const int NUM = 8;
        const int SIZE = 10;

        static int MISS = 0;
        static int HIT = 1;
        static int ALREADY_HIT = 2;

        public const int INTRO_STATE = 0, PLACEMENT_STATE = 1, GAME_STATE = 2, END_STATE = 3;
        public static int current_state = INTRO_STATE;

        KeyboardState keyState;
        GraphicsDeviceManager graphics;

        //Test Stuffs
        SpriteBatch spriteBatch;
        SpriteFont font, ffont;
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

        BoardVisuals visuals;
        bool clicked = false;

        bool placement_done = true;
        bool victory = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;

            this.IsMouseVisible = true;
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

            pPatrolShip = new Ship("Patrol Boat");
            pDestroyerShip = new Ship("Destroyer");
            pSubShip = new Ship("Submarine");
            pBattleShip = new Ship("Battleship");
            pCarrierShip = new Ship("Aircraft Carrier");

            ePatrolShip = new Ship("Patrol Boat");
            eDestroyerShip = new Ship("Destroyer");
            eSubShip = new Ship("Submarine");
            eBattleShip = new Ship("Battleship");
            eCarrierShip = new Ship("Aircraft Carrier");

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

            //Set up a static board

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

            /* Test things
            testShip = new Ship("Scooter");
            testShip2 = new Ship("Falafel");
            primaryBoard[2, 3].placeShip(testShip);
            primaryBoard[0, 0].placeShip(testShip);
            primaryBoard[1, 1].placeShip(testShip2);
            */

            testString = enemyPBoard[2, 3].getShip().getHits() + "Rawr!";

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
            ffont = Content.Load<SpriteFont>("FancyFont");

            visuals = new BoardVisuals(this);
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

                if (ePatrolShip.isSunk() && eBattleShip.isSunk() &&
                    eSubShip.isSunk() && eDestroyerShip.isSunk() &&
                    eCarrierShip.isSunk())
                {
                    testString = "YOU WIN!";
                }
            }

            if (!clicked)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Mouse.GetState().X < 260 && Mouse.GetState().Y > 540)
                {
                    if (current_state == INTRO_STATE)
                    {
                        current_state = PLACEMENT_STATE;
                        visuals.gotoState(PLACEMENT_STATE);
                    }
                    else if (current_state == PLACEMENT_STATE)
                    {
                        current_state = GAME_STATE;
                        visuals.gotoState(GAME_STATE);
                    }
                    else if (current_state == GAME_STATE)
                    {
                        current_state = END_STATE;
                        visuals.gotoState(END_STATE);
                    }

                    clicked = true;
                }
            }
            else if (Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                clicked = false;
            }

            visuals.update(gameTime.ElapsedGameTime.Milliseconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

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

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rs;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            visuals.Draw();


            spriteBatch.Begin();
            if (current_state == INTRO_STATE)
                spriteBatch.DrawString(font, "Click here to start", new Vector2(10, 550), Mouse.GetState().X < 260 && Mouse.GetState().Y > 540 ? Color.White : Color.Gray);
            if (current_state == PLACEMENT_STATE && placement_done)
                spriteBatch.DrawString(font, "Click here to continue", new Vector2(10, 550), Mouse.GetState().X < 260 && Mouse.GetState().Y > 540 ? Color.White : Color.Gray);
            if (current_state == END_STATE && victory)
                spriteBatch.DrawString(ffont, "You won!", new Vector2(450, 200), Color.Green);
            if (current_state == END_STATE && !victory)
                spriteBatch.DrawString(ffont, "You lost...", new Vector2(420, 200), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
