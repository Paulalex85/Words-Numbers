using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LettresChiffres
{
    class Etat
    {
        public int[] tab;
        public Etat(int[] tab)
        {

            this.tab = (int[])tab.Clone();
        }
        public static bool operator ==(Etat a, Etat b)
        {
            int dim1 = a.tab.Length;
            int dim2 = b.tab.Length;
            if (dim1 != dim2) return false;
            for (int i = 0; i < dim1; i++)
                if (a.tab[i] != b.tab[i]) return false;
            return true;
        }
        public static bool operator !=(Etat a, Etat b)
        {
            int dim1 = a.tab.Length;
            int dim2 = b.tab.Length;
            if (dim1 != dim2) return true;
            for (int i = 0; i < dim1; i++)
                if (a.tab[i] != b.tab[i]) return true;
            return false;

        }

        public override bool Equals(object obj)
        {
            // Et1.Equals(null) retourne false
            if (obj == null) return false;

            // Si l'objet ne peut être casté en Etat ex et1.Equals(3) ça retourne false
            Etat et = obj as Etat;
            if ((System.Object)et == null) return false;

            // Retourne true si tous les champs sont égaux:

            int dim1 = et.tab.Length;
            int dim2 = this.tab.Length;
            if (dim1 != dim2) return false;
            for (int i = 0; i < dim1; i++)
                if (et.tab[i] != this.tab[i]) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int aa = tab[0];
            foreach (int i in tab) aa += i << 2;
            return aa;
        }

        public override string ToString()
        {
            string s = string.Empty; ;
            foreach (int i in tab) s += i.ToString().PadRight(7) + ":";
            return s;
        }
    
    }
}
