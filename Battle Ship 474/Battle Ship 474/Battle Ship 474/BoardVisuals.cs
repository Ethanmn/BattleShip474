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
    class BoardVisuals
    {
        Model sship, mship, lship, sub, carrier, comp1, comp2, square, sphere;
        BasicEffect basicEffect, sqEffect, shipEffect, particleEffect;
        private int internalState = Game1.INTRO_STATE;
        Texture2D fader;

        List<Vector3> enemysquares;
        List<Particle> particles;
        int highx, highy;
        TrackTile[,] enemytracker;
        Tile[,] playertracker;
        Ship[] playerships;
        static float FADE = 0.05f;
        static int NUM_EXPLODE = 100;
        static int NUM_SPLASH = 100;
        static int SPLASH = 0;
        static int EXPLODE = 1;
        static int FIRE = 2;


        VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                    {
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                    }
        );
        BasicEffect TbasicEffect;

        private class WaterVertex
        {
            public bool up;
            public Vector3 position;
            public Color color = Color.DarkBlue;

            public WaterVertex(Vector3 position, bool up)
            {
                this.up = up;
                this.position = position;
            }
        }
        List<WaterVertex> water = new List<WaterVertex>();
        public const int NUM_WATER = 100;
        public VertexPositionColor[] getWaterVertices()
        {
            VertexPositionColor[] primitiveList = new VertexPositionColor[water.Count];

            for (int x = 0; x < water.Count; x++)
            {
                primitiveList[x] = new VertexPositionColor(
                    water[x].position, water[x].color);
            }

            return primitiveList;
        }
        public short[] getWaterIndices()
        {
            short[] triangleListIndices = new short[(NUM_WATER - 1) * (NUM_WATER - 1) * 6];

            for (int x = 0; x < NUM_WATER - 1; x++)
            {
                for (int y = 0; y < NUM_WATER - 1; y++)
                {
                    triangleListIndices[(x + y * (NUM_WATER - 1)) * 6 + 0] = (short)(NUM_WATER * x + y);
                    triangleListIndices[(x + y * (NUM_WATER - 1)) * 6 + 1] = (short)(NUM_WATER * x + y + 1);
                    triangleListIndices[(x + y * (NUM_WATER - 1)) * 6 + 2] = (short)(NUM_WATER * (x + 1) + y);

                    triangleListIndices[(x + y * (NUM_WATER - 1)) * 6 + 3] = (short)(NUM_WATER * (x + 1) + y);
                    triangleListIndices[(x + y * (NUM_WATER - 1)) * 6 + 4] = (short)(NUM_WATER * x + 1 + y);
                    triangleListIndices[(x + y * (NUM_WATER - 1)) * 6 + 5] = (short)(NUM_WATER * (x + 1) + 1 + y);
                }
            }

            return triangleListIndices;
        }

        public BoardVisuals(Game1 game)
        {
            sship = game.Content.Load<Model>("smallship");
            mship = game.Content.Load<Model>("mediumship");
            lship = game.Content.Load<Model>("longship");
            carrier = game.Content.Load<Model>("cruiser");
            sub = game.Content.Load<Model>("submarine");
            comp1 = game.Content.Load<Model>("comp1");
            comp2 = game.Content.Load<Model>("comp2");
            square = game.Content.Load<Model>("square"); 
            sphere = game.Content.Load<Model>("sphere");

            fader = game.Content.Load<Texture2D>("black");

            Random rand = new Random();
            
            TbasicEffect = new BasicEffect(game.GraphicsDevice);
            TbasicEffect.VertexColorEnabled = true;

            float xs = -0.55f, dx = 0.14f, zs = -1.8f, dz = -0.105f, y = -0.55f;

            for (int i = 0; i < NUM_WATER; i++)
            {
                for (int j = 0; j < NUM_WATER; j++)
                {
                    float ry = (float)rand.NextDouble() * 0.05f;
                    float tx = (i / (float)NUM_WATER) * 12f - 2f;
                    float ty = (j / (float)NUM_WATER) * 12f - 2f;
                    water.Add(new WaterVertex(new Vector3(xs + dx * tx, y + ry, zs + dz * ty), rand.Next(2) == 0));
                }
            }

            load(game);
        }

        public void load(Game game)
        {
            basicEffect = new BasicEffect(game.GraphicsDevice);

            basicEffect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
            basicEffect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
            basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
            basicEffect.SpecularPower = 5.0f;
            basicEffect.Alpha = 1.0f;

            basicEffect.LightingEnabled = true;
            if (basicEffect.LightingEnabled)
            {
                basicEffect.DirectionalLight0.Enabled = true; // enable each light individually
                if (basicEffect.DirectionalLight0.Enabled)
                {
                    basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f); // range is 0 to 1
                    basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0, 0, 1));
                    basicEffect.DirectionalLight0.SpecularColor = Vector3.One;
                }
                basicEffect.DirectionalLight1.Enabled = true;
                if (basicEffect.DirectionalLight1.Enabled)
                {
                    basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0.5f, 0f, 0f);
                    basicEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(0, -1, 0));
                    basicEffect.DirectionalLight1.SpecularColor = Vector3.Zero;
                }
                basicEffect.DirectionalLight2.Enabled = false;
                if (basicEffect.DirectionalLight2.Enabled)
                {
                    basicEffect.DirectionalLight2.DiffuseColor = new Vector3(0.8f, 0.8f, 1f);
                    basicEffect.DirectionalLight2.Direction = Vector3.Normalize(new Vector3(0.5f, -1, 0.2f));
                    basicEffect.DirectionalLight2.SpecularColor = Vector3.Zero;
                }
            }


            enemysquares = new List<Vector3>();

            float sX = -0.6f, dX = 0.13f, sY = -0.27f, dY = 0.12f, z = -2.95f;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    enemysquares.Add(new Vector3(sX + i * dX, sY + j * dY, z));
                }
            }

            sqEffect = new BasicEffect(game.GraphicsDevice);
            sqEffect.AmbientLightColor = new Vector3(1, 1, 1);
            sqEffect.DiffuseColor = new Vector3(1, 1, 1);
            sqEffect.SpecularColor = new Vector3(1, 1, 1);
            sqEffect.SpecularPower = 0f;
            sqEffect.Alpha = 1.0f;
            sqEffect.LightingEnabled = true;

            shipEffect = new BasicEffect(game.GraphicsDevice);
            shipEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            shipEffect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
            shipEffect.SpecularColor = new Vector3(0, 0, 0);
            shipEffect.SpecularPower = 0f;
            shipEffect.Alpha = 1f;
            shipEffect.LightingEnabled = true;

            particles = new List<Particle>();
            particleEffect = new BasicEffect(game.GraphicsDevice);
            particleEffect.AmbientLightColor = new Vector3(1, 1, 1);
            particleEffect.DiffuseColor = new Vector3(1, 0, 0);
            particleEffect.SpecularColor = new Vector3(0, 0, 0);
            particleEffect.SpecularPower = 0f;
            particleEffect.Alpha = 0.5f;
            particleEffect.LightingEnabled = true;

            if (shipEffect.LightingEnabled)
            {
                shipEffect.DirectionalLight0.Enabled = true; // enable each light individually
                if (shipEffect.DirectionalLight0.Enabled)
                {
                    shipEffect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f); // range is 0 to 1
                    shipEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -1, -1));
                    shipEffect.DirectionalLight0.SpecularColor = Vector3.One;
                }
            }
        }

        public void update(float gameTime, TrackTile[,] track, Tile[,] own, Ship[] pships)
        {
            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;
            Random RNG = new Random();

            highx = -1;
            highy = -1;

            if (mx > 340 && my > 90 && mx < 650 && my < 380)
            {
                highx = (mx - 340) * 10 / (650 - 340);
                highy = (my - 90) * 10 / (380 - 90);
                highy = -highy + 9;
            }
            enemytracker = track;
            playertracker = own;
            playerships = pships;

            float ymin = -0.55f, ymax = -0.53f, dy = 0.000015f;

            foreach (WaterVertex v in water)
            {
                if (v.up)
                {
                    v.position.Y += dy * gameTime;
                    if (v.position.Y > ymax)
                        v.up = !v.up;
                }
                else
                {
                    v.position.Y -= dy * gameTime;
                    if (v.position.Y < ymin)
                        v.up = !v.up;
                }
                v.color = new Color(0f, (v.position.Y - ymin) * 22f, Color.DarkBlue.B);
            }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].update();
                if ((particles[i].type == EXPLODE || particles[i].type == SPLASH) && (particles[i].getPosition().Y - particles[i].initPos.Y < 0 || particles[i].getPosition().X > 100.0))
                {
                    particles.RemoveAt(i--);
                }
                else if (particles[i].type == FIRE && particles[i].getPosition().Y - particles[i].initPos.Y > 0.1f)
                {
                    particles[i].setPosition(particles[i].initPos);
                    particles[i].setVelocity(new Vector3(0, (float)RNG.NextDouble() * 0.002f + 0.002f, 0));
                }
            }
        }

        public void gotoState(int state)
        {
            internalState = state;
        }

        private void copyEffect(BasicEffect copyto, BasicEffect copyfrom, Matrix xforms)
        {
            copyto.Alpha = copyfrom.Alpha;
            copyto.AmbientLightColor = copyfrom.AmbientLightColor;
            copyto.DiffuseColor = copyfrom.DiffuseColor;
            copyto.SpecularColor = copyfrom.SpecularColor;
            copyto.SpecularPower = copyfrom.SpecularPower;
            
            copyto.LightingEnabled = true;
            copyto.DirectionalLight0.Enabled = copyfrom.DirectionalLight0.Enabled;
            copyto.DirectionalLight0.DiffuseColor = copyfrom.DirectionalLight0.DiffuseColor;
            copyto.DirectionalLight0.SpecularColor = copyfrom.DirectionalLight0.SpecularColor;
            copyto.DirectionalLight0.Direction = copyfrom.DirectionalLight0.Direction;

            copyto.DirectionalLight1.Enabled = copyfrom.DirectionalLight1.Enabled;
            copyto.DirectionalLight1.DiffuseColor = copyfrom.DirectionalLight1.DiffuseColor;
            copyto.DirectionalLight1.SpecularColor = copyfrom.DirectionalLight1.SpecularColor;
            copyto.DirectionalLight1.Direction = copyfrom.DirectionalLight1.Direction;

            copyto.DirectionalLight2.Enabled = copyfrom.DirectionalLight2.Enabled;
            copyto.DirectionalLight2.DiffuseColor = copyfrom.DirectionalLight2.DiffuseColor;
            copyto.DirectionalLight2.SpecularColor = copyfrom.DirectionalLight2.SpecularColor;
            copyto.DirectionalLight2.Direction = copyfrom.DirectionalLight2.Direction;
        }

        //public void Draw(GraphicsDevice graphicsDevice, SpriteBatch batch)
        float pxs = -0.585f, pdx = 0.125f, pzs = -1.85f, pdz = -0.110f, py = -0.55f;
        public void addExplosion(int sx, int sy)
        {
            Random RNG = new Random();

            float partX = pxs + sx * pdx;
            float partY = py;
            float partZ = pzs + sy * pdz;

            for (int i = 0; i < NUM_EXPLODE; i++)
            {
                particles.Add(new Particle(new Vector3(partX, partY, partZ), new Vector3((float)RNG.NextDouble() * 0.02f - 0.01f, (float)RNG.NextDouble() * 0.02f, (float)RNG.NextDouble() * 0.02f - 0.01f), new Vector3(0, -0.0005f, 0), EXPLODE));
            }
        }

        public void addFire(int sx, int sy)
        {
            Random RNG = new Random();

            float partX = pxs + sx * pdx;
            float partY = py + 0.07f;
            float partZ = pzs + sy * pdz;

            for (int i = 0; i < NUM_EXPLODE; i++)
            {
                particles.Add(new Particle(new Vector3(partX + (float)RNG.NextDouble() * 0.04f - 0.02f, partY + (float)RNG.NextDouble() * 0.04f - 0.02f, partZ + (float)RNG.NextDouble() * 0.04f - 0.02f), new Vector3(0, (float)RNG.NextDouble() * 0.002f + 0.002f, 0), new Vector3(0, 0, 0), FIRE));
            }
        }

        public void addSplash(int sx, int sy)
        {
            Random RNG = new Random();

            float partX = pxs + sx * pdx;
            float partY = py;
            float partZ = pzs + sy * pdz;

            for (int i = 0; i < NUM_SPLASH; i++)
            {
                particles.Add(new Particle(new Vector3(partX, partY, partZ), new Vector3((float)RNG.NextDouble() * 0.02f - 0.01f, (float)RNG.NextDouble() * 0.02f, (float)RNG.NextDouble() * 0.02f - 0.01f), new Vector3(0, -0.0005f, 0), SPLASH));
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch batch)
        {
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45),
                    1000f / 600f,
                    0.01f, 300.0f
                    );
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 0), new Vector3(0, 0, -1), Vector3.Up);

            if (internalState == Game1.INTRO_STATE)
            {
                Matrix c1_xforms = Matrix.Identity * Matrix.CreateRotationX((float)Math.PI / 3f) * Matrix.CreateTranslation(new Vector3(1.15f, -0.6f, -1.6f));
                Matrix c1_xformsr = Matrix.CreateRotationX((float)Math.PI / 3f);
                Matrix[] c1_transforms = new Matrix[comp1.Bones.Count];
                comp1.CopyAbsoluteBoneTransformsTo(c1_transforms);
                Matrix c2_xforms = Matrix.Identity * Matrix.CreateRotationX((float)Math.PI / 3f) * Matrix.CreateTranslation(new Vector3(-1.5f, -1.8f, -1f));
                Matrix c2_xformsr = Matrix.CreateRotationX((float)Math.PI / 3f);
                Matrix[] c2_transforms = new Matrix[comp2.Bones.Count];
                comp2.CopyAbsoluteBoneTransformsTo(c2_transforms);

                foreach (ModelMesh mesh in comp1.Meshes)
                {
                    foreach (BasicEffect e in mesh.Effects)
                    {
                        copyEffect(e, basicEffect, c2_xformsr);
                        e.World = c1_transforms[mesh.ParentBone.Index] * c1_xforms;
                        e.View = view;
                        e.Projection = projection;
                    }
                    mesh.Draw();
                }
                foreach (ModelMesh mesh in comp2.Meshes)
                {
                    foreach (BasicEffect e in mesh.Effects)
                    {
                        copyEffect(e, basicEffect, c1_xformsr);
                        e.World = c2_transforms[mesh.ParentBone.Index] * c2_xforms;
                        e.View = view;
                        e.Projection = projection;
                    }
                    mesh.Draw();
                }
            }
            else if (internalState == Game1.PLACEMENT_STATE)
            {
                Matrix c1_xforms = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateRotationX((float)Math.PI / 2.5f - Game1.fadeTime * 0.0005f) * Matrix.CreateTranslation(new Vector3(-1.2f, 0.5f, -3f));
                Matrix[] c1_transforms = new Matrix[comp1.Bones.Count];
                comp1.CopyAbsoluteBoneTransformsTo(c1_transforms);
                Matrix c2_xforms = Matrix.Identity * Matrix.CreateRotationX((float)Math.PI / -1f - Game1.fadeTime * 0.0001f) * Matrix.CreateRotationZ((float)Math.PI) * Matrix.CreateTranslation(new Vector3(1.45f, 0.45f, -4.3f));
                Matrix[] c2_transforms = new Matrix[comp2.Bones.Count];
                comp2.CopyAbsoluteBoneTransformsTo(c2_transforms);

                foreach (ModelMesh mesh in comp1.Meshes)
                {
                    foreach (BasicEffect e in mesh.Effects)
                    {
                        copyEffect(e, basicEffect, c1_xforms);
                        e.World = c1_transforms[mesh.ParentBone.Index] * c1_xforms;
                        e.View = view;
                        e.Projection = projection;
                    }
                    mesh.Draw();
                }
                foreach (ModelMesh mesh in comp2.Meshes)
                {
                    foreach (BasicEffect e in mesh.Effects)
                    {
                        copyEffect(e, basicEffect, c2_xforms);
                        e.World = c2_transforms[mesh.ParentBone.Index] * c2_xforms;
                        e.View = view;
                        e.Projection = projection;
                    }
                    mesh.Draw();
                }
                batch.Begin();
                batch.Draw(fader, Vector2.Zero, new Color(1, 1, 1, 0.5f));
                batch.End();
            }
            else if (internalState == Game1.GAME_STATE || internalState == Game1.END_STATE)
            {
                Matrix movein = Matrix.CreateTranslation(new Vector3(0.00003f * Game1.fadeTime, -0.0005f * Game1.fadeTime, -0.001f * Game1.fadeTime));

                Matrix c1_xforms = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(new Vector3(-1.2f, -0.45f, -3f)) * movein;
                Matrix[] c1_transforms = new Matrix[comp1.Bones.Count];
                comp1.CopyAbsoluteBoneTransformsTo(c1_transforms);
                Matrix c2_xforms = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateRotationX((float)Math.PI / -2f) * Matrix.CreateTranslation(new Vector3(1.45f, -1.80f, -2.9f)) * movein;
                Matrix[] c2_transforms = new Matrix[comp2.Bones.Count];
                comp2.CopyAbsoluteBoneTransformsTo(c2_transforms);

                foreach (ModelMesh mesh in comp1.Meshes)
                {
                    foreach (BasicEffect e in mesh.Effects)
                    {
                        copyEffect(e, basicEffect, c1_xforms);
                        e.World = c1_transforms[mesh.ParentBone.Index] * c1_xforms;
                        e.View = view;
                        e.Projection = projection;
                    }
                    mesh.Draw();
                }
                foreach (ModelMesh mesh in comp2.Meshes)
                {
                    foreach (BasicEffect e in mesh.Effects)
                    {
                        copyEffect(e, basicEffect, c2_xforms);
                        e.World = c2_transforms[mesh.ParentBone.Index] * c2_xforms;
                        e.View = view;
                        e.Projection = projection;
                    }
                    mesh.Draw();
                }

                foreach (Particle particle in particles)
                {
                    float SCALE = 0.005f;

                    particleEffect.World = Matrix.Identity;
                    particleEffect.View = view;
                    particleEffect.Projection = projection;

                    Matrix[] particletransforms = new Matrix[sphere.Bones.Count];
                    sphere.CopyAbsoluteBoneTransformsTo(particletransforms);
                    Matrix xform = Matrix.CreateScale(SCALE) * Matrix.CreateTranslation(particle.getPosition());

                    foreach (ModelMesh mesh in sphere.Meshes)
                    {
                        foreach (BasicEffect e in mesh.Effects)
                        {
                            copyEffect(e, particleEffect, xform);
                            if (particle.type == SPLASH)
                            {
                                e.DiffuseColor = new Vector3(0.3f, 0.9f, 1);
                            }
                            else if (particle.type == EXPLODE)
                            {
                                e.DiffuseColor = new Vector3(1, 0.2f, 0.2f);
                            }
                            else if (particle.type == FIRE)
                            {
                                e.DiffuseColor = new Vector3(1.0f / ((particle.getPosition().Y - particle.initPos.Y) / FADE), 0, 0);
                            }
                            e.World = particletransforms[mesh.ParentBone.Index] * xform;
                            e.View = view;
                            e.Projection = projection;
                        }
                        mesh.Draw();
                    }
                }

                if (playerships != null)
                {
                    foreach (Ship ship in playerships)
                    {
                        float SCALE = 0.12f;
                        float xs = -0.55f, dx = 0.13f, zs = -1.8f, dz = -0.105f, y = -0.55f; 

                        int sx = ship.getStartX();
                        int sy = ship.getStartY();
                        if (ship.getOrientation() == Ship.HOR) sx -= 1;
                        if (ship.getName() == "Patrol Boat")
                        {
                            shipEffect.World = Matrix.Identity;
                            shipEffect.View = view;
                            shipEffect.Projection = projection;

                            Matrix[] shiptransforms = new Matrix[sship.Bones.Count];
                            sship.CopyAbsoluteBoneTransformsTo(shiptransforms);
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz) * movein;

                            foreach (ModelMesh mesh in sship.Meshes)
                            {
                                foreach (BasicEffect e in mesh.Effects)
                                {
                                    copyEffect(e, shipEffect, xform);
                                    e.World = shiptransforms[mesh.ParentBone.Index] * xform;
                                    e.View = view;
                                    e.Projection = projection;
                                }
                                mesh.Draw();
                            }
                        }
                        else if (ship.getName() == "Destroyer")
                        {
                            shipEffect.World = Matrix.Identity;
                            shipEffect.View = view;
                            shipEffect.Projection = projection;

                            Matrix[] shiptransforms = new Matrix[mship.Bones.Count];
                            mship.CopyAbsoluteBoneTransformsTo(shiptransforms);
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz) * movein;

                            foreach (ModelMesh mesh in mship.Meshes)
                            {
                                foreach (BasicEffect e in mesh.Effects)
                                {
                                    copyEffect(e, shipEffect, xform);
                                    e.World = shiptransforms[mesh.ParentBone.Index] * xform;
                                    e.View = view;
                                    e.Projection = projection;
                                }
                                mesh.Draw();
                            }
                        }
                        else if (ship.getName() == "Submarine")
                        {
                            shipEffect.World = Matrix.Identity;
                            shipEffect.View = view;
                            shipEffect.Projection = projection;

                            Matrix[] shiptransforms = new Matrix[sub.Bones.Count];
                            sub.CopyAbsoluteBoneTransformsTo(shiptransforms);
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz) * movein;

                            foreach (ModelMesh mesh in sub.Meshes)
                            {
                                foreach (BasicEffect e in mesh.Effects)
                                {
                                    copyEffect(e, shipEffect, xform);
                                    e.World = shiptransforms[mesh.ParentBone.Index] * xform;
                                    e.View = view;
                                    e.Projection = projection;
                                }
                                mesh.Draw();
                            }
                        }
                        else if (ship.getName() == "Battleship")
                        {
                            shipEffect.World = Matrix.Identity;
                            shipEffect.View = view;
                            shipEffect.Projection = projection;

                            Matrix[] shiptransforms = new Matrix[lship.Bones.Count];
                            lship.CopyAbsoluteBoneTransformsTo(shiptransforms);
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz) * movein;

                            foreach (ModelMesh mesh in lship.Meshes)
                            {
                                foreach (BasicEffect e in mesh.Effects)
                                {
                                    copyEffect(e, shipEffect, xform);
                                    e.World = shiptransforms[mesh.ParentBone.Index] * xform;
                                    e.View = view;
                                    e.Projection = projection;
                                }
                                mesh.Draw();
                            }
                        }
                        else if (ship.getName() == "Aircraft Carrier")
                        {
                            shipEffect.World = Matrix.Identity;
                            shipEffect.View = view;
                            shipEffect.Projection = projection;

                            Matrix[] shiptransforms = new Matrix[carrier.Bones.Count];
                            carrier.CopyAbsoluteBoneTransformsTo(shiptransforms);
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz) * movein;

                            foreach (ModelMesh mesh in carrier.Meshes)
                            {
                                foreach (BasicEffect e in mesh.Effects)
                                {
                                    copyEffect(e, shipEffect, xform);
                                    e.World = shiptransforms[mesh.ParentBone.Index] * xform;
                                    e.View = view;
                                    e.Projection = projection;
                                }
                                mesh.Draw();
                            }
                        }
                    }
                }
                
                foreach (EffectPass pass in TbasicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    TbasicEffect.Projection = projection;
                    TbasicEffect.View = view;
                    TbasicEffect.World = movein;

                    graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                        PrimitiveType.TriangleList,
                        getWaterVertices(),
                        0,   // vertex buffer offset to add to each element of the index buffer
                        NUM_WATER * NUM_WATER,   // number of vertices to draw
                        getWaterIndices(),
                        0,   // first index element to read
                        2 * (NUM_WATER - 1) * (NUM_WATER - 1)  // number of primitives to draw
                    );
                }

                sqEffect.World = Matrix.Identity;
                sqEffect.View = view;
                sqEffect.Projection = projection;

                Matrix[] squaretransforms = new Matrix[square.Bones.Count];
                square.CopyAbsoluteBoneTransformsTo(squaretransforms);
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Matrix cubexforms = Matrix.CreateScale(0.05f) * Matrix.CreateRotationX((float)Math.PI / 2f) * Matrix.CreateTranslation(enemysquares[i * 10 + j]) * movein;

                        foreach (ModelMesh mesh in square.Meshes)
                        {
                            foreach (BasicEffect e in mesh.Effects)
                            {
                                copyEffect(e, sqEffect, cubexforms);

                                if (i == highx && j == highy)
                                {
                                    e.AmbientLightColor = Color.Yellow.ToVector3();
                                }
                                if (enemytracker != null && enemytracker[i, j].getStatus() == TrackTile.MISS)
                                {
                                    e.AmbientLightColor = Color.Black.ToVector3();
                                }
                                if (enemytracker != null && enemytracker[i, j].getStatus() == TrackTile.HIT)
                                {
                                    e.AmbientLightColor = Color.Red.ToVector3();
                                }

                                e.World = squaretransforms[mesh.ParentBone.Index] * cubexforms;
                                e.View = view;
                                e.Projection = projection;
                            }
                            mesh.Draw();
                        }
                    }
                }

                if (internalState == Game1.END_STATE)
                {
                    batch.Begin();
                    batch.Draw(fader, Vector2.Zero, new Color(1, 1, 1, 0.8f));
                    batch.End();
                }
            }
        }
    }
}
