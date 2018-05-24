using OKSearchRoom;
using System;
using Xunit;

namespace OKSearchRoomTest
{
    public class TravelingSalesmanTest
    {
        [Fact]
        public void EvolSearchTravelingSalesman()
        {
            /*int test = 5;
            object obj = new Object();
            obj = test;
            test++;
            int so;*/


            /*Place place = new Place(10,20);
            PlaceList list = new PlaceList();
            list.Add(place);
            XmlSerializer serializer = new XmlSerializer(typeof(PlaceList));
            TextWriter writer = new StreamWriter( @"F:\test.xml" );
            serializer.Serialize( writer, list );
            writer.Close();*/

            /*TextReader reader = new StreamReader("places.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(PlaceList));
            PlaceList list = (PlaceList)serializer.Deserialize(reader);
            reader.Close();*/

            // http://www.zib.de/groetschel/news/tsp_loesungen_version2.html
            // min: 2069  

            PlaceList list = new PlaceList();
            list.Add(new Place(144, 158));
            list.Add(new Place(188, 80));
            list.Add(new Place(190, 134));
            list.Add(new Place(238, 125));
            list.Add(new Place(61, 265));
            list.Add(new Place(175, 205));
            list.Add(new Place(248, 216));
            list.Add(new Place(315, 195));
            list.Add(new Place(303, 205));
            list.Add(new Place(222, 300));
            list.Add(new Place(340, 285));
            list.Add(new Place(69, 399));
            list.Add(new Place(120, 354));
            list.Add(new Place(153, 430));
            list.Add(new Place(250, 472));

            list.Add(new Place(163, 57));
            list.Add(new Place(83, 140));
            list.Add(new Place(210, 104));
            list.Add(new Place(298, 85));
            list.Add(new Place(266, 100));
            list.Add(new Place(100, 235));
            list.Add(new Place(205, 208));
            list.Add(new Place(249, 194));
            list.Add(new Place(359, 205));
            list.Add(new Place(65, 288));

            list.Add(new Place(158, 270));
            list.Add(new Place(186, 254));
            list.Add(new Place(281, 259));
            list.Add(new Place(353, 242));
            list.Add(new Place(131, 321));
            list.Add(new Place(252, 298));
            list.Add(new Place(137, 340));
            list.Add(new Place(52, 363));
            list.Add(new Place(133, 394));
            list.Add(new Place(184, 368));
            list.Add(new Place(240, 384));
            list.Add(new Place(98, 477));
            list.Add(new Place(222, 458));
            list.Add(new Place(325, 441));
            list.Add(new Place(195, 499));


            int countPlaces = list.Count;

            // Anzahl aller Distanzen: ((AnzahlOrte*AnzahlOrte)-AnzahlOrte)/2
            int countDistances = (countPlaces * countPlaces - countPlaces) / 2;

            Matrix distanceMatrix = new Matrix(countPlaces, countPlaces);

            double distance;

            for (int i = 0; i < countPlaces; i++)
            {
                distanceMatrix.SetValue(i, i, 0.0);
                for (int j = i + 1; j < countPlaces; j++)
                {
                    distance = list[i].GetDistance(list[j]);
                    distanceMatrix.SetValue(i, j, distance);
                    distanceMatrix.SetValue(j, i, distance);
                }
            }

            // Distancematrix kann als static zugewiesen werden, da sie f¸r alle solutions gleich ist
            TravelingSalesmanSolution.DistanceMatrix = distanceMatrix;

            TravelingSalesmanProblem problem = new TravelingSalesmanProblem(10, 40);

            EvolutionaryAlgorithm evol = new EvolutionaryAlgorithm(problem);

            //evol.Run();












            // Anlegen von 10 Ausgangsindividuen
            /*TravelingSalesmanSolution[] tspList = new TravelingSalesmanSolution[10];
            
            // Speicher reservieren
            for(int i=0; i<tspList.GetLength(0); i++)
            {
                tspList[i] = new TravelingSalesmanSolution();
            }*/

            /*Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            int countPlaces = 100;

            // Anzahl aller Distanzen: ((AnzahlOrte*AnzahlOrte)-AnzahlOrte)/2
            int countDistances = (countPlaces*countPlaces-countPlaces)/2;
            int[] distanceList = new int[countDistances];
            Matrix distanceMatrix = new Matrix(countPlaces, countPlaces);
                
            for (int i=0; i<distanceList.GetLength(0); i++)
            {
                distanceList[i] = Convert.ToInt32(Math.Round(random.NextDouble()*500));
            }

            int index = 0;
            for (int i=0; i<countPlaces; i++)
            {
                distanceMatrix.SetValue(i, i, 0);
                for (int j=i+1;j<countPlaces;j++)
                {
                    distanceMatrix.SetValue(i, j, distanceList[index]);
                    distanceMatrix.SetValue(j, i, distanceList[index]);
                    index++;
                }
            }

            TravelingSalesmanSolution.DistanceMatrix = distanceMatrix;

            TravelingSalesmanSolution[] tspList = new TravelingSalesmanSolution[10];
            for(int i=0; i<tspList.GetLength(0); i++)
            {
                tspList[i] = new TravelingSalesmanSolution();
            }

            int a = 10;*/
        }
    }
}
