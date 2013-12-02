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

        Model sship, mship, lship, sub, carrier, comp1, comp2, square;
        BasicEffect basicEffect, sqEffect, shipEffect;
        private int internalState = Game1.INTRO_STATE;
        Texture2D fader;

        List<Vector3> enemysquares;
        int highx, highy;
        TrackTile[,] enemytracker;
        Tile[,] playertracker;
        Ship[] playerships;

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

            fader = game.Content.Load<Texture2D>("black");

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
            shipEffect.AmbientLightColor = new Vector3(0, 1, 1);
            shipEffect.DiffuseColor = new Vector3(0, 1, 1);
            shipEffect.SpecularColor = new Vector3(0, 0, 0);
            shipEffect.SpecularPower = 0f;
            shipEffect.Alpha = 0.5f;
            shipEffect.LightingEnabled = true;
            if (shipEffect.LightingEnabled)
            {
                shipEffect.DirectionalLight0.Enabled = true; // enable each light individually
                if (shipEffect.DirectionalLight0.Enabled)
                {
                    shipEffect.DirectionalLight0.DiffuseColor = new Vector3(0f, 1f, 1f); // range is 0 to 1
                    shipEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(0, 0, 1));
                    shipEffect.DirectionalLight0.SpecularColor = Vector3.One;
                }
            }
        }

        public void update(float gameTime, TrackTile[,] track, Tile[,] own, Ship[] pships)
        {
            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;

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

        public void Draw(SpriteBatch batch)
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
                Matrix c1_xforms = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateRotationX((float)Math.PI / 2.5f) * Matrix.CreateTranslation(new Vector3(-1.2f, 0.5f, -3f));
                Matrix[] c1_transforms = new Matrix[comp1.Bones.Count];
                comp1.CopyAbsoluteBoneTransformsTo(c1_transforms);
                Matrix c2_xforms = Matrix.Identity * Matrix.CreateRotationX((float)Math.PI / -1f) * Matrix.CreateRotationZ((float)Math.PI) * Matrix.CreateTranslation(new Vector3(1.45f, 0.45f, -4.3f));
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
                Matrix c1_xforms = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateTranslation(new Vector3(-1.2f, -0.45f, -3f));
                Matrix[] c1_transforms = new Matrix[comp1.Bones.Count];
                comp1.CopyAbsoluteBoneTransformsTo(c1_transforms);
                Matrix c2_xforms = Matrix.Identity * Matrix.CreateRotationY((float)Math.PI) * Matrix.CreateRotationX((float)Math.PI / -2f) * Matrix.CreateTranslation(new Vector3(1.45f, -1.80f, -2.9f));
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

                if (playerships != null)
                {
                    foreach (Ship ship in playerships)
                    {
                        float SCALE = 0.12f;
                        float xs = -0.55f, dx = 0.14f, zs = -1.8f, dz = -0.105f, y = -0.55f; 

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
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz);

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
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz);

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
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz);

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
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz);

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
                            Matrix xform = Matrix.CreateScale(SCALE) * (Matrix.CreateRotationY(ship.getOrientation() == Ship.HOR ? (float)Math.PI / -2f : 0)) * Matrix.CreateTranslation(xs + sx * dx, y, zs + sy * dz);

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

                sqEffect.World = Matrix.Identity;
                sqEffect.View = view;
                sqEffect.Projection = projection;

                Matrix[] squaretransforms = new Matrix[square.Bones.Count];
                square.CopyAbsoluteBoneTransformsTo(squaretransforms);
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Matrix cubexforms = Matrix.CreateScale(0.05f) * Matrix.CreateRotationX((float)Math.PI / 2f) * Matrix.CreateTranslation(enemysquares[i * 10 + j]);

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
