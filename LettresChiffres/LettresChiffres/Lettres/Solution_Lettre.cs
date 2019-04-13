using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.IO.IsolatedStorage;
using XMLObject;
using System.Threading;

namespace LettresChiffres.Lettres
{
    class Solution_Lettre : GameScreen
    {
        Texture2D _loading2D;
        Texture2D continuer_2D;

        SpriteFont blabla;
        SpriteFont mots;

        MouseState state = Mouse.GetState();
        MouseState previousState;
        
        Vector2 _positionContinue = new Vector2(225, 660);

        string continuer;
        string valide_string;
        string non_valide_string;
        string pas_de_mot_string, menu;

        string Path_dico;

        // position mots
        int mots1 = 100;
        int mots2 = 140;
        int mots3 = 180;
        int mots4 = 220;
        int mots5 = 260;
        int mots6 = 300;
        int mots7 = 340;
        int mots8 = 380;
        int mots9 = 420;
        int mots10 = 460;
        int mots11 = 500;
        int mots12 = 540;
        int mots13 = 580;
        int mots14 = 620;
        int mots_X = 300;
        int categorie_X = 20;

        string[] mots_trouvee = new string[14];
        int[] count_lettresolution = new int[14];

        string _lettres;
        string _joueur;
        public bool _motvalide;
        public bool _continuer = false;
        int _typepartie;
        float _timer;
        bool start = true;


        bool _loading = true;
        int _frame = 0;
        int _nombre_frame = 7;
        int _loadingWidth = 64;
        float _timeframe = 200;
        float _timeloading;

        int[] opacity_frame = new int[15];

        Dictionnaire dictionnaire10;
        Dictionnaire dictionnaire9;
        Dictionnaire dictionnaire8;
        Dictionnaire dictionnaire7;
        Dictionnaire dictionnaire6;
        Dictionnaire dictionnaire5;
        Dictionnaire dictionnaire4;

        Sauvegarde save;
        Difficulte IA_DIFF;
        Random random;
        string motAdversaire = "";
        XML_Serializer xmls = new XML_Serializer();
        Languages lang = new Languages();
        Langues langue = new Langues();

        private Thread LoadingThread;

        public Solution_Lettre(string lettre, string joueur, int typepartie)
        {
            _typepartie = typepartie;
            _joueur = joueur;
            _lettres = OrdreAlphabetique(lettre);
            random = new Random();
        }

        private void InitilizeLanguages()
        {
            continuer = lang.AffectationLANG("Continuer", langue);
            valide_string = lang.AffectationLANG("Valide_string", langue);
            non_valide_string = lang.AffectationLANG("Non_Valide_string", langue);
            pas_de_mot_string = lang.AffectationLANG("Pas_de_mot_string", langue);
            menu = lang.AffectationLANG("Menu", langue);
        }

       

        public override void LoadContent()
        {
            _loading2D = ScreenManager.Game.Content.Load<Texture2D>("loading");
            continuer_2D = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_valider");

            mots = ScreenManager.Game.Content.Load<SpriteFont>("blabla");
            blabla = ScreenManager.Game.Content.Load<SpriteFont>("Transition");

            dictionnaire10 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path+"dico10");
            dictionnaire9 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico9");
            dictionnaire8 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico8");
            dictionnaire7 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico7");
            dictionnaire6 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico6");
            dictionnaire5 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico5");
            dictionnaire4 = ScreenManager.Game.Content.Load<Dictionnaire>(lang.path + "dico4");

            IA_DIFF = ScreenManager.Game.Content.Load<Difficulte>("Difficulte");
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            save = xmls.DeserializeSauvegarde();
            base.LoadContent();
        }

