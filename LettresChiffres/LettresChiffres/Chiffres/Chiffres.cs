using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Advertising.Mobile.Xna;
using System.Threading;
using XMLObject;

namespace LettresChiffres
{
    class Chiffres : GameScreen
    {
        public Texture2D chiffres_carre;
        public Texture2D chiffres_carre_but;
        public Texture2D diviser;
        public Texture2D plus;
        public Texture2D moins;
        public Texture2D multiplier;
        public Texture2D loading;
        public Texture2D suivant;
        public Texture2D effacer;
        public Texture2D valider;
        public Texture2D fond_timer;
        public Texture2D barre_timer;
        

        SpriteFont chiffre_font;
        SpriteFont menufont;
        SpriteFont blabla;
        SpriteFont bouton;
        SpriteFont transition_font;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        Resoudre _resolution = new Resoudre();


        const float plaqueWidth = 100;
        const float plaqueHeight = 70;
        const float screenWidth = 480.0f;
        const float screenHeight = 800.0f;
        Color opacity_plaque = Color.White * 0.7f;
        Color opacity_plaqueChiffre = Color.Black * 0.7f;

        // Position Plaque
        const int _range1 = 550;
        const int _range2 = 640;
        const int _colonne1 = 130;
        const int _colonne2 = 250;
        const int _colonne3 = 370;

        //info blabla
        Vector2 position_info = new Vector2(30,380);
        public bool _infoDivision = false;
        public bool _infoSoustraction = false;
        public bool _infoResolution = false;
        public bool _infoValider = false;
        public bool _tempsecoule = false;

        //STRING
        string _infoValiderString;
        string _infoDivisionString;
        string _resultatNegatif;
        string _tempsecouleString;
        string _boutonValider;
        string _boutonEffacerTOUT1;
        string _boutonEffacerTOUT2;
        string _boutonEffacer;
        string menu;
        string _comptebonString;

        // Operations
        const int _operation1 = 40;
        const int _operation2 = 110;
        const int _operation3 = 180;
        const int _operation4 = 250;
        const int _operation5 = 320;

        const int _nombre1 = 60;
        const int _action = 120;
        const int _nombre2 = 200;
        const int _egal = 250;
        const int _resultat = 330;

        // plaque action
        const int _plaqueAction_X = 420;
        const int _plaquePlus = 100;
        const int _plaqueMoins = 190;
        const int _plaqueMultiplie = 280;
        const int _plaqueDivise = 370;
        const int _plaqueActionWidth = 50;
        const int _plaqueActionHeight = 50;

        // DIVERS
        public int _nombre_a_trouver;
        public bool nouvellePartie;
        public bool _finir = false;
        public bool _comptebon = false;
        public int _typepartie;
        public bool _continuer = false;
        public int resultatutilisateur = 0;
        bool afficher_nombre_but = false;
        int nombre_aleatoire_intro = 000;

        // Bouton
        int _boutonWidth = 100;
        int _boutonHeight = 62;
        int _bouton_Y = 470;
        int _bouton_X1 = 30;
        int _bouton_X2 = 180;
        int _bouton_X4 = 340;

        //Timer 
        public bool _lancerTimer = false;
        public float _timer = 0;
        Vector2 _positionTimer = new Vector2(30, 430);
        const int _barreTimerWidth = 397;
        public float _timerRectangle = 0;
        public float _tempsdebut = 0f;

        float _timer_message = 0.0f;

        //List nombre
        public List<int> nombreDisponible = new List<int>(){0,0,0,0,0,0};

        // [0] premier nombre [1] + - * / [2] deuxieme nombre
        // [3] = [4] resultat [5] si resultat utilisé
        public int[] Operation1 = new int[6] { 0, 0, 0, 0, 0, 0 };
        public int[] Operation2 = new int[6] { 0, 0, 0, 0, 0, 0 };
        public int[] Operation3 = new int[6] { 0, 0, 0, 0, 0, 0 };
        public int[] Operation4 = new int[6] { 0, 0, 0, 0, 0, 0 };
        public int[] Operation5 = new int[6] { 0, 0, 0, 0, 0, 0 };


            Plaques plaque1 = new Plaques() {position = new Vector2(_colonne1, _range1), utiliser = false , disponible = false};
            Plaques plaque2 = new Plaques() { position = new Vector2(_colonne2, _range1), utiliser = false, disponible = false };
            Plaques plaque3 = new Plaques() { position = new Vector2(_colonne3, _range1), utiliser = false, disponible = false };
            Plaques plaque4 = new Plaques() { position = new Vector2(_colonne1, _range2), utiliser = false, disponible = false };
            Plaques plaque5 = new Plaques() { position = new Vector2(_colonne2, _range2), utiliser = false, disponible = false };
            Plaques plaque6 = new Plaques() { position = new Vector2(_colonne3, _range2), utiliser = false, disponible = false };

            Difficulte diff;
            Sauvegarde save;
            XML_Serializer xmls = new XML_Serializer();
            Languages lang = new Languages();
            Langues langue = new Langues();
            Random random = new Random();

           

        public Chiffres(int typedepartie)
        {
            _typepartie = typedepartie;
            nouvellePartie = true;
            
        }

        private void InitilizeLanguages()
        {
            _infoValiderString = lang.AffectationLANG("Chiffres_infoValiderString", langue);
            _infoDivisionString = lang.AffectationLANG("Chiffres_infoDivisionString", langue);
            _resultatNegatif = lang.AffectationLANG("Chiffres_resultatNegatif", langue);
            _tempsecouleString = lang.AffectationLANG("Chiffres_tempsEcouleeString", langue);
            _boutonValider = lang.AffectationLANG("BOUTON_Valider", langue);
            _boutonEffacerTOUT1 = lang.AffectationLANG("BOUTON_EffacerAll1", langue);
            _boutonEffacerTOUT2 = lang.AffectationLANG("BOUTON_EffacerAll2", langue);
            _boutonEffacer = lang.AffectationLANG("BOUTON_Effacer", langue);
            menu = lang.AffectationLANG("Menu", langue);
            _comptebonString = lang.AffectationLANG("Compte_bon", langue);
        }


        // Random de 100 a 999 pour le nombre a trouver 
        public int nombreATrouver()
        {
             _nombre_a_trouver = new Random().Next(101,1000);
             return _nombre_a_trouver;
        }

