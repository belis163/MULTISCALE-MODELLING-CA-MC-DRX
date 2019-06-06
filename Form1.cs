using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Globalization;

namespace Sumulacja
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        int radius = 0;
        int Map_width = 0;
        int Map_height = 0;
        int numberOfStates;
        static int click_counter = 0;
        int rows = 0;
        int columns = 0;
        Grid Map;
        System.Timers.Timer aTimer;
        System.Timers.Timer mcTimer;
        System.Timers.Timer DRXTimer;
        double precentage;
        int BC = 0;
        int neighbourhoodType;
        List<Color>  colorTable;
        Random random = new Random();
        int monteCarloIterations = 0;
        int mcIndex = 0;
        bool flagMC = false;
        bool flagRec = false;
        double kT = 1.0;
        double deltaT = 0.0;
        double A, B;


        public Form1()
        {
            InitializeComponent();
            monteCarloButton.Enabled = false;
        }

        void setColorTable()
        {
            try
            {
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);

                if (numberOfStates < 0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki dodatnie");
                return;
            }


            colorTable = new List<Color>();
            int R, G, B;
            for (int i = 0; i < numberOfStates + 1; i++)
            {
                R = random.Next(256);
                G = random.Next(256);
                B = random.Next(256);

                if (!colourExist(R, G, B))
                {
                    colorTable.Add(Color.FromArgb(R, G, B));
                }
                else
                {
                    while (colourExist(R, G, B))
                    {
                        R = random.Next(256);
                        G = random.Next(256);
                        B = random.Next(256);
                    }
                }
            }
        }

        bool colourExist(int R, int G, int B)
        {
            bool exist = false;

            for (int i = 0; i < colorTable.Count; i++)
            {
                if (colorTable[i] == Color.FromArgb(R, G, B))
                {
                    exist = true;
                }
            }
            return exist;
        }

        void updateColourTable()
        {
            try
            {
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);

                if (numberOfStates < 0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki dodatnie");
                return;
            }


            int R, G, B;
            for (int i = colorTable.Count; i < Map.getNumberOfStates() + 1; i++)
            {
                R = random.Next(256);
                G = random.Next(256);
                B = random.Next(256);

                if (!colourExist(R, G, B))
                {
                    colorTable.Add(Color.FromArgb(R, G, B));
                }
                else
                {
                    while (colourExist(R, G, B))
                    {
                        R = random.Next(256);
                        G = random.Next(256);
                        B = random.Next(256);
                    }
                }
            }
        }

        private void DrawCells(Cell[,] Maps)
        {
            GridPictureBox.Image = new Bitmap(GridPictureBox.Width, GridPictureBox.Height);
            graphics = Graphics.FromImage(this.GridPictureBox.Image);

            SolidBrush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(Color.Black, (float)0.0001);
            float x = 0, y = 0;


            
            float RecWidth = (float)GridPictureBox.Width / (float)Map_width;
            float RecHeight = (float)GridPictureBox.Height / (float)Map_height;
            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }

            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Maps[i, j].getCellState() != 0)
                    {
                        numberOfStates = Map.getNumberOfStates() + 1;
                        for (int k = 1; k < colorTable.Count + 1; k++)
                        {
                            if (Maps[i, j].getCellState() == k)
                            {
                                brush.Color = colorTable[k];
                            }
                        }
                        //brush.Color = Color.FromArgb(Map[i, j].getCellState() * transition / 2, Map[i, j].getCellState() * transition / 3, Map[i, j].getCellState() * transition / 4);
                    }
                    else
                    {
                        brush.Color = Color.LightGray;
                    }
                    graphics.FillRectangle(brush, x, y, RecWidth, RecHeight);

                    if (gridCheckBox.Checked)
                    {
                        graphics.DrawRectangle(pen, x, y, RecWidth, RecHeight);
                    }

                    x += RecWidth;
                }

                x = 0;
                y += RecHeight;
            }
        }

        private void DrawCells(int[,] Map)
        {
            GridPictureBox.Image = new Bitmap(GridPictureBox.Width, GridPictureBox.Height);
            graphics = Graphics.FromImage(this.GridPictureBox.Image);

            SolidBrush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(Color.Black, (float)0.0001);
            float x = 0, y = 0;

            float RecWidth = (float)GridPictureBox.Width / (float)Map_width;
            float RecHeight = (float)GridPictureBox.Height / (float)Map_height;
            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j] != 0)
                    {
                        brush.Color = Color.Red;
                    }

                    else
                    {
                        brush.Color = Color.White;
                    }

                    graphics.FillRectangle(brush, x, y, RecWidth, RecHeight);
                    if (gridCheckBox.Checked == true)
                    {
                        graphics.DrawRectangle(pen, x, y, RecWidth, RecHeight);
                    }
                    x += RecWidth;
                }

                x = 0;
                y += RecHeight;

            }
        }

        private void DrawDRX(Cell[,] Map)
        {
            GridPictureBox.Image = new Bitmap(GridPictureBox.Width, GridPictureBox.Height);
            graphics = Graphics.FromImage(this.GridPictureBox.Image);

            SolidBrush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(Color.Black, (float)0.0001);
            float x = 0, y = 0;

            float RecWidth = (float)GridPictureBox.Width / (float)Map_width;
            float RecHeight = (float)GridPictureBox.Height / (float)Map_height;
            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }


            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {

                    if (Map[i, j].getRecristalisation())
                    {
                        brush.Color = Color.Red;
                    }

                    else
                    {
                        brush.Color = Color.White;
                    }

                    graphics.FillRectangle(brush, x, y, RecWidth, RecHeight);
                    if (gridCheckBox.Checked == true)
                    {
                        graphics.DrawRectangle(pen, x, y, RecWidth, RecHeight);
                    }
                    x += RecWidth;
                }

                x = 0;
                y += RecHeight;

            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (aTimer.Enabled == true)
            {
                aTimer.Enabled = false;
            }
            else
            {
                /* do nothing */
            }

            if (mcTimer.Enabled == true)
            {
                mcTimer.Enabled = false;
            }
            else
            {
                /* do nothing */
            }

            mcIndex = 0;
            click_counter = 0;
        }

        private void GridPictureBox_Click(object sender, EventArgs e)
        {
            float x;
            float y;

            MouseEventArgs clickOnPB = (MouseEventArgs)e;
            PointF coordinates = clickOnPB.Location;

            x = coordinates.X;
            y = coordinates.Y;

            float RecWidth = (float)GridPictureBox.Width / (float)Map_width;
            float RecHeight = (float)GridPictureBox.Height / (float)Map_height;
            if (RecWidth > RecHeight)
            {
                RecWidth = RecHeight;
            }
            else
            {
                RecHeight = RecWidth;
            }

            int counterX = -1;
            int counterY = -1;
            while (x > 0)
            {
                x -= RecWidth;
                counterX++;
            }

            while (y > 0)
            {
                y -= RecWidth;
                counterY++;
            }


            if (click_counter <= numberOfStates)
            {
                Map.Map[counterY, counterX].setCellState(click_counter);
                DrawCells(Map.Map);
                click_counter++;
            }
        }

        private void DrawGrid_Click(object sender, EventArgs e)
        {
            click_counter = 0;
            GridPictureBox.Refresh();
            try
            {
                Map_width = int.Parse(widthTextBox.Text);
                Map_height = int.Parse(heightTextBox.Text);
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
                radius = int.Parse(promieniowoTextBox.Text);
                BC = bcComboBox.SelectedIndex;
                neighbourhoodType = neighbourhoodTypeTextBox.SelectedIndex;
                if (Map_width < 0 || Map_height < 0 || numberOfStates < 0 || radius < 0)
                    throw new System.FormatException();

                setColorTable();
                Map = new Grid(Map_width, Map_height, numberOfStates + 1, radius, BC, neighbourhoodType);
                DrawCells(Map.Map);
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki");
            }
        }

        private void OnTimedEventPeriodicalVonNeuman(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalVonNeuman();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));
            }
        }

        private void OnTimedEventAbsorbingVonNeuman(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingVonNeuman();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));
            }
        }

        private void OnTimedEventPeriodicalMoore(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalMoore();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));

            }
        }

        private void OnTimedEventAbsorbingMoore(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingMoore();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));
            }
        }

        private void OnTimedEventAbsorbingPentagonal(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingPentagonal();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }
                }));
            }

        }

        private void OnTimedEventPeriodicalPentagonal(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalPentagonal();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));

            }
        }

        private void OnTimedEventPeriodicalHeksaLeft(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalHeksaLeft();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));

            }
        }

        private void OnTimedEventAbsorbingHeksaLeft(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingHeksaLeft();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));
            }
        }

        private void OnTimedEventPeriodicalHeksaRight(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalHeksaRight();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));

            }
        }

        private void OnTimedEventAbsorbingHeksaRight(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingHeksaRight();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));

            }
        }

        private void OnTimedEventPeriodicalHeksaRandom(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalHeksaRadnom();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }
                }));
            }
        }

        private void OnTimedEventAbsorbingHeksaRandom(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingHeksaRandom();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));
            }
        }

        private void OnTimedEventPeriodicalRadius(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorPeriodicalRadius();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));
            }
        }

        private void OnTimedEventAbsorbingRadius(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                Map.UpdateVectorAbsorbingRadius();
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);

                    if (endOfSimulation())
                    {
                        aTimer.Enabled = false;
                        monteCarloButton.Enabled = true;
                    }

                }));
            }
        }

        private void OnTimedEventMonteCarlo(Object source, ElapsedEventArgs e)
        {
            try
            {
                monteCarloIterations = int.Parse(mcTextBox.Text);
                if (monteCarloIterations < 0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Liczba iteracji ujemna");
            }

            for (int i = 0; i < 1; i++)
            {
                Map.calculateMonteCarlo(BC, kT);
                BeginInvoke((Action)(() =>
                {
                    DrawCells(Map.Map);
                    mcIndex++;

                    if (endOfMonteCarlo())
                    {
                        mcTimer.Enabled = false;
                        mcIndex = 0;
                    }
                }));
            }


        }

        bool endOfMonteCarlo()
        {
            if (mcIndex == monteCarloIterations)
            {
                return true;
            }
            return false;
        }


        private void SimulationButton_Click(object sender, EventArgs e)
        {
            monteCarloButton.Enabled = false;

            mcTimer = new System.Timers.Timer();
            mcTimer.Interval = 500;
            mcTimer.AutoReset = true;
            mcTimer.Enabled = true;

            BC = bcComboBox.SelectedIndex;
            neighbourhoodType = neighbourhoodTypeTextBox.SelectedIndex;
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 500;

            if (neighbourhoodType == 0)
            {
                if (BC == 0)
                {
                    aTimer.Elapsed += OnTimedEventPeriodicalVonNeuman;
                }
                else if (BC == 1)
                {
                    aTimer.Elapsed += OnTimedEventAbsorbingVonNeuman;
                }

            }
            else if (neighbourhoodType == 1)
            {
                if (BC == 0)
                {
                    aTimer.Elapsed += OnTimedEventPeriodicalMoore;
                }
                else if (BC == 1)
                {
                    aTimer.Elapsed += OnTimedEventAbsorbingMoore;
                }
            }
            else if (neighbourhoodType == 2)
            {
                if (BC == 0)
                {
                    aTimer.Elapsed += OnTimedEventPeriodicalPentagonal;
                }
                else if (BC == 1)
                {
                    aTimer.Elapsed += OnTimedEventAbsorbingPentagonal;
                }
            }
            else if (neighbourhoodType == 3)
            {
                if (BC == 0)
                {
                    aTimer.Elapsed += OnTimedEventPeriodicalHeksaLeft;
                }
                else if (BC == 1)
                {
                    aTimer.Elapsed += OnTimedEventAbsorbingHeksaLeft;
                }
            }
            else if (neighbourhoodType == 4)
            {
                if (BC == 0)
                {
                    aTimer.Elapsed += OnTimedEventPeriodicalHeksaRight;
                }
                else if (BC == 1)
                {
                    aTimer.Elapsed += OnTimedEventAbsorbingHeksaRight;
                }
            }
            else if (neighbourhoodType == 5)
            {
                if (BC == 0)
                {
                    aTimer.Elapsed += OnTimedEventPeriodicalHeksaRandom;
                }
                else if (BC == 1)
                {
                    aTimer.Elapsed += OnTimedEventAbsorbingHeksaRandom;
                }
            }

            else if (neighbourhoodType == 6)
            {
                if (BC == 0)
                {
                    aTimer.Elapsed += OnTimedEventPeriodicalRadius;
                }
                else if (BC == 1)
                {
                    aTimer.Elapsed += OnTimedEventAbsorbingRadius;
                }
            }

            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        bool endOfSimulation()
        {
            for (int i = 0; i < Map_height; i++)
            {
                for (int j = 0; j < Map_width; j++)
                {
                    if (Map.Map[i, j].getCellState() == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void JednorodnyButton_Click(object sender, EventArgs e)
        {
            setColorTable();
            Random rand = new Random();
            GridPictureBox.Refresh();
            click_counter = 0;
            try
            {
                Map_width = int.Parse(widthTextBox.Text);
                Map_height = int.Parse(heightTextBox.Text);
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
                columns = int.Parse(columnTextBox.Text);
                rows = int.Parse(rowsTextBox.Text);
                radius = int.Parse(promieniowoTextBox.Text);
                BC = bcComboBox.SelectedIndex;
                neighbourhoodType = neighbourhoodTypeTextBox.SelectedIndex;

                if (Map_width < 0 || Map_height < 0 || numberOfStates < 0 || columns < 0 || rows < 0 || radius < 0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki");
                return;
            }

            Map = new Grid(Map_width, Map_height, numberOfStates + 1, radius, BC, neighbourhoodType);
            int deltaRows = Map_height / rows;
            int deltaColumns = Map_width / columns;
            if (deltaRows == 0)
            {
                deltaRows = 1;
            }
            if (deltaColumns == 0)
            {
                deltaRows = 1;
            }

            int cellCounter = 1;
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    if (cellCounter < numberOfStates + 1)
                    {
                        Map.Map[(i * deltaRows), (j * deltaColumns)].setCellState(cellCounter);
                        cellCounter++;
                    }
                }
            }

            DrawCells(Map.Map);
        }

        private void PromieniowyCheckBox_Click(object sender, EventArgs e)
        {
            setColorTable();
            try
            {
                Map_width = int.Parse(widthTextBox.Text);
                Map_height = int.Parse(heightTextBox.Text);
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
                BC = bcComboBox.SelectedIndex;
                radius = int.Parse(promieniowoTextBox.Text);
                neighbourhoodType = neighbourhoodTypeTextBox.SelectedIndex;
                if (Map_width < 0 || Map_height < 0 || numberOfStates < 0 || radius < 0)
                    throw new System.FormatException();
                if (numberOfStates * radius > 2 * Math.Sqrt(Map_height * Map_height + Map_width * Map_width))
                    throw new Exception();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki dodatnie");
                return;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Zły stosunek ilości ziarn do promienia i rozmiarów siatki, change my mind");
                return;
            }
            click_counter = 0;

            Map = new Grid(Map_width, Map_height, numberOfStates + 1, radius, BC, neighbourhoodType);
            int[,] allCoordinates = new int[numberOfStates, 2];
            int cellCounter = 1;
            Random generator = new Random();
            int x = 0;
            int y = 0;

            while (cellCounter < (numberOfStates + 1))
            {
                x = generator.Next(0, Map_width - 1);
                y = generator.Next(0, Map_height - 1);

                if (checkRadius(x, y, allCoordinates, cellCounter - 1, radius)/* && Map.Map[x, y].getCellState() == 0*/)
                {
                    Map.Map[x, y].setCellState(cellCounter);
                    allCoordinates[cellCounter - 1, 0] = x;
                    allCoordinates[cellCounter - 1, 1] = y;
                    ++cellCounter;
                }
            }
            DrawCells(Map.Map);
        }

        private void RandomStateButton_Click(object sender, EventArgs e)
        {
            setColorTable();
            click_counter = 0;
            Random rand = new Random();
            GridPictureBox.Refresh();

            try
            {
                Map_width = int.Parse(widthTextBox.Text);
                Map_height = int.Parse(heightTextBox.Text);
                numberOfStates = int.Parse(liczbaZarodkowCheckBox.Text);
                radius = int.Parse(promieniowoTextBox.Text);
                BC = bcComboBox.SelectedIndex;
                neighbourhoodType = neighbourhoodTypeTextBox.SelectedIndex;

                if (Map_width < 0 || Map_height < 0 || numberOfStates < 0 || radius < 0)
                    throw new System.FormatException();
            }
            catch (System.FormatException)
            {
                System.Windows.Forms.MessageBox.Show("Upośledzony czy niespełna rozumu - tylko cyferki dodatnie");
                return;
            }

            int RandomCounter = 1;

            int indexI, indexJ;

            Map = new Grid(Map_width, Map_height, numberOfStates + 1, radius, BC, neighbourhoodType);

            while (RandomCounter < numberOfStates + 1)
            {
                indexI = rand.Next(0, Map_height);
                indexJ = rand.Next(0, Map_width);
                if (Map.Map[indexI, indexJ].getCellState() == 0)
                {
                    Map.Map[indexI, indexJ].setCellState(RandomCounter);
                    RandomCounter++;
                }
                else
                {
                    indexI = rand.Next(0, Map_height);
                    indexJ = rand.Next(0, Map_width);
                }

            }
            DrawCells(Map.Map);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        bool checkRadius(int x, int y, int[,] allCoordinates, int numberOfDrawnCells, int radius)
        {
            for (int index = 0; index < numberOfDrawnCells; index++)
            {
                if (Math.Sqrt(((x - allCoordinates[index, 0]) * (x - allCoordinates[index, 0])) + ((y - allCoordinates[index, 1]) * (y - allCoordinates[index, 1]))) > radius)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void MonteCarloButton_Click(object sender, EventArgs e)
        {
            BC = bcComboBox.SelectedIndex;
            kT = Double.Parse(ktTextBox.Text, CultureInfo.InvariantCulture);
            precentage = Double.Parse(precentageTextBox.Text, CultureInfo.InvariantCulture);
            neighbourhoodType = neighbourhoodTypeTextBox.SelectedIndex;
     
            mcTimer.Elapsed += OnTimedEventMonteCarlo;


        }

        private void EnergyButton_Click(object sender, EventArgs e)
        {
            if (flagMC == true)
            {
                DrawCells(Map.Map);
                flagMC = !flagMC;
            }
            else
            {
                DrawCells(Map.energyMap);
                flagMC = !flagMC;
            }

        }

        private void RecrystalisationEnergy_Click(object sender, EventArgs e)
        {
            if (flagRec == true)
            {
                DrawDRX(Map.Map);
                flagRec = !flagRec;
            }
            else
            {
                DrawCells(Map.Map);
                flagRec = !flagRec;
            }
        }

        private void DislocationButton_Click(object sender, EventArgs e)
        {
            BC = bcComboBox.SelectedIndex;
            deltaT = Double.Parse(deltaTTextBox.Text, CultureInfo.InvariantCulture);
            precentage = Double.Parse(precentageTextBox.Text, CultureInfo.InvariantCulture);

            for (int i = 0; i < 1; i++)
            {
                Map.calculateRecrystallization(deltaT, precentage);
                BeginInvoke((Action)(() =>
                {
                    updateColourTable();
                    DrawCells(Map.Map);
                }));
            }

        }
    }
}

