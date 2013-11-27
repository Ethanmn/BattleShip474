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

        KeyboardState keyState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Vector2 pos = new Vector2(0.0f, 0.0f);
        float add = 1.0f;
        int[,] wat = new int[NUM, NUM];
        TrackTile[,] trackBoard = new TrackTile[SIZE, SIZE];
        Tile[,] primaryBoard = new Tile[SIZE, SIZE];
        String testString;
        Ship testShip;
        Ship testShip2;
        bool textFlag = false;

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
            // TODO: Add your initialization logic here

            for (int i = 0; i < NUM; i++)
            {
                for (int j = 0; j < NUM; j++)
                {
                    wat[i, j] = i;
                }
            }

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    trackBoard[i, j] = new TrackTile();
                }
            }

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    char let = (char)((int)'A' + j);
                    primaryBoard[i, j] = new Tile(let, i);
                }
            }

            testShip = new Ship("Scooter", 2, 3, 0, 0);
            testShip2 = new Ship("Falafel", 1, 1);
            primaryBoard[2, 3].placeShip(testShip);
            primaryBoard[0, 0].placeShip(testShip);
            primaryBoard[1, 1].placeShip(testShip2);

            testString = primaryBoard[2, 3].getShip().getName();

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

            // TODO: Add your update logic here

            if (pos.X > 100.0f || pos.X < 0.0f)
            {
                add = -add;
                wat[0, 0] = primaryBoard[2, 3].getShip().hit(2, 3);
            }

            pos.X += add;
            pos.Y += add;

            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.A))
                wat[0, 0] = 10;
            if (keyState.IsKeyDown(Keys.B))
                wat[0, 0] = 20;
            if (keyState.IsKeyDown(Keys.C))
                wat[0, 0] = 30;
            if (keyState.IsKeyDown(Keys.D))
                wat[0, 0] = 40;
            if (keyState.IsKeyDown(Keys.E))
                wat[0, 0] = 50;
            if (keyState.IsKeyDown(Keys.F))
                wat[0, 0] = 60;
            if (keyState.IsKeyDown(Keys.G))
                wat[0, 0] = 70;
            if (keyState.IsKeyDown(Keys.H))
                wat[0, 0] = 80;
            if (keyState.IsKeyDown(Keys.I))
                wat[0, 0] = 90;
            if (keyState.IsKeyDown(Keys.J))
                wat[0, 0] = 100;

            if (keyState.IsKeyDown(Keys.D0))
                testString = primaryBoard[2, 3].getShip().getName();
            if (keyState.IsKeyDown(Keys.D1))
                textFlag = !textFlag;
            if (keyState.IsKeyDown(Keys.D2))
                testString = primaryBoard[1, 1].getShip().getName();
            if (keyState.IsKeyDown(Keys.D3))
                primaryBoard[0, 0].getShip().setName("LOLZ");
            if (keyState.IsKeyDown(Keys.D4))
                wat[0, 0] = 4;
            if (keyState.IsKeyDown(Keys.D5))
                wat[0, 0] = 5;
            if (keyState.IsKeyDown(Keys.D6))
                wat[0, 0] = 6;
            if (keyState.IsKeyDown(Keys.D7))
                wat[0, 0] = 7;
            if (keyState.IsKeyDown(Keys.D8))
                wat[0, 0] = 8;
            if (keyState.IsKeyDown(Keys.D9))
                wat[0, 0] = 9;

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

            for (int i = 0; i < NUM; i++)
            {
                for (int j = 0; j < NUM; j++)
                {
                    if (textFlag)
                    {
                        spriteBatch.DrawString(font, primaryBoard[i, j].getLetter() + "", new Vector2((float)i * 100, (float)j * 100), Color.DeepPink);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, primaryBoard[i, j].getNumber() + "", new Vector2((float)i * 100, (float)j * 100), Color.DeepPink);
                    }
                    
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
