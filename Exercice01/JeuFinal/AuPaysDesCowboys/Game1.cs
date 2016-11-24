using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

//Eric Lacroix
//19-11-2916
//Jeu programmé avec Monogame (prog1)

namespace AuPaysDesCowboys
{
    //Début du code
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle fenetre;      

        //Arrière-plan
        Texture2D backgroundTexture;

        //Var. pour les texte
        SpriteFont texte;
        Vector2 fontOrigin;

        //Var. du temps écoulé
        int tempsFin = 0;
        int tempsJeu = 0;

        //Var. cowboy
        GameObject cowboy;  //Personnage heros
        GameObject balle;   //Arme
        SoundEffect sonGalop; //Son déplacement
        SoundEffectInstance galop; 
        SoundEffect sonRevolver; //Son Arme
        SoundEffectInstance revolver;
        SoundEffect sonMort; //Son mort du cowboy
        SoundEffectInstance mort;
        SoundEffect sonWin; //Son du cowboy gagnant
        SoundEffectInstance win;

        //Var. indiens
        GameObject chef; //Personnade de fin quand cowboy mort
        GameObject[] tabIndiens; //Personnage enemis
        GameObject[] tabFleche;  //Arme
        int nbrIndiens = 5;      //Nombre d'indiens
        int cptIndiens = 0;      //Compteur
        int cptMort = 5;        //Compteur
        SoundEffect sonAppache; //Son entré des indiens
        SoundEffectInstance appache;        
        SoundEffect sonChef; //Son rire du chef
        SoundEffectInstance chefRire;
        SoundEffect sonIndien; //Song mort indiens
        SoundEffectInstance indien;
        int gero = 0;

        //Déplacements aléatoire
        Random randomdeplace = new Random();
        Random randomfleche = new Random();       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Affichage de l'écran
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width; 
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            //this.graphics.ToggleFullScreen(); //Mode plein écran sans bordures
            
            //Position de départ à l'écran pour la fenêtre de jeu
            this.Window.Position = new Point(0, 0);
            this.graphics.ApplyChanges();