        // Donne les 6 nombres disponibles pour le calcul
        public void listeNombre()
        {
            List<int> nombre = new List<int>(){1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 25, 50, 75, 100};
            for (int i = 0; i < 6; i++)
            {
                int h = nombre.Count;
                int j = random.Next(0, h);
                int k = nombre.ElementAt(j);
                nombreDisponible.Insert(i, k);
                nombre.RemoveAt(j);
            }
            plaque1.nombre_correspond = nombreDisponible[0];
            plaque2.nombre_correspond = nombreDisponible[1];
            plaque3.nombre_correspond = nombreDisponible[2];
            plaque4.nombre_correspond = nombreDisponible[3];
            plaque5.nombre_correspond = nombreDisponible[4];
            plaque6.nombre_correspond = nombreDisponible[5];
        }

        public void NouvellePartie()
        {
            nombreATrouver();
            listeNombre();
            _timer = 0.0f;
            ScreenManager.Game.ResetElapsedTime();
            nouvellePartie = false;
        }

        public override void LoadContent()
        {
            chiffres_carre = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/chiffres_carre");
            chiffres_carre_but = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/chiffres_carre_but");
            diviser = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/diviser");
            multiplier = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/multiplier");
            plus = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/plus");
            moins = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/moins");
            chiffre_font = ScreenManager.Game.Content.Load<SpriteFont>("chiffre_font");
            menufont = ScreenManager.Game.Content.Load<SpriteFont>("menufont");
            blabla = ScreenManager.Game.Content.Load<SpriteFont>("blabla");
            bouton = ScreenManager.Game.Content.Load<SpriteFont>("bouton");
            loading = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/loading");
            suivant = ScreenManager.Game.Content.Load<Texture2D>("Transition/icone_suivant");
            transition_font = ScreenManager.Game.Content.Load<SpriteFont>("Transition");
            valider = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/valider");
            effacer = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/effacer");
            fond_timer = ScreenManager.Game.Content.Load<Texture2D>("fond_bar");
            barre_timer = ScreenManager.Game.Content.Load<Texture2D>("progress-bar");
            if (_typepartie == 3) { diff = ScreenManager.Game.Content.Load<Difficulte>("Difficulte"); save = xmls.DeserializeSauvegarde(); }

            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }


        #region UPDATE

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            _tempsdebut += time;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
            }

