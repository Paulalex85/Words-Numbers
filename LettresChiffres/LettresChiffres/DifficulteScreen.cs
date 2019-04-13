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
    class DifficulteScreen : MenuScreen
    {
        int _typepartie;
        int _nbrManche;
        string novice, facile, normal, difficile, extreme, retour, manche;
        Languages lang = new Languages();
        Langues langue = new Langues();

        public DifficulteScreen(int typepartie, int NbrManche,string titre)
            : base(titre)
        {
            _typepartie = typepartie;
            _nbrManche = NbrManche;
        }

        private void InitilizeLanguages()
        {
            novice = lang.AffectationLANG("Novice", langue);
            facile = lang.AffectationLANG("Facile", langue);
            normal = lang.AffectationLANG("Normal", langue);
            difficile = lang.AffectationLANG("Difficile", langue);
            extreme = lang.AffectationLANG("Extreme", langue);
            retour = lang.AffectationLANG("Retour", langue);
            manche = lang.AffectationLANG("Manches", langue);


            MenuEntry a = new MenuEntry(novice);
            MenuEntry b = new MenuEntry(facile);
            MenuEntry c = new MenuEntry(normal);
            MenuEntry d = new MenuEntry(difficile);
            MenuEntry e = new MenuEntry(extreme);
            MenuEntry f = new MenuEntry(retour.ToUpper());

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
        }
    

        void StartBlancEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
           ScreenManager.AddScreen(new DebutMatch(_typepartie,_nbrManche,1));
        }
        void StartVertEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 2));
        }
        void StartBleuEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 3));
        }
        void StartJauneEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 4));
        }
        void StartOrangeEntrySelected(object sender, EventArgs e)
        {
            this.ExitScreen();
            ScreenManager.AddScreen(new DebutMatch(_typepartie, _nbrManche, 5));
        }
        void StartRetourEntrySelected(object sender, EventArgs e)
        {

            this.ExitScreen();
            ScreenManager.AddScreen(new NombreDeManche(_typepartie,manche));
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

        public override void LoadContent()
        {
            langue = ScreenManager.Game.Content.Load<Langues>(lang.path + "LANG");
            InitilizeLanguages();
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.ExitScreen();
                ScreenManager.AddScreen(new NombreDeManche(_typepartie,manche));
                
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
