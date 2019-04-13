using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XMLObject;


namespace LettresChiffres
{
    class NombreDeManche : MenuScreen
    {
        int _typepartie;
        string retour, difficiculte, newgame, manches;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public NombreDeManche(int typepartie, string manches)
            : base(manches)
        {  
            _typepartie = typepartie;
        }

        private void InitilizeLanguages()
        {
            retour = lang.AffectationLANG("Retour", langue);
            difficiculte = lang.AffectationLANG("Difficult", langue);
            newgame = lang.AffectationLANG("NewGame", langue);
            manches = lang.AffectationLANG("Manches", langue);

            MenuEntry a = new MenuEntry("3 " + manches);
            MenuEntry b = new MenuEntry("6 " + manches);
            MenuEntry c = new MenuEntry("9 " + manches);
            MenuEntry d = new MenuEntry("12 " + manches);
            MenuEntry e = new MenuEntry(retour.ToUpper());

            a.Selected += Start3EntrySelected;
            b.Selected += Start6EntrySelected;
            c.Selected += Start9EntrySelected;
            d.Selected += Start12EntrySelected;
            e.Selected += RetourSelected;

            MenuEntries.Add(a);
            MenuEntries.Add(b);
            MenuEntries.Add(c);
            MenuEntries.Add(d);
            MenuEntries.Add(e);

        }

        public override void LoadContent()
        {
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            
            base.LoadContent();
        }

        void Start3EntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DifficulteScreen(_typepartie, 3, difficiculte));
        }

        void Start6EntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DifficulteScreen(_typepartie, 6, difficiculte));
        }
        void Start9EntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DifficulteScreen(_typepartie, 9, difficiculte));
        }
        void Start12EntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DifficulteScreen(_typepartie, 12, difficiculte));
        }
        void RetourSelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new NouvellePartieScreen(newgame));
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
                ScreenManager.AddScreen(new NouvellePartieScreen(newgame));
                
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
