using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapaa
{
    public class Starting
    {
        List<Wspolrzedne> wspolrzedne = new List<Wspolrzedne>();
        List<Polaczenie> pol;
        Wczytywanie w;
        Form1 f;
        public int iloscSamochodow = 2;
        public int pojemnoscSamochodu = 2;
        //public String samochod = "-";

        public Starting(Form1 f)
        {
            this.f = f;
        }
        public void Start(String plik)
        {
            int[,] mapp = null;
            try
            {
                w = new Wczytywanie(plik, f);
                w.ReadData();
                w.ReadPackages();
                Tworzenie tworzenie = new Tworzenie(w);//na podstawie zebranych danych tworzy mapę miast dla potrzeb programu
                Kopiec kopiec = new Kopiec();
                mapp = tworzenie.utworzMape();//metoda tworząca mapę, zwraca dwuwymiarową tablicę - mapę - mapp
                List<Zlecenie> z = w.DajZlecenia();//pobiera listę zleceń, (obiekty), po wczytaniu
                kopiec.wprowadzDaneDoKopca(z);//zapis obiektów powyższej listy na kopiec
                //===========================================================================            
                Dijkstra dijkstra = new Dijkstra(mapp);
                Wezel[,] wezel = dijkstra.obliczOdlegloscPomiedzyMiastami(5);
                //wezel = dijkstra.obliczOdlegloscPomiedzyMiastami(0);
                List<Miasto> miasta = w.DajMiasta();// wczytana lista miast
                int xx = 20, y = 145;
                int wymiar = (int)(Math.Sqrt(miasta.Count)) + 1;
                int skokx = 800 / wymiar;
                int skoky = 800 / wymiar;
                int ii = 0;

                while (ii < miasta.Count)
                {
                    Random rnd2 = new Random();
                    xx = 20 + rnd2.Next(-15, 65);
                    while (xx < 800)
                    {
                        Random rnd = new Random();
                        int h = rnd.Next(-15, 65);
                        if (ii == w.dajMiastoStartowe())
                            f.DrawVertices(1, (ii + 1).ToString(), xx, y + h);
                        else
                            f.DrawVertices(-1, (ii + 1).ToString(), xx, y + h);
                        wspolrzedne.Add(new Wspolrzedne(xx, y + h));
                        xx = xx + skokx;
                        ii++;
                        if (ii == miasta.Count)
                            break;
                    }
                    y = y + skoky;
                }
                pol = w.DajPolaczenia();//odczytujemy połączenia
                for (int aaa = 0; aaa < pol.Count; aaa++)//rysuje połączenia na czarno; początek;
                {
                    f.DrawConnection(0, wspolrzedne[pol[aaa].dajPoczatek()].dajA() + 12, wspolrzedne[pol[aaa].dajPoczatek()].dajB() + 12,
                        wspolrzedne[pol[aaa].dajCel()].dajA() + 12, wspolrzedne[pol[aaa].dajCel()].dajB() + 12);
                }
                Przewoz przewoz = new Przewoz();
                List<int> powrot = przewoz.dajWierzcholkiPoDrodze();
                //===============================
                FlotaSamochodow flotaaa = new FlotaSamochodow();
                int iloscPaczek = z.Count;
                //Console.WriteLine(iloscPaczek);
                flotaaa.utworzFlote(iloscSamochodow);
                Jedz jedz = new Jedz(f, wspolrzedne, this, w); //int yy = 1;
                List<Zlecenie> zleceniaZKopca = kopiec.dajKopiec();

                for (int x = 0; x < iloscPaczek / (pojemnoscSamochodu * iloscSamochodow); x++)//iloscPaczek / (pojemnoscSamochodu * iloscSamochodow)
                {
                    //Console.WriteLine(x);
                    for (int i = 0; i < pojemnoscSamochodu; i++)
                        for (int j = 0; j < iloscSamochodow; j++)
                        {
                            //Console.WriteLine(yy); yy++;
                            flotaaa.dodajZlecenieDoSamochodu(kopiec.sciagnijZWierzcholkaKopca(), j);
                        }
                    //rozwieź i wróć
                    jedz.rozwiez(flotaaa, iloscSamochodow, mapp, w.DajIloscMiast() - 2, miasta);
                    f.kom("Flota " + iloscSamochodow.ToString() + " samochodów rozwiozła następne " + (iloscSamochodow * pojemnoscSamochodu).ToString() + " paczek.");
                    //f.DrawVertices(2, (1 + 1).ToString(), 20, 50 + 10);                    
                }
                z = kopiec.dajKopiec();
                Console.WriteLine(z.Count);
                iloscSamochodow = z.Count / pojemnoscSamochodu;
                for (int n = 0; n < iloscSamochodow; n++)
                    flotaaa.flota[n].dajSamochodZPaczkami().Clear();
                for (int i = 0; i < pojemnoscSamochodu; i++)
                    for (int j = 0; j < iloscSamochodow; j++)
                    {
                        flotaaa.dodajZlecenieDoSamochodu(kopiec.sciagnijZWierzcholkaKopca(), j);
                    }
                //rozwieź i wróć
                jedz.rozwiez(flotaaa, iloscSamochodow, mapp, w.DajIloscMiast() - 2, miasta);

                //f.DrawVertices(2, (1 + 1).ToString(), 20, 50 + 10);  

                z = kopiec.dajKopiec();
                Console.WriteLine(z.Count);

                for (int n = 0; n < iloscSamochodow; n++)
                    flotaaa.flota[n].dajSamochodZPaczkami().Clear();
                iloscSamochodow = 1;
                flotaaa.utworzFlote(iloscSamochodow);
                Zlecenie e = kopiec.sciagnijZWierzcholkaKopca();
                for (int i = 0; i < pojemnoscSamochodu; i++)
                    for (int j = 0; j < iloscSamochodow; j++)
                    {
                        //Console.WriteLine(yy); yy++;
                        e = kopiec.sciagnijZWierzcholkaKopca();
                        if (e == null) break;
                        flotaaa.dodajZlecenieDoSamochodu(e, j);
                    }
                //rozwieź i wróć
                if (e != null)
                {
                    jedz.rozwiez(flotaaa, iloscSamochodow, mapp, w.DajIloscMiast() - 2, miasta);
                    //f.DrawVertices(2, (ii + 1).ToString(), xx, y + 10);
                }
                //===========================================================================
            }
            catch (IndexOutOfRangeException)
            {
                //f.PerformCalculations("Nie podano pliku wejściowego!!!", "Fatal error");
            }
            //Console.Write("Naciśnij dowolny przycisk, aby zakończyć program...");
            //Console.ReadKey();
            f.kom("Program zakończył działanie :)");
            }
            public void refresh(List<Miasto> miasta)
            {
                int ii = 0;
                while (ii < miasta.Count)
                {
                    if (ii == w.dajMiastoStartowe())
                        f.DrawVertices(1, (ii + 1).ToString(), wspolrzedne[ii].dajA(), wspolrzedne[ii].dajB());
                    else
                        f.DrawVertices(-1, (ii + 1).ToString(), wspolrzedne[ii].dajA(), wspolrzedne[ii].dajB());
                    ii++;
                }
                pol = w.DajPolaczenia();//odczytujemy połączenia
                for (int aaa = 0; aaa < pol.Count; aaa++)//rysuje połączenia na czarno; początek;
                {
                    f.DrawConnection(0, wspolrzedne[pol[aaa].dajPoczatek()].dajA() + 12, wspolrzedne[pol[aaa].dajPoczatek()].dajB() + 12,
                        wspolrzedne[pol[aaa].dajCel()].dajA() + 12, wspolrzedne[pol[aaa].dajCel()].dajB() + 12);
                }
            }
        }
    }













