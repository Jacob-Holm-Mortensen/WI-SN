using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork
{
    class Laplacian
    {
        public void Start()
        {
            Vector eigenvector;
            List<Person> network;
            if (File.Exists(Directory.GetCurrentDirectory().Replace("bin\\Debug", "eigenvector.txt")))
            {
                List<string> eigenvalueStrings = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("bin\\Debug", "eigenvector.txt")).ToList();
                List<double> eigenvalues = new List<double>();
                foreach (var str in eigenvalueStrings)
                {
                    eigenvalues.Add(double.Parse(str));
                }
                eigenvector = DenseVector.OfArray(eigenvalues.ToArray());
                network = makeNetwork();
            }
            else
            {
                network = makeNetwork();
                eigenvector = makeEigenvector(network);
                List<double> eigenvalues = eigenvector.ToList();
                string[] eigenvalueStrings = new string[eigenvalues.Count];
                for (int i = 0; i < eigenvalues.Count; i++)
                {
                    eigenvalueStrings[i] = eigenvalues[i].ToString();
                }
                File.WriteAllLines(Directory.GetCurrentDirectory().Replace("bin\\Debug", "eigenvector.txt"), eigenvalueStrings);
            }
            List<KeyValuePair<Person, double>> networkSortedByEigenvalue = new List<KeyValuePair<Person, double>>();
            List<double> eigenValues = eigenvector.ToList();

            for (int i = 0; i < eigenValues.Count; i++)
            {
                networkSortedByEigenvalue.Add(new KeyValuePair<Person, double>(network[i], eigenValues[i]));
            }

            networkSortedByEigenvalue.Sort((x, y) => x.Value.CompareTo(y.Value));
                        
            List<List<KeyValuePair<Person, double>>> clusters = SplitAtLargestGap(networkSortedByEigenvalue, 9);

            foreach (var cluster in clusters)
            {
                Console.WriteLine(cluster[0].Key.name + " " + cluster[0].Value);
                Console.WriteLine(cluster.Count - 2);
                Console.WriteLine(cluster[cluster.Count - 1].Key.name + " " + cluster[cluster.Count - 1].Value);
                Console.WriteLine();
            }

            Console.WriteLine("Done");
            Console.Read();
        }

        public List<Person> makeNetwork()
        {
            return GetPersons();
            return MakeTestNetwork();
        }

        public Vector makeEigenvector(List<Person> network)
        {
            Matrix<double> laplacian = MakeLaplacian(network);

            Console.WriteLine(laplacian);

            Matrix<double> eigenMatrix = laplacian.Evd(Symmetricity.Symmetric).EigenVectors;

            Console.WriteLine(eigenMatrix);

            Vector eigenvector = DenseVector.OfArray(eigenMatrix.ToColumnArrays()[1]);

            Console.WriteLine(eigenvector);

            return eigenvector;
        }

        public List<List<KeyValuePair<Person, double>>> SplitAtLargestGap(List<KeyValuePair<Person, double>> eigenvectorNetworkList, int splitAmount)
        {
            List<List<KeyValuePair<Person, double>>> clusters = new List<List<KeyValuePair<Person, double>>>();
            List<double> currentHighest = new List<double>() { 0 };
            List<int> splitIndex = new List<int>() { 0 };

            if (splitAmount > 0)
            {
                for (int i = 1; i < eigenvectorNetworkList.Count; i++)
                {
                    for (int j = 0; j < currentHighest.Count; j++)
                    {
                        if (Math.Abs(eigenvectorNetworkList[i].Value - eigenvectorNetworkList[i - 1].Value) > currentHighest[j])
                        {
                            currentHighest.Insert(j, Math.Abs(eigenvectorNetworkList[i].Value - eigenvectorNetworkList[i - 1].Value));
                            splitIndex.Insert(j, i);
                            if (currentHighest.Count > splitAmount)
                            {
                                currentHighest.RemoveAt(splitAmount);
                                splitIndex.RemoveAt(splitAmount);
                            }
                            break;
                        }
                    }
                }
                splitIndex.RemoveAll(x => x == 0);
                splitIndex = splitIndex.Distinct().ToList();
                splitIndex.Sort();
                clusters.Add(eigenvectorNetworkList.GetRange(0, splitIndex.First()));
                for (int i = 1; i < splitIndex.Count; i++)
                {
                    clusters.Add(eigenvectorNetworkList.GetRange(splitIndex[i - 1], (splitIndex[i] - splitIndex[i - 1])));
                }
                clusters.Add(eigenvectorNetworkList.GetRange(splitIndex.Last(), (eigenvectorNetworkList.Count - splitIndex.Last())));

                return clusters;
            }
            else
            {
                clusters.Add(eigenvectorNetworkList);
                return clusters;
            }
        }

        public Matrix<double> MakeLaplacian(List<Person> n)
        {
            List<List<double>> ARows = new List<List<double>>();
            List<List<double>> DRows = new List<List<double>>();
            List<double> currentRow = new List<double>();
            List<double> DRow = new List<double>();
            foreach (var person in n)
            {
                currentRow.Clear();
                foreach (var p in n) currentRow.Add(0);
                DRow = new List<double> (currentRow);
                DRow[n.IndexOf(person)] = person.friends.Count();
                DRows.Add(new List<double> (DRow));
                foreach (var friend in person.friends) currentRow[n.IndexOf(friend)] = 1;
                ARows.Add(new List<double>(currentRow));
            }
            Matrix<double> A = DenseMatrix.OfRows(ARows);
            Matrix<double> D = DenseMatrix.OfRows(DRows);
            return D - A;
        }

        public List<Person> GetPersons()
        {
            List<Person> network = new List<Person>();
            List<string> networkFileLines = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("bin\\Debug", "friendships.txt")).ToList();
            int lineCount = networkFileLines.Count;
            for (int i = 0; i < lineCount; i+=5)
            {
                network.Add(new Person(networkFileLines[i].Replace("user: ",""), new List<string>(), new List<string>()));
            }
            for (int i = 1; i < lineCount; i += 5)
            {
                List<string> friendNames = networkFileLines[i].Replace("friends:","").Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
                List<Person> friends = new List<Person>();
                friends.AddRange(network.Where(x => friendNames.Contains(x.name)).ToList());

                if ((i - 1) == 0) network[0].AddFriends(friends);
                else network[(i - 1) / 5].AddFriends(friends);
            }
            return network;
        }

        public List<Person> MakeTestNetwork()
        {
            Person p1 = new Person("p1", new List<string>(), new List<string>());
            Person p2 = new Person("p2", new List<string>(), new List<string>());
            Person p3 = new Person("p3", new List<string>(), new List<string>());
            Person p4 = new Person("p4", new List<string>(), new List<string>());
            Person p5 = new Person("p5", new List<string>(), new List<string>());
            Person p6 = new Person("p6", new List<string>(), new List<string>());
            Person p7 = new Person("p7", new List<string>(), new List<string>());
            Person p8 = new Person("p8", new List<string>(), new List<string>());
            Person p9 = new Person("p9", new List<string>(), new List<string>());

            p1.AddFriends(new List<Person>() { p2, p3 });
            p2.AddFriends(new List<Person>() { p1, p3 });
            p3.AddFriends(new List<Person>() { p1, p2, p4, p5 });
            p4.AddFriends(new List<Person>() { p3, p5, p6, p7 });
            p5.AddFriends(new List<Person>() { p3, p4, p6, p7 });
            p6.AddFriends(new List<Person>() { p4, p5, p7, p8 });
            p7.AddFriends(new List<Person>() { p4, p5, p6, p8 });
            p8.AddFriends(new List<Person>() { p6, p7, p9 });
            p9.AddFriends(new List<Person>() { p8 });

            return new List<Person>() { p1, p2, p3, p4, p5, p6, p7, p8, p9 };
        }
    }
}
