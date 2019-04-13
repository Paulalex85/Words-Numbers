using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LettresChiffres
{
    class Calcul
    {
        static Dictionary<Etat, int> Dic;
        static int[][] MatCalc; // Matrice de Calcul
        static int Chiffre;
        static int Dif;
        static long NbCombi;
        static bool DicActive;
        static int[][] MatRes;
        static string resultatdemerde;



        struct Resultat
        {
            public int a1;
            public int a2;
            public int op;
        }
        static Resultat[] tResultat;
        static int Profondeur; // Profondeur de calcul de combinaison

        // Genere une matrice de tableaux pour les calculs
        // si tab vaut : 100 75 10 10 6 5 on genere une matrice à 5 tableaux
        // Le 1er tableau a 6 elements c'est tab
        // le 2eme a 5 elements c'est tab - 1 element correspondant  à une operation
        // ex 100+75 10 10 6 5
        // le 3eme tableau a 4 elements, le 4 eme 3 elements et le 5 eme 2 elements
        static void Init(int[] tab)
        {
            int dimtab = tab.Length;
            MatCalc = new int[dimtab - 1][];
            for (int i = 0; i < dimtab - 1; i++)
            {
                MatCalc[i] = new int[dimtab - i];
            }
            for (int i = 0; i < dimtab; i++)
                MatCalc[0][i] = tab[i];
            Profondeur = dimtab - 1;
            tResultat = new Resultat[dimtab - 1]; // le tableau ainsi que les structures sous jacentes allouées

            MatRes = new int[dimtab - 1][];
            for (int i = 0; i < dimtab - 1; i++)
                MatRes[i] = new int[4];
        }


        /// <summary>
        /// On intialise les variables puis on lance un nouveau Thread que effectue le calcul
        /// Ce lancement est necessaire pour conserver une homogenéité d'interaction avec la Form pendant le calcul notamment pour le bouton Stop
        /// </summary>
        /// <param name="tab">Les plaquettes</param>
        /// <param name="chiffre">Le chiffre à trouver avec les plaquettes</param>
        /// <param name="dicactive">On active ou pas la table de hashage</param>
        /// <param name="frm">WinForm de la boite de dialogue</param>
        public static string Cherche(int[] tab, int chiffre)
        {

            Init(tab);
            Chiffre = chiffre;
            Dif = chiffre;  // Difference entre le resultat et les valeurs de recherches
            if (DicActive) Dic = new Dictionary<Etat, int>();
            Combinaison(0);
            return resultatdemerde;
        }

        static void InvokeAfficheSolution(int curprof)
        {
            string s = "";
            int a1, a2, op, res;
            char[] cop = new char[] { '+', 'x', '-', '/' };
            for (int i = 0; i <= curprof; i++)
            {
                a1 = tResultat[i].a1;
                a2 = tResultat[i].a2;
                op = tResultat[i].op;
                s = s + a1 + " " + cop[op] + " " + a2 + " = ";
                switch (op)
                {
                    case 0: res = a1 + a2; break;
                    case 1: res = a1 * a2; break;
                    case 2: res = a1 - a2; break;
                    case 3: res = a1 / a2; break;
                    default: res = 0; break;
                }
                s += res + "|";
            }
            resultatdemerde = s;
        }


        static void Combinaison(int curprof)
        {
            int[] curtab = MatCalc[curprof];
            int curdim = curtab.Length;
            int[] tres = MatRes[curprof];  // tableau resultat pour 2 nombres
            if (DicActive && curprof < 6 && ExisteDeja(curtab)) return; // Reduit combinatoire facteur 10 à 100
            for (int i = 0; i < curdim - 1; i++)
            {
                // ex 5 5 4 3 avec i=1 on ne refait pas ce qu'on a fait avec i=0 c-a-d (5 4) et (5 3)
                if ((i > 0) && (curtab[i - 1] == curtab[i])) continue;
                for (int j = i + 1; j < curdim; j++)
                {

                    // ex 10 5 5 ou i=0 et j=2 on a déja fait (10 5) avec i=0 et j=1 
                    if ((j > i + 1) && (curtab[j - 1] == curtab[j])) continue;// pas refaite meme op 
                    Operations(curtab[i], curtab[j], tres);

                    for (int op = 0; op < 4; op++)
                    {
                        if (tres[op] == -1) continue;  // pas possible
                        NbCombi++;  // nombre d'operation
                        int curdif = Math.Abs(tres[op] - Chiffre);
                        if (curdif <= Dif)
                        {
                            Dif = curdif;

                            if (Dif == 0)
                            { // on a trouvé
                                EmpileResultat(curtab[i], curtab[j], op, curprof);
                                InvokeAfficheSolution(curprof);
                            }
                        }
                        if (curdim > 2)
                        {
                            EmpileResultat(curtab[i], curtab[j], op, curprof);
                            Transfer(tres[op], i, j, curprof);

                            Combinaison(curprof + 1);
                        }
                    }
                }

            }

        }

        // retourne un tableau de dim 2 à 4 contenant les possibilités d'opérations pour les 2 nombres
        // a1 toujours >= a2
        static void Operations(int a1, int a2, int[] tres)
        {

            tres[0] = a1 + a2;
            // pour * si a2 vaut 1 ça sert à rien val <=0 en cas de debordement 
            tres[1] = a1 * a2; if (a2 == 1 || tres[1] <= 0) tres[1] = -1;
            // pour - le resultat ne doit pas être nul
            if (a1 != a2 && (a1 - a2) > 0) tres[2] = a1 - a2; else tres[2] = -1;
            // pour / a2 doit diviser a1 et a2 est !=1 sinon ça sert à rien
            if (a2 != 0 && (a1 % a2) == 0 && a2 != 1) tres[3] = a1 / a2;
            else tres[3] = -1;
        }

        // 100 75 10 10 5 1 avec res=15, i=2, j=4, curprof=0
        // devient 100 75 15 10 1
        static void Transfer(int res, int i, int j, int curprof)
        {
            int aa = 0, a1;
            bool inseres = false;
            int suiv = curprof + 1;
            int[] curtab = MatCalc[curprof];
            for (int k = 0; k < curtab.Length; k++)
            {
                if (k == i || k == j) continue;
                a1 = curtab[k];
                if (a1 > res) MatCalc[suiv][aa++] = a1;
                else
                {
                    if (!inseres)
                    {
                        inseres = true;
                        MatCalc[suiv][aa++] = res;
                    }
                    MatCalc[suiv][aa++] = a1;
                }
            }
            if (inseres == false) // cas ou tous les elements sont sup aux resultats
                MatCalc[suiv][aa++] = res;
        }


        static void EmpileResultat(int a1, int a2, int op, int curprof)
        {
            tResultat[curprof].a1 = a1;
            tResultat[curprof].a2 = a2;
            tResultat[curprof].op = op;
        }

        /// <summary>
        /// Met à jour un dictionaire de données sur les elements déjà calculés
        /// Reduit de façon drastique la combinatoire environ entre 10 et 100 fs moins de calcul
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        static bool ExisteDeja(int[] tab)
        {
            //string s=string.Empty;

            //foreach (int i in tab) s += i + "#";
            //if (!Dic.ContainsKey(s))
            //{
            //    Dic[s] = 1;
            //    return false;
            //}
            if (tab.Length <= 3) return false;
            Etat et = new Etat(tab);

            if (!Dic.ContainsKey(et))
            {
                Dic[et] = 1;
                return false;
            }

            return true;
        }
    }
}
