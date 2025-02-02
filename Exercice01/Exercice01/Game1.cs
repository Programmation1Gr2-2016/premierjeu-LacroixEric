﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//Eric Lacroix
//15-11-2916
//Jeux Exercice01

namespace Exercice01
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle fenetre;
        GameObject heros;
        GameObject mechant;
        GameObject bouleFeu;
        Texture2D backgroundTexture;       


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
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            this.Window.Position = new Point(0, 0);            
            this.graphics.ApplyChanges();
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
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;
            this.backgroundTexture = this.Content.Load<Texture2D>("Objet//arriere.jpg");


            //Hero
            heros = new GameObject();
            heros.estVivant = true;
            heros.vitesse = 5;
            heros.sprite = Content.Load<Texture2D>("Objet//hero.png");
            heros.position = heros.sprite.Bounds;

            //Enemi
            mechant = new GameObject();
            mechant.estVivant = true;
            mechant.vitesse = 5;
            mechant.sprite = Content.Load<Texture2D>("Objet//DrNefar.png");
            mechant.position = mechant.sprite.Bounds;
            mechant.position.X = fenetre.Right;
            mechant.position.Y = fenetre.Top;

            //Arme
            bouleFeu = new GameObject();
            bouleFeu.estVivant = true;
            bouleFeu.vitesse = 5;
            bouleFeu.sprite = Content.Load<Texture2D>("Objet//bouleFeu.png");
            bouleFeu.position = bouleFeu.sprite.Bounds;
            bouleFeu.position.X = mechant.position.X;
            bouleFeu.position.Y = mechant.position.Y;

        // TODO: use this.Content to load your game content here

    }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Touches de contrôle

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                heros.position.X += heros.vitesse;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                heros.position.X -= heros.vitesse;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                heros.position.Y += heros.vitesse;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                heros.position.Y -= heros.vitesse;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                heros.position.X += heros.vitesse;
                heros.position.Y += heros.vitesse;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                heros.position.Y -= heros.vitesse;
                heros.position.X -= heros.vitesse;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                heros.position.X += heros.vitesse;
                heros.position.Y -= heros.vitesse;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                heros.position.X -= heros.vitesse;
                heros.position.Y += heros.vitesse;
            }
            
            UpdateHeros();
            UpdateMechant();
            UpdateBouleFeu();

            base.Update(gameTime);
        }

        

        protected void UpdateHeros()

            //Délimitation
        {
            if (heros.position.X < fenetre.Left)
            {
                heros.position.X = fenetre.Left;
            }
            if (heros.position.X > fenetre.Right - heros.sprite.Bounds.Width)
            {
                heros.position.X = fenetre.Right - heros.sprite.Bounds.Width;
            }
            if (heros.position.Y < fenetre.Top)
            {
               heros.position.Y = fenetre.Top;
            }
            if (heros.position.Y > fenetre.Bottom - heros.sprite.Bounds.Height)
            {
                heros.position.Y = fenetre.Bottom - heros.sprite.Bounds.Height;
            }
        }



   protected void UpdateMechant()

{
            mechant.position.X += mechant.vitesse;
            if (mechant.position.X < fenetre.Left)
            {
                mechant.position.X = fenetre.Left;
                mechant.vitesse *= -1;
            }
            if (mechant.position.X > fenetre.Right - mechant.sprite.Bounds.Width)
            {
                mechant.position.X = fenetre.Right - mechant.sprite.Bounds.Width;
                mechant.vitesse *= -1;
            }

            if (mechant.position.Intersects(heros.position))

            {                
                mechant.vitesse = 0;
                bouleFeu.vitesse = 0;
                mechant.sprite = Content.Load<Texture2D>("Objet//bombe.png");
                bouleFeu.sprite = Content.Load<Texture2D>("Objet//bombe.png");
            }


        }

        protected void UpdateBouleFeu()

{

            bouleFeu.position.X -= bouleFeu.vitesse;
            bouleFeu.position.Y += bouleFeu.vitesse;

            if (bouleFeu.position.X < fenetre.Left)
            {
               // bouleFeu.estVivant = false;
                bouleFeu.position.X = mechant.position.X;
                bouleFeu.position.Y = mechant.position.Y;
               // mechant.estVivant = true;
            }
            if (heros.position.Intersects(bouleFeu.position))
            {               
                heros.vitesse = 0;
                heros.sprite = Content.Load<Texture2D>("Objet//bombe.png");
             }
        }

/// <summary>
/// This is called when the game should draw itself.
/// </summary>
/// <param name="gameTime">Provides a snapshot of timing values.</param>
protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.GreenYellow);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            this.spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0,fenetre.Width, fenetre.Height), Color.White);


            spriteBatch.Draw(heros.sprite, heros.position, Color.White);
            spriteBatch.Draw(bouleFeu.sprite, bouleFeu.position, Color.White);
            spriteBatch.Draw(mechant.sprite, mechant.position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
