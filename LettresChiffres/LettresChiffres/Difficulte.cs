using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LettresChiffres
{
    class Difficulte : MenuScreen
    {
        int _typepartie;
        int _nbrManche;
        public Difficulte(int typepartie, int NbrManche)
            : base("Main")
        {
            MenuEntry a = new MenuEntry("BLANC");
            MenuEntry b = new MenuEntry("VERT");
            MenuEntry c = new MenuEntry("BLEU");
            MenuEntry d = new MenuEntry("JAUNE");
            MenuEntry e = new MenuEntry("ORANGE");
            MenuEntry f = new MenuEntry("RETOUR");

            a.Selected += StartBlancEntrySelected;
            b.Selected += StartVertEntrySelected;
            c.Selected += StartBleuEntrySelected;
            d.Selected += StartJauneEntrySelected;
            e.Selected += StartOrangeEntrySelected;
            f.Selected += StartRetourEntrySelected;

            MenuEntries.Add(a);
            MenuEntries.Add(b);
            MenuEntries.Add(c);
            MenuEntries.Add(d);
            MenuEntries.Add(e);
            MenuEntries.Add(f);

            _typepartie = typepartie;
            _nbrManche = NbrManche;

        }
        void StartBlancEntrySelected(object sender, EventArgs e)
        {
           ScreenManager.AddScreen(new DebutMatch(_typepartie,_nbrManche,1));
        }

        void StartVertEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 2));
        }
        void StartBleuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 3));
        }
        void StartJauneEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 4));
        }
        void StartOrangeEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 5));
        }
        void StartRetourEntrySelected(object sender, EventArgs e)
        {

            this.ExitScreen();
            ScreenManager.AddScreen(new NombreDeManche(_typepartie));
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
                ScreenManager.AddScreen(new NombreDeManche(_typepartie));
                
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
