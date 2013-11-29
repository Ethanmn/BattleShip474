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

        public const int INTRO_STATE = 0, PLACEMENT_STATE = 1, GAME_STATE = 2, END_STATE = 3;
        public static int current_state = INTRO_STATE;

        KeyboardState keyState;
        GraphicsDeviceManager graphics;

        //Test Stuffs
        SpriteBatch spriteBatch;
        SpriteFont font, ffont;
        Vector2 pos = new Vector2(30.0f, 55.0f);
        String testString;
        String XString;
        String YString;
        Ship testShip;
        Ship testShip2;
        int status;
        
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

        BoardVisuals visuals;
        bool clicked = false;
        bool scrolled = false;
        int pscroll = 0;

        bool placement_done = false;
        bool victory = false;
        bool gameover = false;
        bool playerturn = true;
        float enemyturntimer = 0;
        int currentlyPlacing = 0;

        //AI stuffs
        AI enemy;

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

            playerPBoard[0, 0].placeShip(pPatrolShip);
            playerPBoard[1, 0].placeShip(pPatrolShip);
            pPatrolShip.setStart(0, 0);
            pPatrolShip.setOrientation(Ship.HOR);

            playerPBoard[4, 6].placeShip(pDestroyerShip);
            playerPBoard[5, 6].placeShip(pDestroyerShip);
            playerPBoard[6, 6].placeShip(pDestroyerShip);
            pDestroyerShip.setStart(4, 6);
            pDestroyerShip.setOrientation(Ship.HOR);

            playerPBoard[7, 9].placeShip(pSubShip);
            playerPBoard[8, 9].placeShip(pSubShip);
            playerPBoard[9, 9].placeShip(pSubShip);
            pSubShip.setStart(7, 9);
            pSubShip.setOrientation(Ship.HOR);

            playerPBoard[4, 1].placeShip(pBattleShip);
            playerPBoard[4, 2].placeShip(pBattleShip);
            playerPBoard[4, 3].placeShip(pBattleShip);
            playerPBoard[4, 4].placeShip(pBattleShip);
            pBattleShip.setStart(4, 1);
            pBattleShip.setOrientation(Ship.VER);

            playerPBoard[0, 1].placeShip(pCarrierShip);
            playerPBoard[0, 2].placeShip(pCarrierShip);
            playerPBoard[0, 3].placeShip(pCarrierShip);
            playerPBoard[0, 4].placeShip(pCarrierShip);
            playerPBoard[0, 5].placeShip(pCarrierShip);
            pCarrierShip.setStart(0, 1);
            pCarrierShip.setOrientation(Ship.VER);

            testString = "Falafel";

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

            /*if (keyState.IsKeyDown(Keys.A))
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
            YString = (char)((char)Y + 'A') + "";*/

            /*if (keyState.IsKeyDown(Keys.Enter))
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
            /*
                if (enemyShips[0].isSunk() && enemyShips[1].isSunk() &&
                    enemyShips[2].isSunk() && enemyShips[3].isSunk() &&
                    enemyShips[4].isSunk())
                {
                    testString = "YOU WIN!";
                }
            }*/

            if (current_state == PLACEMENT_STATE)
            {
                int mx = Mouse.GetState().X;
                int my = Mouse.GetState().Y;

                int x = -1;
                int y = -1;

                if (pPatrolShip.getStartX() != -1 && pSubShip.getStartX() != -1 && pBattleShip.getStartX() != -1 &&
                    pDestroyerShip.getStartX() != -1 && pCarrierShip.getStartX() != -1)
                    placement_done = true;

                if (mx > 325 && my > 210 && mx < 670 && my < 535)
                {
                    x = (mx - 325) * 10 / (670 - 325);
                    y = (my - 210) * 10 / (535 - 210);
                    y = -y + 9;
                }

                /*bool rotate = false;
                if (x >= 0 && x < 10 && y >= 0 && y < 10)
                {
                    if ((Mouse.GetState().ScrollWheelValue != pscroll || Mouse.GetState().RightButton == ButtonState.Pressed) && !scrolled)
                    {
                        scrolled = true;
                        pscroll = Mouse.GetState().ScrollWheelValue;
                        rotate = true;
                    }
                    if (!(Mouse.GetState().ScrollWheelValue != pscroll || Mouse.GetState().RightButton == ButtonState.Pressed))
                        scrolled = false;

                    if (currentlyPlacing == 0) // patrol boat
                    {
                        if (pPatrolShip.getOrientation() == Ship.HOR)
                        {
                            if (x + pPatrolShip.getSize() < 10)
                            {
                                pPatrolShip.setStart(x, y);
                            }
                        }
                        else
                        {
                            if (y + pPatrolShip.getSize() < 10)
                            {
                                pPatrolShip.setStart(x, y);                                
                            }
                        }

                        if (rotate)
                            pPatrolShip.setOrientation(pPatrolShip.getOrientation() == Ship.HOR ? Ship.VER : Ship.HOR);
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !clicked)
                        {
                            if (pPatrolShip.getOrientation() == Ship.HOR)
                            {
                                if (y + pPatrolShip.getSize() < 10)
                                {
                                    currentlyPlacing++;
                                    clicked = true;
                                }
                            }
                            else
                            {
                                if (x + pPatrolShip.getSize() < 10)
                                {
                                    currentlyPlacing++;
                                    clicked = true;
                                }
                            }
                        }
                    }
                    else if (currentlyPlacing == 1) // Sub
                    {

                    }
                }*/

            }


            if (current_state == GAME_STATE)
            {
                int mx = Mouse.GetState().X;
                int my = Mouse.GetState().Y;

                int x = -1;
                int y = -1;

                if (mx > 340 && my > 90 && mx < 650 && my < 380)
                {
                    x = (mx - 340) * 10 / (650 - 340);
                    y = (my - 90) * 10 / (380 - 90);
                    y = -y + 9;
                }

                if (!clicked && playerturn && x != -1 && y != -1)
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        if (playerTBoard[x, y].getStatus() == TrackTile.EMPTY)
                        {
                            status = enemyPBoard[x, y].hitShip();

                            if (status == HIT)
                            {
                                playerTBoard[x, y].hitEm();
                            }
                            else if (status == MISS)
                            {
                                playerTBoard[x, y].missEm();
                            }

                            playerturn = false;
                            enemyturntimer = 200;
                        }
                    }

                    if (enemyShips[0].isSunk() && enemyShips[1].isSunk() &&
                    enemyShips[2].isSunk() && enemyShips[3].isSunk() &&
                    enemyShips[4].isSunk())
                    {
                        gameover = true;
                        victory = true;
                    }
                }
                else if (Mouse.GetState().LeftButton != ButtonState.Pressed)
                {
                    clicked = false;
                }

                if (enemyturntimer > 0)
                {
                    enemyturntimer -= gameTime.ElapsedGameTime.Milliseconds;
                }
                if (enemyturntimer < 0) { testString = enemy.attack() + ""; enemyturntimer = 0; playerturn = true; }
            }

            if (!clicked)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (current_state == INTRO_STATE && Mouse.GetState().X < 260 && Mouse.GetState().Y < 50)
                    {
                        current_state = PLACEMENT_STATE;
                        visuals.gotoState(PLACEMENT_STATE);
                    }
                    else if (current_state == PLACEMENT_STATE && Mouse.GetState().X < 260 && Mouse.GetState().Y > 540 && placement_done)
                    {
                        current_state = GAME_STATE;
                        visuals.gotoState(GAME_STATE);
                    }
                    else if (current_state == GAME_STATE && gameover)
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

            visuals.update(gameTime.ElapsedGameTime.Milliseconds, playerTBoard, playerPBoard, playerShips);

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

            /*for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    spriteBatch.DrawString(font, playerTBoard[i, j].getStatus() + "", new Vector2((float)i * 85, (float)j * 50), Color.DeepPink); 
                }
            }

            spriteBatch.DrawString(font, XString, new Vector2(60, 35), Color.BurlyWood);
            spriteBatch.DrawString(font, YString, new Vector2(50, 35), Color.BurlyWood);*/
                            
            spriteBatch.End();

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            GraphicsDevice.RasterizerState = rs;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            visuals.Draw(graphics, spriteBatch);


            spriteBatch.Begin();
            if (current_state == INTRO_STATE)
                spriteBatch.DrawString(font, "Click here to start", new Vector2(10, 10), Mouse.GetState().X < 260 && Mouse.GetState().Y < 50 ? Color.White : Color.Gray);
            if (current_state == PLACEMENT_STATE && placement_done)
                spriteBatch.DrawString(font, "Click here to continue", new Vector2(10, 550), Mouse.GetState().X < 260 && Mouse.GetState().Y > 540 ? Color.White : Color.Gray);
            if (current_state == END_STATE && victory)
                spriteBatch.DrawString(ffont, "You won!", new Vector2(450, 200), Color.LimeGreen);
            if (current_state == END_STATE && !victory)
                spriteBatch.DrawString(ffont, "You lost...", new Vector2(420, 200), Color.Red);

            spriteBatch.DrawString(font, Mouse.GetState().X + " " + Mouse.GetState().Y, Vector2.Zero, Color.Red);

            if (current_state == GAME_STATE)
                spriteBatch.DrawString(ffont, playerturn ? "Your turn" : "Enemy's turn", new Vector2(10, 10), playerturn ? Color.LimeGreen : Color.Red);
            if (current_state == PLACEMENT_STATE)
                spriteBatch.DrawString(ffont, "Place your ships!", new Vector2(10, 10), Color.LimeGreen);

            if (current_state == PLACEMENT_STATE)
            {
                spriteBatch.DrawString(font, "Patrol Boat", new Vector2(10, 60), pPatrolShip.getStartX() == -1 ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Submarine", new Vector2(10, 90), pSubShip.getStartX() == -1 ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Destroyer", new Vector2(10, 120), pDestroyerShip.getStartX() == -1 ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Battleship", new Vector2(10, 150), pBattleShip.getStartX() == -1 ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Aircraft Carrier", new Vector2(10, 180), pCarrierShip.getStartX() == -1 ? Color.Green : Color.LimeGreen);
            }
            if (current_state == GAME_STATE)
            {
                spriteBatch.DrawString(font, "Patrol Boat", new Vector2(10, 60), pPatrolShip.isSunk() ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Submarine", new Vector2(10, 90), pSubShip.isSunk() ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Destroyer", new Vector2(10, 120), pDestroyerShip.isSunk() ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Battleship", new Vector2(10, 150), pBattleShip.isSunk() ? Color.Green : Color.LimeGreen);
                spriteBatch.DrawString(font, "Aircraft Carrier", new Vector2(10, 180), pCarrierShip.isSunk() ? Color.Green : Color.LimeGreen);

                spriteBatch.DrawString(font, "Patrol Boat", new Vector2(800, 60), enemyShips[0].isSunk() ? Color.DarkRed : Color.Red);
                spriteBatch.DrawString(font, "Submarine", new Vector2(800, 90), enemyShips[2].isSunk() ? Color.DarkRed : Color.Red);
                spriteBatch.DrawString(font, "Destroyer", new Vector2(800, 120), enemyShips[1].isSunk() ? Color.DarkRed : Color.Red);
                spriteBatch.DrawString(font, "Battleship", new Vector2(800, 150), enemyShips[3].isSunk() ? Color.DarkRed : Color.Red);
                spriteBatch.DrawString(font, "Aircraft Carrier", new Vector2(800, 180), enemyShips[4].isSunk() ? Color.DarkRed : Color.Red);
                
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
