using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Phone.Info;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XMLObject;

namespace LettresChiffres
{
    class Transition : GameScreen
    {
        Texture2D icone_suivant;
        Texture2D icone_chiffres;
        Texture2D resoudre;
        Texture2D icone_refaire;
        Texture2D icone_lettres;
        Texture2D retour_menu;

        Vector2 position_retour = new Vector2(150, 650);

        SpriteFont chiffre_font;
        SpriteFont transition_font;

        Color opacity_titre = Color.White * 0.0f;
        Color opacity_1 = Color.White * 0.0f;
        Color opacity_2 = Color.White * 0.0f;
        Color opacity_3 = Color.White * 0.0f;


        MouseState state = Mouse.GetState();
        MouseState previousState;

        string nombreatrouverString;
        string nombretrouveeString;
        string pointString;
        string titre_chiffre;
        string titre_lettre;
        string Recommencer_string;
        string Chiffres_string;
        string Lettres_string;
        string Lettres_mini_string;
        string Pas_de_mot_string;
        string menu;

        int _typepartie;
        int _nombreatrouver;
        int _nombrejoueurtrouvee;
        int _point;

        bool _motvalide;
        string _motjoueur;

        int _bouton_Y = 570;
        int _bouton1 = 15;
        int _bouton2 = 300;

        float tempstotal;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public Transition(int typepartie, int nombreatrouver, int nombrejoueurtrouvee)
        {
            _typepartie = typepartie;
            _nombreatrouver = nombreatrouver;
            _nombrejoueurtrouvee = nombrejoueurtrouvee;
            ValiderResultat(nombrejoueurtrouvee);
        }

        public Transition(int typepartie, bool motvalide, string mot)
        {
            _typepartie = typepartie;
            _motvalide = motvalide;
            _motjoueur = mot;
            _point = Point(_motvalide, _motjoueur);
        }


        private void InitilizeLanguages()
        {
            nombreatrouverString = lang.AffectationLANG("Transition_Nombre_A_Trouver", langue);
            nombretrouveeString = lang.AffectationLANG("Transition_Nombre_Trouver", langue);
            pointString = lang.AffectationLANG("Transition_Points", langue);
            titre_lettre = lang.AffectationLANG("TITRE_Lettres", langue);
            titre_chiffre = lang.AffectationLANG("TITRE_Chiffres", langue);
            Recommencer_string = lang.AffectationLANG("Recommencer_String", langue);
            Chiffres_string = lang.AffectationLANG("Chiffres_String", langue);
            Lettres_string = lang.AffectationLANG("Lettres_String", langue);
            Lettres_mini_string = lang.AffectationLANG("Lettres_mini_String", langue);
            Pas_de_mot_string = lang.AffectationLANG("Pas_de_mot_string", langue);
            menu = lang.AffectationLANG("Menu", langue);
        }

        private int Point(bool motvalide, string motjoueur)
        {
            if (motvalide)
            {
                return motjoueur.Count();
            }
            else
            {
                return 0;
            }
        }


