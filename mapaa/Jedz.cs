using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mapaa
{
    class Jedz
    {
        FlotaSamochodow samochody = new FlotaSamochodow();
        List<Zlecenie> zlecenia;
        List<Wspolrzedne> wspolrzedne;
        Starting s;
        Stopwatch stopwatch = Stopwatch.StartNew();
        Form1 f;
        Wczytywanie w;

        public Jedz(Form1 f, List<Wspolrzedne> wspolrzedne, Starting s, Wczytywanie w)
        {
            this.f = f;
            this.wspolrzedne = wspolrzedne;
            this.s = s;
            this.w = w;
        }

        public void rozwiez(FlotaSamochodow flotaaa, int iloscSamochodow, int[,] mapp, int iloscMiast, List<Miasto> listaMiast)
        {
            Dijkstra dijkstra = new Dijkstra(mapp);//wczytujemy do algorytmu Dijkstry mapę
            Przewoz przewoz = new Przewoz();
            Wezel[,] wezel;

            for (int i = 0; i < iloscSamochodow; i++)
            {
                zlecenia = flotaaa.flota[i].dajSamochodZPaczkami();
                int start = zlecenia[0].dajPoczatek();//id miasta startowego
                int start2 = start;//zapamiętujemy w zmiennej pomocniczej powyższe id

                while (flotaaa.flota[i].dajSamochodZPaczkami().Count > 0)
                {
                    flotaaa.polozenieSamochodu(zlecenia[0].dajCel(), i);
                    wezel = dijkstra.obliczOdlegloscPomiedzyMiastami(start2);
                    przewoz.okreslDrogePowrotna(start2, zlecenia[0].dajCel(), wezel, iloscMiast);
                    List<int> powrotne = new List<int>();
                    powrotne = przewoz.dajWierzcholkiPoDrodze();
                    for (int jj = 0; jj < powrotne.Count; jj++)
                    {
                        if (jj == powrotne.Count - 1)
                        {
                            if (jj > 0)
                                f.DrawConnection(1, wspolrzedne[powrotne[powrotne.Count - jj]].dajA() + 12, wspolrzedne[powrotne[powrotne.Count - jj]].dajB() + 12,
                                wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA() + 12, wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB() + 12);
                            f.DrawVertices(0, (powrotne[powrotne.Count - 1 - jj] + 1).ToString(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA(),
                                wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB());
                            stopwatch = Stopwatch.StartNew();
                            Thread.Sleep(500);
                            stopwatch.Stop();
                            if (powrotne[powrotne.Count - 1 - jj] == w.dajMiastoStartowe())
                                f.DrawVertices(1, (powrotne[powrotne.Count - 1 - jj] + 1).ToString(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB());
                            else
                                f.DrawVertices(-1, (powrotne[powrotne.Count - 1 - jj] + 1).ToString(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB());

                        }
                        else
                        {
                            if (jj > 0)
                                f.DrawConnection(1, wspolrzedne[powrotne[powrotne.Count - jj]].dajA() + 12, wspolrzedne[powrotne[powrotne.Count - jj]].dajB() + 12,
                                wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA() + 12, wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB() + 12);
                            f.DrawVertices(2, (powrotne[powrotne.Count - 1 - jj] + 1).ToString(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA(),
                                wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB());
                            stopwatch = Stopwatch.StartNew();
                            Thread.Sleep(500);
                            stopwatch.Stop();
                            if (powrotne[powrotne.Count - 1 - jj] == w.dajMiastoStartowe())
                                f.DrawVertices(1, (powrotne[powrotne.Count - 1 - jj] + 1).ToString(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB());
                            else
                                f.DrawVertices(-1, (powrotne[powrotne.Count - 1 - jj] + 1).ToString(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajA(), wspolrzedne[powrotne[powrotne.Count - 1 - jj]].dajB());
                        }
                    }
                    Console.WriteLine("Samochód nr: " + (i + 1).ToString() + " Przebieg: " + flotaaa.odczytajPrzebiegZTablicyPojazdow(i).ToString() + " Pobrano przesyłkę " + zlecenia[0].dajId().ToString() + " z miasta " + listaMiast[start2].dajNazwe());
                    if (start2 != zlecenia[0].dajCel())
                        flotaaa.dodajOdleglosc(wezel[iloscMiast, zlecenia[0].dajCel()].dajOdleglosc(), i);
                    else
                        flotaaa.dodajOdleglosc(0, i);
                    Console.WriteLine("Samochód nr: " + (i + 1) + " Przebieg: " + flotaaa.odczytajPrzebiegZTablicyPojazdow(i) + " Dostarczono przesyłkę " + zlecenia[0].dajId() + " do miasta " + listaMiast[zlecenia[0].dajCel()].dajNazwe());
                    start2 = zlecenia[0].dajCel();
                    stopwatch = Stopwatch.StartNew();
                    Thread.Sleep(500);
                    stopwatch.Stop();
                    zlecenia.RemoveAt(0);

                }
                //powrót do miasta startowego po następne paczki, także dodajemy odległość
                wezel = dijkstra.obliczOdlegloscPomiedzyMiastami(flotaaa.flota[i].dajPolozenieSamochodu());
                flotaaa.dodajOdleglosc(wezel[iloscMiast, start].dajOdleglosc(), i);//dopisujemy do przebiegu odległość powrotną, tzn musimy wrócić do miasta-bazy po następne paczki
                flotaaa.polozenieSamochodu(start, i);
                s.refresh(listaMiast);
            }
        }

    }
}
