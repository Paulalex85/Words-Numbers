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
    class OptionScreen : GameScreen
    {
        Texture2D bouton_rename;

        SpriteFont transition_font;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        string rename, Joueur, _joueur1, _joueur2;
        bool start = false;

        Vector2 joueur1Text = new Vector2();
        Vector2 joueur1Bouton = new Vector2();
        Vector2 joueur2Text = new Vector2();
        Vector2 joueur2Bouton = new Vector2();

        XML_Serializer xmls = new XML_Serializer();
        Sauvegarde save;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public OptionScreen()
        {
            start = true;
        }

        private void InitilizeLanguages()
        {
            rename = lang.AffectationLANG("Renommer", langue);
            Joueur = lang.AffectationLANG("Joueur_String", langue);
        }

        public override void LoadContent()
        {
            bouton_rename = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_rename");
            transition_font = ScreenManager.Game.Content.Load<SpriteFont>("Transition");

            save = xmls.DeserializeSauvegarde();

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
            if (start) { Initialize(); }
                
            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Initialize()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("player1"))
            {
                _joueur1 = (string)IsolatedStorageSettings.ApplicationSettings["player1"];
            }
            if (IsolatedStorageSettings.ApplicationSettings.Contains("player2"))
            {
                _joueur2 = (string)IsolatedStorageSettings.ApplicationSettings["player2"];
            }

            start = false;
        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
            }
            previousState = state;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            DrawInterface();
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawInterface()
        {
            ScreenManager.SpriteBatch.DrawString(transition_font, Joueur + " 1 : " + _joueur1, joueur1Text, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, rename, new Vector2(joueur1Bouton.X + bouton_rename.Width / 2 - transition_font.MeasureString(rename).X / 2, joueur1Bouton.Y + bouton_rename.Height / 2 - transition_font.MeasureString(rename).Y / 2), Color.White);
            ScreenManager.SpriteBatch.Draw(bouton_rename, joueur1Bouton, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, Joueur + " 2 : " + _joueur2, joueur2Text, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, rename, new Vector2(joueur2Bouton.X + bouton_rename.Width / 2 - transition_font.MeasureString(rename).X / 2, joueur2Bouton.Y + bouton_rename.Height / 2 - transition_font.MeasureString(rename).Y / 2), Color.White);
            ScreenManager.SpriteBatch.Draw(bouton_rename, joueur2Bouton, Color.White);
        }
    }
}