            base.Initialize();
        }

      //Partie pour initialiser les caractéristique des éléments du jeu_____________________________________________________________________________________________

        protected override void LoadContent()
        {
            //Parametrage de la fenêtre de jeu
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;
            this.backgroundTexture = this.Content.Load<Texture2D>("Image//arrierePlan.jpg");

            //Theme musique du jeu
            Song song = Content.Load<Song>("Sounds\\themeJeu");
            MediaPlayer.Volume = (float)0.50;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            
            //Création des Fonts
            texte = Content.Load<SpriteFont>("Texte//Font");
            fontOrigin = texte.MeasureString(" ") / 2;
            
            //Définition du cowboy
            cowboy = new GameObject();
            cowboy.estVivant = true;
            cowboy.vitesse = 5;
            cowboy.sprite = Content.Load<Texture2D>("Image//cowboy.png");
            cowboy.position = cowboy.sprite.Bounds;
            cowboy.position.X = (fenetre.Width/2);
            cowboy.position.Y = (fenetre.Height / 2);

            //Arme du cowboy balle de fusil
            balle = new GameObject();
            balle.estVivant = false;
            balle.vitesse = 25;
            balle.sprite = Content.Load<Texture2D>("Image\\balle.png");
            balle.position = balle.sprite.Bounds;            

            //Sons associés au cowboy
            sonGalop = Content.Load<SoundEffect>("Sounds\\galop");
            galop = sonGalop.CreateInstance();
            sonRevolver = Content.Load<SoundEffect>("Sounds\\revolver");
            revolver = sonRevolver.CreateInstance();
            sonMort = Content.Load<SoundEffect>("Sounds\\mortCowboy");
            mort = sonMort.CreateInstance();
            sonWin = Content.Load<SoundEffect>("Sounds\\win");
            win = sonWin.CreateInstance();

            //Définition des indiens
            //Grand Chef
            chef = new GameObject();
            chef.estVivant = true;
            chef.vitesse = 5;
            chef.sprite = Content.Load<Texture2D>("Image\\chef.png");
            chef.position = chef.sprite.Bounds;

            //Son chef
            sonChef = Content.Load<SoundEffect>("Sounds\\rire");
            chefRire = sonChef.CreateInstance();
            
            //Boucle définisant tableau indiens
            randomdeplace = new Random();
            randomfleche = new Random();
            tabIndiens = new GameObject[nbrIndiens];
            tabFleche = new GameObject[nbrIndiens];
            for (int i = 0; i < tabIndiens.Length; i++)
            {
                tabIndiens[i] = new GameObject();
                tabIndiens[i].estVivant = false;
                tabIndiens[i].vitesse = 5;
                tabIndiens[i].sprite = Content.Load<Texture2D>("Image//indien.png");
                tabIndiens[i].position = tabIndiens[i].sprite.Bounds;
                tabIndiens[i].position.X = randomdeplace.Next(graphics.GraphicsDevice.DisplayMode.Width);
                tabIndiens[i].position.Y = randomdeplace.Next(graphics.GraphicsDevice.DisplayMode.Height);
                tabIndiens[i].direction.X = randomdeplace.Next(-4, 5);
                tabIndiens[i].direction.Y = randomdeplace.Next(-4, 5);               

                //Arme indiens
                tabFleche[i] = new GameObject();
                tabFleche[i].estVivant = false;
                tabFleche[i].vitesse = 5;
                tabFleche[i].sprite = Content.Load<Texture2D>("Image//hache.png");
                tabFleche[i].position = tabFleche[i].sprite.Bounds;
                tabFleche[i].direction.X = randomdeplace.Next(-4,5);
                tabFleche[i].direction.Y = randomdeplace.Next(-4,5);
                }

            //Son indiens
            sonAppache = Content.Load<SoundEffect>("Sounds\\cri");
            appache = sonAppache.CreateInstance();
            sonIndien = Content.Load<SoundEffect>("Sounds\\mortC");
            indien = sonIndien.CreateInstance();            
        }

        //Partie______________________________________________________________________________________________

        protected override void UnloadContent()
        {
            
        }

    //Partie pour définir tous les mouvements et autres pour le jeu (coeur du prog.)________________________
       
        protected override void Update(GameTime gameTime)
        {
            tempsJeu = gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Controle pour les déplacements du cowboy            
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) //Déplacement droit
                {
                    cowboy.position.X += cowboy.vitesse;
                    galop.Play();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left)) //Déplacement gauche
                {
                    cowboy.position.X -= cowboy.vitesse;
                    galop.Play();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down)) //Déplacement vers le bas
                {
                    cowboy.position.Y += cowboy.vitesse;
                    galop.Play();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up)) //Déplacement vers le haut
                {
                    cowboy.position.Y -= cowboy.vitesse;
                    galop.Play();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Q)) //Déplacement diago. gauche_haut
                {
                    cowboy.position.Y -= cowboy.vitesse;
                    cowboy.position.X -= cowboy.vitesse;
                    galop.Play();
                }                        
                              
                //Position arme_cowboy
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    balle.estVivant = true;
                    revolver.Play();
                    balle.position.X = cowboy.position.X +15;
                    balle.position.Y = cowboy.position.Y +50;                  
                }
                
                //Entré des indiens après 10 secondes
                if (cptIndiens * 5 < gameTime.TotalGameTime.Seconds && cptIndiens < nbrIndiens && cowboy.estVivant == true)
                {
                    tabIndiens[cptIndiens].estVivant = true;
                    tabFleche[cptIndiens].estVivant = true;
                    if (cowboy.estVivant == true)
                    {
                        appache.Play();
                    }
                    cptIndiens++;
                }

                //Déplacement des indiens
                for (int i = 0; i < tabIndiens.Length; i++)
                {
                    if (tabIndiens[i].estVivant == true)
                    {
                        tabIndiens[i].position.X += (int)tabIndiens[i].direction.X;
                        tabIndiens[i].position.Y += (int)tabIndiens[i].direction.Y;
                        if (tabIndiens[i].position.X < fenetre.Left)
                        {
                            tabIndiens[i].position.X = fenetre.Left;
                            tabIndiens[i].direction *= -1;
                        }
                        else if (tabIndiens[i].position.X > fenetre.Right - tabIndiens[i].sprite.Bounds.Width)
                        {
                            tabIndiens[i].position.X = fenetre.Right - tabIndiens[i].sprite.Bounds.Width;
                            tabIndiens[i].direction *= -1;                           
                        }
                        else if (tabIndiens[i].position.Y < fenetre.Top)
                        {
                            tabIndiens[i].position.Y = fenetre.Top;
                            tabIndiens[i].direction *= -1;                           
                        }
                        else if (tabIndiens[i].position.Y > fenetre.Bottom - tabIndiens[i].sprite.Bounds.Height)
                        {
                            tabIndiens[i].position.Y = fenetre.Bottom - tabIndiens[i].sprite.Bounds.Height;
                            tabIndiens[i].direction *= -1;                         
                        }

                        //Tuer cowboy
                        else if (cowboy.position.Intersects(tabFleche[i].position) && tabFleche[i].estVivant == true)
                        {                                                     
                            if (cowboy.estVivant == true)
                            {
                                mort.Play();
                                cowboy.estVivant = false;
                                tabIndiens[i].estVivant = false;
                                balle.estVivant = false;
                            }
                            tempsFin = tempsJeu;
                        }
                    }
                }
            }

            //Mise à jour des fonctions
            UpdateChef();
            UpdateCowboy();
            UpdateArmeIndien();
            UpdateArmeCowboy();
            base.Update(gameTime);
        }

        //Mise à jour du cowboy

        protected void UpdateCowboy()

        //Délimitation du cadre pour déplacement du cowboy
        {
            if (cowboy.position.X < fenetre.Left) //Gauche
            {
                cowboy.position.X = fenetre.Left;
            }
            else if (cowboy.position.X > fenetre.Right - cowboy.sprite.Bounds.Width) //Droit
            {
                cowboy.position.X = fenetre.Right - cowboy.sprite.Bounds.Width;
            }
            else if (cowboy.position.Y < fenetre.Top) //Haut
            {
                cowboy.position.Y = fenetre.Top;
            }
            else if (cowboy.position.Y > fenetre.Bottom - cowboy.sprite.Bounds.Height) //Bas
            {
                cowboy.position.Y = fenetre.Bottom - cowboy.sprite.Bounds.Height;
            }

            //Tuer les indiens
           //else if (cowboy.estVivant == true)
           /// {
                //for (int i = 0; i < tabIndiens.Length; i++)
                //{
                    if (balle.position.Intersects(tabIndiens[gero].position))
                    {
                        
                        indien.Play();
                        tabIndiens[gero].estVivant = false;
                        tabFleche[gero].estVivant = false;
                        balle.estVivant = false;
                        cptMort--;
                        gero++;
                       
                    }
                    
                //}
                
               
            //}
            

            //Victoire du cowboy        
            if (cptMort == 0 )
            {
                 cowboy.position.X += cowboy.vitesse;
                if (cowboy.position.X < fenetre.Left)
                {
                    cowboy.position.X = fenetre.Left;
                    cowboy.vitesse *= -1;
                    cowboy.position.Y += 40;
                }
                if (cowboy.position.X > fenetre.Right - cowboy.sprite.Bounds.Width)
                {
                    cowboy.position.X = fenetre.Right - cowboy.sprite.Bounds.Width;
                    cowboy.vitesse *= -1;
                    cowboy.position.Y -= 40;
                }
                MediaPlayer.Volume = (float)0.0;
                win.Play();
            }
        }

        //Mise à jour arme cowboy
        protected void UpdateArmeCowboy()
        {            
            balle.position.X += balle.vitesse;
            if (balle.position.X > fenetre.Right)
            {
                balle.estVivant = false;
            }
        }

        //Mise à jour du chef
        protected void UpdateChef()
        {
            if (cowboy.estVivant == false)
            { 
                chef.position.X += chef.vitesse;
                 if (chef.position.X < fenetre.Left)
                 {
                     chef.position.X = fenetre.Left;
                     chef.vitesse *= -1;
                 }
                 if (chef.position.X > fenetre.Right - chef.sprite.Bounds.Width)
                 {
                     chef.position.X = fenetre.Right - chef.sprite.Bounds.Width;
                     chef.vitesse *= -1;
                 }
              chefRire.Play();
              } 
         }

        //Mise à jour des indiens
        protected void UpdateArmeIndien()
        {
            for (int i = 0; i < tabIndiens.Length; i++)
            {            
                tabFleche[i].position.X -= tabFleche[i].vitesse;
                tabFleche[i].position.Y -= tabFleche[i].vitesse;
                tabFleche[i].position.X += (int)tabFleche[i].direction.X;
                tabFleche[i].position.Y += (int)tabFleche[i].direction.Y;
                if (tabFleche[i].position.X < fenetre.Left)
                {
                    tabFleche[i].position.X = tabIndiens[i].position.X;
                    tabFleche[i].position.Y = tabIndiens[i].position.Y;
                }
            }
        }

       
        //Partie de l'affichage_______________________________________________________________________________ 
        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            //Affichage de l'arrière-plan
            this.spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, fenetre.Width, fenetre.Height), Color.White);

            //Affiche le temps écoulé
            spriteBatch.DrawString(texte, "Temps: " + tempsJeu, new Vector2(fenetre.Width -200, fenetre.Top +50), Color.Blue, 0, fontOrigin, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(texte, "Comp " + cptMort, new Vector2(fenetre.Width -200, fenetre.Top +75), Color.Blue, 0, fontOrigin, 1, SpriteEffects.None, 0);
            //Affichage du cowboy et arme cowboy
            if (balle.estVivant == true)
            {
                spriteBatch.Draw(balle.sprite, balle.position, Color.White);
            }
            if (cowboy.estVivant == true)
            {
                spriteBatch.Draw(cowboy.sprite, cowboy.position, Color.White);                
            }         

            //Affichage des indiens
            for (int i = 0; i < tabIndiens.Length; i++)
            {
                if (tabIndiens[i].estVivant == true)
                {
                    spriteBatch.Draw(tabIndiens[i].sprite, tabIndiens[i].position, Color.White);
                    spriteBatch.Draw(tabFleche[i].sprite, tabFleche[i].position, Color.White);
                }
            }
            if (cptMort == 0)
            {             
             spriteBatch.DrawString(texte, "CHAMPION!!!", new Vector2((fenetre.Width / 2) - 200, fenetre.Height / 2), Color.Red, 0, fontOrigin, 2, SpriteEffects.None, 0);
            }
            if (cowboy.estVivant == false)
                {               
                for (int i = 0; i < tabIndiens.Length; i++)
                    {
                        tabIndiens[i].estVivant = false;
                    }
                    MediaPlayer.Volume = (float)0.0;
                    spriteBatch.Draw(chef.sprite, chef.position, Color.White);
                    spriteBatch.DrawString(texte, "FIN DU JEU!!!", new Vector2((fenetre.Width / 2) -200, fenetre.Height / 2), Color.Red, 0, fontOrigin, 2, SpriteEffects.None, 0);
                    spriteBatch.DrawString(texte, tempsFin + " Secondes", new Vector2((fenetre.Width / 2) - 200, fenetre.Height / 2 + 100), Color.Red, 0, fontOrigin, 2, SpriteEffects.None, 0);
                }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
