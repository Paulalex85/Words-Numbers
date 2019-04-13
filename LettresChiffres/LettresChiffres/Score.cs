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
using System.Xml.Serialization;
using System.IO;

namespace LettresChiffres
{
    class Score : GameScreen
    {
        Texture2D continuer_2D;

        SpriteFont transition_font;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        Vector2 positionContinuer = new Vector2(225, 630);
        int positionTOTAL_Y = 580;

        int _nbrManche;
        int _typepartieMatch;
        int _nbrmanchejouer;
        string[] _ScoreJoueur;
        string[] _ScoreAdversaire;
        string _nomAdversaire;

        int ScoreTOTALJoueur;
        int ScoreTOTALAdversaire;
        
        int positionLigneX = 20;
        int positionJoueurScore = 250;
        int positionAdversaireScore = 400;
        int PositionMultiplicateur = 38;

        string manche, menu, joueur, total, continuer;

        Sauvegarde save;
        XML_Serializer xmls = new XML_Serializer();
        Languages lang = new Languages();
        Langues langue = new Langues();

        public Score()
        {
        }

        private void InitilizeLanguages()
        {
            manche = lang.AffectationLANG("Manches", langue);
            menu = lang.AffectationLANG("Menu", langue);
            total = lang.AffectationLANG("Total", langue);
            continuer = lang.AffectationLANG("Continuer", langue);

            if (IsolatedStorageSettings.ApplicationSettings.Contains("player1"))
            {
                joueur = (string)IsolatedStorageSettings.ApplicationSettings["player1"];
            }
        }

        public override void LoadContent()
        {
            continuer_2D = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_valider");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
            }
            if (_nbrManche == 0) { Initialize(); }
            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Initialize()
        {
            _ScoreAdversaire = save.ListeSave.ScoreAdversaire.Split(new char[] { ' ' });
            _ScoreJoueur = save.ListeSave.ScoreJoueur.Split(new char[] { ' ' });
            _nbrManche = save.ListeSave.NombreDeManches;
            _nbrmanchejouer = save.ListeSave.NombreManchesJouer;
            _typepartieMatch = save.ListeSave.TypeMatch;
            _nomAdversaire = save.ListeSave.NomAdversaire;

            for (int i = 0; i < _ScoreAdversaire.Count(); i++)
            {
                ScoreTOTALAdversaire += int.Parse(_ScoreAdversaire[i]);
            }
            for (int i = 0; i < _ScoreJoueur.Count(); i++)
            {
                ScoreTOTALJoueur += int.Parse(_ScoreJoueur[i]);
            }
        }

        private void Continuer()
        {
            if (_nbrManche < _nbrmanchejouer)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new FinMatch(save.ListeSave.TypeMatch,ScoreTOTALJoueur, ScoreTOTALAdversaire, _nomAdversaire));
            }
            else
            {

                switch (_nbrmanchejouer)
                {
                    case 1: EnchainementLettres(); break;
                    case 2: EnchainementChiffres(); break;
                    case 3: EnchainementLettres(); break;
                    case 4: EnchainementLettres(); break;
                    case 5: EnchainementChiffres(); break;
                    case 6: EnchainementLettres(); break;
                    case 7: EnchainementLettres(); break;
                    case 8: EnchainementChiffres(); break;
                    case 9: EnchainementLettres(); break;
                    case 10: EnchainementLettres(); break;
                    case 11: EnchainementChiffres(); break;
                    case 12: EnchainementLettres(); break;

                }
            }
        }

        private void EnchainementChiffres()
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new Chiffres(3));
        }

        private void EnchainementLettres()
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new Lettres.Lettres(4));
        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (state.X > positionContinuer.X && state.X < positionContinuer.X + continuer_2D.Width &&
                        state.Y > positionContinuer.Y && state.Y < positionContinuer.Y +continuer_2D.Height)
                {
                    Continuer();
                }
            }
            previousState = state;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.DrawString(transition_font, manche, new Vector2(positionLigneX, 20), Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, joueur, new Vector2(positionJoueurScore - transition_font.MeasureString(joueur).X / 2, 20), Color.White);
            if (_nomAdversaire != null) { ScreenManager.SpriteBatch.DrawString(transition_font, _nomAdversaire, new Vector2(positionAdversaireScore - transition_font.MeasureString(_nomAdversaire).X / 2, 20), Color.White); }

            DrawJeuxLignes();
            if (_ScoreJoueur != null && _ScoreJoueur[0] != "99") { DrawScore(); }
            ScreenManager.SpriteBatch.DrawString(transition_font, total, new Vector2(positionLigneX,positionTOTAL_Y), Color.White);

            //BOUTONS
            ScreenManager.SpriteBatch.Draw(continuer_2D, positionContinuer, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, continuer, new Vector2(positionContinuer.X + continuer_2D.Width / 2 - 20 - transition_font.MeasureString(continuer).X / 2, positionContinuer.Y + continuer_2D.Height / 2 - transition_font.MeasureString(continuer).Y / 2), Color.White);
            

            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawScore()
        {
            if (_ScoreJoueur != null)
            {
                for (int i = 0; i < _ScoreJoueur.Count(); i++)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, _ScoreJoueur[i], new Vector2(positionJoueurScore, 80 + (PositionMultiplicateur * i)), Color.White);
                }
            }
            if (_ScoreAdversaire != null)
            {
                for (int i = 0; i < _ScoreAdversaire.Count(); i++)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, _ScoreAdversaire[i], new Vector2(positionAdversaireScore, 80 + (PositionMultiplicateur * i)), Color.White);
                }
            }
            ScreenManager.SpriteBatch.DrawString(transition_font, ScoreTOTALJoueur.ToString(), new Vector2(positionJoueurScore, positionTOTAL_Y), Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, ScoreTOTALAdversaire.ToString(), new Vector2(positionAdversaireScore, positionTOTAL_Y), Color.White);

        }

        private void DrawJeuxLignes()
        {
            if (_nbrManche == 3)
            {
                for (int i = 1; i <= 3; i++)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, i.ToString(), new Vector2(positionLigneX, PositionMultiplicateur + (PositionMultiplicateur * i)), Color.White);
                }
            }
            else if (_nbrManche == 6)
            {
                for (int i = 1; i <= 6; i++)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, i.ToString(), new Vector2(positionLigneX, PositionMultiplicateur + (PositionMultiplicateur * i)), Color.White);
                }
            }
            else if (_nbrManche == 9)
            {
                for (int i = 1; i <= 9; i++)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, i.ToString(), new Vector2(positionLigneX, PositionMultiplicateur + (PositionMultiplicateur * i)), Color.White);
                }
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, i.ToString(), new Vector2(positionLigneX, PositionMultiplicateur + (PositionMultiplicateur * i)), Color.White);
                }
            }
        }
    }
}
