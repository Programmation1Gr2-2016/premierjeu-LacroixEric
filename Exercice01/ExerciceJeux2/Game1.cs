



      
       

        protected override void LoadContent()
        {
            


            //Creation des font
            //timerScoreSpriteFront = Content.Load<SpriteFont>("TimerScore");
            font = Content.Load<SpriteFont>("Font");
            fontOrigin = font.MeasureString("Fin !") / 2;


          
          

           


           

        }

       
       
        protected override void Update(GameTime gameTime)

 
            
        {

           // tempsFin = gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60;
            //Fait apparaitre les méchant apres 5s

           


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
               Exit();

            //// TODO: Add your update logic here

           
                if (Keyboard.GetState().IsKeyDown(Keys.R))

                {

                    //Réaparaitre quand on est mort

                    heros.estVivant = true;

                }

                // heros.position.Y += heros.vitesse;
            }

            //Collision
            if (heros.estVivant == true)

            {

                for (int i = 0; i < tabBouleFeu.Length; i++)

                {

                    if (heros.position.Intersects(tabBouleFeu[i].position) && tabBouleFeu[i].estVivant == true)

                    {



                        heros.estVivant = false;

                        tempsFin = gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60;

                    }

                }

                if (heros.estVivant == true)

                {

                    for (int i = 0; i < tabMechant.Length; i++)

                    {

                        if (bouleEau.position.Intersects(tabMechant[i].position))

                        {
                            tabMechant[i].estVivant = false;
                            tabBouleFeu[i].estVivant = false;
                            cptMort++;
                            if (cptMort == 10)
                            {
                                heros.estVivant = false;
                                tempsFin = gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60;
                            }
                        }

                    }
                }
            }

            UpdateHeros();
            UpdateMechants();
            UpdateBouleFeu();


            base.Update(gameTime);
        }



       



        protected void UpdateMechants()
            //Détermine les déplacements des méchants
        {             
            
                //bouleFeu.position.X -= bouleFeu.vitesse;
                //bouleFeu.position.Y += bouleFeu.vitesse;


                //if (bouleFeu.position.X < fenetre.Left)
                //{
                //    // bouleFeu.estVivant = false;
                //    bouleFeu.position.X = tabMechant[i].position.X;
                //    bouleFeu.position.Y = tabMechant[i].position.Y;
                //    bouleFeu.direction.X = tabMechant[i].direction.X;
                //    bouleFeu.direction.Y = tabMechant[i].direction.Y;
                //    // mechant.estVivant = true;
                //}

                //    if (mechant.position.Intersects(heros.position))

                //    {
                //        mechant.vitesse = 0;
                //        bouleFeu.vitesse = 0;
                //        mechant.sprite = Content.Load<Texture2D>("Objet//bombe.png");
                //        bouleFeu.sprite = Content.Load<Texture2D>("Objet//bombe.png");
                //    }

            }
            }

        protected void UpdateBouleFeu()

        {
            //Arme méchants
           
            for (int i = 0; i < tabMechant.Length; i++)
            {
                tabBouleFeu[i].position.X -= tabBouleFeu[i].vitesse;
                tabBouleFeu[i].position.Y += tabBouleFeu[i].vitesse;
                if (tabBouleFeu[i].position.X < fenetre.Left)
                {
                    tabBouleFeu[i].position.X = tabMechant[i].position.X;
                    tabBouleFeu[i].position.Y = tabMechant[i].position.Y;
                    tabBouleFeu[i].direction.X = tabMechant[i].direction.X;
                    tabBouleFeu[i].direction.Y = tabMechant[i].direction.Y;
                }

                //if (tabBouleFeu[i].position.Intersects(heros.position))

                //  {
                //    //heros.vitesse = 0;
                ////        bouleFeu.vitesse = 0;
                //    //heros.sprite = Content.Load<Texture2D>("Objet//bombe.png");
                //    //        bouleFeu.sprite = Content.Load<Texture2D>("Objet//bombe.png");
                //    heros.estVivant = false;
                //  }
            }

           

           



            

        }
        //}

       
        protected override void Draw(GameTime gameTime)
        {
           
            //Affiche texte

            spriteBatch.DrawString(font, tempsFin,new Vector2(fenetre.Width -100, fenetre.Top +20),
    Color.Yellow, 0, fontOrigin, 1, SpriteEffects.None, 0);
            



            

           
            if (heros.estVivant == false)

            {

              

                spriteBatch.DrawString(font, "FIN DU JEU!!!", new Vector2((fenetre.Width / 2) - 100, fenetre.Height / 2), Color.WhiteSmoke, 0, fontOrigin, 2, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, tempsFin + " Secondes", new Vector2((fenetre.Width / 2) - 80, fenetre.Height / 2 + 100), Color.WhiteSmoke, 0, fontOrigin, 2, SpriteEffects.None, 0);

                for (int i = 0; i < tabMechant.Length; i++)
                {
                    tabMechant[i].estVivant = false;
                    if (tabMechant[i].estVivant == true)
                    {
                        spriteBatch.Draw(tabMechant[i].sprite, tabMechant[i].position, Color.White);
                        spriteBatch.Draw(tabBouleFeu[i].sprite, tabBouleFeu[i].position, Color.White);

                    }

                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}