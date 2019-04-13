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
using System.IO;

namespace LettresChiffres
{
    class DebutMatch : GameScreen
    {
        Texture2D retour;
        Texture2D valider;

        SpriteFont transition_font;

        MouseState state = Mouse.GetState();
        MouseState previousState;

        int _typepartie, _nbrmanche, _difficulte, _roundMatch, _nbrMancheJouer;
        string typepartieString;
        string DifficulteString;
        string _coupeString;
        bool chargement = false;
        bool caca = true;
        bool start = false;

        string Simple_string, Coupe_string, Difficulte_string, novice_string, facile_string, normal_string, difficile_string, extrem_string, commencer_string, retour_string;
        string qualification, huitieme, quart, demi, finale, manches, NBR_manches, adversaire;
        string menu, nouvellepartie;
        List<string> ListeNoms;

        Vector2 positionRetour = new Vector2(30, 600);
        Vector2 positionCommencer = new Vector2(250, 600);
        Vector2 positionResume = new Vector2(20, 150);
        Vector2 positionDfficulte = new Vector2(20, 250);
        Vector2 positionManches = new Vector2(20, 350);
        Vector2 positionAdversaire = new Vector2(20, 450);
        Vector2 positionCoupe = new Vector2(20, 550);

        Random random = new Random();
        XML_Serializer xmls = new XML_Serializer();
        Sauvegarde save;
        Languages lang = new Languages();
        Langues langue = new Langues();
        string _nomAdversaire;

        public DebutMatch(int typepartie, int Nbrmanche, int difficulte)
        {
            _typepartie = typepartie;
            _nbrmanche = Nbrmanche;
            _difficulte = difficulte;

            caca = false;
        }

        public DebutMatch(int typepartie, bool _caca)
        {
            start = true;
            _typepartie = typepartie;
            caca = _caca;
        }

        private void InitilizeLanguages()
        {
            ListeNoms = lang.NomsAdversaires();


            Simple_string = lang.AffectationLANG("Simple_string", langue);
            Coupe_string = lang.AffectationLANG("Coupe_string", langue);
            Difficulte_string = lang.AffectationLANG("Difficulte_String", langue);
            novice_string = lang.AffectationLANG("Novice", langue);
            facile_string = lang.AffectationLANG("Facile", langue);
            normal_string = lang.AffectationLANG("Normal", langue);
            difficile_string = lang.AffectationLANG("Difficile", langue);
            extrem_string = lang.AffectationLANG("Extreme", langue);
            commencer_string = lang.AffectationLANG("Commencer", langue);
            retour_string = lang.AffectationLANG("Retour", langue);
            qualification = lang.AffectationLANG("Qualification", langue);
            huitieme = lang.AffectationLANG("Huitieme", langue);
            quart = lang.AffectationLANG("Quart", langue);
            demi = lang.AffectationLANG("Demi", langue);
            finale = lang.AffectationLANG("Finale", langue);
            manches = lang.AffectationLANG("Manches", langue);
            NBR_manches = lang.AffectationLANG("NBR_manches", langue);
            adversaire = lang.AffectationLANG("Adversaire", langue);
            nouvellepartie = lang.AffectationLANG("NewGame", langue);
            menu = lang.AffectationLANG("Menu", langue);

            if (!caca)
            {
                _nomAdversaire = AleatoireNom();
                if (_typepartie == 1)
                {
                    typepartieString = Simple_string;
                }
                else
                {
                    typepartieString = Coupe_string;
                }
                switch (_difficulte)
                {
                    case 1: DifficulteString = Difficulte_string + " : " + novice_string; break;
                    case 2: DifficulteString = Difficulte_string + " : " + facile_string; break;
                    case 3: DifficulteString = Difficulte_string + " : " + normal_string; break;
                    case 4: DifficulteString = Difficulte_string + " : " + difficile_string; break;
                    case 5: DifficulteString = Difficulte_string + " : " + extrem_string; break;
                }
                if (_typepartie == 2)
                {
                    PositionCoupe(1);
                }
            }
        }

        public override void LoadContent()
        {
            retour = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_retour_bis");
            valider = ScreenManager.Game.Content.Load<Texture2D>("Bouton/bouton_valider");

            transition_font = ScreenManager.Game.Content.Load<SpriteFont>("Transition");
            if (start)
            {
                save = xmls.DeserializeSauvegarde();
            }
            else
            {
                save = ScreenManager.Game.Content.Load<Sauvegarde>("Sauvegardes");
            }

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
            if (start)
            {
                Initialize();
            }
            if (save.ListeSave.NomAdversaire == "xxx")
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new NouvellePartieScreen(true,nouvellepartie));
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
            }
            Input();
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void Initialize()
        {
            if (caca)
            {
                _difficulte = save.ListeSave.Difficulte;
                _nbrmanche = save.ListeSave.NombreDeManches;
                _nbrMancheJouer = 1;
                _roundMatch = save.ListeSave.RoundMatch;
                _roundMatch++;
                PositionCoupe(_roundMatch);
                _nomAdversaire = AleatoireNom();
                if (_typepartie == 1)
                {
                    typepartieString = Simple_string;
                }
                else
                {
                    typepartieString = Coupe_string;
                }
            }
            else
            {
                _difficulte = save.ListeSave.Difficulte;
                _nbrmanche = save.ListeSave.NombreDeManches;
                _nbrMancheJouer = save.ListeSave.NombreManchesJouer;
                _roundMatch = save.ListeSave.RoundMatch;
                PositionCoupe(_roundMatch);
                if (_typepartie == 3)
                {
                    chargement = true;
                }
                if (chargement)
                {
                    _typepartie = save.ListeSave.TypeMatch;
                    _nomAdversaire = save.ListeSave.NomAdversaire;
                    if (_typepartie == 1)
                    {
                        typepartieString = Simple_string;
                    }
                    else
                    {
                        typepartieString = Coupe_string;
                    }
                }
                else
                {
                    _nomAdversaire = AleatoireNom();
                }
            }

            switch (_difficulte)
            {
                case 1: DifficulteString = Difficulte_string + " : " + novice_string; break;
                case 2: DifficulteString = Difficulte_string + " : " + facile_string; break;
                case 3: DifficulteString = Difficulte_string + " : " + normal_string; break;
                case 4: DifficulteString = Difficulte_string + " : " + difficile_string; break;
                case 5: DifficulteString = Difficulte_string + " : " + extrem_string; break;
            }

            start = false;
        }