        #region UPDATE
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
            }
            if (start) { Start(); }
            if (!_loading) { _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds; }
            else
            {
                _timeloading += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Loading(_timeloading);
            }
            Input();
            OpacityTimer();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Start()
        {
            if (LoadingThread == null)
            {
                LoadingThread = new Thread(RechercheDebut);
                LoadingThread.Start();
            }
        }

        private void RechercheDebut()
        {
            _motvalide = MotValide(_joueur);
            MotsSolution();
            start = false;
        }

        private void Loading(float time)
        {
            _timeloading += time;
            if (_timeloading > _timeframe)
            {
                if (_frame == _nombre_frame)
                {
                    _frame = 0;
                }
                else
                {
                    _frame++;
                }
                _timeloading = 0;
            }
        }

        public bool MotValide(string mots)
        {
            bool pd = false;
            int cunt = mots.Count();
            string motId = OrdreAlphabetique(mots);
            Dictionnaire _dictionnaire;
            switch (cunt)
            {
                case 4: _dictionnaire = dictionnaire4; break;
                case 5: _dictionnaire = dictionnaire5; break;
                case 6: _dictionnaire = dictionnaire6; break;
                case 7: _dictionnaire = dictionnaire7; break;
                case 8: _dictionnaire = dictionnaire8; break;
                case 9: _dictionnaire = dictionnaire9; break;
                case 10: _dictionnaire = dictionnaire10; break;
                default: return false;
            }
            foreach (var caca in _dictionnaire.DicoMots)
            {
                if (caca.Id == motId)
                {
                    if (caca.a.Count() > cunt)
                    {
                        string[] values = caca.a.Split(new char[] { ' ' });
                        foreach (var fiotte in values)
                        {
                            if (mots.ToLower() == fiotte)
                            {
                                pd = true; break;
                            }
                        }
                    }
                    else if (mots.ToLower() == caca.a)
                    { pd = true; break; }
                }

            }
            return pd;
                
        }

        private void MotsSolution()
        {

            VerificationMotsSolution10(dictionnaire10);
            if (mots_trouvee[13] == null)
            {
                VerificationMotsSolutionelse(dictionnaire9, 9);
                if (mots_trouvee[13] == null)
                {
                    VerificationMotsSolutionelse(dictionnaire8, 8);
                    if (mots_trouvee[13] == null)
                    {

                        VerificationMotsSolutionelse(dictionnaire7, 7);
                        if (mots_trouvee[13] == null)
                        {
                            VerificationMotsSolutionelse(dictionnaire6, 6);
                            if (mots_trouvee[13] == null)
                            {

                                VerificationMotsSolutionelse(dictionnaire5, 5);
                                if (mots_trouvee[13] == null)
                                {
                                    VerificationMotsSolutionelse(dictionnaire4, 4);
                                }
                            }
                        }
                    }
                }
            }
            _loading = false;
        }

        private void VerificationMotsSolution10(Dictionnaire dico)
        {
            foreach (var caca in dictionnaire10.DicoMots)
            {
                if (caca.Id == _lettres)
                {
                    if (caca.a.Count() > caca.Id.Count())
                    {
                        string[] values = caca.a.Split(new char[] { ' ' });
                        foreach (var fiotte in values)
                        {
                            if (mots_trouvee[13] == null)
                            {
                                mots_trouvee.SetValue(fiotte, PositionTableauNull());
                            }
                        }
                    }
                    else
                    {
                        if (mots_trouvee[13] == null)
                        {
                            mots_trouvee.SetValue(caca.a, PositionTableauNull());
                        }
                    }
                }
            }
        }

        private void VerificationMotsSolutionelse(Dictionnaire dico, int nombre)
        {
            List<string> listeID = new List<string>();
            char[] lettre_array = _lettres.ToArray();
            if (nombre == 9)
            {
                for (int k = 0; k < 10; k++)
                {
                    List<char> jetepine = lettre_array.ToList();
                    jetepine.RemoveAt(k);
                    string cacaString = string.Join("", jetepine.ToArray());
                    if (!listeID.Contains(cacaString))
                    {
                        listeID.Add(cacaString);
                    }
                }
                foreach (var jetebaise in listeID)
                {
                    foreach (var caca in dictionnaire9.DicoMots)
                    {
                        if (caca.Id == jetebaise)
                        {
                            if (caca.a.Count() > caca.Id.Count())
                            {
                                string[] values = caca.a.Split(new char[] { ' ' });
                                foreach (var fiotte in values)
                                {
                                    if (mots_trouvee[13] == null)
                                    {
                                        mots_trouvee.SetValue(fiotte, PositionTableauNull());
                                    }
                                }
                            }
                            else
                            {
                                if (mots_trouvee[13] == null)
                                {
                                    mots_trouvee.SetValue(caca.a, PositionTableauNull());
                                }
                            }
                        }
                    }
                }
            }
            else if (nombre == 8)
            {
                for (int k = 0; k < 10; k++)
                {
                    List<char> caca1 = lettre_array.ToList();
                    caca1.RemoveAt(k);
                    for (int h = 0; h < 9; h++)
                    {
                        List<char> caca2 = new List<char>(caca1);
                        caca2.RemoveAt(h);
                        string cacaString = string.Join("", caca2.ToArray());
                        if (!listeID.Contains(cacaString) && cacaString.Count() == 8)
                        {
                            listeID.Add(cacaString);
                        }
                    }
                }
                foreach (var jetebaise in listeID)
                {
                    foreach (var caca in dictionnaire8.DicoMots)
                    {
                        if (caca.Id == jetebaise)
                        {
                            if (caca.a.Count() > caca.Id.Count())
                            {
                                string[] values = caca.a.Split(new char[] { ' ' });
                                foreach (var fiotte in values)
                                {
                                    if (mots_trouvee[13] == null)
                                    {
                                        mots_trouvee.SetValue(fiotte, PositionTableauNull());
                                    }
                                }
                            }
                            else
                            {
                                if (mots_trouvee[13] == null)
                                {
                                    mots_trouvee.SetValue(caca.a, PositionTableauNull());
                                }
                            }
                        }
                    }
                }
            }
            else if (nombre == 7)
            {
                for (int k = 0; k < 10; k++)
                {
                    List<char> caca1 = lettre_array.ToList();
                    caca1.RemoveAt(k);
                    for (int h = 0; h < 9; h++)
                    {
                        List<char> caca2 = new List<char>(caca1);
                        caca2.RemoveAt(h);
                        for (int j = 0; j < 8; j++)
                        {
                            List<char> caca3 = new List<char>(caca2);
                            caca3.RemoveAt(j);
                            string cacaString = string.Join("", caca3.ToArray());
                            if (!listeID.Contains(cacaString) && cacaString.Count() == 7)
                            {
                                listeID.Add(cacaString);
                            }
                        }
                    }
                }
                foreach (var jetebaise in listeID)
                {
                    foreach (var caca in dictionnaire7.DicoMots)
                    {
                        if (caca.Id == jetebaise)
                        {
                            if (caca.a.Count() > caca.Id.Count())
                            {
                                string[] values = caca.a.Split(new char[] { ' ' });
                                foreach (var fiotte in values)
                                {
                                    if (mots_trouvee[13] == null)
                                    {
                                        mots_trouvee.SetValue(fiotte, PositionTableauNull());
                                    }
                                }
                            }
                            else
                            {
                                if (mots_trouvee[13] == null)
                                {
                                    mots_trouvee.SetValue(caca.a, PositionTableauNull());
                                }
                            }
                        }
                    }
                }
            }
            else if (nombre == 6)
            {
                for (int k = 0; k < 10; k++)
                {
                    List<char> caca1 = lettre_array.ToList();
                    caca1.RemoveAt(k);
                    for (int h = 0; h < 9; h++)
                    {
                        List<char> caca2 = new List<char>(caca1);
                        caca2.RemoveAt(h);
                        for (int j = 0; j < 8; j++)
                        {
                            List<char> caca3 = new List<char>(caca2);
                            caca3.RemoveAt(j);
                            for (int g = 0; g < 7; g++)
                            {
                                List<char> caca4 = new List<char>(caca3);
                                caca4.RemoveAt(g);
                                string cacaString = string.Join("", caca4.ToArray());
                                if (!listeID.Contains(cacaString) && cacaString.Count() == 6)
                                {
                                    listeID.Add(cacaString);
                                }
                            }
                        }
                    }
                }
                foreach (var jetebaise in listeID)
                {
                    foreach (var caca in dictionnaire6.DicoMots)
                    {
                        if (caca.Id == jetebaise)
                        {
                            if (caca.a.Count() > caca.Id.Count())
                            {
                                string[] values = caca.a.Split(new char[] { ' ' });
                                foreach (var fiotte in values)
                                {
                                    if (mots_trouvee[13] == null)
                                    {
                                        mots_trouvee.SetValue(fiotte, PositionTableauNull());
                                    }
                                }
                            }
                            else
                            {
                                if (mots_trouvee[13] == null)
                                {
                                    mots_trouvee.SetValue(caca.a, PositionTableauNull());
                                }
                            }
                        }
                    }
                }
            }
            else if (nombre == 5)
            {
                for (int k = 0; k < 10; k++)
                {
                    List<char> caca1 = lettre_array.ToList();
                    caca1.RemoveAt(k);
                    for (int h = 0; h < 9; h++)
                    {
                        List<char> caca2 = new List<char>(caca1);
                        caca2.RemoveAt(h);
                        for (int j = 0; j < 8; j++)
                        {
                            List<char> caca3 = new List<char>(caca2);
                            caca3.RemoveAt(j);
                            for (int g = 0; g < 7; g++)
                            {
                                List<char> caca4 = new List<char>(caca3);
                                caca4.RemoveAt(g);
                                for (int d = 0; d < 6; d++)
                                {
                                    List<char> caca5 = new List<char>(caca4);
                                    caca5.RemoveAt(d);
                                    string cacaString = string.Join("", caca5.ToArray());
                                    if (!listeID.Contains(cacaString) && cacaString.Count() == 5)
                                    {
                                        listeID.Add(cacaString);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var jetebaise in listeID)
                {
                    foreach (var caca in dictionnaire5.DicoMots)
                    {
                        if (caca.Id == jetebaise)
                        {
                            if (caca.a.Count() > caca.Id.Count())
                            {
                                string[] values = caca.a.Split(new char[] { ' ' });
                                foreach (var fiotte in values)
                                {
                                    if (mots_trouvee[13] == null)
                                    {
                                        mots_trouvee.SetValue(fiotte, PositionTableauNull());
                                    }
                                }
                            }
                            else
                            {
                                if (mots_trouvee[13] == null)
                                {
                                    mots_trouvee.SetValue(caca.a, PositionTableauNull());
                                }
                            }
                        }
                    }
                }
            }
            else if (nombre == 4)
            {
                for (int k = 0; k < 10; k++)
                {
                    List<char> caca1 = lettre_array.ToList();
                    caca1.RemoveAt(k);
                    for (int h = 0; h < 9; h++)
                    {
                        List<char> caca2 = new List<char>(caca1);
                        caca2.RemoveAt(h);
                        for (int j = 0; j < 8; j++)
                        {
                            List<char> caca3 = new List<char>(caca2);
                            caca3.RemoveAt(j);
                            for (int g = 0; g < 7; g++)
                            {
                                List<char> caca4 = new List<char>(caca3);
                                caca4.RemoveAt(g);
                                for (int d = 0; d < 6; d++)
                                {
                                    List<char> caca5 = new List<char>(caca4);
                                    caca5.RemoveAt(d);
                                    for (int z = 0; z < 5; z++)
                                    {
                                        List<char> caca6 = new List<char>(caca5);
                                        caca6.RemoveAt(z);
                                        string cacaString = string.Join("", caca6.ToArray());
                                        if (!listeID.Contains(cacaString) && cacaString.Count() == 4)
                                        {
                                            listeID.Add(cacaString);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var jetebaise in listeID)
                {
                    foreach (var caca in dictionnaire4.DicoMots)
                    {
                        if (caca.Id == jetebaise)
                        {
                            if (caca.a.Count() > caca.Id.Count())
                            {
                                string[] values = caca.a.Split(new char[] { ' ' });
                                foreach (var fiotte in values)
                                {
                                    if (mots_trouvee[13] == null)
                                    {
                                        mots_trouvee.SetValue(fiotte, PositionTableauNull());
                                    }
                                }
                            }
                            else
                            {
                                if (mots_trouvee[13] == null)
                                {
                                    mots_trouvee.SetValue(caca.a, PositionTableauNull());
                                }
                            }
                        }
                    }
                }
            }
            
        }

        private int PositionTableauNull()
        {
            if (mots_trouvee[0] == null) { return 0; }
            else if (mots_trouvee[1] == null) { return 1; }
            else if (mots_trouvee[2] == null) { return 2; }
            else if (mots_trouvee[3] == null) { return 3; }
            else if (mots_trouvee[4] == null) { return 4; }
            else if (mots_trouvee[5] == null) { return 5; }
            else if (mots_trouvee[6] == null) { return 6; }
            else if (mots_trouvee[7] == null) { return 7; }
            else if (mots_trouvee[8] == null) { return 8; }
            else if (mots_trouvee[9] == null) { return 9; }
            else if (mots_trouvee[10] == null) { return 10; }
            else if (mots_trouvee[11] == null) { return 11; }
            else if (mots_trouvee[12] == null) { return 12; }
            else{ return 13; }
        }

        private void OpacityTimer()
        {
            for (int i = 0; i < 15; i++)
            {
                if (_timer > (1500f + 100f * i) && opacity_frame[i] != 20) { opacity_frame[i]++; }
            }
        }

        static string OrdreAlphabetique(string mot)
        {
            char[] caca = mot.ToCharArray();
            Array.Sort<char>(caca);
            return new String(caca);
        }

#endregion
        public void Input()
        {
            state = Mouse.GetState();

            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (state.X > _positionContinue.X && state.X < (_positionContinue.X + continuer_2D.Width) &&
                    state.Y > _positionContinue.Y && state.Y < (_positionContinue.Y + continuer_2D.Height) &&
                    _timer > 3000)
                {
                    if (_typepartie == 2)
                    {
                        this.ExitScreen();
                        ScreenManager.AddScreen(new Transition(_typepartie, _motvalide, _joueur));
                    }
                    else
                    {
                        IA_Initialize();
                        Sauvegarder();
                        this.ExitScreen();
                        ScreenManager.AddScreen(new TransitionAdversaire(4,_motvalide,_joueur,motAdversaire));
                    }
                }
            }
            previousState = state;
        }

        #region IA

        private void IA_Initialize()
        {
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
            string[] pourcentage_Array;
            foreach (var caca in IA_DIFF.ListeDifficulte)
            {
                if (caca.Difficulte == Diff_string)
                {
                    pourcentage_Array = caca.ListePourcetageLettres.Split(new char[] { ' ' });
                    int hazard = random.Next(1, 101);
                    int somme = 0;
                    int i = 1;
                    foreach (string foutre in pourcentage_Array)
                    {
                        somme += int.Parse(foutre);
                        if (somme > hazard)
                        {
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    int Nombre_lettre_mot_a_chercher = count_lettresolution[0] - i;
                    if (Nombre_lettre_mot_a_chercher < 4)
                    {
                        motAdversaire = "A";
                    }
                    else
                    {
                        motAdversaire = IA_XML_MOTADVERSAIRE(Nombre_lettre_mot_a_chercher);
                    }
                    break;

                }
            }

        }

        private string IA_XML_MOTADVERSAIRE(int nombre_de_lettre)
        {
            string bukkake;
            Dictionnaire _dictionnaire;
            switch (nombre_de_lettre)
            {
                case 4: _dictionnaire = dictionnaire4; bukkake = XML_ADVERSAIRE(_dictionnaire, 4); break;
                case 5: _dictionnaire = dictionnaire5; bukkake = XML_ADVERSAIRE(_dictionnaire, 5); break;
                case 6: _dictionnaire = dictionnaire6; bukkake = XML_ADVERSAIRE(_dictionnaire, 6); break;
                case 7: _dictionnaire = dictionnaire7; bukkake = XML_ADVERSAIRE(_dictionnaire, 7); break;
                case 8: _dictionnaire = dictionnaire8; bukkake = XML_ADVERSAIRE(_dictionnaire, 8); break;
                case 9: _dictionnaire = dictionnaire9; bukkake = XML_ADVERSAIRE(_dictionnaire, 9); break;
                case 10: _dictionnaire = dictionnaire10; bukkake = XML_ADVERSAIRE(_dictionnaire, 10); break;
                default: return "A";
            }
            return bukkake;
            
        }

        private string XML_ADVERSAIRE(Dictionnaire dico, int lettre)
        {
            string mots_ADVERSAIRE = "";
            if (lettre == 10)
            {
                foreach (var caca in dico.DicoMots)
                {
                    if (caca.Id == _lettres)
                    {
                        if (caca.a.Count() > caca.Id.Count())
                        {
                            string[] values = caca.a.Split(new char[] { ' ' });
                            mots_ADVERSAIRE = values[0]; break;
                        }
                        else
                        {
                            mots_ADVERSAIRE = caca.a; break;
                        }
                    }
                }
            }
            else
            {
                List<string> listeID = new List<string>();
                char[] lettre_array = _lettres.ToArray();
                if (lettre == 9)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        List<char> jetepine = lettre_array.ToList();
                        jetepine.RemoveAt(k);
                        string cacaString = string.Join("", jetepine.ToArray());
                        if (!listeID.Contains(cacaString))
                        {
                            listeID.Add(cacaString);
                        }
                    }
                    mots_ADVERSAIRE = MotXML(dico, listeID);
                }
                else if (lettre == 8)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        List<char> caca1 = lettre_array.ToList();
                        caca1.RemoveAt(k);
                        for (int h = 0; h < 9; h++)
                        {
                            List<char> caca2 = new List<char>(caca1);
                            caca2.RemoveAt(h);
                            string cacaString = string.Join("", caca2.ToArray());
                            if (!listeID.Contains(cacaString) && cacaString.Count() == 8)
                            {
                                listeID.Add(cacaString);
                            }
                        }
                    }
                    mots_ADVERSAIRE = MotXML(dico, listeID);
                }
                else if (lettre == 7)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        List<char> caca1 = lettre_array.ToList();
                        caca1.RemoveAt(k);
                        for (int h = 0; h < 9; h++)
                        {
                            List<char> caca2 = new List<char>(caca1);
                            caca2.RemoveAt(h);
                            for (int j = 0; j < 8; j++)
                            {
                                List<char> caca3 = new List<char>(caca2);
                                caca3.RemoveAt(j);
                                string cacaString = string.Join("", caca3.ToArray());
                                if (!listeID.Contains(cacaString) && cacaString.Count() == 7)
                                {
                                    listeID.Add(cacaString);
                                }
                            }
                        }
                    }
                    mots_ADVERSAIRE = MotXML(dico, listeID);
                }
                else if (lettre == 6)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        List<char> caca1 = lettre_array.ToList();
                        caca1.RemoveAt(k);
                        for (int h = 0; h < 9; h++)
                        {
                            List<char> caca2 = new List<char>(caca1);
                            caca2.RemoveAt(h);
                            for (int j = 0; j < 8; j++)
                            {
                                List<char> caca3 = new List<char>(caca2);
                                caca3.RemoveAt(j);
                                for (int g = 0; g < 7; g++)
                                {
                                    List<char> caca4 = new List<char>(caca3);
                                    caca4.RemoveAt(g);
                                    string cacaString = string.Join("", caca4.ToArray());
                                    if (!listeID.Contains(cacaString) && cacaString.Count() == 6)
                                    {
                                        listeID.Add(cacaString);
                                    }
                                }
                            }
                        }
                    }
                    mots_ADVERSAIRE = MotXML(dico, listeID);
                }
                else if (lettre == 5)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        List<char> caca1 = lettre_array.ToList();
                        caca1.RemoveAt(k);
                        for (int h = 0; h < 9; h++)
                        {
                            List<char> caca2 = new List<char>(caca1);
                            caca2.RemoveAt(h);
                            for (int j = 0; j < 8; j++)
                            {
                                List<char> caca3 = new List<char>(caca2);
                                caca3.RemoveAt(j);
                                for (int g = 0; g < 7; g++)
                                {
                                    List<char> caca4 = new List<char>(caca3);
                                    caca4.RemoveAt(g);
                                    for (int d = 0; d < 6; d++)
                                    {
                                        List<char> caca5 = new List<char>(caca4);
                                        caca5.RemoveAt(d);
                                        string cacaString = string.Join("", caca5.ToArray());
                                        if (!listeID.Contains(cacaString) && cacaString.Count() == 5)
                                        {
                                            listeID.Add(cacaString);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    mots_ADVERSAIRE = MotXML(dico, listeID);
                }
                else if (lettre == 4)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        List<char> caca1 = lettre_array.ToList();
                        caca1.RemoveAt(k);
                        for (int h = 0; h < 9; h++)
                        {
                            List<char> caca2 = new List<char>(caca1);
                            caca2.RemoveAt(h);
                            for (int j = 0; j < 8; j++)
                            {
                                List<char> caca3 = new List<char>(caca2);
                                caca3.RemoveAt(j);
                                for (int g = 0; g < 7; g++)
                                {
                                    List<char> caca4 = new List<char>(caca3);
                                    caca4.RemoveAt(g);
                                    for (int d = 0; d < 6; d++)
                                    {
                                        List<char> caca5 = new List<char>(caca4);
                                        caca5.RemoveAt(d);
                                        for (int z = 0; z < 5; z++)
                                        {
                                            List<char> caca6 = new List<char>(caca5);
                                            caca6.RemoveAt(z);
                                            string cacaString = string.Join("", caca6.ToArray());
                                            if (!listeID.Contains(cacaString) && cacaString.Count() == 4)
                                            {
                                                listeID.Add(cacaString);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    mots_ADVERSAIRE = MotXML(dico, listeID);
                }
            }
            return mots_ADVERSAIRE;
        }

        private string MotXML(Dictionnaire dico, List<string> listeID)
        {
            string mots = null;
            foreach (var jetebaise in listeID)
            {
                if (mots != null) { break; }
                foreach (var caca in dico.DicoMots)
                {
                    if (caca.Id == jetebaise)
                    {
                        if (caca.a.Count() > caca.Id.Count())
                        {
                            string[] values = caca.a.Split(new char[] { ' ' });
                            mots = values[0]; break;
                        }
                        else
                        {
                            mots = caca.a; break;
                        }
                    }
                }
            }
            return mots;
        }

        private void Sauvegarder()
        {
            int pointJoueur = 0;
            int pointAdversaire = 0;
            string scoreJoueur = save.ListeSave.ScoreJoueur;
            string scoreAdversaire = save.ListeSave.ScoreAdversaire;
            int _joueurCount;
            int _adversaireCount;


            _joueurCount = _joueur.Count();

            if (motAdversaire.Count() == 1)
            {
                _adversaireCount = 0;
            }
            else
            {
                _adversaireCount = motAdversaire.Count();
            }

            if (_joueurCount > _adversaireCount && _motvalide)
            {
                pointJoueur = _joueurCount;
            }
            else if (_joueurCount < _adversaireCount)
            {
                pointAdversaire = _adversaireCount;
            }
            else if (_joueurCount == _adversaireCount && _motvalide)
            {
                pointJoueur = _joueurCount;
                pointAdversaire = _adversaireCount;
            }
            else if (!_motvalide)
            {
                if (_joueurCount > _adversaireCount)
                {
                    pointAdversaire = _joueurCount;
                }
                else
                {
                    pointAdversaire = _adversaireCount;
                }
            }


            if (scoreJoueur == "99")
            {
                scoreJoueur = pointJoueur.ToString();
            }
            else
            {
                scoreJoueur += " " + pointJoueur.ToString();
            }
            if (scoreAdversaire == "99")
            {
                scoreAdversaire = pointAdversaire.ToString();
            }
            else
            {
                scoreAdversaire += " " + pointAdversaire.ToString();
            }

            save.ListeSave.ScoreAdversaire = scoreAdversaire;
            save.ListeSave.ScoreJoueur = scoreJoueur;
            save.ListeSave.NombreManchesJouer++;

            xmls.SerialiseSauvegarde(save);
        }

        #endregion
        #region DRAW
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            
            if (_loading) { Loading(); }
            else
            {
                DrawMotsValide();
                DrawMotsSolution();
                DrawCategorie();
                if (_timer > 1500) 
                {
                    ScreenManager.SpriteBatch.Draw(continuer_2D, _positionContinue, OpacityColor(opacity_frame[14]));
                    ScreenManager.SpriteBatch.DrawString(blabla, continuer, new Vector2(_positionContinue.X + continuer_2D.Width / 2 - 20 - blabla.MeasureString(continuer).X / 2, _positionContinue.Y + continuer_2D.Height / 2 - blabla.MeasureString(continuer).Y / 2), OpacityColor(opacity_frame[14])); 
                }
            }
            ScreenManager.SpriteBatch.End();
        }

        private void Loading()
        {
            int _afficherWidth = _frame * _loadingWidth;
            Rectangle source = new Rectangle(_afficherWidth, 0, _loadingWidth, _loading2D.Height);
            ScreenManager.SpriteBatch.Draw(_loading2D, new Vector2(210, 300), source, Color.White);
        }

        private void DrawMotsValide()
        {
            if (_joueur.Count() > 3)
            {
                ScreenManager.SpriteBatch.DrawString(blabla, _joueur, new Vector2(30, 30), Color.White);
                if (_timer > 500f)
                {
                    if (_motvalide) { ScreenManager.SpriteBatch.DrawString(blabla, valide_string, new Vector2(270, 30), Color.White); }
                    else if (!_motvalide) { ScreenManager.SpriteBatch.DrawString(blabla, non_valide_string, new Vector2(250, 30), Color.White); }
                }
            }
            else
            {
                ScreenManager.SpriteBatch.DrawString(blabla, pas_de_mot_string, new Vector2(30, 30), Color.White);
            }
        }

        private void DrawMotsSolution()
        {
            if (mots_trouvee[0] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[0], new Vector2(mots_X, mots1), OpacityColor(opacity_frame[0])); }
            if (mots_trouvee[1] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[1], new Vector2(mots_X, mots2), OpacityColor(opacity_frame[1])); }
            if (mots_trouvee[2] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[2], new Vector2(mots_X, mots3), OpacityColor(opacity_frame[2])); }
            if (mots_trouvee[3] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[3], new Vector2(mots_X, mots4), OpacityColor(opacity_frame[3])); }
            if (mots_trouvee[4] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[4], new Vector2(mots_X, mots5), OpacityColor(opacity_frame[4])); }
            if (mots_trouvee[5] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[5], new Vector2(mots_X, mots6), OpacityColor(opacity_frame[5])); }
            if (mots_trouvee[6] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[6], new Vector2(mots_X, mots7), OpacityColor(opacity_frame[6])); }
            if (mots_trouvee[7] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[7], new Vector2(mots_X, mots8), OpacityColor(opacity_frame[7])); }
            if (mots_trouvee[8] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[8], new Vector2(mots_X, mots9), OpacityColor(opacity_frame[8])); }
            if (mots_trouvee[9] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[9], new Vector2(mots_X, mots10), OpacityColor(opacity_frame[9])); }
            if (mots_trouvee[10] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[10], new Vector2(mots_X, mots11), OpacityColor(opacity_frame[10])); }
            if (mots_trouvee[11] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[11], new Vector2(mots_X, mots12), OpacityColor(opacity_frame[11])); }
            if (mots_trouvee[12] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[12], new Vector2(mots_X, mots13), OpacityColor(opacity_frame[12])); }
            if (mots_trouvee[13] != null) { ScreenManager.SpriteBatch.DrawString(mots, mots_trouvee[13], new Vector2(mots_X, mots14), OpacityColor(opacity_frame[13])); }
        }

        private void DrawCategorie()
        {
            if (count_lettresolution[0] == 0) { AjoutIntCount(); }
            int nombre;
            if (count_lettresolution[0] != 0)
            {
                if (_timer > 1500f) { ScreenManager.SpriteBatch.DrawString(blabla, CategorieMots(count_lettresolution[0]), positionCategorie(0), OpacityColor(opacity_frame[0])); }
                nombre = count_lettresolution[0];
                for (int i = 1; i <= 13; i++)
                {
                    if (count_lettresolution[i] != 0 && count_lettresolution[i] < nombre && _timer > 1500f) { nombre = count_lettresolution[i]; ScreenManager.SpriteBatch.DrawString(blabla, CategorieMots(count_lettresolution[i]), positionCategorie(i), OpacityColor(opacity_frame[i])); }
                }
            }

        }

        private string CategorieMots(int nombre)
        {
            switch (nombre)
            {
                case 10: return "10 lettres";
                case 9: return "9 lettres";
                case 8: return "8 lettres";
                case 7: return "7 lettres";
                case 6: return "6 lettres";
                case 5: return "5 lettres";
                case 4: return "4 lettres";
                default: return "";
            }
        }

        private Vector2 positionCategorie(int position_array)
        {
            if (position_array == 0) { return new Vector2(categorie_X, mots1); }
            else if (position_array == 1) { return new Vector2(categorie_X, mots2); }
            else if (position_array == 2) { return new Vector2(categorie_X, mots3); }
            else if (position_array == 3) { return new Vector2(categorie_X, mots4); }
            else if (position_array == 4) { return new Vector2(categorie_X, mots5); }
            else if (position_array == 5) { return new Vector2(categorie_X, mots6); }
            else if (position_array == 6) { return new Vector2(categorie_X, mots7); }
            else if (position_array == 7) { return new Vector2(categorie_X, mots8); }
            else if (position_array == 8) { return new Vector2(categorie_X, mots9); }
            else if (position_array == 9) { return new Vector2(categorie_X, mots10); }
            else if (position_array == 10) { return new Vector2(categorie_X, mots11); }
            else if (position_array == 11) { return new Vector2(categorie_X, mots12); }
            else if (position_array == 12) { return new Vector2(categorie_X, mots13); }
            else return new Vector2(categorie_X, mots14); 
        }

        private void AjoutIntCount()
        {
            for (int i = 0; i < 14; i++)
            {
                if (mots_trouvee[i] != null) { count_lettresolution.SetValue(mots_trouvee[i].Count(), i); }
            }
        }
               
        private static Color OpacityColor( float frame)
        {
            return Color.White * (frame / 20.0f);
        }
        #endregion
    }
}
