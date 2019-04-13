using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using XMLObject;
using System.IO.IsolatedStorage;

namespace LettresChiffres
{
    class FinMatch : GameScreen
    {
        Texture2D continuer_2D;
        Texture2D retour_2D;

        SpriteFont transition_font;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        Vector2 positionContinuer = new Vector2(100, 580);

        Vector2 x1 = new Vector2(75, 150);
        Vector2 x2 = new Vector2(300, 150);
        Vector2 y1 = new Vector2(125, 250);
        Vector2 y2 = new Vector2(325, 250);
        Vector2 z1 = new Vector2(75, 350);
        Vector2 z2 = new Vector2(300, 350);
        int RoundGame;
        int _typepartie;
        int _scoreJoueur;
        int _scoreAdversaire;
        string _nomAdversaire;
        string ResultatJoueur;
        string ResultatAdversaire = "";
        bool win;
        XML_Serializer xmls = new XML_Serializer();
        Sauvegarde save;
        Languages lang = new Languages();
        Langues langue = new Langues();

        string victoire, defaite, nul, continuer_string, retourmenu, victoire_cup, joueur_strig, menu;

        public FinMatch(int typepartie,int scoreJoueur, int scoreAdversaire, string nomAdversaire)
        {
            _typepartie = typepartie;
            _scoreJoueur = scoreJoueur;
            _scoreAdversaire = scoreAdversaire;
            _nomAdversaire = nomAdversaire;
        }

        private void InitilizeLanguages()
        {
            victoire = lang.AffectationLANG("Victoire", langue);
            defaite = lang.AffectationLANG("Defaite", langue);
            nul = lang.AffectationLANG("Nul", langue);
            continuer_string = lang.AffectationLANG("Continuer", langue);
            retourmenu = lang.AffectationLANG("Retour", langue);
            victoire_cup = lang.AffectationLANG("victoire_coupe", langue);
            menu = lang.AffectationLANG("Menu", langue);

            if (IsolatedStorageSettings.ApplicationSettings.Contains("player1"))
            {
                joueur_strig = (string)IsolatedStorageSettings.ApplicationSettings["player1"];
            }

            if (_scoreJoueur > _scoreAdversaire)
            {
                ResultatJoueur = victoire;
                ResultatAdversaire = defaite;
                win = true;
            }
            else if (_scoreJoueur == _scoreAdversaire)
            {
                ResultatJoueur = nul;
                win = true;
            }
            else
            {
                ResultatAdversaire = victoire;
                ResultatJoueur = defaite;
                win = false;
            }
        }

        public override void LoadContent()
        {
            continuer_2D = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_valider");
            retour_2D = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_menu");

            save = xmls.DeserializeSauvegarde();
            transition_font = ScreenManager.Game.Content.Load<SpriteFont>("Transition");

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            RoundGame = save.ListeSave.RoundMatch;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
            }
            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (_typepartie == 2 && win && RoundGame < 5)
                {
                    if (state.X > positionContinuer.X && state.X < positionContinuer.X + continuer_2D.Width &&
                            state.Y > positionContinuer.Y && state.Y < positionContinuer.Y + continuer_2D.Height)
                    {
                        this.ExitScreen();
                        ScreenManager.AddScreen(new DebutMatch(2, true));
                    }
                }
                else
                {
                    if (state.X > positionContinuer.X && state.X < positionContinuer.X + retour_2D.Width &&
                            state.Y > positionContinuer.Y && state.Y < positionContinuer.Y + retour_2D.Height)
                    {
                        this.ExitScreen();
                        ScreenManager.AddScreen(new MainMenuScreen(menu));
                    }
                }
            }
            previousState = state;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.DrawString(transition_font, joueur_strig, x1, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, _nomAdversaire, x2, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, _scoreJoueur.ToString(), y1, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, _scoreAdversaire.ToString(), y2, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, ResultatJoueur, z1, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, ResultatAdversaire, z2, Color.White);

            if (_typepartie == 2 && win && RoundGame < 5)
            {
                ScreenManager.SpriteBatch.Draw(continuer_2D, positionContinuer, Color.White);
                ScreenManager.SpriteBatch.DrawString(transition_font, continuer_string, new Vector2(positionContinuer.X + continuer_2D.Width / 2 - 20 - transition_font.MeasureString(continuer_string).X / 2, positionContinuer.Y + continuer_2D.Height / 2 - transition_font.MeasureString(continuer_string).Y / 2), Color.White);
            }
            else if (_typepartie == 2 && win && RoundGame == 5)
            {
                ScreenManager.SpriteBatch.DrawString(transition_font, victoire_cup, new Vector2(50, 450), Color.White);
                ScreenManager.SpriteBatch.Draw(retour_2D, positionContinuer, Color.White);
                ScreenManager.SpriteBatch.DrawString(transition_font, menu, new Vector2(positionContinuer.X + retour_2D.Width / 2 - transition_font.MeasureString(menu).X / 2, positionContinuer.Y + retour_2D.Height / 2 - transition_font.MeasureString(menu).Y / 2), Color.White);
            }
            else
            {
                ScreenManager.SpriteBatch.Draw(retour_2D, positionContinuer, Color.White);
                ScreenManager.SpriteBatch.DrawString(transition_font, menu, new Vector2(positionContinuer.X + retour_2D.Width / 2 - transition_font.MeasureString(menu).X / 2, positionContinuer.Y + retour_2D.Height / 2 - transition_font.MeasureString(menu).Y / 2), Color.White);
            }
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
