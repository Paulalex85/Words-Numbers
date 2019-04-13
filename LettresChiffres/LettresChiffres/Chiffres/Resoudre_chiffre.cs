using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LettresChiffres
{
    class Resoudre
    {
        List<int> l0;
        List<List<int>> l1;
        List<List<int>> l2;
        List<List<int>> l3;
        List<List<int>> l4;
        List<List<int>> l5;
        List<int> l6 = new List<int>() { 0, 0, 0, 0, 0, 0 };
        public string operation_resultat1;
        public string operation_resultat2 = "";
        public string operation_resultat3 ="";
        public string operation_resultat4= "";
        public string operation_resultat5 = "";
        public string operation_fin;

        public void Start_Resolution(List<int> nombre, int nombreatrouver)
        {
            l6.Clear();
            l6.Insert(0, nombre[0]);
            l6.Insert(1, nombre[1]);
            l6.Insert(2, nombre[2]);
            l6.Insert(3, nombre[3]);
            l6.Insert(4, nombre[4]);
            l6.Insert(5, nombre[5]);
            try
            {
                l5 = gen(l6);
                l4 = gen(l5);
                l3 = gen(l4);
                l2 = gen(l3);
                l1 = gen(l2);
                l0 = convert_gen(l1);


                int ValeurAtrouver = nombreatrouver;

                int q1 = l0.IndexOf(ValeurAtrouver, 0, l0.Count);
                if (q1 != -1)
                {
                    int q2 = (q1 - q1 % 4) / 4;
                    int q3 = (q2 - q2 % 12) / 12;
                    int q4 = (q3 - q3 % 24) / 24;
                    int q5 = (q4 - q4 % 40) / 40;
                    int q6 = (q5 - q5 % 60) / 60;


                    operation_fin = "Affichage Basic : " + l0[q1];
                    operation_resultat5 = Affiche_operation(l2[q2][0], l2[q2][1], q1 % 4 + 1);
                    operation_resultat4 = Affiche_operation(Dif(l3[q3], l2[q2])[0], Dif(l3[q3], l2[q2])[1], q2 % 4 + 1);
                    operation_resultat3 = Affiche_operation(Dif(l4[q4], l3[q3])[0], Dif(l4[q4], l3[q3])[1], q3 % 4 + 1);
                    operation_resultat2 = Affiche_operation(Dif(l5[q5], l4[q4])[0], Dif(l5[q5], l4[q4])[1], q4 % 4 + 1);
                    operation_resultat1 = Affiche_operation(Dif(l6, l5[q5])[0], Dif(l6, l5[q5])[1], q5 % 4 + 1);
                }
            }
            catch (System.OutOfMemoryException)
            {
                operation_resultat2 = "Impossible";
                operation_resultat3 = "de trouver";
                operation_resultat4 = "une combinaison";
            }

        }




        private List<List<int>> gen(List<int> entry)
        {
            List<List<int>> result = new List<List<int>>();

            for (int i = 0; i < entry.Count; i++)
            {
                for (int j = i + 1; j < entry.Count; j++)
                {

                    List<int> list_add = new List<int>();
                    List<int> list_mult = new List<int>();
                    List<int> list_sub1 = new List<int>();
                    List<int> list_sub2 = new List<int>();

                    list_add.Add(entry[i] + entry[j]);
                    list_mult.Add(entry[i] * entry[j]);
                    list_sub1.Add(entry[i] - entry[j]);
                    list_sub2.Add(entry[j] - entry[i]);

                    for (int k = 0; k < entry.Count; k++)
                        if (k != i && k != j)
                        {
                            list_add.Add(entry[k]);
                            list_mult.Add(entry[k]);
                            list_sub1.Add(entry[k]);
                            list_sub2.Add(entry[k]);
                        }

                    result.Add(list_add);
                    result.Add(list_mult);
                    result.Add(list_sub1);
                    result.Add(list_sub2);
                }
            }

            return result;
        }

        private List<List<int>> gen(List<List<int>> entry)
        {
            List<List<int>> result = new List<List<int>>();
            for (int i = 0; i < entry.Count; i++)
                result.AddRange(gen(entry[i]));
            return result;
        }

        private List<int> convert_gen(List<List<int>> entry)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < entry.Count; i++)
                result.AddRange(entry[i]);
            return result;
        }

        private List<int> Dif(List<int> l1, List<int> l2)
        {
            List<int> ss = new List<int>();
            List<int> others = new List<int>();


            for (int i = 0; i < l1.Count; i++)
            {
                for (int j = 0; j < l2.Count; j++)
                    if (l1[i] == l2[j])
                    {
                        others.Add(l1[i]);   // Le autres nombres
                        break;
                    }
                    else if (j == l2.Count - 1 && l2[j] != l1[i])
                        ss.Add(l1[i]);  //  (Les 2 nombres qui vont subir une opération)
            }

            ss.AddRange(others);

            return ss;
        }


        private string Affiche_operation(int ch1, int ch2, int opp)
        {
            if (opp == 1)
                return ch1 + " + " + ch2 + " = " + (ch1 + ch2);
            else if (opp == 2)
                return ch1 + " * " + ch2 + " = " + (ch1 * ch2);
            else if (opp == 3 || opp == 4)
                if (ch1 > ch2)
                    return ch1 + " - " + ch2 + " = " + (ch1 - ch2);
                else
                    return ch2 + " - " + ch1 + " = " + (ch2 - ch1);
            else
                return "NONE";
        }
        private string operation(int opp)
        {
            if (opp == 1)
                return "+";
            else if (opp == 2)
                return "*";
            else if (opp == 3 || opp == 4)
                return "-";
            else
                return "NONE";
        }

    }
}
