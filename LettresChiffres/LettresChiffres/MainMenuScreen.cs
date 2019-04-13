using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XMLObject;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.GamerServices;

namespace LettresChiffres
{
    class MainMenuScreen : MenuScreen
    {
        string entrainement, jouer, quitter, newgame, joueur, changer_nom, _nom_joueur, lettre_max;
        Languages lang = new Languages();
        Langues langue = new Langues();

        bool start = true;

        public MainMenuScreen(string menu)
            : base(menu)
        {

        }

        private void InitilizeLanguages()
        {
            entrainement = lang.AffectationLANG("Entrainement", langue);
            jouer = lang.AffectationLANG("Jouer", langue);
            quitter = lang.AffectationLANG("Quitter", langue);
            newgame = lang.AffectationLANG("NewGame", langue);
            joueur = lang.AffectationLANG("Joueur_String", langue);
            changer_nom = lang.AffectationLANG("Menu_changer_nom", langue);
            lettre_max = lang.AffectationLANG("Menu_changer_nom_lettres_max", langue);


            MenuEntry startRapideGameMenuEntry = new MenuEntry(entrainement);
            MenuEntry startGameMenuEntry = new MenuEntry(jouer);
            MenuEntry Changer_nom = new MenuEntry(changer_nom);
            MenuEntry exitMenuEntry = new MenuEntry(quitter.ToUpper());

            startRapideGameMenuEntry.Selected += StartRapideGameMenuEntrySelected;
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            Changer_nom.Selected += ChangerNom;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(startRapideGameMenuEntry);
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(Changer_nom);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void LoadContent()
        {
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            
            base.LoadContent();
        }

        void StartRapideGameMenuEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new PartieRapideScreen(entrainement)); 
        }

        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new NouvellePartieScreen(newgame));
        }

        void ChangerNom(object sender, EventArgs e)
        {
            Guide.BeginShowKeyboardInput(PlayerIndex.One, changer_nom, lettre_max, _nom_joueur, new AsyncCallback(gotText), null);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }

        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

            foreach ( var entry in MenuEntries)
            {
                var position = entry.Position;
                position.Y += 60;
                entry.Position = position;
                }
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.OnCancel(PlayerIndex.One);

            if (start) { InitializePlayerName(); }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void gotText(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                string caca = Guide.EndShowKeyboardInput(result);
                if (caca != null && caca.Count() < 11)
                {
                    IsolatedStorageSettings.ApplicationSettings["player1"] = Guide.EndShowKeyboardInput(result);
                }
            }
        }
        private void InitializePlayerName()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("player1"))
            {
                IsolatedStorageSettings.ApplicationSettings["player1"] = joueur;
            }
            else
            {
                _nom_joueur = (string)IsolatedStorageSettings.ApplicationSettings["player1"];
            }
            start = false;
        }
    }
}
