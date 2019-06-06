using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumulacja
{
    class Cell
    {
        private int state;
        private double massX;
        private double massY;
        static private Random randomMass = new Random();
        private double dislocationDensity;
        private bool recristalisation;

        public Cell()
        {
            this.state = 0;

            this.massX = randomMass.NextDouble();
            this.massX = randomMass.NextDouble();
            this.dislocationDensity = 0.0;
            this.recristalisation = false;
        }

        public void copyCell(Cell cell)
        {
            this.dislocationDensity = cell.dislocationDensity;
            this.massX = cell.massX;
            this.massY = cell.massY;
            this.recristalisation = cell.recristalisation;
            this.state = cell.state;                
        }

        public Cell(int state)
        {
            this.state = state;
        }

        public void setCellState(int state)
        {
            this.state = state;
        }

        public int getCellState()
        {
            return this.state;
        }

        public double getMassX()
        {
            return this.massX;
        }

        public double getMassY()
        {
            return this.massY;
        }

        public void setDislocationDensity(double densisty)
        {
            this.dislocationDensity += densisty;
        }
        public void setRecristalisation(bool recristalisation)
        {
            this.recristalisation = recristalisation;
        }

        public bool getRecristalisation()
        {
            return this.recristalisation;
        }

        public double getDislocationDensity()
        {
            return this.dislocationDensity;
        }

        /*public int GetNeighbours(Cell[] cells, int index, int numberOfCells)
        {
            int numberOfNeighbours = 0;
      
            if(index == 0)
            {
                if(cells[index + 1].state == true)
                {
                    ++numberOfNeighbours;
                }
                if(cells[numberOfCells].state == true)
                {
                    ++numberOfCells;
                }
                
            }
            else if(index == numberOfCells)
            {
                if (cells[index - 1].state == true)
                {
                    ++numberOfNeighbours;
                }
                if (cells[0].state == true)
                {
                    ++numberOfCells;
                }
            }
            else
            {
                if(cells[index + 1].state == true)
                {
                    ++numberOfNeighbours;
                }
                if (cells[index - 1].state == true)
                {
                    ++numberOfNeighbours;
                }
            }

            return numberOfNeighbours;
        }
        */
        /*public bool HasNeighboursBefore(Cell[] cells, int index, int numberOfCells)
        {
            bool state = false;

            if(index == 0)
            {
                if(cells[numberOfCells].state == true)
                {
                    state = true;
                }
            }
            else if(cells[index - 1].state == true)
            {
                state = true;
            }

            return state;
        }*/
        /*public bool HasNeighboursAfter(Cell[] cells, int index, int numberOfCells)
        {
            bool state = false;

            if (index == numberOfCells)
            {
                if (cells[0].state == true)
                {
                    state = true;
                }
            }
            else if (cells[index + 1].state == true)
            {
                state = true;
            }
            return state;
        }*/
    }
}
