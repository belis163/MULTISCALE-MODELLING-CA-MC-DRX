using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumulacja
{
    class Grid
    {
        public Cell[,] Map;
        int Map_height, Map_width;
        int[] statesTable;
        public int numberOfStates;
        int maxOfLiczbaZarodkow;
        Cell[,] previousIteration;
        public int[,] energyMap;
        int radius;
        static Random randomHeksaGenerator;
        static Random randomPentagonalGenerator;
        static Random mcGenerator;
        static Random propabilityGenerator;
        static Random recrystallizationGenerator;
        static Random propabilityGen;
        private double recrystallizationPackage;
        private double deltaDislocationDensity;
        private double dislocationDensity;
        private double t;
        string docPath;
        private int boundaryCondition;
        private int neighbourhoodType;
        Cell pierdolonaKumurkaZero;

        public Grid()
        {

        }

        public Grid(int Map_width, int Map_height, int numberOfStates, int radius, int boundaryCondition, int neighbourhoodTypes)
        {
            this.numberOfStates = numberOfStates + 1;
            this.Map_width = Map_width;
            this.Map_height = Map_height;
            this.statesTable = new int[this.numberOfStates];
            previousIteration = new Cell[Map_height, Map_width];
            energyMap = new int[Map_height, Map_width];
            this.radius = radius;
            randomHeksaGenerator = new Random();
            randomPentagonalGenerator = new Random();
            mcGenerator = new Random();
            propabilityGenerator = new Random();
            recrystallizationGenerator = new Random();
            propabilityGen = new Random();
            recrystallizationPackage = 0.0;
            t = 0.0;
            docPath = @"C:\Users\Belis\Desktop\dislocation.txt";
            File.WriteAllText(docPath, String.Empty);
            this.boundaryCondition = boundaryCondition;
            this.neighbourhoodType = neighbourhoodTypes;
            pierdolonaKumurkaZero = new Cell();
            dislocationDensity = 0.0;



            this.Map = new Cell[Map_height, Map_width];

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    this.Map[i, j] = new Cell();
                    this.previousIteration[i, j] = new Cell();
                }
            }

            for (int i = 0; i < numberOfStates; i++)
            {
                this.statesTable[i] = 0;
            }
        }

        public Cell[,] getMap()
        {
            return Map;
        }

        public int getMapEnergyState(int x, int y)
        {
            return energyMap[x, y];
        }

        public void calculatePreviousIteration()
        {
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    previousIteration[i, j].copyCell(this.Map[i, j]);
                }
            }
        }

        public void changeMapState(int i, int j, int state)
        {
            this.Map[i, j].setCellState(state);
        }

        Cell[] neighbourhoodPeriodicalVonNeuman(int i, int j)
        {
            Cell[] neighbourhood = new Cell[4];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, 0];
                neighbourhood[1] = previousIteration[0, Map_width - 1];
                neighbourhood[2] = previousIteration[0, 1];
                neighbourhood[3] = previousIteration[1, 0];
            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[0, Map_width - 2];
                neighbourhood[2] = previousIteration[0, 0];
                neighbourhood[3] = previousIteration[1, Map_width - 1];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                neighbourhood[3] = previousIteration[0, 0];
            }

            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[2] = previousIteration[Map_height - 1, 0];
                neighbourhood[3] = previousIteration[0, Map_width - 1];
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, Map_width - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, 0];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[0, j];
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalVonNeuman()
        {
            Cell[] neighbourhood = new Cell[4];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalVonNeuman(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 1; l < numberOfStates; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }
        }

        Cell[] neighbourhoodAbsorbingMoore(int i, int j)
        {
            Cell[] neighbourhood = new Cell[8];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;
                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = pierdolonaKumurkaZero;
                neighbourhood[4] = previousIteration[0, 1];

                neighbourhood[5] = pierdolonaKumurkaZero;
                neighbourhood[6] = previousIteration[1, 0];
                neighbourhood[7] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;
                neighbourhood[2] = pierdolonaKumurkaZero;

                neighbourhood[3] = previousIteration[0, Map_width - 2];
                neighbourhood[4] = pierdolonaKumurkaZero;

                neighbourhood[5] = previousIteration[1, Map_width - 2];
                neighbourhood[6] = previousIteration[1, Map_width - 1];
                neighbourhood[7] = pierdolonaKumurkaZero;
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = previousIteration[Map_height - 2, 0];
                neighbourhood[2] = previousIteration[Map_height - 2, 1];

                neighbourhood[3] = pierdolonaKumurkaZero;
                neighbourhood[4] = previousIteration[Map_height - 1, 1];

                neighbourhood[5] = pierdolonaKumurkaZero;
                neighbourhood[6] = pierdolonaKumurkaZero;
                neighbourhood[7] = pierdolonaKumurkaZero;
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[2] = pierdolonaKumurkaZero;

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[4] = pierdolonaKumurkaZero;

                neighbourhood[5] = pierdolonaKumurkaZero;
                neighbourhood[6] = pierdolonaKumurkaZero;
                neighbourhood[7] = pierdolonaKumurkaZero;
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = pierdolonaKumurkaZero;
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = pierdolonaKumurkaZero;
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // góna krawędź
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;
                neighbourhood[2] = pierdolonaKumurkaZero;

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // prawa krawędź
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = pierdolonaKumurkaZero;

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = pierdolonaKumurkaZero;

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = pierdolonaKumurkaZero;
            }
            // dolna krawędź
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = pierdolonaKumurkaZero;
                neighbourhood[6] = pierdolonaKumurkaZero;
                neighbourhood[7] = pierdolonaKumurkaZero;
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }

            return neighbourhood;
        }

        public void UpdateVectorAbsorbingMoore()
        {

            Cell[] neighbourhood = new Cell[8];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {

                        neighbourhood = neighbourhoodAbsorbingMoore(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        Cell[] neighbourhoodPeriodicalHeksaRandom(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];


            switch (randomHeksaGenerator.Next(2))
            {

                case 0:
                    if (j == 0 && i == 0)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, 0];


                        neighbourhood[2] = previousIteration[0, Map_width - 1];
                        neighbourhood[3] = previousIteration[0, 1];


                        neighbourhood[4] = previousIteration[1, 0];
                        neighbourhood[5] = previousIteration[1, 1];
                    }
                    else if (j == Map_width - 1 && i == 0)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];


                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = previousIteration[0, 0];


                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = previousIteration[1, 0];
                    }
                    else if (j == 0 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];


                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];


                        neighbourhood[4] = previousIteration[0, 0];
                        neighbourhood[5] = previousIteration[0, 1];
                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];


                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = previousIteration[Map_height - 1, 0];


                        neighbourhood[4] = previousIteration[0, Map_width - 1];
                        neighbourhood[5] = previousIteration[0, 0];
                    }
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, Map_width - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, 0];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, 0];
                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[0, j];
                        neighbourhood[5] = previousIteration[0, j + 1];
                    }

                    else
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];


                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];


                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    break;
                case 1:
                    if (j == 0 && i == 0)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, 0];
                        neighbourhood[1] = previousIteration[Map_height - 1, 1];

                        neighbourhood[2] = previousIteration[0, Map_width - 1];
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = previousIteration[1, 0];

                    }
                    else if (j == Map_width - 1 && i == 0)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 1, 0];

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = previousIteration[0, 0];

                        neighbourhood[4] = previousIteration[1, Map_width - 2];
                        neighbourhood[5] = previousIteration[1, Map_width - 1];

                    }
                    else if (j == 0 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, 0];
                        neighbourhood[1] = previousIteration[Map_height - 2, 1];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = previousIteration[0, Map_width - 1];
                        neighbourhood[5] = previousIteration[0, 0];

                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = previousIteration[Map_height - 1, 0];

                        neighbourhood[4] = previousIteration[0, Map_width - 2];
                        neighbourhood[5] = previousIteration[0, Map_width - 1];

                    }
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, Map_width - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, Map_width - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[Map_height - 1, j];
                        neighbourhood[1] = previousIteration[Map_height - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, 0];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, 0];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[0, j - 1];
                        neighbourhood[5] = previousIteration[0, j];

                    }

                    else
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    break;
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalHeksaRadnom()
        {

            Cell[] neighbourhood = new Cell[6];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalHeksaRandom(i, j);

                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        Cell[] neighbourhoodAbsorbingHeksaRandom(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];


            switch (randomHeksaGenerator.Next(2))
            {

                case 0:
                    if (j == 0 && i == 0)
                    {

                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = pierdolonaKumurkaZero;
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = previousIteration[1, 0];

                    }
                    else if (j == Map_width - 1 && i == 0)
                    {

                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = pierdolonaKumurkaZero;

                        neighbourhood[4] = previousIteration[1, Map_width - 2];
                        neighbourhood[5] = previousIteration[1, Map_width - 1];

                    }
                    else if (j == 0 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, 0];
                        neighbourhood[1] = previousIteration[Map_height - 2, 1];

                        neighbourhood[2] = pierdolonaKumurkaZero;
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = pierdolonaKumurkaZero;

                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {

                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = pierdolonaKumurkaZero;

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = pierdolonaKumurkaZero;

                    }
                    //edges
                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = pierdolonaKumurkaZero;
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {

                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = pierdolonaKumurkaZero;

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = pierdolonaKumurkaZero;

                    }

                    else
                    {

                        neighbourhood[0] = previousIteration[i - 1, j];
                        neighbourhood[1] = previousIteration[i - 1, j + 1];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j - 1];
                        neighbourhood[5] = previousIteration[i + 1, j];

                    }
                    break;
                case 1:
                    if (j == 0 && i == 0)
                    {
                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = pierdolonaKumurkaZero;
                        neighbourhood[3] = previousIteration[0, 1];

                        neighbourhood[4] = previousIteration[1, 0];
                        neighbourhood[5] = previousIteration[1, 1];
                    }
                    else if (j == Map_width - 1 && i == 0)
                    {
                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = previousIteration[0, Map_width - 2];
                        neighbourhood[3] = pierdolonaKumurkaZero;

                        neighbourhood[4] = previousIteration[1, Map_width - 1];
                        neighbourhood[5] = pierdolonaKumurkaZero;
                    }
                    else if (j == 0 && i == Map_height - 1)
                    {
                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = previousIteration[Map_height - 2, 0];

                        neighbourhood[2] = pierdolonaKumurkaZero;
                        neighbourhood[3] = previousIteration[Map_height - 1, 1];

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = pierdolonaKumurkaZero;
                    }
                    else if (j == Map_width - 1 && i == Map_height - 1)
                    {
                        neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                        neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];

                        neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                        neighbourhood[3] = pierdolonaKumurkaZero;

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = pierdolonaKumurkaZero;
                    }

                    else if (j == 0 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = pierdolonaKumurkaZero;
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }

                    else if (j != 0 && i == 0 && j != (Map_width - 1))
                    {
                        neighbourhood[0] = pierdolonaKumurkaZero;
                        neighbourhood[1] = pierdolonaKumurkaZero;

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }

                    else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = pierdolonaKumurkaZero;

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = pierdolonaKumurkaZero;
                    }


                    else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = pierdolonaKumurkaZero;
                        neighbourhood[5] = pierdolonaKumurkaZero;
                    }

                    else
                    {
                        neighbourhood[0] = previousIteration[i - 1, j - 1];
                        neighbourhood[1] = previousIteration[i - 1, j];

                        neighbourhood[2] = previousIteration[i, j - 1];
                        neighbourhood[3] = previousIteration[i, j + 1];

                        neighbourhood[4] = previousIteration[i + 1, j];
                        neighbourhood[5] = previousIteration[i + 1, j + 1];
                    }
                    break;
            }


            return neighbourhood;
        }

        public void UpdateVectorAbsorbingHeksaRandom()
        {

            Cell[] neighbourhood = new Cell[6];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingHeksaRandom(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }

        }

        Cell[] neighbourhoodAbsorbingHeksaRight(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];


            if (j == 0 && i == 0)
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = previousIteration[1, 0];
                neighbourhood[5] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = pierdolonaKumurkaZero;

                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = pierdolonaKumurkaZero;
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = previousIteration[Map_height - 2, 0];

                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = pierdolonaKumurkaZero;
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = pierdolonaKumurkaZero;

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = pierdolonaKumurkaZero;
            }

            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = pierdolonaKumurkaZero;

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = pierdolonaKumurkaZero;
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = pierdolonaKumurkaZero;
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }



            return neighbourhood;
        }

        public void UpdateVectorAbsorbingHeksaRight()
        {

            Cell[] neighbourhood = new Cell[6];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingHeksaRight(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        Cell[] neighbourhoodPeriodicalHeksaRight(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];

            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];


                neighbourhood[2] = previousIteration[0, Map_width - 1];
                neighbourhood[3] = previousIteration[0, 1];


                neighbourhood[4] = previousIteration[1, 0];
                neighbourhood[5] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];


                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = previousIteration[0, 0];


                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = previousIteration[1, 0];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];


                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[3] = previousIteration[Map_height - 1, 1];


                neighbourhood[4] = previousIteration[0, 0];
                neighbourhood[5] = previousIteration[0, 1];
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];


                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = previousIteration[Map_height - 1, 0];


                neighbourhood[4] = previousIteration[0, Map_width - 1];
                neighbourhood[5] = previousIteration[0, 0];
            }
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, Map_width - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, 0];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, 0];
            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[0, j];
                neighbourhood[5] = previousIteration[0, j + 1];
            }

            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];


                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];


                neighbourhood[4] = previousIteration[i + 1, j];
                neighbourhood[5] = previousIteration[i + 1, j + 1];
            }
            return neighbourhood;
        }

        public void UpdateVectorPeriodicalHeksaRight()
        {
            Cell[] neighbourhood = new Cell[6];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalHeksaRight(i, j);
                        //////////////////////////////////////////////////////


                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        Cell[] neighbourhoodAbsorbingHeksaLeft(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];

            if (j == 0 && i == 0)
            {

                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = previousIteration[1, 0];

            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = pierdolonaKumurkaZero;

                neighbourhood[4] = previousIteration[1, Map_width - 2];
                neighbourhood[5] = previousIteration[1, Map_width - 1];

            }
            else if (j == 0 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 2, 1];

                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = pierdolonaKumurkaZero;

            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = pierdolonaKumurkaZero;

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = pierdolonaKumurkaZero;

            }
            //edges
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {

                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = pierdolonaKumurkaZero;

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = pierdolonaKumurkaZero;

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = pierdolonaKumurkaZero;
                neighbourhood[5] = pierdolonaKumurkaZero;

            }

            else
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }

            return neighbourhood;
        }

        public void UpdateVectorAbsorbingHeksaLeft()
        {
            Cell[] neighbourhood = new Cell[6];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingHeksaLeft(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        Cell[] neighbourhoodPeriodicalHeksaLeft(int i, int j)
        {
            Cell[] neighbourhood = new Cell[6];

            if (j == 0 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, 0];
                neighbourhood[1] = previousIteration[Map_height - 1, 1];

                neighbourhood[2] = previousIteration[0, Map_width - 1];
                neighbourhood[3] = previousIteration[0, 1];

                neighbourhood[4] = previousIteration[1, Map_width - 1];
                neighbourhood[5] = previousIteration[1, 0];

            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];

                neighbourhood[2] = previousIteration[0, Map_width - 2];
                neighbourhood[3] = previousIteration[0, 0];

                neighbourhood[4] = previousIteration[1, Map_width - 2];
                neighbourhood[5] = previousIteration[1, Map_width - 1];

            }
            else if (j == 0 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = previousIteration[Map_height - 2, 1];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[3] = previousIteration[Map_height - 1, 1];

                neighbourhood[4] = previousIteration[0, Map_width - 1];
                neighbourhood[5] = previousIteration[0, 0];

            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {

                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];

                neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[3] = previousIteration[Map_height - 1, 0];

                neighbourhood[4] = previousIteration[0, Map_width - 2];
                neighbourhood[5] = previousIteration[0, Map_width - 1];

            }
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, Map_width - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, Map_width - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[Map_height - 1, j];
                neighbourhood[1] = previousIteration[Map_height - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, 0];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, 0];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[0, j - 1];
                neighbourhood[5] = previousIteration[0, j];

            }

            else
            {

                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i - 1, j + 1];

                neighbourhood[2] = previousIteration[i, j - 1];
                neighbourhood[3] = previousIteration[i, j + 1];

                neighbourhood[4] = previousIteration[i + 1, j - 1];
                neighbourhood[5] = previousIteration[i + 1, j];

            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalHeksaLeft()
        {
            Cell[] neighbourhood = new Cell[6];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalHeksaLeft(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 6; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }

        }

        Cell[] neighbourhoodAbsorbingVonNeuman(int i, int j)
        {
            Cell[] neighbourhood = new Cell[4];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = pierdolonaKumurkaZero;
                neighbourhood[2] = previousIteration[0, 1];
                neighbourhood[3] = previousIteration[1, 0];
            }
            else if (j == Map_width - 1 && i == 0)
            {

                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = previousIteration[0, Map_width - 2];
                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[1, Map_width - 1];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, 0];
                neighbourhood[1] = pierdolonaKumurkaZero;
                neighbourhood[2] = previousIteration[Map_height - 1, 1];
                neighbourhood[3] = pierdolonaKumurkaZero;
            }

            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = pierdolonaKumurkaZero;
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = pierdolonaKumurkaZero;
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = pierdolonaKumurkaZero;
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = pierdolonaKumurkaZero;
                neighbourhood[3] = previousIteration[i + 1, j];
            }

            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = pierdolonaKumurkaZero;
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j];
                neighbourhood[1] = previousIteration[i, j - 1];
                neighbourhood[2] = previousIteration[i, j + 1];
                neighbourhood[3] = previousIteration[i + 1, j];
            }
            return neighbourhood;
        }

        public void UpdateVectorAbsorbingVonNeuman()
        {
            Cell[] neighbourhood = new Cell[4];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingVonNeuman(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 1; l < numberOfStates; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }

        }

        Cell[] neighbourhoodPeriodicalMoore(int i, int j)
        {
            Cell[] neighbourhood = new Cell[8];

            //winkle
            if (j == 0 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, 0];
                neighbourhood[2] = previousIteration[Map_height - 1, 1];

                neighbourhood[3] = previousIteration[0, Map_width - 1];
                neighbourhood[4] = previousIteration[0, 1];

                neighbourhood[5] = previousIteration[1, Map_width - 1];
                neighbourhood[6] = previousIteration[1, 0];
                neighbourhood[7] = previousIteration[1, 1];
            }
            else if (j == Map_width - 1 && i == 0)
            {
                neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 1, 0];

                neighbourhood[3] = previousIteration[0, Map_width - 2];
                neighbourhood[4] = previousIteration[0, 0];

                neighbourhood[5] = previousIteration[1, Map_width - 2];
                neighbourhood[6] = previousIteration[1, Map_width - 1];
                neighbourhood[7] = previousIteration[1, 0];
            }
            else if (j == 0 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[1] = previousIteration[Map_height - 2, 0];
                neighbourhood[2] = previousIteration[Map_height - 2, 1];

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 1];
                neighbourhood[4] = previousIteration[Map_height - 1, 1];

                neighbourhood[5] = previousIteration[0, Map_width - 1];
                neighbourhood[6] = previousIteration[0, 0];
                neighbourhood[7] = previousIteration[0, 1];
            }
            else if (j == Map_width - 1 && i == Map_height - 1)
            {
                neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                neighbourhood[2] = previousIteration[Map_height - 2, 0];

                neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                neighbourhood[4] = previousIteration[Map_height - 1, 0];

                neighbourhood[5] = previousIteration[0, Map_width - 2];
                neighbourhood[6] = previousIteration[0, Map_width - 1];
                neighbourhood[7] = previousIteration[0, 0];
            }
            //krawedzie lewa
            else if (j == 0 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, Map_width - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, Map_width - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // góna krawędź
            else if (j != 0 && i == 0 && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                neighbourhood[1] = previousIteration[Map_height - 1, j];
                neighbourhood[2] = previousIteration[Map_height - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }
            // prawa krawędź
            else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, 0];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, 0];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, 0];
            }
            // dolna krawędź
            else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[0, j - 1];
                neighbourhood[6] = previousIteration[0, j];
                neighbourhood[7] = previousIteration[0, j + 1];
            }

            //reszta
            else
            {
                neighbourhood[0] = previousIteration[i - 1, j - 1];
                neighbourhood[1] = previousIteration[i - 1, j];
                neighbourhood[2] = previousIteration[i - 1, j + 1];

                neighbourhood[3] = previousIteration[i, j - 1];
                neighbourhood[4] = previousIteration[i, j + 1];

                neighbourhood[5] = previousIteration[i + 1, j - 1];
                neighbourhood[6] = previousIteration[i + 1, j];
                neighbourhood[7] = previousIteration[i + 1, j + 1];
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalMoore()
        {
            Cell[] neighbourhood = new Cell[8];
            calculatePreviousIteration();


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalMoore(i, j);
                        //////////////////////////////////////////////////////

                        for (int k = 0; k < 8; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }


                        Map[i, j].setCellState(maxOfLiczbaZarodkow);

                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }
                    }
                }
            }
        }

        Cell[] neighbourhoodPeriodicalPentagonal(int i, int j)
        {
            Cell[] neighbourhood = new Cell[5];


            int whichPentagonal = 0;

            whichPentagonal = randomPentagonalGenerator.Next(0, 3);

            switch (whichPentagonal)
            {
                case 0:
                    {
                        //winkle lewa góra
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, Map_width - 1];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];

                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 2];
                            neighbourhood[4] = previousIteration[1, Map_width - 1];

                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];

                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = previousIteration[0, Map_width - 1];

                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, Map_width - 1];
                            neighbourhood[3] = previousIteration[i + 1, Map_width - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[0, j - 1];
                            neighbourhood[4] = previousIteration[0, j];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];
                        }

                        break;
                    }
                case 1:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, 0];
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = previousIteration[0, 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, 0];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, 0];
                            neighbourhood[1] = previousIteration[Map_height - 2, 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = previousIteration[0, 0];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, 0];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j];
                            neighbourhood[1] = previousIteration[Map_height - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, 0];
                            neighbourhood[2] = previousIteration[i, 0];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, 0];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[0, j];
                            neighbourhood[4] = previousIteration[0, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        break;
                    }
                case 2:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 1];
                            neighbourhood[1] = previousIteration[0, 1];
                            neighbourhood[2] = previousIteration[1, Map_width - 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 2];
                            neighbourhood[1] = previousIteration[0, 0];
                            neighbourhood[2] = previousIteration[1, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = previousIteration[1, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = previousIteration[0, Map_width - 1];
                            neighbourhood[3] = previousIteration[0, 0];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, Map_width - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, Map_width - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, 0];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, 0];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[0, j - 1];
                            neighbourhood[3] = previousIteration[0, j];
                            neighbourhood[4] = previousIteration[0, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 3:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, 0];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = previousIteration[0, Map_width - 1];
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 0];
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = previousIteration[0, 0];
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 2, 1];
                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 1];
                            neighbourhood[4] = previousIteration[Map_height - 1, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 2, 0];

                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[4] = previousIteration[Map_height - 1, 0];
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, Map_width - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, Map_width - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, j - 1];
                            neighbourhood[1] = previousIteration[Map_height - 1, j];
                            neighbourhood[2] = previousIteration[Map_height - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, 0];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, 0];
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        break;
                    }
                default:
                    break;
            }

            return neighbourhood;
        }

        public void UpdateVectorPeriodicalPentagonal()
        {
            Cell[] neighbourhood = new Cell[5];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodPeriodicalPentagonal(i, j);
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }

        }

        Cell[] neighbourhoodAbsorbingPentagonal(int i, int j)
        {
            Cell[] neighbourhood = new Cell[5];

            int whichPentagonal = 0;
            whichPentagonal = randomPentagonalGenerator.Next(0, 4);

            switch (whichPentagonal)
            {
                case 0:
                    {
                        //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = previousIteration[1, 0];

                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = previousIteration[0, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 2];
                            neighbourhood[4] = previousIteration[1, Map_width - 1];

                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;

                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;

                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;

                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j - 1];
                            neighbourhood[4] = previousIteration[i + 1, j];

                        }
                        break;

                    }
                case 1:
                    { //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = previousIteration[0, 1];
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, 0];
                            neighbourhood[1] = previousIteration[Map_height - 2, 1];
                            neighbourhood[2] = previousIteration[Map_height - 1, 1];
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j];
                            neighbourhood[1] = previousIteration[i - 1, j + 1];
                            neighbourhood[2] = previousIteration[i, j + 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 2:
                    { //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = previousIteration[0, 1];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[1, 0];
                            neighbourhood[4] = previousIteration[1, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = previousIteration[0, Map_width - 2];
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = previousIteration[1, Map_width - 2];
                            neighbourhood[3] = previousIteration[1, Map_width - 1];
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = previousIteration[Map_height - 1, 1];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i, j - 1];
                            neighbourhood[1] = previousIteration[i, j + 1];
                            neighbourhood[2] = previousIteration[i + 1, j - 1];
                            neighbourhood[3] = previousIteration[i + 1, j];
                            neighbourhood[4] = previousIteration[i + 1, j + 1];
                        }

                        break;
                    }
                case 3:
                    { //winkle
                        if (j == 0 && i == 0)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = previousIteration[0, 1];
                        }
                        else if (j == Map_width - 1 && i == 0)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[0, Map_width - 2];
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        else if (j == 0 && i == Map_height - 1)
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = previousIteration[Map_height - 2, 0];
                            neighbourhood[2] = previousIteration[Map_height - 2, 1];
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = previousIteration[Map_height - 1, 1];
                        }
                        else if (j == Map_width - 1 && i == Map_height - 1)
                        {
                            neighbourhood[0] = previousIteration[Map_height - 2, Map_width - 2];
                            neighbourhood[1] = previousIteration[Map_height - 2, Map_width - 1];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[Map_height - 1, Map_width - 2];
                            neighbourhood[4] = pierdolonaKumurkaZero;

                        }
                        //krawedzie lewa
                        else if (j == 0 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = pierdolonaKumurkaZero;
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // góna krawędź
                        else if (j != 0 && i == 0 && j != (Map_width - 1))
                        {
                            neighbourhood[0] = pierdolonaKumurkaZero;
                            neighbourhood[1] = pierdolonaKumurkaZero;
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }
                        // prawa krawędź
                        else if (j == Map_width - 1 && i != 0 && i != (Map_height - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = pierdolonaKumurkaZero;
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = pierdolonaKumurkaZero;
                        }
                        // dolna krawędź
                        else if (j != 0 && i == (Map_height - 1) && j != (Map_width - 1))
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        //reszta
                        else
                        {
                            neighbourhood[0] = previousIteration[i - 1, j - 1];
                            neighbourhood[1] = previousIteration[i - 1, j];
                            neighbourhood[2] = previousIteration[i - 1, j + 1];
                            neighbourhood[3] = previousIteration[i, j - 1];
                            neighbourhood[4] = previousIteration[i, j + 1];
                        }

                        break;
                    }
                default:
                    break;
            }

            return neighbourhood;
        }

        public void UpdateVectorAbsorbingPentagonal()
        {

            Cell[] neighbourhood = new Cell[5];
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map[i, j].getCellState() == 0)
                    {
                        neighbourhood = neighbourhoodAbsorbingPentagonal(i, j);
                        for (int k = 0; k < 5; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }


                    }
                }

            }
        }

        bool CheckRadius(int circlex, int circley, int x, int y)
        {
            if (Math.Sqrt((((double)Math.Abs(x) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].getMassX()) - circlex) * (((double)Math.Abs(x) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].getMassX()) - circlex) + (((double)Math.Abs(y) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].getMassY()) - circley) * (((double)Math.Abs(y) + Map[(x + Map_height) % Map_height, (y + Map_width) % Map_width].getMassY()) - circley)) <= (double)radius)
                return true;
            else
            {
                return false;
            }


        }

        public List<Cell> neighbourhoodPeriodicalRadius(int x, int y)
        {
            int startx = x - radius;
            int starty = y - radius;
            int endx = x + radius;
            int endy = y + radius;

            List<Cell> neighbourhood = new List<Cell>();

            for (int i = startx; i <= endx; i++)
            {
                for (int j = starty; j <= endy; j++)
                {
                    if (CheckRadius(x, y, i, j))
                    {
                        neighbourhood.Add(previousIteration[Math.Abs((i + Map_height) % Map_height), Math.Abs((j + Map_width) % Map_width)]);
                    }

                }
            }
            return neighbourhood;
        }

        public void UpdateVectorPeriodicalRadius()
        {
            List<Cell> neighbourhood;
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {

                        neighbourhood = neighbourhoodPeriodicalRadius(i, j);

                        for (int k = 0; k < neighbourhood.Count; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }
        }

        public List<Cell> neighbourhoodAbsorbingRadius(int x, int y)
        {
            int startx = x - radius;
            int starty = y - radius;
            int endx = x + radius;
            int endy = y + radius;
            List<Cell> neighbourhood = new List<Cell>();

            for (int i = startx; i <= endx; i++)
            {
                for (int j = starty; j <= endy; j++)
                {
                    if (i >= 0 && i < Map_height && j >= 0 && j < Map_width)
                    {
                        if (CheckRadius(x, y, i, j))
                        {

                            neighbourhood.Add(previousIteration[i, j]);

                        }
                    }
                }
            }
            return neighbourhood;
        }

        public void UpdateVectorAbsorbingRadius()
        {
            List<Cell> neighbourhood;
            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getCellState() == 0)
                    {

                        neighbourhood = neighbourhoodAbsorbingRadius(i, j);

                        for (int k = 0; k < neighbourhood.Count; k++)
                        {
                            for (int l = 1; l < numberOfStates + 1; l++)
                            {
                                if (neighbourhood[k].getCellState() == l)
                                {
                                    statesTable[l]++;
                                }
                            }
                        }

                        maxOfLiczbaZarodkow = statesTable[0];
                        for (int k = 1; k < numberOfStates; k++)
                        {
                            if (statesTable[k] > maxOfLiczbaZarodkow)
                            {
                                maxOfLiczbaZarodkow = k;
                            }
                        }

                        Map[i, j].setCellState(maxOfLiczbaZarodkow);
                        for (int k = 0; k < numberOfStates; k++)
                        {
                            statesTable[k] = 0;
                        }

                    }
                }
            }
        }

        public void calculateMonteCarlo(int boundaryCondition, double kT)
        {
            bool[,] visitMap = new bool[Map_height, Map_width];

            int x, y;
            int energy = 0;
            int randomNeighbour = 0;
            int energyRandomNeighbour = 0;
            int iterator = 0;
            Cell[] neighbours;
            List<Cell> neighboursRadius = null;

            calculatePreviousIteration();
            while (iterator < (Map_height * Map_width))
            {

                x = mcGenerator.Next(0, Map_height);
                y = mcGenerator.Next(0, Map_width);

                if (visitMap[x, y] == false)
                {
                    if (boundaryCondition == 0) //periodic
                    {
                        switch (neighbourhoodType)
                        {
                            case 0: //VonNeuman
                                {
                                    neighbours = neighbourhoodPeriodicalVonNeuman(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        
                                        if (propabilityGenerator.NextDouble() < Math.Exp(-((energyRandomNeighbour - energy) / (kT))))
                                        {
                                            previousIteration[x, y].setCellState(randomNeighbour);
                                            Map[x, y].setCellState(randomNeighbour);
                                            energyMap[x, y] = energyRandomNeighbour;
                                        }
                                    }
                                   
                                    break;
                                }
                            case 1: //Moore'a
                                {
                                    neighbours = neighbourhoodPeriodicalMoore(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }
                                    break;
                                }
                            case 2: //PentaRandom
                                {
                                    neighbours = neighbourhoodPeriodicalPentagonal(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }
                                    break;
                                }
                            case 3: //heksaLeft
                                {
                                    neighbours = neighbourhoodPeriodicalHeksaLeft(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }
                                    break;
                                }
                            case 4: //heksaRight
                                {
                                    neighbours = neighbourhoodPeriodicalHeksaRight(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }
                                    break;
                                }
                            case 5: //heksaRand
                                {
                                    neighbours = neighbourhoodPeriodicalHeksaRandom(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }
                                    break;
                                }

                            case 6: //radiusowe
                                {
                                    neighboursRadius = neighbourhoodPeriodicalRadius(x, y);
                                    for (int i = 0; i < neighboursRadius.Count; i++)
                                    {
                                        if (neighboursRadius[i].getCellState() != Map[x, y].getCellState())
                                        {
                                            energy++;
                                        }
                                    }
                                    energyMap[x, y] = energy;
                                    randomNeighbour = neighboursRadius[mcGenerator.Next(neighboursRadius.Count)].getCellState();

                                    for (int i = 0; i < neighboursRadius.Count; i++)
                                    {
                                        if (neighboursRadius[i].getCellState() != randomNeighbour)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (neighbourhoodType)
                        {
                            case 0: //VonNeuman
                                {
                                    neighbours = neighbourhoodAbsorbingVonNeuman(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState() && neighbours[i].getCellState() != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour && neighbours[i].getCellState() != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }

                                    break;
                                }
                            case 1: //Moore'a
                                {
                                    neighbours = neighbourhoodAbsorbingMoore(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState() && neighbours[i].getCellState() != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour && neighbours[i].getCellState() != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }

                                    break;
                                }
                            case 2: //PentaRandom
                                {
                                    neighbours = neighbourhoodAbsorbingPentagonal(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState() && neighbours[i].getCellState() != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour && neighbours[i].getCellState() != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }

                                    break;
                                }
                            case 3: //heksaLeft
                                {
                                    neighbours = neighbourhoodAbsorbingHeksaLeft(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState() && neighbours[i].getCellState() != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour && neighbours[i].getCellState() != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }

                                    break;
                                }
                            case 4: //heksaRight
                                {
                                    neighbours = neighbourhoodAbsorbingHeksaRight(x, y);
                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != Map[x, y].getCellState() && neighbours[i].getCellState() != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighbours[mcGenerator.Next(neighbours.Length)].getCellState();
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighbours.Length; i++)
                                    {
                                        if (neighbours[i].getCellState() != randomNeighbour && neighbours[i].getCellState() != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }

                                    break;
                                }

                            case 6: //radius
                                {
                                    neighboursRadius = neighbourhoodAbsorbingRadius(x, y);
                                    for (int i = 0; i < neighboursRadius.Count; i++)
                                    {
                                        if (neighboursRadius[i].getCellState() != Map[x, y].getCellState() && neighboursRadius[i].getCellState() != 0)
                                        {
                                            energy++;
                                        }
                                    }

                                    energyMap[x, y] = energy;
                                    do
                                    {
                                        randomNeighbour = neighboursRadius[mcGenerator.Next(neighboursRadius.Count)].getCellState();
                                    } while (randomNeighbour == 0);

                                    for (int i = 0; i < neighboursRadius.Count; i++)
                                    {
                                        if (neighboursRadius[i].getCellState() != randomNeighbour && neighboursRadius[i].getCellState() != 0)
                                        {
                                            energyRandomNeighbour++;
                                        }
                                    }

                                    if (energy >= energyRandomNeighbour)
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else if (Math.Exp(-((energyRandomNeighbour - energy) / (kT))) >= propabilityGenerator.NextDouble())
                                    {
                                        previousIteration[x, y].setCellState(randomNeighbour);
                                        Map[x, y].setCellState(randomNeighbour);
                                        energyMap[x, y] = energyRandomNeighbour;
                                    }
                                    else
                                    {
                                        /* do nothing */
                                    }

                                    break;
                                }
                            default:
                                break;
                        }

                    }
                    visitMap[x, y] = true;
                    ++iterator;
                }
                energy = 0;
                energyRandomNeighbour = 0;
            }
        }

        bool isOnBorder(int x, int y)
        {
            Cell[] neighbourhood;
            List<Cell> neighbourhoodRadius;
            if (boundaryCondition == 0)//periodcal
            {
                switch (neighbourhoodType)
                {
                    case 0: // von neuman
                        {
                            neighbourhood = neighbourhoodPeriodicalVonNeuman(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState())
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 1: //moore
                        {
                            neighbourhood = neighbourhoodPeriodicalMoore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState())
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 2: //penta
                        {
                            neighbourhood = neighbourhoodPeriodicalPentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState())
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 3: //heksalewe
                        {
                            neighbourhood = neighbourhoodPeriodicalHeksaLeft(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState())
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 4://heksa prawe
                        {
                            neighbourhood = neighbourhoodPeriodicalHeksaRight(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState())
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 5://heksa random
                        {
                            neighbourhood = neighbourhoodPeriodicalHeksaRandom(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState())
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 6://radiusowe
                        {
                            neighbourhoodRadius = neighbourhoodPeriodicalRadius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].getCellState() != Map[x, y].getCellState())
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            else //absorbujące
            {
                switch (neighbourhoodType)
                {
                    case 0: // von neuman
                        {
                            neighbourhood = neighbourhoodAbsorbingVonNeuman(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState() && neighbourhood[i].getCellState() != 0)
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 1: //moore
                        {
                            neighbourhood = neighbourhoodAbsorbingMoore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState() && neighbourhood[i].getCellState() != 0)
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 2: //penta
                        {
                            neighbourhood = neighbourhoodAbsorbingPentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState() && neighbourhood[i].getCellState() != 0)
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 3: //heksalewe
                        {
                            neighbourhood = neighbourhoodAbsorbingHeksaLeft(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState() && neighbourhood[i].getCellState() != 0)
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 4://heksa prawe
                        {
                            neighbourhood = neighbourhoodAbsorbingHeksaRight(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState() && neighbourhood[i].getCellState() != 0)
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 5://heksa random
                        {
                            neighbourhood = neighbourhoodAbsorbingHeksaRandom(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getCellState() != Map[x, y].getCellState() && neighbourhood[i].getCellState() != 0)
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    case 6://radiusowe
                        {
                            neighbourhoodRadius = neighbourhoodAbsorbingRadius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].getCellState() != Map[x, y].getCellState() && neighbourhoodRadius[i].getCellState() != 0)
                                {
                                    return true;
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            return false;
        }

        int isRecrystalizedNeighbour(int x, int y)
        {
            Cell[] neighbourhood;
            List<Cell> neighbourhoodRadius;
            if (boundaryCondition == 0)//periodcal
            {
                switch (neighbourhoodType)
                {
                    case 0: // von neuman
                        {
                            neighbourhood = neighbourhoodPeriodicalVonNeuman(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 1: //moore
                        {
                            neighbourhood = neighbourhoodPeriodicalMoore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 2: //penta
                        {
                            neighbourhood = neighbourhoodPeriodicalPentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 3: //heksalewe
                        {
                            neighbourhood = neighbourhoodPeriodicalHeksaLeft(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 4://heksa prawe
                        {
                            neighbourhood = neighbourhoodPeriodicalHeksaRight(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 5://heksa random
                        {
                            neighbourhood = neighbourhoodPeriodicalHeksaRandom(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 6://radiusowe
                        {
                            neighbourhoodRadius = neighbourhoodPeriodicalRadius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].getRecristalisation() == true)
                                {
                                    return neighbourhoodRadius[i].getCellState();
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            else //absorbujące
            {
                switch (neighbourhoodType)
                {
                    case 0: // von neuman
                        {
                            neighbourhood = neighbourhoodAbsorbingVonNeuman(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 1: //moore
                        {
                            neighbourhood = neighbourhoodAbsorbingMoore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 2: //penta
                        {
                            neighbourhood = neighbourhoodAbsorbingPentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 3: //heksalewe
                        {
                            neighbourhood = neighbourhoodAbsorbingHeksaLeft(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 4://heksa prawe
                        {
                            neighbourhood = neighbourhoodAbsorbingHeksaRight(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 5://heksa random
                        {
                            neighbourhood = neighbourhoodAbsorbingHeksaRandom(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getRecristalisation() == true)
                                {
                                    return neighbourhood[i].getCellState();
                                }
                            }
                            break;
                        }
                    case 6://radiusowe
                        {
                            neighbourhoodRadius = neighbourhoodAbsorbingRadius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].getRecristalisation() == true)
                                {
                                    return neighbourhoodRadius[i].getCellState();
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            return 0;
        }

        public int getNumberOfStates()
        {
            return this.numberOfStates;
        }

        bool CheckIfNeighbourhoodDislocationsIsSmaller(int x, int y)
        {
            {
                Cell[] neighbourhood;
                List<Cell> neighbourhoodRadius;

                if (boundaryCondition == 0)
                {
                    switch (neighbourhoodType)
                    {
                        case 0:
                            neighbourhood = neighbourhoodPeriodicalVonNeuman(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 1:
                            neighbourhood = neighbourhoodPeriodicalMoore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 2:
                            neighbourhood = neighbourhoodPeriodicalPentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 3:
                            neighbourhood = neighbourhoodAbsorbingHeksaLeft(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 4:
                            neighbourhood = neighbourhoodAbsorbingHeksaRight(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 5:
                            neighbourhood = neighbourhoodPeriodicalHeksaRandom(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 6:
                            neighbourhoodRadius = neighbourhoodPeriodicalRadius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;

                    }
                }
                else if (boundaryCondition == 1)
                {
                    switch (neighbourhoodType)
                    {
                        case 0:
                            neighbourhood = neighbourhoodAbsorbingVonNeuman(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 1:
                            neighbourhood = neighbourhoodAbsorbingMoore(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 2:
                            neighbourhood = neighbourhoodAbsorbingPentagonal(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 3:
                            neighbourhood = neighbourhoodAbsorbingHeksaLeft(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 4:
                            neighbourhood = neighbourhoodAbsorbingHeksaRight(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 5:
                            neighbourhood = neighbourhoodPeriodicalHeksaRandom(x, y);
                            for (int i = 0; i < neighbourhood.Length; i++)
                            {
                                if (neighbourhood[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;
                        case 6:
                            neighbourhoodRadius = neighbourhoodAbsorbingRadius(x, y);
                            for (int i = 0; i < neighbourhoodRadius.Count; i++)
                            {
                                if (neighbourhoodRadius[i].getDislocationDensity() < previousIteration[x, y].getDislocationDensity())
                                    return false;
                            }
                            break;

                    }
                }
                return true;
            }
        }

        public void calculateRecrystallization(double deltaT, double precentage)
        {
            double A = 86710969050178.5;
            double B = 9.41268203527779;
            double propability;
            double roCritical;

            if (t == 0.0)
            {
                deltaDislocationDensity = (A / B) + (1 - (A / B)) * Math.Exp(-1 * B * t);
            }
            else
            {
                deltaDislocationDensity = (A / B) + (1 - (A / B)) * Math.Exp(-1 * B * t) - dislocationDensity;
            }
            dislocationDensity = (A / B) + (1 - (A / B)) * Math.Exp(-1 * B * t);
            t = t + deltaT;


            File.AppendAllText(docPath, dislocationDensity + Environment.NewLine);

            recrystallizationPackage = (deltaDislocationDensity / (Map_height * Map_width)) * precentage;
            
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    Map[i, j].setDislocationDensity(recrystallizationPackage);
                    deltaDislocationDensity -= recrystallizationPackage;
                }
            }

            double randomPackage;
            int randX;
            int randY;


            while (deltaDislocationDensity > 0)
            {
                randX = recrystallizationGenerator.Next(0, Map_height);
                randY = recrystallizationGenerator.Next(0, Map_width);

                if (isOnBorder(randX, randY))
                {
                    randomPackage = (deltaDislocationDensity * recrystallizationGenerator.NextDouble()) / 100.0;
                    propability = propabilityGen.NextDouble();

                    if (propability >= 0.2)
                    {
                        Map[randX, randY].setDislocationDensity(deltaDislocationDensity);
                        deltaDislocationDensity -= randomPackage;
                    }

                    if (deltaDislocationDensity < 0.00001)
                    {
                        deltaDislocationDensity = 0.0;
                    }

                }
                else
                {
                    randomPackage = (deltaDislocationDensity * recrystallizationGenerator.NextDouble()) / 100.0;
                    propability = propabilityGen.NextDouble();

                    if (propability < 0.2)
                    {
                        Map[randX, randY].setDislocationDensity(deltaDislocationDensity);
                        deltaDislocationDensity -= randomPackage;
                    }

                    if (deltaDislocationDensity < 0.00001)
                    {
                        deltaDislocationDensity = 0.0;
                    }
                }
            }

            roCritical = dislocationDensity / (Map_height * Map_width);

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {                  

                    if (isOnBorder(i, j) && (roCritical < Map[i,j].getDislocationDensity()) && !Map[i,j].getRecristalisation())
                    {
                        Map[i, j].setRecristalisation(true);
                        Map[i, j].setDislocationDensity(0.0);
                        Map[i, j].setCellState(++numberOfStates);
                    }
                }
            }

            calculatePreviousIteration();

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    int tmp = isRecrystalizedNeighbour(i, j);
                    if (tmp != 0 && !CheckIfNeighbourhoodDislocationsIsSmaller(i, j))
                    {
                        Map[i, j].setRecristalisation(true);
                        Map[i, j].setDislocationDensity(0.0);
                        Map[i, j].setCellState(tmp);
                    }
                }
            }
            
        }

    }
}