/*sciąga z kopca dane
 * ================================================================
 * foreach (Zlecenie value in z)
            {
                Zlecenie d = kopiec.sciagnijZWierzcholkaKopca();
                Console.WriteLine(d.dajPriorytet());
            }
 * 
 * =========================================================
 * for (int x = 0; x < w.DajIloscMiast(); x++)
            {
                for (int y = 0; y < w.DajIloscMiast(); y++)
                {
                    Console.Write(mapp[x, y] + " ");
                }
                Console.WriteLine();
 * ==============================================
 * kod do prostego testowania ściągania z kopca
 * ============================================
 *  List<int> tab = new List<int>();
            tab.Add(10);
            tab.Add(8);
            tab.Add(5);
            tab.Add(-1);
            tab.Add(2);
            tab.Add(4);
            tab.Add(4);
            
            for (int r = 0; r < 7; r++)
            {
                //foreach (int value in tab)
                   // Console.Write(value + ",");
                //Console.WriteLine();
                int szczyt = tab[0];//zapamiętuję korzeń, tzn ten który zabieram
                int top = tab[0];
                tab[0] = tab[tab.Count - 1];//zamieniam z ostatnim liściem                
                tab.RemoveAt(tab.Count - 1);//usuwam liść
                int n = 0;
                int maks = -1;

                while (n < tab.Count )
                {
                    int t = 2*n+1;
                    

                    if (2 * n + 1 < tab.Count && tab[n] < tab[2 * n + 1])
                    { maks = 2 * n + 1; t = maks; }
                    if (2 * n + 2 < tab.Count && tab[n] < tab[2 * n + 2] && tab[2 * n + 1] < tab[2 * n + 2])
                    { maks = 2 * n + 2; t = maks; }
                    if(maks > 0)
                    {
                        int tmp = tab[n];
                        tab[n] = tab[maks];
                        tab[maks] = tmp;
                    }
                    n = t;
                }
                Console.WriteLine(top);
            }
 * 
 * =============================================================================================
 * 
            }*/

/* for (int o = 0; o < 50; o++ )
            {
                //yy++;
                //Zlecenie d = kopiec.sciagnijZWierzcholkaKopca();
                //Console.WriteLine(yy + " " + d.dajPriorytet());

            }
            
            
            /*for (int x = 0; x < w.DajIloscMiast(); x++)
            {
                for (int y = 0; y < w.DajIloscMiast(); y++)
                {
                    Console.Write(mapp[x, y] + " ");
                }
                Console.WriteLine();
            }*/

/*
 foreach (int value in powrot)
                Console.Write(value + ", ");
            Console.WriteLine();
            for (int i = 0; i < w.DajIloscMiast() - 1; i++)
            {
                for (int j = 0; j < w.DajIloscMiast(); j++)
                {
                    if (i == w.DajIloscMiast() - 2)
                        Console.Write("<" + wezel[i, j].dajOdleglosc() + ">" + "(" + wezel[i, j].dajPoprzedniWierzcholek() + ")");
                    //else
                    //Console.Write("<" + wezel[i, j].dajOdleglosc() + "> ");
                }
                // Console.WriteLine();
            }
*/