        public override void LoadContent()
        {
            retour_menu = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_menu");
            resoudre = ScreenManager.Game.Content.Load<Texture2D>("Transition/resoudre");
            icone_suivant = ScreenManager.Game.Content.Load<Texture2D>("Transition/icone_suivant");
            icone_refaire = ScreenManager.Game.Content.Load<Texture2D>("Transition/icone_repeter");
            icone_lettres = ScreenManager.Game.Content.Load<Texture2D>("Transition/icone_lettres");
            icone_chiffres = ScreenManager.Game.Content.Load<Texture2D>("Transition/icone_chiffres");

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
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Intro(time);
            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void ValiderResultat(int resultat)
        {
            int nombre;
            if (_nombreatrouver == resultat)
            {
                _point = 10;
            }
            else if (_nombreatrouver > resultat)
            {
                nombre = _nombreatrouver - resultat;
                _point = 10 - nombre;
                if (_point < 0)
                {
                    _point = 0;
                }
            }
            else
            {
                nombre = resultat - _nombreatrouver;
                _point = 10 - nombre;
                if (_point < 0)
                {
                    _point = 0;
                }
            }
        }

        private void Intro(float time)
        {
            tempstotal += time;

            if (tempstotal > 100f)
            {
                opacity_titre = Color.White;
            }
            if (tempstotal > 400) { opacity_1 = Color.White; }
            if (tempstotal > 800) { opacity_2 = Color.White; }
            if (tempstotal > 1200) { opacity_3 = Color.Orange; }
        }

        private void Recommencer()
        {
            if (_typepartie == 1)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new Chiffres(1));
            }
            else if (_typepartie == 2)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new Lettres.Lettres(2));
            }
        }


        private void AutreJeu()
        {
            if (_typepartie == 1)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new Lettres.Lettres(2));
            }
            else if (_typepartie == 2)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new Chiffres(1));
            }
        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (_typepartie == 1)
                {
                    if (state.X > _bouton1 && state.X < _bouton1 + (35 + transition_font.MeasureString(Recommencer_string).X) &&
                        state.Y > _bouton_Y && state.Y < _bouton_Y + (transition_font.MeasureString(Recommencer_string).Y))
                    {
                        Recommencer();
                    }
                    if (state.X > _bouton2 && state.X < _bouton2 + (35 + transition_font.MeasureString(Lettres_string).X) &&
                        state.Y > _bouton_Y && state.Y < _bouton_Y + (transition_font.MeasureString(Lettres_string).Y))
                    {
                        AutreJeu();
                    }
                }
                if (_typepartie == 2)
                {
                    if (state.X > _bouton1 && state.X < _bouton1 + (35 + transition_font.MeasureString(Recommencer_string).X) &&
                        state.Y > _bouton_Y && state.Y < _bouton_Y + (transition_font.MeasureString(Recommencer_string).Y))
                    {
                        Recommencer();
                    }
                    if (state.X > _bouton2 && state.X < _bouton2 + (35 + transition_font.MeasureString(Chiffres_string).X) &&
                        state.Y > _bouton_Y && state.Y < _bouton_Y + (transition_font.MeasureString(Chiffres_string).Y))
                    {
                        AutreJeu();
                    }
                }
                if (state.X > position_retour.X && state.X < position_retour.X + retour_menu.Width &&
                        state.Y > position_retour.Y && state.Y < position_retour.Y + retour_menu.Height)
                {
                    this.ExitScreen();
                    ScreenManager.AddScreen(new MainMenuScreen(menu));
                }


            }
            previousState = state;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            DrawInterface();
            DrawBouton();
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawInterface()
        {
            if (_typepartie == 1)
            {
                int text_align = 20;
                ScreenManager.SpriteBatch.DrawString(chiffre_font, titre_chiffre, new Vector2(480 / 2 - (chiffre_font.MeasureString(titre_chiffre).X / 2), 150), opacity_titre);
                // Chiffres TEXT
                ScreenManager.SpriteBatch.DrawString(transition_font, nombreatrouverString, new Vector2(text_align, 300), opacity_1);
                ScreenManager.SpriteBatch.DrawString(transition_font, nombretrouveeString, new Vector2(text_align, 400), opacity_2);
                ScreenManager.SpriteBatch.DrawString(transition_font, pointString, new Vector2(text_align, 500), opacity_3);

                //Chiffres chifres
                ScreenManager.SpriteBatch.DrawString(chiffre_font, _nombreatrouver.ToString(), new Vector2(380, 300), opacity_1);
                ScreenManager.SpriteBatch.DrawString(chiffre_font, _nombrejoueurtrouvee.ToString(), new Vector2(380, 400), opacity_2);
                ScreenManager.SpriteBatch.DrawString(chiffre_font, _point.ToString(), new Vector2(410, 500), opacity_3);
            }
            else if (_typepartie == 2)
            {
                int text_align = 20;
                ScreenManager.SpriteBatch.DrawString(chiffre_font, titre_lettre, new Vector2(480 / 2 - (chiffre_font.MeasureString(titre_lettre).X / 2), 150), opacity_titre);
                ScreenManager.SpriteBatch.DrawString(transition_font, _motjoueur, new Vector2(text_align, 350), opacity_1);
                int pd = _motjoueur.Count();
                if (pd == 0)
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, CategorieMots(pd), new Vector2(150, 350), opacity_2);
                }
                else
                {
                    ScreenManager.SpriteBatch.DrawString(transition_font, CategorieMots(pd), new Vector2(300, 350), opacity_2);
                }
                ScreenManager.SpriteBatch.DrawString(transition_font, pointString, new Vector2(text_align, 500), opacity_3);
                ScreenManager.SpriteBatch.DrawString(chiffre_font, _point.ToString(), new Vector2(410, 500), opacity_3);
            }
        }

        private void DrawBouton()
        {
            if (_typepartie == 1)
            {
                DrawRecommencer();
                DrawLettres();
            }
            else if (_typepartie == 2)
            {
                DrawRecommencer();
                DrawChiffres();
            }
            DrawRetourMenu();
        }

        private void DrawRetourMenu()
        {
            ScreenManager.SpriteBatch.Draw(retour_menu, position_retour, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, menu, new Vector2(position_retour.X + 20 + retour_menu.Width / 2 - transition_font.MeasureString(menu).X / 2, position_retour.Y + retour_menu.Height / 2 - transition_font.MeasureString(menu).Y / 2), Color.White);
        }

        private void DrawRecommencer()
        {
            ScreenManager.SpriteBatch.DrawString(transition_font,Recommencer_string, new Vector2(_bouton1 + 35, _bouton_Y), Color.White);
            ScreenManager.SpriteBatch.Draw(icone_refaire, new Vector2(_bouton1, _bouton_Y + 10), Color.White);
        }
        private void DrawLettres()
        {
            ScreenManager.SpriteBatch.DrawString(transition_font, Lettres_string, new Vector2(_bouton2 + 35, _bouton_Y), Color.White);
            ScreenManager.SpriteBatch.Draw(icone_lettres, new Vector2(_bouton2, _bouton_Y + 10), Color.White);
        }
        private void DrawChiffres()
        {
            ScreenManager.SpriteBatch.DrawString(transition_font, Chiffres_string, new Vector2(_bouton2 + 20, _bouton_Y), Color.White);
            ScreenManager.SpriteBatch.Draw(icone_chiffres, new Vector2(_bouton2-15, _bouton_Y + 10), Color.White);
        }
        private string CategorieMots(int nombre)
        {
            switch (nombre)
            {
                case 10: return "10 "+Lettres_mini_string;
                case 9: return "9 " + Lettres_mini_string;
                case 8: return "8 " + Lettres_mini_string;
                case 7: return "7 " + Lettres_mini_string;
                case 6: return "6 " + Lettres_mini_string;
                case 5: return "5 " + Lettres_mini_string;
                case 4: return "4 " + Lettres_mini_string;
                case 0: return Pas_de_mot_string;
                default: return Pas_de_mot_string;
            }
        }
    }
}
