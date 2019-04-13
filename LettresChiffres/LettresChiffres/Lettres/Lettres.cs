using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XMLObject;

namespace LettresChiffres.Lettres
{
    class Lettres : GameScreen
    {

        Texture2D plaque_lettre;
        Texture2D effacer;
        Texture2D valider;
        Texture2D fond_timer;
        Texture2D barre_timer;
        Texture2D plaque_choix;

        SpriteFont Lettre_plaque_font;
        SpriteFont blabla;
        SpriteFont bouton;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        Color opacity_plaque = Color.White * 0.7f;
        Color opacity_chiffre = Color.Black * 0.7f;

        const float screenWidth = 480.0f;
        const float screenHeight = 800.0f;
        const int _plaqueWidth = 90;
        const int _plaqueHeight = 90;

        int _typepartie;
        bool nouvellePartie;
        public bool _continuer = false;
        public string mot_tab_bis;
        public string[] lettre_propose;
        public string lettre_propose_bis;
        bool _erreur4lettres = false;

        // position plaque 
        const int _range1 = 500;
        const int _range2 = 600;
        const int _colonne1 = 15;
        const int _colonne2 = 105;
        const int _colonne3 = 195;
        const int _colonne4 = 285;
        const int _colonne5 = 375;

        // intro
        int _IntroPosition1 = 180;
        int _IntroPosition2 = 300;
        int _IntroPosition3 = 380;
        int _introPosition_X = 140;
        float _timer_lock = 0.0f;
        bool joueur = false;
        bool debut_choix = true;


        //MEssage ecoulé
        public bool _tempsecoule = false;
        Vector2 _positionMessage = new Vector2(50, 600);

        //bouton
        int _bouton_Y = 400;
        int _bouton_X1 = 20;
        int _bouton_X2 = 180;
        int _bouton_X4 = 340;

        //Timer 
        public bool _lancerTimer = false;
        public float _timer = 0;
        Vector2 _positionTimer = new Vector2(30, 370);
        const int _barreTimerWidth = 397;
        public float _timerRectangle = 0;
        public float _tempsdebut = 0f;

        //STRING
        string _IntroString1;
        string _IntroString2;
        string _IntroString3;
        string _tempsecouleString;
        string _boutonValider;
        string _boutonEffacerTOUT1;
        string _boutonEffacerTOUT2;
        string _boutonEffacer;
        string message_IA1;
        string message_IA2;
        string menu;
        string _4lettres;

        List<string> voyelle; 
        List<string> consonne;


        string[] mots_tab = new string[10];
        Random random = new Random();

        Plaques plaque1 = new Plaques(){position = new Vector2(_colonne1, _range1), utiliser = false, disponible = false};
        Plaques plaque2 = new Plaques(){position = new Vector2(_colonne2, _range1), utiliser = false, disponible = false};
        Plaques plaque3 = new Plaques(){position = new Vector2(_colonne3, _range1), utiliser = false, disponible = false};
        Plaques plaque4 = new Plaques(){position = new Vector2(_colonne4, _range1), utiliser = false, disponible = false};
        Plaques plaque5 = new Plaques(){position = new Vector2(_colonne5, _range1), utiliser = false, disponible = false};
        Plaques plaque6 = new Plaques(){position = new Vector2(_colonne1, _range2), utiliser = false, disponible = false};
        Plaques plaque7 = new Plaques(){position = new Vector2(_colonne2, _range2), utiliser = false, disponible = false};
        Plaques plaque8 = new Plaques(){position = new Vector2(_colonne3, _range2), utiliser = false, disponible = false};
        Plaques plaque9 = new Plaques(){position = new Vector2(_colonne4, _range2), utiliser = false, disponible = false};
        Plaques plaque10 = new Plaques() { position = new Vector2(_colonne5, _range2), utiliser = false, disponible = false };

        Difficulte diff;
        Sauvegarde save;
        XML_Serializer xmls = new XML_Serializer();
        Languages lang = new Languages();
        Langues langue = new Langues();

