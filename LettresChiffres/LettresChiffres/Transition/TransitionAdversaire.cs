using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XMLObject;
using System.IO.IsolatedStorage;

namespace LettresChiffres
{
    class TransitionAdversaire : GameScreen
    {
        Texture2D continuer_2D;

        SpriteFont chiffre_font;
        SpriteFont transition_font;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        string titre_chiffre;
        string titre_lettre;
        string nombreatrouverString;
        string joueurString;
        string scoreString;
        string Continuer_string;
        string Mot_Invalide_String;
        string menu;

        Vector2 positionContinuer = new Vector2(230, 580);

        Color opacity_titre = Color.White * 0.0f;
        Color opacity_1 = Color.White * 0.0f;
        Color opacity_2 = Color.White * 0.0f;
        Color opacity_3 = Color.White * 0.0f;

        float tempstotal;

        string nomAdversaire;
        string pointJoueur;
        string pointAdversaire;

        int _typepartie;
        int _nombreatrouver;
        int _nombreJoueur;
        int _nombreAdversaire;
        bool _motvalide;
        string _motJoueur;
        string _motAdversaire;

        Sauvegarde save;
        XML_Serializer xmls = new XML_Serializer();
        Languages lang = new Languages();
        Langues langue = new Langues();


        public TransitionAdversaire(int typepartie, int nombreatrouver, int nombrejoueurtrouvee, int nombreAdversairetrouvee)
        {
            _typepartie = typepartie;
            _nombreatrouver = nombreatrouver;
            _nombreJoueur = nombrejoueurtrouvee;
            _nombreAdversaire = nombreAdversairetrouvee;
        }

        public TransitionAdversaire(int typepartie, bool motvalide, string motJOueur, string motAdversaire)
        {
            _typepartie = typepartie;
            _motvalide = motvalide;
            _motJoueur = motJOueur;
            _motAdversaire = motAdversaire;
        }

        private void InitilizeLanguages()
        {
            titre_lettre = lang.AffectationLANG("TITRE_Lettres", langue);
            titre_chiffre = lang.AffectationLANG("TITRE_Chiffres", langue);
            nombreatrouverString = lang.AffectationLANG("Transition_Nombre_A_Trouver", langue);
            scoreString = lang.AffectationLANG("Score_String", langue);
            Continuer_string = lang.AffectationLANG("Continuer", langue);
            Mot_Invalide_String = lang.AffectationLANG("Mot_invalide_String", langue);
            menu = lang.AffectationLANG("Menu", langue);

            if (IsolatedStorageSettings.ApplicationSettings.Contains("player1"))
            {
                joueurString = (string)IsolatedStorageSettings.ApplicationSettings["player1"];
            }
        }

        public override void LoadContent()
        {
            continuer_2D = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_valider");

            save = xmls.DeserializeSauvegarde();

            chiffre_font = ScreenManager.Game.Content.Load<SpriteFont>("chiffre_font");
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
            if (nomAdversaire == null) { nomAdversaire = save.ListeSave.NomAdversaire; Initialize_Point(); }
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Intro(time);
            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Initialize_Point()
        {
            string[] joueur = save.ListeSave.ScoreJoueur.Split(new char[] {' '});
            string[] adversaire = save.ListeSave.ScoreAdversaire.Split(new char[] { ' ' });
            pointJoueur = joueur.Last();
            pointAdversaire = adversaire.Last();
        }

        private void Intro(float time)
        {

            tempstotal += time;

            if (tempstotal > 100f)
            {
                opacity_titre = Color.White;
            }
            if (tempstotal > 400f) { opacity_1 = Color.White; }
            if (tempstotal > 800f) { opacity_2 = Color.White; }
            if (tempstotal > 1200f) { opacity_3 = Color.Orange; }


        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (_typepartie == 3 || _typepartie == 4)
                {
                    if (state.X > positionContinuer.X && state.X < positionContinuer.X + continuer_2D.Width &&
                        state.Y > positionContinuer.Y && state.Y < positionContinuer.Y + continuer_2D.Height)
                    {
                        this.ExitScreen();
                        ScreenManager.AddScreen(new Score());
                    }
                }
            }
            previousState = state;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            DrawInterface();
            DrawContinuer();
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawInterface()
        {
            if (_typepartie == 3)
            {
                int text_align = 20;
                ScreenManager.SpriteBatch.DrawString(chiffre_font, titre_chiffre, new Vector2(480 / 2 - (chiffre_font.MeasureString(titre_chiffre).X / 2), 30), opacity_titre);
                // Chiffres TEXT
                if (nomAdversaire != null)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, nombreatrouverString, new Vector2(text_align, 100), opacity_1);
                    ScreenManager.SpriteBatch.DrawString(transition_font, joueurString, new Vector2(text_align, 200), opacity_2);
                    ScreenManager.SpriteBatch.DrawString(transition_font, nomAdversaire, new Vector2(text_align, 300), opacity_2);
                    ScreenManager.SpriteBatch.DrawString(transition_font, scoreString + " " + joueurString, new Vector2(text_align, 400), opacity_3);
                    ScreenManager.SpriteBatch.DrawString(transition_font, scoreString + " " + nomAdversaire, new Vector2(text_align, 500), opacity_3);

                    //Chiffres chifres
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, _nombreatrouver.ToString(), new Vector2(380, 100), opacity_1);
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, _nombreJoueur.ToString(), new Vector2(380, 200), opacity_2);
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, _nombreAdversaire.ToString(), new Vector2(380, 300), opacity_2);
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, pointJoueur, new Vector2(410, 400), opacity_3);
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, pointAdversaire, new Vector2(410, 500), opacity_3);
                }
            }
            else if (_typepartie == 4)
            {
                int text_align = 20;
                ScreenManager.SpriteBatch.DrawString(chiffre_font, titre_lettre, new Vector2(480 / 2 - (chiffre_font.MeasureString(titre_lettre).X / 2), 30), opacity_titre);
                if (nomAdversaire != null)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, joueurString, new Vector2(text_align, 150), opacity_1);
                    if (_motJoueur != null && _motvalide) { ScreenManager.SpriteBatch.DrawString(transition_font, _motJoueur, new Vector2(240, 150), opacity_1); }
                    else { ScreenManager.SpriteBatch.DrawString(transition_font, Mot_Invalide_String, new Vector2(200, 150), opacity_1); }
                    ScreenManager.SpriteBatch.DrawString(transition_font, nomAdversaire, new Vector2(text_align, 250), opacity_2);
                    if (_motAdversaire != "A") { ScreenManager.SpriteBatch.DrawString(transition_font, _motAdversaire, new Vector2(240, 250), opacity_2); }
                    ScreenManager.SpriteBatch.DrawString(transition_font, scoreString + " " + joueurString, new Vector2(text_align, 350), opacity_3);
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, pointJoueur, new Vector2(410, 350), opacity_3);
                    ScreenManager.SpriteBatch.DrawString(transition_font, scoreString + " " + nomAdversaire, new Vector2(text_align, 450), opacity_3);
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, pointAdversaire, new Vector2(410, 450), opacity_3);
                }
            }
        }

        private void DrawContinuer()
        {
            ScreenManager.SpriteBatch.Draw(continuer_2D, positionContinuer, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, Continuer_string, new Vector2(positionContinuer.X + continuer_2D.Width / 2 - 20 - transition_font.MeasureString(Continuer_string).X / 2, positionContinuer.Y + continuer_2D.Height / 2 - transition_font.MeasureString(Continuer_string).Y / 2), Color.White);
        }

        
    }
}