            if (nouvellePartie == true)
            {
                NouvellePartie();
            }
            AffichagePlaque();
            if (_lancerTimer && _typepartie == 3) { Time(time); }
            if (_comptebon) { CompteBonTimer(time); }
            Input();
            Calculer();

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
            else if (_timer > diff_time && _timer < diff_time +2000)
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
        }

        private void AffichagePlaque()
        {

            if (_tempsdebut > 500f) { plaque1.disponible = true; }
            if (_tempsdebut > 1000f) { plaque2.disponible = true; }
            if (_tempsdebut > 1500f) { plaque3.disponible = true; }
            if (_tempsdebut > 2000f) { plaque4.disponible = true; }
            if (_tempsdebut > 2500f) { plaque5.disponible = true; }
            if (_tempsdebut > 3000f) { plaque6.disponible = true; }
            if (_tempsdebut > 3000f && _tempsdebut < 4000f) { NombreAleatoireIntro(); }
            if (_tempsdebut > 4000f) { afficher_nombre_but = true; }
            if (_tempsdebut > 4000f && _typepartie == 3) { afficher_nombre_but = true; _lancerTimer = true; }

        }

        private void NombreAleatoireIntro()
        {
            nombre_aleatoire_intro = random.Next(101, 1000);
        }

        private void Valider()
        {
            if (Operation5[4] != 0) { resultatutilisateur = Operation5[4]; }
            else if (Operation4[4] != 0) { resultatutilisateur = Operation4[4]; }
            else if (Operation3[4] != 0) { resultatutilisateur = Operation3[4]; }
            else if (Operation2[4] != 0) { resultatutilisateur = Operation2[4]; }
            else if (Operation1[4] != 0) { resultatutilisateur = Operation1[4]; }
            if (_typepartie == 1)
            {
                this.ExitScreen();
                if (_nombre_a_trouver == resultatutilisateur)
                {
                    ScreenManager.AddScreen(new Transition(1, _nombre_a_trouver, resultatutilisateur));
                }
                else
                {
                    ScreenManager.AddScreen(new Solution_Chiffres(1, _nombre_a_trouver, resultatutilisateur, nombreDisponible));
                }
            }
            else
            {
                this.ExitScreen();

                ScreenManager.AddScreen(new Solution_Chiffres(3, _nombre_a_trouver, resultatutilisateur, nombreDisponible));

            }
        }

        void Clear()
        {
            if (!_tempsecoule)
            {
                if (Operation5[2] != 0)
                {

                    if (Operation5[2] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation5[2] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation5[2] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation5[2] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation5[2] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation5[2] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation5[2] == Operation4[4]) { Operation4.SetValue(0, 5); }
                    else if (Operation5[2] == Operation3[4]) { Operation3.SetValue(0, 5); }
                    else if (Operation5[2] == Operation2[4]) { Operation2.SetValue(0, 5); }
                    else if (Operation5[2] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation5[2] = 0;
                    EffacerResultat(Operation5);
                }
                else if (Operation5[1] != 0) { Operation5.SetValue(0, 1); }
                else if (Operation5[0] != 0)
                {

                    if (Operation5[0] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation5[0] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation5[0] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation5[0] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation5[0] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation5[0] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation5[0] == Operation4[4]) { Operation4.SetValue(0, 5); }
                    else if (Operation5[0] == Operation3[4]) { Operation3.SetValue(0, 5); }
                    else if (Operation5[0] == Operation2[4]) { Operation2.SetValue(0, 5); }
                    else if (Operation5[0] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation5[0] = 0;
                }
                else if (Operation4[2] != 0)
                {

                    if (Operation4[2] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation4[2] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation4[2] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation4[2] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation4[2] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation4[2] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation4[2] == Operation3[4]) { Operation3.SetValue(0, 5); }
                    else if (Operation4[2] == Operation2[4]) { Operation2.SetValue(0, 5); }
                    else if (Operation4[2] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation4[2] = 0;
                    EffacerResultat(Operation4);
                }
                else if (Operation4[1] != 0) { Operation4.SetValue(0, 1); }
                else if (Operation4[0] != 0)
                {
                    if (Operation4[0] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation4[0] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation4[0] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation4[0] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation4[0] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation4[0] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation4[0] == Operation3[4]) { Operation3.SetValue(0, 5); }
                    else if (Operation4[0] == Operation2[4]) { Operation2.SetValue(0, 5); }
                    else if (Operation4[0] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation4[0] = 0;
                }
                else if (Operation3[2] != 0)
                {

                    if (Operation3[2] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation3[2] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation3[2] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation3[2] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation3[2] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation3[2] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation3[2] == Operation2[4]) { Operation2.SetValue(0, 5); }
                    else if (Operation3[2] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation3[2] = 0;
                    EffacerResultat(Operation3);
                }
                else if (Operation3[1] != 0) { Operation3.SetValue(0, 1); }
                else if (Operation3[0] != 0)
                {
                    if (Operation3[0] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation3[0] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation3[0] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation3[0] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation3[0] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation3[0] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation3[0] == Operation2[4]) { Operation2.SetValue(0, 5); }
                    else if (Operation3[0] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation3[0] = 0;
                }
                else if (Operation2[2] != 0)
                {
                    if (Operation2[2] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation2[2] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation2[2] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation2[2] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation2[2] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation2[2] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation2[2] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation2[2] = 0;
                    EffacerResultat(Operation2);
                }
                else if (Operation2[1] != 0) { Operation2.SetValue(0, 1); }
                else if (Operation2[0] != 0)
                {
                    if (Operation2[0] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation2[0] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation2[0] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation2[0] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation2[0] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation2[0] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    else if (Operation2[0] == Operation1[4]) { Operation1.SetValue(0, 5); }
                    Operation2[0] = 0;
                }
                else if (Operation1[2] != 0)
                {
                    if (Operation1[2] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation1[2] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation1[2] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation1[2] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation1[2] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation1[2] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    Operation1[2] = 0;
                    EffacerResultat(Operation1);
                }
                else if (Operation1[1] != 0) { Operation1.SetValue(0, 1); }
                else if (Operation1[0] != 0)
                {
                    if (Operation1[0] == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                    else if (Operation1[0] == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                    else if (Operation1[0] == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                    else if (Operation1[0] == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                    else if (Operation1[0] == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                    else if (Operation1[0] == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                    Operation1[0] = 0;
                }
            }

        }

        private void EffacerResultat(int[] operation)
        {
            operation[3] = 0;
            operation[4] = 0;
            operation[5] = 0;
        }


        void ClearAll()
        {
            if (!_tempsecoule)
            {
                if (Operation1.Equals(new List<int>() { 0, 0, 0, 0, 0, 0 }) == false)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Operation1.SetValue(0, i);
                    }
                }
                if (Operation2.Equals(new List<int>() { 0, 0, 0, 0, 0, 0 }) == false)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Operation2.SetValue(0, i);
                    }
                }
                if (Operation3.Equals(new List<int>() { 0, 0, 0, 0, 0, 0 }) == false)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Operation3.SetValue(0, i);
                    }
                }
                if (Operation4.Equals(new List<int>() { 0, 0, 0, 0, 0, 0 }) == false)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Operation4.SetValue(0, i);
                    }
                }
                if (Operation5.Equals(new List<int>() { 0, 0, 0, 0, 0, 0 }) == false)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Operation5.SetValue(0, i);
                    }
                }
                plaque1.utiliser = false;
                plaque2.utiliser = false;
                plaque3.utiliser = false;
                plaque4.utiliser = false;
                plaque5.utiliser = false;
                plaque6.utiliser = false;
            }
        }

        public void Calculer()
        {
            if (Operation1[0] != 0 && Operation1[1] != 0 && Operation1[2] != 0)
            {
                Operation1[3] = 1;
                if (Operation1[1] == 1) { Operation1[4] = Operation1[0] + Operation1[2]; }
                else if (Operation1[1] == 2) { Soustraction(Operation1); }
                else if (Operation1[1] == 3) { Operation1[4] = Operation1[0] * Operation1[2]; }
                else if (Operation1[1] == 4) { Division(Operation1); }
                Comptebon(Operation1);

            }
            if (Operation2[0] != 0 && Operation2[1] != 0 && Operation2[2] != 0)
            {
                Operation2[3] = 1;
                if (Operation2[1] == 1) { Operation2[4] = Operation2[0] + Operation2[2]; }
                else if (Operation2[1] == 2) { Soustraction(Operation2); }
                else if (Operation2[1] == 3) { Operation2[4] = Operation2[0] * Operation2[2]; }
                else if (Operation2[1] == 4) { Division(Operation2); }
                Comptebon(Operation2);
            }
            if (Operation3[0] != 0 && Operation3[1] != 0 && Operation3[2] != 0)
            {
                Operation3[3] = 1;
                if (Operation3[1] == 1) { Operation3[4] = Operation3[0] + Operation3[2]; }
                else if (Operation3[1] == 2) { Soustraction(Operation3); }
                else if (Operation3[1] == 3) { Operation3[4] = Operation3[0] * Operation3[2]; }
                else if (Operation3[1] == 4) { Division(Operation3); }
                Comptebon(Operation3);

            }
            if (Operation4[0] != 0 && Operation4[1] != 0 && Operation4[2] != 0)
            {
                Operation4[3] = 1;
                if (Operation4[1] == 1) { Operation4[4] = Operation4[0] + Operation4[2]; }
                else if (Operation4[1] == 2) { Soustraction(Operation4); }
                else if (Operation4[1] == 3) { Operation4[4] = Operation4[0] * Operation4[2]; }
                else if (Operation4[1] == 4) { Division(Operation4); }
                Comptebon(Operation4);
            }
            if (Operation5[0] != 0 && Operation5[1] != 0 && Operation5[2] != 0)
            {
                Operation5[3] = 1;
                if (Operation5[1] == 1) { Operation5[4] = Operation5[0] + Operation5[2]; }
                else if (Operation5[1] == 2) { Soustraction(Operation5); }
                else if (Operation5[1] == 3) { Operation5[4] = Operation5[0] * Operation5[2]; }
                else if (Operation5[1] == 4) { Division(Operation5); }
                Comptebon(Operation5);
            }
        }

        private void Comptebon(int[] operation)
        {
            if( operation[4] == _nombre_a_trouver)
            {
                _comptebon = true;
            }
        }

        private void CompteBonTimer(float time)
        {
            _timer_message += time;
            BlockPlaque();
            if(_timer_message > 2000)
            {
                Valider();
            }
        }

        private void Soustraction(int[] operation)
        {
            int k = operation[0] - operation[2];
            if (k > 0)
            {
                operation[4] = operation[0] - operation[2];
            }
            else
            {
                int nombre1 = operation[0];
                int nombre2 = operation[2];
                
                operation.SetValue(0, 0);
                operation.SetValue(0, 1);
                operation.SetValue(0, 2);
                operation.SetValue(0, 3);
                _infoSoustraction = true;
                EffacerOperation(nombre1, nombre2);
            }
        }

        private void Division(int[] operation)
        {
            int k = operation[0] % operation[2];
            if (k == 0)
            {
                operation[4] = operation[0] / operation[2];
            }
            else
            {
                int nombre1 = operation[0];
                int nombre2 = operation[2];
                
                operation.SetValue(0, 0);
                operation.SetValue(0, 1);
                operation.SetValue(0, 2);
                operation.SetValue(0, 3);
                _infoDivision = true;
                EffacerOperation(nombre1, nombre2);
            }
        }

        private void EffacerOperation(int nombre1, int nombre2)
        {
            if (nombre2 != 0)
            {
                if (nombre2 == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                else if (nombre2 == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                else if (nombre2 == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                else if (nombre2 == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                else if (nombre2 == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                else if (nombre2 == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                else if (nombre2 == Operation4[4]) { Operation4.SetValue(0, 5); }
                else if (nombre2 == Operation3[4]) { Operation3.SetValue(0, 5); }
                else if (nombre2 == Operation2[4]) { Operation2.SetValue(0, 5); }
                else if (nombre2 == Operation1[4]) { Operation1.SetValue(0, 5); }
            }
            if (nombre1 != 0)
            {
                if (nombre1 == plaque1.nombre_correspond && plaque1.utiliser == true) { plaque1.utiliser = false; }
                else if (nombre1 == plaque2.nombre_correspond && plaque2.utiliser == true) { plaque2.utiliser = false; }
                else if (nombre1 == plaque3.nombre_correspond && plaque3.utiliser == true) { plaque3.utiliser = false; }
                else if (nombre1 == plaque4.nombre_correspond && plaque4.utiliser == true) { plaque4.utiliser = false; }
                else if (nombre1 == plaque5.nombre_correspond && plaque5.utiliser == true) { plaque5.utiliser = false; }
                else if (nombre1 == plaque6.nombre_correspond && plaque6.utiliser == true) { plaque6.utiliser = false; }
                else if (nombre1 == Operation4[4]) { Operation4.SetValue(0, 5); }
                else if (nombre1 == Operation3[4]) { Operation3.SetValue(0, 5); }
                else if (nombre1 == Operation2[4]) { Operation2.SetValue(0, 5); }
                else if (nombre1 == Operation1[4]) { Operation1.SetValue(0, 5); }
            }
        }

#endregion

        #region Input
        public void Input()
        {
           state = Mouse.GetState();

           if (previousState.LeftButton == ButtonState.Pressed &&
               state.LeftButton == ButtonState.Released)
            {
                // plaques nombres
                if (state.X > plaque1.position.X && state.X < (plaque1.position.X + plaqueWidth) &&
                    state.Y > plaque1.position.Y && state.Y < (plaque1.position.Y + plaqueHeight) &&
                    plaque1.disponible == true && plaque1.utiliser == false)
                {
                    AjoutNombreOperation(plaque1);
                }
                if (state.X > plaque2.position.X && state.X < (plaque2.position.X + plaqueWidth) &&
                    state.Y > plaque2.position.Y && state.Y < (plaque2.position.Y + plaqueHeight) &&
                    plaque2.disponible == true && plaque2.utiliser == false)
                {
                    AjoutNombreOperation(plaque2);
                }
                if (state.X > plaque3.position.X && state.X < (plaque3.position.X + plaqueWidth) &&
                    state.Y > plaque3.position.Y && state.Y < (plaque3.position.Y + plaqueHeight) &&
                    plaque3.disponible == true && plaque3.utiliser == false)
                {
                    AjoutNombreOperation(plaque3);
                }
                if (state.X > plaque4.position.X && state.X < (plaque4.position.X + plaqueWidth) &&
                    state.Y > plaque4.position.Y && state.Y < (plaque4.position.Y + plaqueHeight) &&
                    plaque4.disponible == true && plaque4.utiliser == false)
                {
                    AjoutNombreOperation(plaque4);
                }
                if (state.X > plaque5.position.X && state.X < (plaque5.position.X + plaqueWidth) &&
                    state.Y > plaque5.position.Y && state.Y < (plaque5.position.Y + plaqueHeight) &&
                    plaque5.disponible == true && plaque5.utiliser == false)
                {
                    AjoutNombreOperation(plaque5);
                }
                if (state.X > plaque6.position.X && state.X < (plaque6.position.X + plaqueWidth) &&
                    state.Y > plaque6.position.Y && state.Y < (plaque6.position.Y + plaqueHeight) &&
                    plaque6.disponible == true && plaque6.utiliser == false)
                {
                    AjoutNombreOperation(plaque6);
                }
                // Plaque opérations
                if (state.X > _plaqueAction_X && state.X < (_plaqueAction_X + _plaqueActionWidth) &&
                    state.Y > _plaquePlus && state.Y < (_plaquePlus + _plaqueActionHeight))
                    {
                        VerificationAction(1);
                    }
                if (state.X > _plaqueAction_X && state.X < (_plaqueAction_X + _plaqueActionWidth) &&
                    state.Y > _plaqueMoins && state.Y < (_plaqueMoins + _plaqueActionHeight))
                    {
                        VerificationAction(2);
                    }
                if (state.X > _plaqueAction_X && state.X < (_plaqueAction_X + _plaqueActionWidth) &&
                    state.Y > _plaqueMultiplie && state.Y < (_plaqueMultiplie + _plaqueActionHeight))
                    {
                        VerificationAction(3);
                    }
                if (state.X > _plaqueAction_X && state.X < (_plaqueAction_X + _plaqueActionWidth) &&
                    state.Y > _plaqueDivise && state.Y < (_plaqueDivise + _plaqueActionHeight))
                    {
                        VerificationAction(4);
                    }
                // Résultats Opérations
                if (state.X > _resultat - (chiffre_font.MeasureString(Operation1[4].ToString()).X/2)  && state.X < (_resultat + chiffre_font.MeasureString(Operation1[4].ToString()).X) &&
                    state.Y > _operation1 && state.Y < (_operation1 + chiffre_font.MeasureString(Operation1[4].ToString()).Y) &&
                    Operation1[5] == 0)
                {
                    if( Operation2[0] == 0){Operation2.SetValue(Operation1[4],0); Operation1.SetValue(1,5);}
                    else if( Operation2[2] == 0){Operation2.SetValue(Operation1[4],2); Operation1.SetValue(1,5);}
                    else if( Operation3[0] == 0){Operation3.SetValue(Operation1[4],0); Operation1.SetValue(1,5);}
                    else if( Operation3[2] == 0){Operation3.SetValue(Operation1[4],2); Operation1.SetValue(1,5);}
                    else if( Operation4[0] == 0){Operation4.SetValue(Operation1[4],0); Operation1.SetValue(1,5);}
                    else if( Operation4[2] == 0){Operation4.SetValue(Operation1[4],2); Operation1.SetValue(1,5);}
                    else if( Operation5[0] == 0){Operation5.SetValue(Operation1[4],0); Operation1.SetValue(1,5);}
                    else if( Operation5[2] == 0){Operation5.SetValue(Operation1[4],2); Operation1.SetValue(1,5);}
                }

                if (state.X > _resultat - (chiffre_font.MeasureString(Operation2[4].ToString()).X / 2) && state.X < (_resultat + chiffre_font.MeasureString(Operation2[4].ToString()).X) &&
                    state.Y > _operation2 && state.Y < (_operation2 + chiffre_font.MeasureString(Operation2[4].ToString()).Y) &&
                    Operation2[5] == 0)
                {
                    if( Operation3[0] == 0){Operation3.SetValue(Operation2[4],0); Operation2.SetValue(1,5);}
                    else if( Operation3[2] == 0){Operation3.SetValue(Operation2[4],2); Operation2.SetValue(1,5);}
                    else if( Operation4[0] == 0){Operation4.SetValue(Operation2[4],0); Operation2.SetValue(1,5);}
                    else if( Operation4[2] == 0){Operation4.SetValue(Operation2[4],2); Operation2.SetValue(1,5);}
                    else if( Operation5[0] == 0){Operation5.SetValue(Operation2[4],0); Operation2.SetValue(1,5);}
                    else if( Operation5[2] == 0){Operation5.SetValue(Operation2[4],2); Operation2.SetValue(1,5);}
                }

                if (state.X > _resultat - (chiffre_font.MeasureString(Operation3[4].ToString()).X / 2) && state.X < (_resultat + chiffre_font.MeasureString(Operation3[4].ToString()).X) &&
                    state.Y > _operation3 && state.Y < (_operation3 + chiffre_font.MeasureString(Operation3[4].ToString()).Y) &&
                    Operation3[5] == 0)
                {
                    if( Operation4[0] == 0){Operation4.SetValue(Operation3[4],0); Operation3.SetValue(1,5);}
                    else if( Operation4[2] == 0){Operation4.SetValue(Operation3[4],2); Operation3.SetValue(1,5);}
                    else if( Operation5[0] == 0){Operation5.SetValue(Operation3[4],0); Operation3.SetValue(1,5);}
                    else if( Operation5[2] == 0){Operation5.SetValue(Operation3[4],2); Operation3.SetValue(1,5);}
                }

                if (state.X > _resultat - (chiffre_font.MeasureString(Operation4[4].ToString()).X / 2) && state.X < (_resultat + chiffre_font.MeasureString(Operation4[4].ToString()).X) &&
                    state.Y > _operation4 && state.Y < (_operation4 + chiffre_font.MeasureString(Operation4[4].ToString()).Y) &&
                    Operation4[5] == 0)
                {
                    if( Operation5[0] == 0){Operation5.SetValue(Operation4[4],0); Operation4.SetValue(1,5);}
                    else if( Operation5[2] == 0){Operation5.SetValue(Operation4[4],2); Operation4.SetValue(1,5);}
                }
      
            }

            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                // Clear
                if (state.X > _bouton_X1 && state.X < (_bouton_X1 + _boutonWidth) &&
                    state.Y > _bouton_Y && state.Y < (_bouton_Y + _boutonHeight))
                {
                    Clear();
                }
                // Clear All
                if (state.X > _bouton_X2 && state.X < (_bouton_X2 + _boutonWidth) &&
                    state.Y > _bouton_Y && state.Y < (_bouton_Y + _boutonHeight))
                {
                    ClearAll();
                }

                if (state.X > _bouton_X4 && state.X < (_bouton_X4 + _boutonWidth) &&
                    state.Y > _bouton_Y && state.Y < (_bouton_Y + _boutonHeight))
                {
                    Valider();
                }

            }
                previousState = state;
        }


        private void VerificationAction(int identificateur)
        {
            if (Operation1[1] == 0 && Operation1[0] != 0)
            {
                switch ( identificateur)
                {
                    case 1: { Operation1.SetValue(1, 1); }; break;
                    case 2: { Operation1.SetValue(2, 1); }; break;
                    case 3: { Operation1.SetValue(3, 1); }; break;
                    case 4: { Operation1.SetValue(4, 1); }; break;
                }
            }
            else if (Operation2[1] == 0 && Operation2[0] != 0)
            {
                switch ( identificateur)
                {
                    case 1: { Operation2.SetValue(1, 1); }; break;
                    case 2: { Operation2.SetValue(2, 1); }; break;
                    case 3: { Operation2.SetValue(3, 1); }; break;
                    case 4: { Operation2.SetValue(4, 1); }; break;
                }
            }
            else if (Operation3[1] == 0 && Operation3[0] != 0)
            {
                switch ( identificateur)
                {
                    case 1: { Operation3.SetValue(1, 1); }; break;
                    case 2: { Operation3.SetValue(2, 1); }; break;
                    case 3: { Operation3.SetValue(3, 1); }; break;
                    case 4: { Operation3.SetValue(4, 1); }; break;
                }
            }
            else if (Operation4[1] == 0 && Operation4[0] != 0)
            {
                switch ( identificateur)
                {
                    case 1: { Operation4.SetValue(1, 1); }; break;
                    case 2: { Operation4.SetValue(2, 1); }; break;
                    case 3: { Operation4.SetValue(3, 1); }; break;
                    case 4: { Operation4.SetValue(4, 1); }; break;
                }
            }
            else if (Operation5[1] == 0 && Operation5[0] != 0)
            {
                switch ( identificateur)
                {
                    case 1: { Operation5.SetValue(1, 1); }; break;
                    case 2: { Operation5.SetValue(2, 1); }; break;
                    case 3: { Operation5.SetValue(3, 1); }; break;
                    case 4: { Operation5.SetValue(4, 1); }; break;
                }
            }
        }

        private void AjoutNombreOperation(Plaques _plaque)
        {
            Plaques plaque = _plaque;
            // operation 1
            if (Operation1[0] == 0)
            {
                Operation1.SetValue(plaque.nombre_correspond, 0);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
            else if (Operation1[2] == 0 && Operation1[1] != 0)
            {
                Operation1.SetValue(plaque.nombre_correspond, 2);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
                // operation 2
            else if (Operation2[0] == 0 && Operation1[4] != 0)
            {
                Operation2.SetValue(plaque.nombre_correspond, 0);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
            else if (Operation2[2] == 0 && Operation2[1] != 0)
            {
                Operation2.SetValue(plaque.nombre_correspond, 2);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
                // operation 3
            else if (Operation3[0] == 0 && Operation2[4] != 0)
            {
                Operation3.SetValue(plaque.nombre_correspond, 0);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
            else if (Operation3[2] == 0 && Operation3[1] != 0)
            {
                Operation3.SetValue(plaque.nombre_correspond, 2);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
            //operation 4 
            else if (Operation4[0] == 0 && Operation3[4] != 0)
            {
                Operation4.SetValue(plaque.nombre_correspond, 0);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
            else if (Operation4[2] == 0 && Operation4[1] != 0)
            {
                Operation4.SetValue(plaque.nombre_correspond, 2);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
            //operation 5 
            else if (Operation5[0] == 0 && Operation4[4] != 0)
            {
                Operation5.SetValue(plaque.nombre_correspond, 0);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
            else if (Operation5[2] == 0 && Operation5[1] != 0)
            {
                Operation5.SetValue(plaque.nombre_correspond, 2);
                plaque.utiliser = true;
                _infoDivision = false;
                _infoSoustraction = false;
            }
        }

        private void VerificationOperation(Plaques _plaque)
        {
            Plaques plaque = _plaque;
            switch (VerificationAjout(Operation1))
            {
                case 1:
                    {
                        Operation1.SetValue(plaque.nombre_correspond,0);
                        plaque.utiliser = true;
                    };break;
                case 2:
                    {
                        Operation1.SetValue(plaque.nombre_correspond, 2);
                        plaque.utiliser = true;
                    };break;
                case 3:
                    {
                        switch (VerificationAjout(Operation2))
                        {
                            case 1:
                                {
                                    Operation2.SetValue(plaque.nombre_correspond, 0);
                                    plaque.utiliser = true;
                                }; break;
                            case 2:
                                {
                                    Operation2.SetValue(plaque.nombre_correspond, 2);
                                    plaque.utiliser = true;
                                }; break;
                            case 3:
                                {
                                    switch (VerificationAjout(Operation3))
                                    {
                                        case 1:
                                            {
                                                Operation3.SetValue(plaque.nombre_correspond, 0);
                                                plaque.utiliser = true;
                                            }; break;
                                        case 2:
                                            {
                                                Operation3.SetValue(plaque.nombre_correspond, 2);
                                                plaque.utiliser = true;
                                            }; break;
                                        case 3:
                                            {
                                                switch (VerificationAjout(Operation4))
                                                {
                                                    case 1:
                                                        {
                                                            Operation4.SetValue(plaque.nombre_correspond, 0);
                                                            plaque.utiliser = true;
                                                        }; break;
                                                    case 2:
                                                        {
                                                            Operation4.SetValue(plaque.nombre_correspond, 2);
                                                            plaque.utiliser = true;
                                                        }; break;
                                                    case 3:
                                                        {
                                                            switch (VerificationAjout(Operation5))
                                                            {
                                                                case 1:
                                                                    {
                                                                        Operation5.SetValue(plaque.nombre_correspond, 0);
                                                                        plaque.utiliser = true;
                                                                    }; break;
                                                                case 2:
                                                                    {
                                                                        Operation5.SetValue(plaque.nombre_correspond, 2);
                                                                        plaque.utiliser = true;
                                                                    }; break;
                                                            }
                                                        };break;
                                                }
                                            };break;
                                    }
                                }break;
                        }
                    }break;
            }
        }

        private int VerificationAjout(int[] array)
        {
            if (array[0] == 0)
            {
                return 1;
            }
            else if (array[2] == 0)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        #endregion

        #region DRAW
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            ScreenManager.SpriteBatch.Begin();
            DrawInterface();
            DrawPlaque();
            DrawChiffres();
            DrawInformation();

            if (_typepartie == 3) { DrawTimer(); }
            DrawOperation();

            ScreenManager.SpriteBatch.End();
        }
        
        private void DrawTimer()
        {
            ScreenManager.SpriteBatch.Draw(barre_timer, new Rectangle((int)_positionTimer.X + 2, (int)_positionTimer.Y + 1, (int)_timerRectangle, 18), new Rectangle(0, 0, (int)_timerRectangle, 18), Color.White);
            
        }

        private void DrawInterface()
        {
            ScreenManager.SpriteBatch.Draw(effacer, new Vector2(_bouton_X1, _bouton_Y), Color.White);
            ScreenManager.SpriteBatch.Draw(effacer, new Vector2(_bouton_X2, _bouton_Y), Color.White);
            ScreenManager.SpriteBatch.Draw(valider, new Vector2(_bouton_X4, _bouton_Y), Color.White);

            ScreenManager.SpriteBatch.DrawString(bouton, _boutonEffacer, new Vector2(_bouton_X1 + (effacer.Width / 2) - (bouton.MeasureString(_boutonEffacer).X / 2), _bouton_Y + effacer.Height / 2 - (bouton.MeasureString(_boutonEffacer).Y / 2)), Color.White);
            ScreenManager.SpriteBatch.DrawString(bouton, _boutonEffacerTOUT1, new Vector2(_bouton_X2 + (effacer.Width / 2) - (bouton.MeasureString(_boutonEffacerTOUT1).X / 2), _bouton_Y + 5), Color.White);
            ScreenManager.SpriteBatch.DrawString(bouton, _boutonEffacerTOUT2, new Vector2(_bouton_X2 + (effacer.Width / 2) - (bouton.MeasureString(_boutonEffacerTOUT2).X / 2), _bouton_Y + 25), Color.White);
            ScreenManager.SpriteBatch.DrawString(bouton, _boutonValider, new Vector2(_bouton_X4 + (valider.Width / 2) - (bouton.MeasureString(_boutonValider).X / 2), _bouton_Y + _boutonHeight / 2 - (bouton.MeasureString(_boutonValider).Y / 2)), Color.White);


            if (_typepartie == 3) { ScreenManager.SpriteBatch.Draw(fond_timer, _positionTimer, Color.White); }

        }

        private void DrawPlaque()
        {
            // affichage des plaques       
            ScreenManager.SpriteBatch.Draw(chiffres_carre_but, new Vector2(10, _range1 + plaqueHeight / 2), Color.White);

            if (plaque1.disponible == true) { Opacity(plaque1); }
            if (plaque2.disponible == true) { Opacity(plaque2); }
            if (plaque3.disponible == true) { Opacity(plaque3); }
            if (plaque4.disponible == true) { Opacity(plaque4); }
            if (plaque5.disponible == true) { Opacity(plaque5); }
            if (plaque6.disponible == true) { Opacity(plaque6); }


            //affichage des opérateurs
            ScreenManager.SpriteBatch.Draw(plus, new Vector2(_plaqueAction_X, _plaquePlus), Color.White);
            ScreenManager.SpriteBatch.Draw(moins, new Vector2(_plaqueAction_X, _plaqueMoins), Color.White);
            ScreenManager.SpriteBatch.Draw(multiplier, new Vector2(_plaqueAction_X, _plaqueMultiplie), Color.White);
            ScreenManager.SpriteBatch.Draw(diviser, new Vector2(_plaqueAction_X, _plaqueDivise), Color.White);

        }

        private void DrawChiffres()
        {
            string nombre = _nombre_a_trouver.ToString();
            string nombre1 = nombreDisponible.ElementAt(0).ToString();
            string nombre2 = nombreDisponible.ElementAt(1).ToString();
            string nombre3 = nombreDisponible.ElementAt(2).ToString();
            string nombre4 = nombreDisponible.ElementAt(3).ToString();
            string nombre5 = nombreDisponible.ElementAt(4).ToString();
            string nombre6 = nombreDisponible.ElementAt(5).ToString();

            Vector2 position = new Vector2(10 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre).X / 2), _range1 + plaqueHeight / 2 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre).Y / 2));
            Vector2 position1 = new Vector2(plaque1.position.X + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre1).X / 2), plaque1.position.Y + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre1).Y / 2));
            Vector2 position2 = new Vector2(plaque2.position.X + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre2).X / 2), plaque2.position.Y + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre2).Y / 2));
            Vector2 position3 = new Vector2(plaque3.position.X + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre3).X / 2), plaque3.position.Y + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre3).Y / 2));
            Vector2 position4 = new Vector2(plaque4.position.X + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre4).X / 2), plaque4.position.Y + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre4).Y / 2));
            Vector2 position5 = new Vector2(plaque5.position.X + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre5).X / 2), plaque5.position.Y + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre5).Y / 2));
            Vector2 position6 = new Vector2(plaque6.position.X + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre6).X / 2), plaque6.position.Y + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre6).Y / 2));

            if (afficher_nombre_but) { ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre, position, Color.Black); }
            else { ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre_aleatoire_intro.ToString(), position, Color.Black); }

            if (plaque1.disponible == true)
            {
                if (plaque1.utiliser == false)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre1, position1, Color.Black);
                }
                else
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre1, position1, opacity_plaqueChiffre);
                }
            }
            if (plaque2.disponible == true)
            {
                if (plaque2.utiliser == false)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre2, position2, Color.Black);
                }
                else
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre2, position2, opacity_plaqueChiffre);
                }
            }
            if (plaque3.disponible == true)
            {
                if (plaque3.utiliser == false)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre3, position3, Color.Black);
                }
                else
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre3, position3, opacity_plaqueChiffre);
                }
            }
            if (plaque4.disponible == true)
            {
                if (plaque4.utiliser == false)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre4, position4, Color.Black);
                }
                else
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre4, position4, opacity_plaqueChiffre);
                }
            }
            if (plaque5.disponible == true)
            {
                if (plaque5.utiliser == false)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre5, position5, Color.Black);
                }
                else
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre5, position5, opacity_plaqueChiffre);
                }
            }
            if (plaque6.disponible == true)
            {
                if (plaque6.utiliser == false)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre6, position6, Color.Black);
                }
                else
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre6, position6, opacity_plaqueChiffre);
                }
            }


        }

        private void DrawInformation()
        {
            if (_infoDivision == true)
            {
                ScreenManager.SpriteBatch.DrawString(blabla, _infoDivisionString, position_info, Color.White);
            }
            if (_infoSoustraction == true)
            {
                ScreenManager.SpriteBatch.DrawString(blabla, _resultatNegatif, position_info, Color.White);
            }
            if (_infoValider == true)
            {
                ScreenManager.SpriteBatch.DrawString(blabla, _infoValiderString, position_info, Color.White);
            }
            if (_tempsecoule == true)
            {
                ScreenManager.SpriteBatch.DrawString(blabla, _tempsecouleString, position_info, Color.White);
            }
            if(_comptebon)
            {
                ScreenManager.SpriteBatch.DrawString(blabla, _comptebonString , position_info, Color.White);
            }
        }

        private void Opacity(Plaques _plaque)
        {
            if ( _plaque.utiliser == true)
            {
                ScreenManager.SpriteBatch.Draw(chiffres_carre, _plaque.position, opacity_plaque);
            }
            else
            {
                ScreenManager.SpriteBatch.Draw(chiffres_carre, _plaque.position, Color.White);
            }
        }

        private int Vector2_DrawOperation(int nombre, int position)
        {
            int caca = 0;
            string _nombre = nombre.ToString();
            caca = position - (int)(chiffre_font.MeasureString(_nombre).X / 2);
            return caca;
        }

        private void DrawOperation()
        {
            if (IsEmpty(Operation1) == false)
            {
                if( Operation1[0] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[0].ToString(), new Vector2(Vector2_DrawOperation(Operation1[0], _nombre1), _operation1), Color.White);
                }
                if( Operation1[1] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operateur(Operation1[1]), new Vector2(_action, _operation1), Color.White);
                }
                if( Operation1[2] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[2].ToString(), new Vector2(Vector2_DrawOperation(Operation1[2], _nombre2), _operation1), Color.White);
                }
                if( Operation1[3] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, "=", new Vector2(_egal, _operation1), Color.White);
                }
                if( Operation1[4] != 0 && Operation1[5] == 0)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[4].ToString(), new Vector2(Vector2_DrawOperation(Operation1[4], _resultat), _operation1), Color.Orange);
                }
                if( Operation1[4] != 0 && Operation1[5] == 1)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[4].ToString(), new Vector2(Vector2_DrawOperation(Operation1[4], _resultat), _operation1), Color.White);
                }
            }
            if (IsEmpty(Operation2) == false)
            {
                if( Operation2[0] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[0].ToString(), new Vector2(Vector2_DrawOperation(Operation2[0], _nombre1), _operation2), Color.White);
                }
                if( Operation2[1] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operateur(Operation2[1]), new Vector2(_action, _operation2), Color.White);
                }
                if( Operation2[2] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[2].ToString(), new Vector2(Vector2_DrawOperation(Operation2[2], _nombre2), _operation2), Color.White);
                }
                if( Operation2[3] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, "=", new Vector2(_egal, _operation2), Color.White);
                }
                if( Operation2[4] != 0 && Operation2[5] == 0)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[4].ToString(), new Vector2(Vector2_DrawOperation(Operation2[4], _resultat), _operation2), Color.Orange);
                }
                if( Operation2[4] != 0 && Operation2[5] == 1)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[4].ToString(), new Vector2(Vector2_DrawOperation(Operation2[4], _resultat), _operation2), Color.White);
                }
            }
            if (IsEmpty(Operation3) == false)
            {
                if( Operation3[0] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[0].ToString(), new Vector2(Vector2_DrawOperation(Operation3[0], _nombre1), _operation3), Color.White);
                }
                if( Operation3[1] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operateur(Operation3[1]), new Vector2(_action, _operation3), Color.White);
                }
                if( Operation3[2] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[2].ToString(), new Vector2(Vector2_DrawOperation(Operation3[2], _nombre2), _operation3), Color.White);
                }
                if( Operation3[3] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, "=", new Vector2(_egal, _operation3), Color.White);
                }
                if( Operation3[4] != 0 && Operation3[5] == 0)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[4].ToString(), new Vector2(Vector2_DrawOperation(Operation3[4], _resultat), _operation3), Color.Orange);
                }
                if( Operation3[4] != 0 && Operation3[5] == 1)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[4].ToString(), new Vector2(Vector2_DrawOperation(Operation3[4], _resultat), _operation3), Color.White);
                }
            }
            if (IsEmpty(Operation4) == false)
            {
                if( Operation4[0] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[0].ToString(), new Vector2(Vector2_DrawOperation(Operation4[0], _nombre1), _operation4), Color.White);
                }
                if( Operation4[1] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operateur(Operation4[1]), new Vector2(_action, _operation4), Color.White);
                }
                if( Operation4[2] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[2].ToString(), new Vector2(Vector2_DrawOperation(Operation4[2], _nombre2), _operation4), Color.White);
                }
                if( Operation4[3] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, "=", new Vector2(_egal, _operation4), Color.White);
                }
                if( Operation4[4] != 0 && Operation4[5] == 0)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[4].ToString(), new Vector2(Vector2_DrawOperation(Operation4[4], _resultat), _operation4), Color.Orange);
                }
                if( Operation4[4] != 0 && Operation4[5] == 1)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[4].ToString(), new Vector2(Vector2_DrawOperation(Operation4[4], _resultat), _operation4), Color.White);
                }
            }
            if (IsEmpty(Operation5) == false)
            {
                if( Operation5[0] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[0].ToString(), new Vector2(Vector2_DrawOperation(Operation5[0], _nombre1), _operation5), Color.White);
                }
                if( Operation5[1] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operateur(Operation5[1]), new Vector2(_action, _operation5), Color.White);
                }
                if( Operation5[2] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[2].ToString(), new Vector2(Vector2_DrawOperation(Operation5[2], _nombre2), _operation5), Color.White);
                }
                if( Operation5[3] != 0 )
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, "=", new Vector2(_egal, _operation5), Color.White);
                }
                if( Operation5[4] != 0 && Operation5[5] == 0)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[4].ToString(), new Vector2(Vector2_DrawOperation(Operation5[4], _resultat), _operation5), Color.Orange);
                }
                if( Operation5[4] != 0 && Operation5[5] == 1)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[4].ToString(), new Vector2(Vector2_DrawOperation(Operation5[4], _resultat), _operation5), Color.White);
                }
            }
        }


        public string Operateur(int valeur)
        {
            string resultat = "";
            switch (valeur)
            {

                case 1 : resultat = "+"; break;
                case 2: resultat = "-"; break;
                case 3: resultat = "x"; break;
                case 4 : resultat = "/"; break;
            }
            return resultat;
        }

        public bool IsEmpty(int[] l)
        {
            if (l.Equals(new List<int>() { 0, 0, 0, 0, 0,0 }))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    public class Plaques
    {
        public Vector2 position;
        public Texture2D texture;
        public bool utiliser;
        public bool disponible;
        public int nombre_correspond;
    }
}