        public Lettres(int typedepartie)
        {
            _typepartie = typedepartie;

            nouvellePartie = true;
        }

        private void InitilizeLanguages()
        {
            message_IA1 = lang.AffectationLANG("Message_IA_Lettre", langue);
            message_IA2 = lang.AffectationLANG("Message_IA_Lettre_bis", langue);
            _boutonValider = lang.AffectationLANG("BOUTON_Valider", langue);
            menu = lang.AffectationLANG("Menu", langue);
            _boutonEffacerTOUT1 = lang.AffectationLANG("BOUTON_EffacerAll1", langue);
            _boutonEffacerTOUT2 = lang.AffectationLANG("BOUTON_EffacerAll2", langue);
            _boutonEffacer = lang.AffectationLANG("BOUTON_Effacer", langue);
            _IntroString1 = lang.AffectationLANG("IntroString1", langue);
            _IntroString2 = lang.AffectationLANG("IntroString2", langue);
            _IntroString3 = lang.AffectationLANG("IntroString3", langue);
            _tempsecouleString = lang.AffectationLANG("Chiffres_tempsEcouleeString", langue);
            _4lettres = lang.AffectationLANG("Quatre_lettres_mini", langue);

            voyelle = lang.LettresTirage(true);
            consonne = lang.LettresTirage(false);
        }

        private void AjoutLettre(int typelettre, Plaques plaque)
        {
            if (typelettre == 1)
            {
                int k = random.Next(0, voyelle.Count);
                plaque.lettre_correspond = voyelle.ElementAt(k);
                voyelle.RemoveAt(k);
                plaque.disponible = true;
            }
            else
            {
                int k = random.Next(0, consonne.Count);
                plaque.lettre_correspond = consonne.ElementAt(k);
                consonne.RemoveAt(k);
                plaque.disponible = true;
            }

        }

        private void VerificationPlaquesVideIntro(int typelettre)
        {
            if (!plaque1.disponible) { AjoutLettre(typelettre, plaque1); }
            else if (!plaque2.disponible) { AjoutLettre(typelettre, plaque2); }
            else if (!plaque3.disponible) { AjoutLettre(typelettre, plaque3); }
            else if (!plaque4.disponible) { AjoutLettre(typelettre, plaque4); }
            else if (!plaque5.disponible) { AjoutLettre(typelettre, plaque5); }
            else if (!plaque6.disponible) { AjoutLettre(typelettre, plaque6); }
            else if (!plaque7.disponible) { AjoutLettre(typelettre, plaque7); }
            else if (!plaque8.disponible) { AjoutLettre(typelettre, plaque8); }
            else if (!plaque9.disponible) { AjoutLettre(typelettre, plaque9); }
            else if (!plaque10.disponible) { AjoutLettre(typelettre, plaque10); nouvellePartie = false; _lancerTimer = true; }
        }

        public override void LoadContent()
        {
            plaque_choix = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_blanc");
            plaque_lettre = ScreenManager.Game.Content.Load<Texture2D>("Lettres/plaque_lettre");
            valider = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/valider");
            effacer = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/effacer");
            fond_timer = ScreenManager.Game.Content.Load<Texture2D>("fond_bar");
            barre_timer = ScreenManager.Game.Content.Load<Texture2D>("progress-bar");

            Lettre_plaque_font = ScreenManager.Game.Content.Load<SpriteFont>("chiffre_font");
            blabla = ScreenManager.Game.Content.Load<SpriteFont>("Transition");
            bouton = ScreenManager.Game.Content.Load<SpriteFont>("bouton");

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();

            if (_typepartie == 4) { diff = ScreenManager.Game.Content.Load<Difficulte>("Difficulte"); save = xmls.DeserializeSauvegarde(); }
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #region UPDATE
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
            }

            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (nouvellePartie) { LockIA_lettres(time); }
            Input();
            if (_lancerTimer && _typepartie == 4) { Time(time); }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public void Time(float time)
        {
            float diff_time = 0.0f;
            int diff_int = save.ListeSave.Difficulte;
            string Diff_string = "";
            switch (diff_int)
            {
                case 1: Diff_string = "Blanc"; break;
                case 2: Diff_string = "Vert"; break;
                case 3: Diff_string = "Bleu"; break;
                case 4: Diff_string = "Jaune"; break;
                case 5: Diff_string = "Orange"; break;
            }
            foreach (var caca in diff.ListeDifficulte)
            {
                if (caca.Difficulte == Diff_string)
                {
                    diff_time = caca.TempsLettres * 1000;
                    break;
                }
            }
            
            _timer += time;

            if (_timer < diff_time)
            {
                _timerRectangle = _barreTimerWidth * _timer / diff_time;
            }
            else if (_timer > diff_time && _timer < diff_time + 2000)
            {
                _tempsecoule = true;
                BlockPlaque();
            }
            else
            {
                Valider();
            }
        }

