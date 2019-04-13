using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XMLObject;

namespace LettresChiffres
{
    class PartieRapideScreen : MenuScreen
    {
        string lettre, chiffre, menu, retour;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public PartieRapideScreen(string entrainement)
            : base(entrainement)
        {

        }

        private void InitilizeLanguages()
        {
            lettre = lang.AffectationLANG("Lettres_String", langue);
            chiffre = lang.AffectationLANG("Chiffres_String", langue);
            menu = lang.AffectationLANG("Menu", langue);
            retour = lang.AffectationLANG("Retour", langue);


            MenuEntry startLettresEntry = new MenuEntry(lettre);
            MenuEntry startChiffresEntry = new MenuEntry(chiffre);
            MenuEntry retourEntry = new MenuEntry(retour.ToUpper());

            startLettresEntry.Selected += StartLettresEntrySelected;
            startChiffresEntry.Selected += StartChiffresEntrySelected;
            retourEntry.Selected += retourEntrySelected;

            MenuEntries.Add(startLettresEntry);
            MenuEntries.Add(startChiffresEntry);
            MenuEntries.Add(retourEntry);
        }

        public override void LoadContent()
        {
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            
            base.LoadContent();
        }

        void StartLettresEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
           ScreenManager.AddScreen(new Lettres.Lettres(2));
        }

        void StartChiffresEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new Chiffres(1));
        }

        void retourEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(menu));
        }

        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

            foreach (var entry in MenuEntries)
            {
                var position = entry.Position;
                position.Y += 60;
                entry.Position = position;
            }
        }
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen(menu));
                
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