        private void Input()
        {
            state = Mouse.GetState();
            if (previousState.LeftButton == ButtonState.Pressed &&
                state.LeftButton == ButtonState.Released)
            {
                if (state.X > positionRetour.X && state.X < positionRetour.X + retour.Width &&
                        state.Y > positionRetour.Y && state.Y < positionRetour.Y + retour.Height)
                {
                    this.ExitScreen();
                    ScreenManager.AddScreen(new NouvellePartieScreen(nouvellepartie));
                }
                if (caca)
                {
                    if (state.X > positionCommencer.X && state.X < positionCommencer.X + valider.Width &&
                            state.Y > positionCommencer.Y && state.Y < positionCommencer.Y + valider.Height)
                    {
                        CreateSauvegardeTournoi();
                        this.ExitScreen();
                        ScreenManager.AddScreen(new Score());
                    }
                }
                else if (!chargement && !caca)
                {
                    if (state.X > positionCommencer.X && state.X < positionCommencer.X + valider.Width &&
                            state.Y > positionCommencer.Y && state.Y < positionCommencer.Y + valider.Height)
                    {
                        CreateSauvegarde();
                        this.ExitScreen();
                        ScreenManager.AddScreen(new Score());
                    }
                }
                else if (chargement)
                {
                    if (state.X > positionCommencer.X && state.X < positionCommencer.X + valider.Width &&
                            state.Y > positionCommencer.Y && state.Y < positionCommencer.Y + valider.Height)
                    {
                        this.ExitScreen();
                        ScreenManager.AddScreen(new Score());
                    }
                }
            }
            previousState = state;
        }

        private void PositionCoupe(int manche)
        {
            switch (manche)
            {
                case 1: _coupeString = qualification; break;
                case 2: _coupeString = huitieme; break;
                case 3: _coupeString = quart; break;
                case 4: _coupeString = demi; break;
                case 5: _coupeString = finale; break;
            }
        }

        private void CreateSauvegardeTournoi()
        {
            Parties newSave = new Parties()
            {
                TypeMatch = _typepartie,
                RoundMatch = _roundMatch,
                NombreDeManches = _nbrmanche,
                NombreManchesJouer = 1,
                Difficulte = _difficulte,
                ScoreJoueur = "99",
                ScoreAdversaire = "99",
                NomAdversaire = _nomAdversaire
            };

            Sauvegarde save = new Sauvegarde();
            save.ListeSave = newSave;
            xmls.SerialiseSauvegarde(save);
        }

        private void CreateSauvegarde()
        {

            Parties newSave = new Parties()
            {
                TypeMatch = _typepartie,
                RoundMatch = 1,
                NombreDeManches = _nbrmanche,
                NombreManchesJouer = 1,
                Difficulte = _difficulte,
                ScoreJoueur = "99",
                ScoreAdversaire = "99",
                NomAdversaire = _nomAdversaire
            };

            Sauvegarde save = new Sauvegarde();
            save.ListeSave = newSave;
            xmls.SerialiseSauvegarde(save);



        }

        private string AleatoireNom()
        {
            int pd = random.Next(0,ListeNoms.Count());
            return ListeNoms.ElementAt(pd);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();


            if (_typepartie == 2)
            {
                if (typepartieString != null) { ScreenManager.SpriteBatch.DrawString(transition_font, typepartieString + " : " + _coupeString, positionResume, Color.White); }
            }
            else
            {
                if (typepartieString != null) { ScreenManager.SpriteBatch.DrawString(transition_font, typepartieString, positionResume, Color.White); }
            }
            if (DifficulteString != null) { ScreenManager.SpriteBatch.DrawString(transition_font, DifficulteString, positionDfficulte, Color.White); }
            if (chargement)
            {
                ScreenManager.SpriteBatch.DrawString(transition_font, manches + " : " + _nbrMancheJouer + "/" + _nbrmanche, positionManches, Color.White);
            }
            else
            {
                ScreenManager.SpriteBatch.DrawString(transition_font, NBR_manches + " : " + _nbrmanche, positionManches, Color.White);
            }
            ScreenManager.SpriteBatch.DrawString(transition_font, adversaire + " : " + _nomAdversaire, positionAdversaire, Color.White);

            //BOUTONS
            ScreenManager.SpriteBatch.Draw(retour, positionRetour, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, retour_string, new Vector2(positionRetour.X + 120 - transition_font.MeasureString(retour_string).X / 2, positionRetour.Y + retour.Height / 2 - transition_font.MeasureString(retour_string).Y/2), Color.White);

            ScreenManager.SpriteBatch.Draw(valider, positionCommencer, Color.White);
            ScreenManager.SpriteBatch.DrawString(transition_font, commencer_string, new Vector2(positionCommencer.X  + valider.Width / 2 - 20 - transition_font.MeasureString(commencer_string).X / 2, positionCommencer.Y + valider.Height / 2 - transition_font.MeasureString(commencer_string).Y / 2), Color.White);

            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