        private void BlockPlaque()
        {
            plaque1.disponible = false;
            plaque2.disponible = false;
            plaque3.disponible = false;
            plaque4.disponible = false;
            plaque5.disponible = false;
            plaque6.disponible = false;
            plaque7.disponible = false;
            plaque8.disponible = false;
            plaque9.disponible = false;
            plaque10.disponible = false;
        }

        private void LockIA_lettres(float time)
        {
            _timer_lock += time;

            if (debut_choix)
            {
                int caca = random.Next(1, 3);
                if (caca == 1)
                {
                    joueur = true;
                    _timer_lock = 0;
                    int pd = random.Next(1, 3);
                    AjoutLettre(pd, plaque1);
                }
                else
                {
                    joueur = true;
                }
                debut_choix = false;
            }
            else if (_timer_lock > 500f && !joueur)
            {
                int pd = random.Next(1, 3);
                Plaques plaque = PlaqueAjout();
                AjoutLettre(pd, plaque);
                joueur = true;

            }

            if (plaque10.disponible)
            {
                nouvellePartie = false; _lancerTimer = true;
            }
        }

        private Plaques PlaqueAjout()
        {
            if (!plaque1.disponible) { return plaque1; }
            else if (!plaque2.disponible) { return plaque2; }
            else if (!plaque3.disponible) { return plaque3; }
            else if (!plaque4.disponible) { return plaque4; }
            else if (!plaque5.disponible) { return plaque5; }
            else if (!plaque6.disponible) { return plaque6; }
            else if (!plaque7.disponible) { return plaque7; }
            else if (!plaque8.disponible) { return plaque8; }
            else if (!plaque9.disponible) { return plaque9; }
            else return plaque10; 
        }
        #endregion
        #region INPUT
        public void Input()
        {
            state = Mouse.GetState();

            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (state.X > _bouton_X1 && state.X < (_bouton_X1 + effacer.Width) &&
                    state.Y > _bouton_Y && state.Y < (_bouton_Y + effacer.Height))
                {
                    Clear();
                }
                // Clear All
                if (state.X > _bouton_X2 && state.X < (_bouton_X2 + effacer.Width) &&
                    state.Y > _bouton_Y && state.Y < (_bouton_Y + effacer.Height))
                {
                    ClearAll();
                }

                if (state.X > _bouton_X4 && state.X < (_bouton_X4 + valider.Width) &&
                    state.Y > _bouton_Y && state.Y < (_bouton_Y + valider.Height))
                {
                    Valider();
                }
                if (state.X > plaque1.position.X && state.X < (plaque1.position.X + _plaqueWidth) &&
                    state.Y > plaque1.position.Y && state.Y < (plaque1.position.Y + _plaqueHeight)&&
                    plaque1.disponible && !plaque1.utiliser)
                {
                    AjoutLettre(plaque1);
                }
                if (state.X > plaque2.position.X && state.X < (plaque2.position.X + _plaqueWidth) &&
                    state.Y > plaque2.position.Y && state.Y < (plaque2.position.Y + _plaqueHeight)&&
                    plaque2.disponible && !plaque2.utiliser)
                {
                    AjoutLettre(plaque2);
                }
                if (state.X > plaque3.position.X && state.X < (plaque3.position.X + _plaqueWidth) &&
                    state.Y > plaque3.position.Y && state.Y < (plaque3.position.Y + _plaqueHeight)&&
                    plaque3.disponible && !plaque3.utiliser)
                {
                    AjoutLettre(plaque3);
                }
                if (state.X > plaque4.position.X && state.X < (plaque4.position.X + _plaqueWidth) &&
                    state.Y > plaque4.position.Y && state.Y < (plaque4.position.Y + _plaqueHeight)&&
                    plaque4.disponible && !plaque4.utiliser)
                {
                    AjoutLettre(plaque4);
                }
                if (state.X > plaque5.position.X && state.X < (plaque5.position.X + _plaqueWidth) &&
                    state.Y > plaque5.position.Y && state.Y < (plaque5.position.Y + _plaqueHeight)&&
                    plaque5.disponible && !plaque5.utiliser)
                {
                    AjoutLettre(plaque5);
                }
                if (state.X > plaque6.position.X && state.X < (plaque6.position.X + _plaqueWidth) &&
                    state.Y > plaque6.position.Y && state.Y < (plaque6.position.Y + _plaqueHeight)&&
                    plaque6.disponible && !plaque6.utiliser)
                {
                    AjoutLettre(plaque6);
                }
                if (state.X > plaque7.position.X && state.X < (plaque7.position.X + _plaqueWidth) &&
                    state.Y > plaque7.position.Y && state.Y < (plaque7.position.Y + _plaqueHeight)&&
                    plaque7.disponible && !plaque7.utiliser)
                {
                    AjoutLettre(plaque7);
                }
                if (state.X > plaque8.position.X && state.X < (plaque8.position.X + _plaqueWidth) &&
                    state.Y > plaque8.position.Y && state.Y < (plaque8.position.Y + _plaqueHeight)&&
                    plaque8.disponible && !plaque8.utiliser)
                {
                    AjoutLettre(plaque8);
                }
                if (state.X > plaque9.position.X && state.X < (plaque9.position.X + _plaqueWidth) &&
                    state.Y > plaque9.position.Y && state.Y < (plaque9.position.Y + _plaqueHeight)&&
                    plaque9.disponible && !plaque9.utiliser)
                {
                    AjoutLettre(plaque9);
                }
                if (state.X > plaque10.position.X && state.X < (plaque10.position.X + _plaqueWidth) &&
                    state.Y > plaque10.position.Y && state.Y < (plaque10.position.Y + _plaqueHeight)&&
                    plaque10.disponible && !plaque10.utiliser)
                {
                    AjoutLettre(plaque10);
                }
            }

            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released && nouvellePartie)
            {
                if (state.X > _introPosition_X && state.X < _introPosition_X + plaque_choix.Width &&
                    state.Y > _IntroPosition1 && state.Y < _IntroPosition1 + plaque_choix.Height && joueur)
                {
                    joueur = false;
                    _timer_lock = 0.0f;
                    VerificationPlaquesVideIntro(1);
                }
                if (state.X > _introPosition_X && state.X < _introPosition_X + plaque_choix.Width &&
                state.Y > _IntroPosition3 && state.Y < _IntroPosition3 + plaque_choix.Height && joueur)
                {
                    joueur = false;
                    _timer_lock = 0.0f;
                    VerificationPlaquesVideIntro(2);
                }
            }
            previousState = state;
        }

        private void AjoutLettre(Plaques plaque)
        {
            _erreur4lettres = false;
            plaque.utiliser = true;
            if (mots_tab[0] == null) { mots_tab.SetValue(plaque.lettre_correspond, 0); }
            else if (mots_tab[1] == null) { mots_tab.SetValue(plaque.lettre_correspond, 1); }
            else if (mots_tab[2] == null) { mots_tab.SetValue(plaque.lettre_correspond, 2); }
            else if (mots_tab[3] == null) { mots_tab.SetValue(plaque.lettre_correspond, 3); }
            else if (mots_tab[4] == null) { mots_tab.SetValue(plaque.lettre_correspond, 4); }
            else if (mots_tab[5] == null) { mots_tab.SetValue(plaque.lettre_correspond, 5); }
            else if (mots_tab[6] == null) { mots_tab.SetValue(plaque.lettre_correspond, 6); }
            else if (mots_tab[7] == null) { mots_tab.SetValue(plaque.lettre_correspond, 7); }
            else if (mots_tab[8] == null) { mots_tab.SetValue(plaque.lettre_correspond, 8); }
            else if (mots_tab[9] == null) { mots_tab.SetValue(plaque.lettre_correspond, 9); }
        }

        private void Clear()
        {
            if (mots_tab[9] != null) { ClearMethod(mots_tab[9]); mots_tab[9] = null; }
            else if (mots_tab[8] != null) { ClearMethod(mots_tab[8]); mots_tab[8] = null; }
            else if (mots_tab[7] != null) { ClearMethod(mots_tab[7]); mots_tab[7] = null; }
            else if (mots_tab[6] != null) { ClearMethod(mots_tab[6]); mots_tab[6] = null; }
            else if (mots_tab[5] != null) { ClearMethod(mots_tab[5]); mots_tab[5] = null; }
            else if (mots_tab[4] != null) { ClearMethod(mots_tab[4]); mots_tab[4] = null; }
            else if (mots_tab[3] != null) { ClearMethod(mots_tab[3]); mots_tab[3] = null; }
            else if (mots_tab[2] != null) { ClearMethod(mots_tab[2]); mots_tab[2] = null; }
            else if (mots_tab[1] != null) { ClearMethod(mots_tab[1]); mots_tab[1] = null; }
            else if (mots_tab[0] != null) { ClearMethod(mots_tab[0]); mots_tab[0] = null; }
        }

        private void ClearMethod(string lettre)
        {
            if (plaque1.utiliser && plaque1.lettre_correspond == lettre) { plaque1.utiliser = false; }
            else if (plaque2.utiliser && plaque2.lettre_correspond == lettre) { plaque2.utiliser = false; }
            else if (plaque3.utiliser && plaque3.lettre_correspond == lettre) { plaque3.utiliser = false; }
            else if (plaque4.utiliser && plaque4.lettre_correspond == lettre) { plaque4.utiliser = false; }
            else if (plaque5.utiliser && plaque5.lettre_correspond == lettre) { plaque5.utiliser = false; }
            else if (plaque6.utiliser && plaque6.lettre_correspond == lettre) { plaque6.utiliser = false; }
            else if (plaque7.utiliser && plaque7.lettre_correspond == lettre) { plaque7.utiliser = false; }
            else if (plaque8.utiliser && plaque8.lettre_correspond == lettre) { plaque8.utiliser = false; }
            else if (plaque9.utiliser && plaque9.lettre_correspond == lettre) { plaque9.utiliser = false; }
            else if (plaque10.utiliser && plaque10.lettre_correspond == lettre) { plaque10.utiliser = false; }
        }

        private void ClearAll()
        {
            plaque1.utiliser = false;
            plaque2.utiliser = false;
            plaque3.utiliser = false;
            plaque4.utiliser = false;
            plaque5.utiliser = false;
            plaque6.utiliser = false;
            plaque7.utiliser = false;
            plaque8.utiliser = false;
            plaque9.utiliser = false;
            plaque10.utiliser = false;
            mots_tab = new string[10];
        }

        private void Valider()
        {
            mot_tab_bis = ConvertStringArrayToStringJoin(mots_tab);
            if (!_tempsecoule)
            {
                if (mot_tab_bis.Count() < 4)
                {

                    _erreur4lettres = true;
                    ClearAll();
                }
                else
                {
                    lettre_propose = new string[] { plaque1.lettre_correspond, plaque2.lettre_correspond, plaque3.lettre_correspond, plaque4.lettre_correspond, plaque5.lettre_correspond, plaque6.lettre_correspond, plaque7.lettre_correspond, plaque8.lettre_correspond, plaque9.lettre_correspond, plaque10.lettre_correspond };
                    lettre_propose_bis = ConvertStringArrayToStringJoin(lettre_propose);

                    this.ExitScreen();
                    ScreenManager.AddScreen(new Solution_Lettre(lettre_propose_bis, mot_tab_bis, _typepartie));
                }
            }
            else
            {
                lettre_propose = new string[] { plaque1.lettre_correspond, plaque2.lettre_correspond, plaque3.lettre_correspond, plaque4.lettre_correspond, plaque5.lettre_correspond, plaque6.lettre_correspond, plaque7.lettre_correspond, plaque8.lettre_correspond, plaque9.lettre_correspond, plaque10.lettre_correspond };
                lettre_propose_bis = ConvertStringArrayToStringJoin(lettre_propose);

                this.ExitScreen();
                ScreenManager.AddScreen(new Solution_Lettre(lettre_propose_bis, mot_tab_bis, _typepartie));
            }
        }

        #endregion
        #region DRAW
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();


            if (nouvellePartie) { DrawIntro(); }
            else { DrawInterface(); }
            DrawPlaque();
            if (_typepartie == 4 && !nouvellePartie) { DrawTimer(); }
            if (_tempsecoule) { DrawMessage(); }
            if (_erreur4lettres) { DrawMessageErreur4Lettres(); }
            ScreenManager.SpriteBatch.End();
        }

        private void DrawTimer()
        {
            ScreenManager.SpriteBatch.Draw(barre_timer, new Rectangle((int)_positionTimer.X + 2, (int)_positionTimer.Y + 1, (int)_timerRectangle, 18), new Rectangle(0, 0, (int)_timerRectangle, 18), Color.White);

        }

        private void DrawMessage()
        {
            ScreenManager.SpriteBatch.DrawString(blabla, _tempsecouleString,_positionMessage, Color.White);
        }

        private void DrawMessageErreur4Lettres()
        {
            ScreenManager.SpriteBatch.DrawString(blabla, _4lettres,new Vector2(50,300), Color.White);
        }

        private void DrawIntro()
        {
            ScreenManager.SpriteBatch.DrawString(blabla, message_IA1, new Vector2(screenWidth / 2 - blabla.MeasureString(message_IA1).X / 2, 50), Color.White);
            ScreenManager.SpriteBatch.DrawString(blabla, message_IA2, new Vector2(screenWidth / 2 - blabla.MeasureString(message_IA2).X / 2, 80), Color.White);

            ScreenManager.SpriteBatch.Draw(plaque_choix, new Vector2(_introPosition_X, _IntroPosition1), Color.White);
            ScreenManager.SpriteBatch.Draw(plaque_choix, new Vector2(_introPosition_X, _IntroPosition3), Color.White);

            ScreenManager.SpriteBatch.DrawString(blabla, _IntroString1, new Vector2(_introPosition_X + plaque_choix.Width / 2 - blabla.MeasureString(_IntroString1).X / 2, _IntroPosition1 + plaque_choix.Height / 2 - blabla.MeasureString(_IntroString1).Y / 2), Color.Black);
            ScreenManager.SpriteBatch.DrawString(blabla, _IntroString2, new Vector2(screenWidth / 2 - blabla.MeasureString(_IntroString2).X / 2, _IntroPosition2), Color.White);
            ScreenManager.SpriteBatch.DrawString(blabla, _IntroString3, new Vector2(_introPosition_X + plaque_choix.Width / 2 - blabla.MeasureString(_IntroString3).X / 2, _IntroPosition3 + plaque_choix.Height / 2 - blabla.MeasureString(_IntroString3).Y / 2), Color.Black);
        }

        private void DrawPlaque()
        {
            if (plaque1.disponible) { DrawPlaqueMethod(plaque1); }
            if (plaque2.disponible) { DrawPlaqueMethod(plaque2); }
            if (plaque3.disponible) { DrawPlaqueMethod(plaque3); }
            if (plaque4.disponible) { DrawPlaqueMethod(plaque4); }
            if (plaque5.disponible) { DrawPlaqueMethod(plaque5); }
            if (plaque6.disponible) { DrawPlaqueMethod(plaque6); }
            if (plaque7.disponible) { DrawPlaqueMethod(plaque7); }
            if (plaque8.disponible) { DrawPlaqueMethod(plaque8); }
            if (plaque9.disponible) { DrawPlaqueMethod(plaque9); }
            if (plaque10.disponible) { DrawPlaqueMethod(plaque10); }
        }

        private void DrawPlaqueMethod(Plaques plaque)
        {
            if (!plaque.utiliser)
            {
                ScreenManager.SpriteBatch.Draw(plaque_lettre, plaque.position, Color.White);
                ScreenManager.SpriteBatch.DrawString(Lettre_plaque_font, plaque.lettre_correspond, new Vector2(plaque.position.X + _plaqueWidth / 2 - (Lettre_plaque_font.MeasureString(plaque.lettre_correspond).X / 2), plaque.position.Y + _plaqueHeight / 2 - (Lettre_plaque_font.MeasureString(plaque.lettre_correspond).Y / 2)), Color.Black);
            }
            else
            {
                ScreenManager.SpriteBatch.Draw(plaque_lettre, plaque.position, opacity_plaque);
                ScreenManager.SpriteBatch.DrawString(Lettre_plaque_font, plaque.lettre_correspond, new Vector2(plaque.position.X + _plaqueWidth / 2 - (Lettre_plaque_font.MeasureString(plaque.lettre_correspond).X / 2), plaque.position.Y + _plaqueHeight / 2 - (Lettre_plaque_font.MeasureString(plaque.lettre_correspond).Y / 2)), opacity_chiffre);
            }
        }

        private void DrawInterface()
        {
            ScreenManager.SpriteBatch.Draw(effacer, new Vector2(_bouton_X1, _bouton_Y), Color.White);
            ScreenManager.SpriteBatch.Draw(effacer, new Vector2(_bouton_X2, _bouton_Y), Color.White);
            ScreenManager.SpriteBatch.Draw(valider, new Vector2(_bouton_X4, _bouton_Y), Color.White);

            ScreenManager.SpriteBatch.DrawString(bouton, _boutonEffacer, new Vector2(_bouton_X1 + (effacer.Width / 2) - (bouton.MeasureString(_boutonEffacer).X / 2), _bouton_Y + effacer.Height / 2 - (bouton.MeasureString(_boutonEffacer).Y / 2)), Color.White);
            ScreenManager.SpriteBatch.DrawString(bouton, _boutonEffacerTOUT1, new Vector2(_bouton_X2 + (effacer.Width / 2) - (bouton.MeasureString(_boutonEffacerTOUT1).X / 2), _bouton_Y + 5), Color.White);
            ScreenManager.SpriteBatch.DrawString(bouton, _boutonEffacerTOUT2, new Vector2(_bouton_X2 + (effacer.Width / 2) - (bouton.MeasureString(_boutonEffacerTOUT2).X / 2), _bouton_Y + 25), Color.White);
            ScreenManager.SpriteBatch.DrawString(bouton, _boutonValider, new Vector2(_bouton_X4 + (valider.Width / 2) - (bouton.MeasureString(_boutonValider).X / 2), _bouton_Y + valider.Height / 2 - (bouton.MeasureString(_boutonValider).Y / 2)), Color.White);

            string mot = ConvertStringArrayToStringJoin(mots_tab);
            ScreenManager.SpriteBatch.DrawString(Lettre_plaque_font, mot, new Vector2(40, 220), Color.White);

            if (_typepartie == 4) { ScreenManager.SpriteBatch.Draw(fond_timer, _positionTimer, Color.White); }
            
        }

        static string ConvertStringArrayToStringJoin(string[] array)
        {
            //
            // Use string Join to concatenate the string elements.
            //
            string result = string.Join("", array);
            return result;
        }

        #endregion
    }
    public class Plaques
    {
        public Vector2 position;
        public Texture2D texture;
        public bool utiliser;
        public bool disponible;
        public string lettre_correspond;
    }

}
