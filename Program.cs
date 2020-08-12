using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SwarmAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ParticleData> allParticles = new List<ParticleData>();
            int DesiredPSO = 0;
            do
            {
                Console.WriteLine("\t \t \t \t Particle Swarm Optimization by Miguel Villasis");
                Console.WriteLine("\nPlease enter your desired type of PSO: ");
                Console.WriteLine("0) Original PSO \t 1) Max Velocity PSO \t 2) Inertia Weight Controlled PSO \t3) Exit Program");
                Console.WriteLine("\nPlease wait till algorithm finishes");
                DesiredPSO = int.Parse(Console.ReadLine().ToString());
                if (DesiredPSO > 2)
                    return;
                PSOAlgorithm(allParticles, DesiredPSO);
                allParticles.Clear();
            } while (DesiredPSO >= 0 && DesiredPSO <= 2);
            

        }

        public static double FindBestSolution(List<ParticleData> Particles)
        {
            double result = 0;
            double CurrentParticleX = 0;
            double CurrentParticleY = 0;
            double NextParticleX = 0;
            double NextParticleY = 0;
            double sumx = 0;
            double sumy = 0;
            int dimension = Particles.Count;

            for (int i = 0; i < dimension - 1; i++)
            {
                CurrentParticleX = Particles[i].corx;
                CurrentParticleY = Particles[i].cory;
                NextParticleX = Particles[i + 1].corx;
                NextParticleY = Particles[i + 1].cory;
                sumx += 0.5 + ((Math.Pow(Math.Sin(Math.Pow(CurrentParticleX, 2) - Math.Pow(NextParticleX, 2)), 2) - 0.5)
                                / Math.Pow((1 + (0.001) * (Math.Pow(CurrentParticleX, 2) + Math.Pow(NextParticleX, 2))), 2));
                sumy += 0.5 + ((Math.Pow(Math.Sin(Math.Pow(CurrentParticleY, 2) - Math.Pow(NextParticleY, 2)), 2) - 0.5)
                                / Math.Pow((1 + (0.001) * (Math.Pow(CurrentParticleY, 2) + Math.Pow(NextParticleY, 2))), 2));
            }
            result = sumx + sumy;

            return result;


        }

        public static void PSOAlgorithm(List<ParticleData> AllParticles, int PSOtype)
        {
            int numberofParticles = 50;
            int numberofDimensions = 2;
            double[] velocity = new double[numberofParticles];
            Position bestPosition = new Position();
            List<Position> BestPositions = new List<Position>();
            double upperBound = 100;
            double lowerBound = -100;
            double[] solutions1 = new double[numberofParticles];
            double[] solutionsAfterUpdate = new double[numberofParticles];
            double BestSolution = 0;
            Random rand = new Random();
            double numberOfIterations = 0;
            double maxIterations = 1000;
            double C1 = 2;
            double C2 = 2;
            int particleCounter = 0;
            double maxVelocityx = 0;
            double maxVelocityy = 0;
            double maxdx = 0;
            double maxdy = 0;
            double mindx = 0;
            double mindy = 0;
            double inertiaWeight = 1;
            double maxInertia = .9;
            double minInertia = .4;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < numberofParticles; i++)
            {
                Position currentPosition = new Position();
                ParticleData currentData = new ParticleData();
                currentData.corx = rand.NextDouble() * (upperBound - lowerBound) + lowerBound;
                currentData.cory = rand.NextDouble() * (upperBound - lowerBound) + lowerBound;
                currentData.velx = rand.NextDouble() * (upperBound - lowerBound) + lowerBound;
                currentData.vely = rand.NextDouble() * (upperBound - lowerBound) + lowerBound;

                currentPosition.x = currentData.corx;
                currentPosition.y = currentData.cory;

                AllParticles.Add(currentData);
                BestPositions.Add(currentPosition);
                solutions1[i] = FindBestSolution(AllParticles);

                if (BestSolution == 0)
                {
                    BestSolution = solutions1[i];
                    bestPosition.x = currentData.corx;
                    bestPosition.y = currentData.cory;
                }

                else
                {
                    if (solutions1[i] < BestSolution)
                    {
                        BestSolution = solutions1[i];
                        bestPosition.x = currentData.corx;
                        bestPosition.y = currentData.cory;
                    }
                }

            }
            if (PSOtype == 1)
            {
                for (int i = 0; i < numberofParticles; i++)
                {
                    for (int j = 0; j < numberofDimensions; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                if (AllParticles[i].corx > maxdx)
                                    maxdx = AllParticles[i].corx;
                                if (AllParticles[i].corx < mindx)
                                    mindx = AllParticles[i].corx;
                                break;
                            case 1:
                                if (AllParticles[i].cory > maxdy)
                                    maxdy = AllParticles[i].cory;
                                if (AllParticles[i].cory < mindy)
                                    mindy = AllParticles[i].cory;
                                break;
                        }

                    }
                }
                maxVelocityx = (maxdx - mindx) / 2;
                maxVelocityy = (maxdy - mindy) / 2;
            }

            while (numberOfIterations <= 1000)
            {
                if (PSOtype == 2)
                {
                    inertiaWeight = maxInertia - (maxInertia - minInertia) * (numberOfIterations / maxIterations);
                }
                for (int i = 0; i < numberofParticles; i++)
                {
                    for (int j = 0; j < numberofDimensions; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                AllParticles[i].velx = inertiaWeight * AllParticles[i].velx
                                                       + (C1 * rand.NextDouble() * (BestPositions[i].x - AllParticles[i].corx))
                                                       + (C2 * rand.NextDouble() * (bestPosition.x) - AllParticles[i].corx);


                                if (PSOtype == 1)
                                {
                                    if (AllParticles[i].velx > maxVelocityx)
                                        AllParticles[i].velx = maxVelocityx;
                                    if (AllParticles[i].velx < -maxVelocityx)
                                        AllParticles[i].velx = -maxVelocityx;
                                }
                                AllParticles[i].corx = AllParticles[i].corx + AllParticles[i].velx;
                                break;
                            case 1:
                                AllParticles[i].vely = inertiaWeight * AllParticles[i].vely
                                                       + (C1 * rand.NextDouble() * (BestPositions[i].y - AllParticles[i].cory))
                                                       + (C2 * rand.NextDouble() * (bestPosition.y) - AllParticles[i].cory);
                                if (PSOtype == 1)
                                {
                                    if (AllParticles[i].vely > maxVelocityy)
                                        AllParticles[i].vely = maxVelocityy;
                                    if (AllParticles[i].vely < -maxVelocityy)
                                        AllParticles[i].vely = -maxVelocityy;
                                }
                                AllParticles[i].cory = AllParticles[i].cory + AllParticles[i].vely;
                                break;
                        }
                    }
                    solutionsAfterUpdate[i] = FindBestSolution(AllParticles);

                    if (solutionsAfterUpdate[i] < solutions1[i])
                    {
                        BestPositions[i].x = AllParticles[i].corx;
                        BestPositions[i].y = AllParticles[i].cory;
                        solutions1[i] = solutionsAfterUpdate[i];
                    }

                    if (solutionsAfterUpdate[i] < BestSolution)
                    {
                        bestPosition.x = AllParticles[i].corx;
                        BestSolution = solutionsAfterUpdate[i];
                    }
                }

                numberOfIterations++;
            }

            timer.Stop();
            Console.WriteLine("\nThe algorithm took " + timer.ElapsedMilliseconds.ToString() + " millisenconds to complete. ");
            Console.WriteLine("The Best Solution was: " + BestSolution + "\n With the best position being: (" + bestPosition.x + ", " + bestPosition.y + ")");
            foreach (Position position in BestPositions)
            {
                Console.WriteLine("\n The best position for Particle Number " + particleCounter + " was: (" + BestPositions[particleCounter].x + ", " + BestPositions[particleCounter].y + ")");
                particleCounter++;
            }
        }




        public class ParticleData
        {
            public double corx;
            public double cory;
            public double velx;
            public double vely;
        }

        public class Position
        {
            public double x;
            public double y;
        }
    }
}
