using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using XMLObject;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace LettresChiffres
{
    class Solution_Chiffres : GameScreen
    {

        Texture2D plaque;
        Texture2D plaque_but;
        Texture2D continuer_2D;

        SpriteFont chiffre_font;
        SpriteFont blabla;
        SpriteFont continuer_font;

        // STRING
        string continuer;
        Vector2 _positionContinue = new Vector2(225, 430);
        string solution_string;
        string noSolutionString;

        int _typepartie;
        int _nombreatrouver;
        int _resultatjoueur;
        int _resultatAdversaire;
        int[] _listeNombre = new int[6];
        bool start;
        float _timer;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        string[] Operation1 = new string[5];
        string[] Operation2 = new string[5];
        string[] Operation3 = new string[5];
        string[] Operation4 = new string[5];
        string[] Operation5 = new string[5];
        int[] opacity_frame = new int[15];

        // Operations
        const int _operation1 = 70;
        const int _operation2 = 140;
        const int _operation3 = 210;
        const int _operation4 = 280;
        const int _operation5 = 350;

        const int _nombre1 = 75;
        const int _action = 145;
        const int _nombre2 = 220;
        const int _egal = 275;
        const int _resultat = 390;

        private Thread _loadingThread;
        bool pasdesolution = false;
        bool comptebon = true;
        int plusProche;
        bool _loading = true;
        public bool _continuer = false;

        Difficulte difficulte;
        Random random;
        Sauvegarde save;
        XML_Serializer xmls = new XML_Serializer();
        Languages lang = new Languages();
        Langues langue = new Langues();

        // Position Plaque
        const int _range1 = 550;
        const int _range2 = 640;
        const int _colonne1 = 130;
        const int _colonne2 = 250;
        const int _colonne3 = 370;
        const float plaqueWidth = 100;
        const float plaqueHeight = 70;
        const float screenWidth = 480.0f;
        const float screenHeight = 800.0f;

        public Solution_Chiffres(int typepartie, int nombreatrouver, int nombrejoueurtrouvee, List<int> listeNombre)
        {
            _typepartie = typepartie;
            _nombreatrouver = nombreatrouver;
            _resultatjoueur = nombrejoueurtrouvee;

            random = new Random();

            _listeNombre.SetValue(listeNombre[0], 0);
            _listeNombre.SetValue(listeNombre[1], 1);
            _listeNombre.SetValue(listeNombre[2], 2);
            _listeNombre.SetValue(listeNombre[3], 3);
            _listeNombre.SetValue(listeNombre[4], 4);
            _listeNombre.SetValue(listeNombre[5], 5);
            start = true;
        }

        private void InitilizeLanguages()
        {
            continuer = lang.AffectationLANG("Continuer", langue);
            solution_string = lang.AffectationLANG("Solution", langue);
            noSolutionString = lang.AffectationLANG("noSolution", langue);
        }


        public override void LoadContent()
        {
            continuer_2D = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_valider");
            plaque = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/chiffres_carre");
            plaque_but = ScreenManager.Game.Content.Load<Texture2D>("Chiffre/chiffres_carre_but");
            chiffre_font = ScreenManager.Game.Content.Load<SpriteFont>("chiffre_font");
            blabla = ScreenManager.Game.Content.Load<SpriteFont>("blabla");
            continuer_font = ScreenManager.Game.Content.Load<SpriteFont>("Transition");

            difficulte = ScreenManager.Game.Content.Load<Difficulte>("Difficulte");
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
            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (start) { Start(); start = false; }
            OpacityTimer();
            Input();

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void OpacityTimer()
        {
            for (int i = 0; i < 15; i++)
            {
                if (_timer > (1500f + 100f * i) && opacity_frame[i] != 20) { opacity_frame[i]++; }
            }
        }

        private void Start()
        {
            if (_loadingThread == null)
            {
                _loadingThread = new Thread(Resolution_alternative);
                _loadingThread.Start();
            }
        }

        private void Resolution_alternative()
        {
            string solution;
            solution = Calcul.Cherche(_listeNombre, _nombreatrouver);
            if (solution != null)
            {
                plusProche = _nombreatrouver;
                ValidationOperation(solution);
                _loading = false;
            }
            else
            {
                comptebon = false;
                solution = Calcul.Cherche(_listeNombre, _nombreatrouver - 1);
                if (solution != null)
                {
                    plusProche = _nombreatrouver - 1;
                    ValidationOperation(solution);
                    _loading = false;
                }
                else
                {
                    solution = Calcul.Cherche(_listeNombre, _nombreatrouver + 1);
                    if (solution != null)
                    {
                        plusProche = _nombreatrouver + 1;
                        ValidationOperation(solution);
                        _loading = false;
                    }
                    else
                    {
                        solution = Calcul.Cherche(_listeNombre, _nombreatrouver + 2);
                        if (solution != null)
                        {
                            plusProche = _nombreatrouver + 2;
                            ValidationOperation(solution);
                            _loading = false;
                        }
                        else
                        {
                            solution = Calcul.Cherche(_listeNombre, _nombreatrouver - 2);
                            if (solution != null)
                            {
                                plusProche = _nombreatrouver - 2;
                                ValidationOperation(solution);
                                _loading = false;
                            }

                            else
                            {
                                pasdesolution = true;
                                _loading = false;
                            }
                        }

                    }
                }
            }
        }

        private int PlaceSetOperation(string[] array)
        {
            if (array[0] == null || array[0] == "") { return 0; }
            else if (array[1] == null || array[1] == "") { return 1; }
            else if (array[2] == null || array[2] == "") { return 2; }
            else if (array[3] == null || array[3] == "") { return 3; }
            else return 4;
        }

        private void ValidationOperation(string caca)
        {
            string[] bukkake = caca.Split(new char[] { '|' });
            string[] a = bukkake[0].Split(new char[] { ' ' });
            foreach (string facial in a)
            {
                if (facial != "" && facial != null)
                {
                    Operation1.SetValue(facial, PlaceSetOperation(Operation1));
                }
            }

            if (bukkake.Count() >= 2)
            {
                string[] b = bukkake[1].Split(new char[] { ' ' });
                foreach (string facial in b)
                {
                    if (facial != "" && facial != null)
                    {
                        Operation2.SetValue(facial, PlaceSetOperation(Operation2));
                    }
                }
                if (bukkake.Count() >= 3)
                {
                    string[] c = bukkake[2].Split(new char[] { ' ' });
                    foreach (string facial in c)
                    {
                        if (facial != "" && facial != null)
                        {
                            Operation3.SetValue(facial, PlaceSetOperation(Operation3));
                        }
                    }
                    if (bukkake.Count() >= 4)
                    {
                        string[] d = bukkake[3].Split(new char[] { ' ' });
                        foreach (string facial in d)
                        {
                            if (facial != "" && facial != null)
                            {
                                Operation4.SetValue(facial, PlaceSetOperation(Operation4));
                            }
                        }
                        if (bukkake.Count() >= 5)
                        {
                            string[] e = bukkake[4].Split(new char[] { ' ' });
                            foreach (string facial in e)
                            {
                                if (facial != "" && facial != null)
                                {
                                    Operation5.SetValue(facial, PlaceSetOperation(Operation5));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (state.X > _positionContinue.X && state.X < (_positionContinue.X + continuer_2D.Width) &&
                    state.Y > _positionContinue.Y && state.Y < (_positionContinue.Y + continuer_2D.Height))
                {
                    if (_typepartie == 1)
                    {
                        this.ExitScreen();
                        ScreenManager.AddScreen(new Transition(1, _nombreatrouver, _resultatjoueur));
                    }
                    else
                    {
                        IA_Initialize();
                        Sauvegarde();
                        this.ExitScreen();
                        ScreenManager.AddScreen(new TransitionAdversaire(3, _nombreatrouver, _resultatjoueur, _resultatAdversaire));
                    }
                }
            }
            previousState = state;
        }

        #region IA

        private void Sauvegarde()
        {
            int pointJoueur = 0;
            int pointAdversaire = 0;
            string scoreJoueur = save.ListeSave.ScoreJoueur;
            string scoreAdversaire = save.ListeSave.ScoreAdversaire;
            int distance_compteJoueur;
            int distance_compteAdversaire;

            if (_nombreatrouver > _resultatjoueur)
            {
                distance_compteJoueur = _nombreatrouver - _resultatjoueur;
            }
            else
            {
                distance_compteJoueur = _resultatjoueur - _nombreatrouver;
            }

            if (_nombreatrouver > _resultatAdversaire)
            {
                distance_compteAdversaire = _nombreatrouver - _resultatAdversaire;
            }
            else
            {
                distance_compteAdversaire = _resultatAdversaire - _nombreatrouver;
            }

            if (comptebon)
            {
                if (_resultatjoueur == _nombreatrouver)
                {
                    pointJoueur = 10;
                }
                if (_resultatAdversaire == _nombreatrouver)
                {
                    pointAdversaire = 10;
                }
            }

            if (distance_compteJoueur < distance_compteAdversaire && _resultatjoueur != _nombreatrouver)
            {
                if (plusProche == _resultatjoueur)
                {
                    pointJoueur = 10;
                }
                else
                {
                    pointJoueur = 7;
                }

            }
            else if (distance_compteJoueur > distance_compteAdversaire && _resultatAdversaire != _nombreatrouver)
            {
                if (plusProche == _resultatAdversaire)
                {
                    pointAdversaire = 10;
                }
                else
                {
                    pointAdversaire = 7;
                }
            }
            else if (distance_compteJoueur == distance_compteAdversaire && _resultatjoueur != _nombreatrouver)
            {
                if (plusProche == _resultatjoueur)
                {
                    pointJoueur = 10;
                    pointAdversaire = 10;
                }
                else
                {
                    pointJoueur = 7;
                    pointAdversaire = 7;
                }
            }

            scoreJoueur += " " + pointJoueur.ToString();
            scoreAdversaire += " " + pointAdversaire.ToString();

            save.ListeSave.ScoreAdversaire = scoreAdversaire;
            save.ListeSave.ScoreJoueur = scoreJoueur;
            save.ListeSave.NombreManchesJouer++;

            xmls.SerialiseSauvegarde(save);
        }

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

            int bonus = 0;
            string[] pourcentageArray;
            if (!comptebon)
            {
                bonus++;
            }
            foreach (var caca in difficulte.ListeDifficulte)
            {
                if (caca.Difficulte == Diff_string)
                {
                    pourcentageArray = caca.ListePourcentageChiffres.Split(new char[] { ' ' });
                    int hazard = random.Next(1, 101);
                    int somme = 0;
                    int i = 1;
                    foreach (string foutre in pourcentageArray)
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
                    int Nombre_a_chercher = i + bonus;
                    if (i == 7)
                    {
                        _resultatAdversaire = 0;
                    }
                    else
                    {
                        string solution;
                        solution = Calcul.Cherche(_listeNombre, _nombreatrouver - Nombre_a_chercher);
                        if (solution != null)
                        {
                            _resultatAdversaire = _nombreatrouver - Nombre_a_chercher;
                        }
                        else
                        {
                            solution = Calcul.Cherche(_listeNombre, _nombreatrouver + Nombre_a_chercher);
                            if (solution != null)
                            {
                                _resultatAdversaire = _nombreatrouver + Nombre_a_chercher;
                            }
                            else
                            {
                                solution = Calcul.Cherche(_listeNombre, _nombreatrouver + Nombre_a_chercher + 1);
                                if (solution != null)
                                {
                                    _resultatAdversaire = _nombreatrouver + Nombre_a_chercher + 1;
                                }
                                else
                                {
                                    solution = Calcul.Cherche(_listeNombre, _nombreatrouver - Nombre_a_chercher - 1);
                                    if (solution != null)
                                    {
                                        _resultatAdversaire = _nombreatrouver - Nombre_a_chercher - 1;
                                    }
                                    else
                                    {
                                        solution = Calcul.Cherche(_listeNombre, _nombreatrouver + Nombre_a_chercher + 2);
                                        if (solution != null)
                                        {
                                            _resultatAdversaire = _nombreatrouver + Nombre_a_chercher + 2;
                                        }
                                        else
                                        {
                                            solution = Calcul.Cherche(_listeNombre, _nombreatrouver - Nombre_a_chercher - 2);
                                            if (solution != null)
                                            {
                                                _resultatAdversaire = _nombreatrouver - Nombre_a_chercher - 2;
                                            }
                                            else
                                            {
                                                _resultatAdversaire = _resultatjoueur;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        #endregion

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();

            ScreenManager.SpriteBatch.DrawString(chiffre_font, solution_string, new Vector2(240 - chiffre_font.MeasureString(solution_string).X / 2, 10), Color.White);

            //BOUTONS
            ScreenManager.SpriteBatch.Draw(continuer_2D, _positionContinue, Color.White);
            ScreenManager.SpriteBatch.DrawString(continuer_font, continuer, new Vector2(_positionContinue.X + continuer_2D.Width / 2 - 20 - continuer_font.MeasureString(continuer).X / 2, _positionContinue.Y + continuer_2D.Height / 2 - continuer_font.MeasureString(continuer).Y / 2), Color.White);


            DrawInterface();
            if (!_loading)
            {
                if (pasdesolution)
                {
                    DrawMessage();
                }
                else
                {
                    DrawOperation();
                }
            }

            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawMessage()
        {
            ScreenManager.SpriteBatch.DrawString(blabla, noSolutionString, new Vector2(50, 250), Color.White);
        }

        private void DrawInterface()
        {
            ScreenManager.SpriteBatch.Draw(plaque_but, new Vector2(10, _range1 + plaqueHeight / 2), Color.White);
            ScreenManager.SpriteBatch.Draw(plaque, new Vector2(_colonne1, _range1), Color.White);
            ScreenManager.SpriteBatch.Draw(plaque, new Vector2(_colonne2, _range1), Color.White);
            ScreenManager.SpriteBatch.Draw(plaque, new Vector2(_colonne3, _range1), Color.White);
            ScreenManager.SpriteBatch.Draw(plaque, new Vector2(_colonne1, _range2), Color.White);
            ScreenManager.SpriteBatch.Draw(plaque, new Vector2(_colonne2, _range2), Color.White);
            ScreenManager.SpriteBatch.Draw(plaque, new Vector2(_colonne3, _range2), Color.White);

            string nombre = _nombreatrouver.ToString();
            string nombre1 = _listeNombre[0].ToString();
            string nombre2 = _listeNombre[1].ToString();
            string nombre3 = _listeNombre[2].ToString();
            string nombre4 = _listeNombre[3].ToString();
            string nombre5 = _listeNombre[4].ToString();
            string nombre6 = _listeNombre[5].ToString();

            Vector2 position = new Vector2(10 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre).X / 2), _range1 + plaqueHeight / 2 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre).Y / 2));
            Vector2 position1 = new Vector2(_colonne1 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre1).X / 2), _range1 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre1).Y / 2));
            Vector2 position2 = new Vector2(_colonne2 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre2).X / 2), _range1 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre2).Y / 2));
            Vector2 position3 = new Vector2(_colonne3 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre3).X / 2), _range1 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre3).Y / 2));
            Vector2 position4 = new Vector2(_colonne1 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre4).X / 2), _range2 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre4).Y / 2));
            Vector2 position5 = new Vector2(_colonne2 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre5).X / 2), _range2 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre5).Y / 2));
            Vector2 position6 = new Vector2(_colonne3 + (plaqueWidth / 2) - (chiffre_font.MeasureString(nombre6).X / 2), _range2 + (plaqueHeight / 2) - (chiffre_font.MeasureString(nombre6).Y / 2));

            ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre, position, Color.Black);
            ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre1, position1, Color.Black);
            ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre2, position2, Color.Black);
            ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre3, position3, Color.Black);
            ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre4, position4, Color.Black);
            ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre5, position5, Color.Black);
            ScreenManager.SpriteBatch.DrawString(chiffre_font, nombre6, position6, Color.Black);
        }

        private void DrawOperation()
        {
            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[0], new Vector2(_nombre1 - chiffre_font.MeasureString(Operation1[0]).X / 2, _operation1), OpacityColor(opacity_frame[0]));
            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[1], new Vector2(_action - chiffre_font.MeasureString(Operation1[1]).X / 2, _operation1), OpacityColor(opacity_frame[0]));
            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[2], new Vector2(_nombre2 - chiffre_font.MeasureString(Operation1[2]).X / 2, _operation1), OpacityColor(opacity_frame[0]));
            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[3], new Vector2(_egal, _operation1), OpacityColor(opacity_frame[0]));
            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation1[4], new Vector2(_resultat - chiffre_font.MeasureString(Operation1[4]).X / 2, _operation1), OpacityColor(opacity_frame[0]));
            if (Operation2[0] != null)
            {
                ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[0], new Vector2(_nombre1 - chiffre_font.MeasureString(Operation2[0]).X / 2, _operation2), OpacityColor(opacity_frame[2]));
                ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[1], new Vector2(_action - chiffre_font.MeasureString(Operation2[1]).X / 2, _operation2), OpacityColor(opacity_frame[2]));
                ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[2], new Vector2(_nombre2 - chiffre_font.MeasureString(Operation2[2]).X / 2, _operation2), OpacityColor(opacity_frame[2]));
                ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[3], new Vector2(_egal, _operation2), OpacityColor(opacity_frame[2]));
                ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation2[4], new Vector2(_resultat - chiffre_font.MeasureString(Operation2[4]).X / 2, _operation2), OpacityColor(opacity_frame[2]));
                if (Operation3[0] != null)
                {
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[0], new Vector2(_nombre1 - chiffre_font.MeasureString(Operation3[0]).X / 2, _operation3), OpacityColor(opacity_frame[4]));
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[1], new Vector2(_action - chiffre_font.MeasureString(Operation3[1]).X / 2, _operation3), OpacityColor(opacity_frame[4]));
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[2], new Vector2(_nombre2 - chiffre_font.MeasureString(Operation3[2]).X / 2, _operation3), OpacityColor(opacity_frame[4]));
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[3], new Vector2(_egal, _operation3), OpacityColor(opacity_frame[4]));
                    ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation3[4], new Vector2(_resultat - chiffre_font.MeasureString(Operation3[4]).X / 2, _operation3), OpacityColor(opacity_frame[4]));
                    if (Operation4[0] != null)
                    {
                        ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[0], new Vector2(_nombre1 - chiffre_font.MeasureString(Operation4[0]).X / 2, _operation4), OpacityColor(opacity_frame[6]));
                        ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[1], new Vector2(_action - chiffre_font.MeasureString(Operation4[1]).X / 2, _operation4), OpacityColor(opacity_frame[6]));
                        ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[2], new Vector2(_nombre2 - chiffre_font.MeasureString(Operation4[2]).X / 2, _operation4), OpacityColor(opacity_frame[6]));
                        ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[3], new Vector2(_egal, _operation4), OpacityColor(opacity_frame[6]));
                        ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation4[4], new Vector2(_resultat - chiffre_font.MeasureString(Operation4[4]).X / 2, _operation4), OpacityColor(opacity_frame[6]));

                        if (Operation5[0] != null)
                        {
                            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[0], new Vector2(_nombre1 - chiffre_font.MeasureString(Operation5[0]).X / 2, _operation5), OpacityColor(opacity_frame[8]));
                            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[1], new Vector2(_action - chiffre_font.MeasureString(Operation5[1]).X / 2, _operation5), OpacityColor(opacity_frame[8]));
                            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[2], new Vector2(_nombre2 - chiffre_font.MeasureString(Operation5[2]).X / 2, _operation5), OpacityColor(opacity_frame[8]));
                            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[3], new Vector2(_egal, _operation5), OpacityColor(opacity_frame[8]));
                            ScreenManager.SpriteBatch.DrawString(chiffre_font, Operation5[4], new Vector2(_resultat - chiffre_font.MeasureString(Operation5[4]).X / 2, _operation5), OpacityColor(opacity_frame[8]));
                        }
                    }
                }
            }
        }
        private static Color OpacityColor(float frame)
        {
            return Color.White * (frame / 20.0f);
        }
    }
}
