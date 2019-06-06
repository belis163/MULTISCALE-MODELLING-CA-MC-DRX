namespace Sumulacja
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.GridPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.widthTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.heightTextBox = new System.Windows.Forms.TextBox();
            this.drawGrid = new System.Windows.Forms.Button();
            this.simulationButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.liczbaZarodkowCheckBox = new System.Windows.Forms.TextBox();
            this.jednorodnyButton = new System.Windows.Forms.Button();
            this.promieniowyCheckBox = new System.Windows.Forms.Button();
            this.randomStateButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.promieniowoTextBox = new System.Windows.Forms.TextBox();
            this.bcComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.columnTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rowsTextBox = new System.Windows.Forms.TextBox();
            this.neighbourhoodTypeTextBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gridCheckBox = new System.Windows.Forms.CheckBox();
            this.monteCarloButton = new System.Windows.Forms.Button();
            this.mcTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.energyButton = new System.Windows.Forms.Button();
            this.ktTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.deltaTTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dislocationButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.AtextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.BtextBox = new System.Windows.Forms.TextBox();
            this.recrystalisationEnergy = new System.Windows.Forms.Button();
            this.precentageTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GridPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // GridPictureBox
            // 
            this.GridPictureBox.Location = new System.Drawing.Point(9, 120);
            this.GridPictureBox.Name = "GridPictureBox";
            this.GridPictureBox.Size = new System.Drawing.Size(727, 490);
            this.GridPictureBox.TabIndex = 0;
            this.GridPictureBox.TabStop = false;
            this.GridPictureBox.Click += new System.EventHandler(this.GridPictureBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Szerokość Siatki";
            // 
            // widthTextBox
            // 
            this.widthTextBox.Location = new System.Drawing.Point(12, 28);
            this.widthTextBox.Name = "widthTextBox";
            this.widthTextBox.Size = new System.Drawing.Size(100, 20);
            this.widthTextBox.TabIndex = 4;
            this.widthTextBox.Text = "50";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Wysokość siatki";
            // 
            // heightTextBox
            // 
            this.heightTextBox.Location = new System.Drawing.Point(12, 75);
            this.heightTextBox.Name = "heightTextBox";
            this.heightTextBox.Size = new System.Drawing.Size(100, 20);
            this.heightTextBox.TabIndex = 14;
            this.heightTextBox.Text = "50";
            // 
            // drawGrid
            // 
            this.drawGrid.Location = new System.Drawing.Point(434, 12);
            this.drawGrid.Name = "drawGrid";
            this.drawGrid.Size = new System.Drawing.Size(155, 23);
            this.drawGrid.TabIndex = 19;
            this.drawGrid.Text = "Pusta siatka";
            this.drawGrid.UseVisualStyleBackColor = true;
            this.drawGrid.Click += new System.EventHandler(this.DrawGrid_Click);
            // 
            // simulationButton
            // 
            this.simulationButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.simulationButton.Location = new System.Drawing.Point(434, 41);
            this.simulationButton.Name = "simulationButton";
            this.simulationButton.Size = new System.Drawing.Size(184, 23);
            this.simulationButton.TabIndex = 20;
            this.simulationButton.Text = "Symuluj";
            this.simulationButton.UseVisualStyleBackColor = false;
            this.simulationButton.Click += new System.EventHandler(this.SimulationButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.stopButton.Location = new System.Drawing.Point(434, 70);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(184, 23);
            this.stopButton.TabIndex = 10;
            this.stopButton.Text = "Zatrzymaj";
            this.stopButton.UseVisualStyleBackColor = false;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Liczba zarodków";
            // 
            // liczbaZarodkowCheckBox
            // 
            this.liczbaZarodkowCheckBox.Location = new System.Drawing.Point(140, 28);
            this.liczbaZarodkowCheckBox.Name = "liczbaZarodkowCheckBox";
            this.liczbaZarodkowCheckBox.Size = new System.Drawing.Size(100, 20);
            this.liczbaZarodkowCheckBox.TabIndex = 24;
            this.liczbaZarodkowCheckBox.Text = "50";
            // 
            // jednorodnyButton
            // 
            this.jednorodnyButton.Location = new System.Drawing.Point(260, 12);
            this.jednorodnyButton.Name = "jednorodnyButton";
            this.jednorodnyButton.Size = new System.Drawing.Size(155, 23);
            this.jednorodnyButton.TabIndex = 25;
            this.jednorodnyButton.Text = "Jednorodny";
            this.jednorodnyButton.UseVisualStyleBackColor = true;
            this.jednorodnyButton.Click += new System.EventHandler(this.JednorodnyButton_Click);
            // 
            // promieniowyCheckBox
            // 
            this.promieniowyCheckBox.Location = new System.Drawing.Point(260, 41);
            this.promieniowyCheckBox.Name = "promieniowyCheckBox";
            this.promieniowyCheckBox.Size = new System.Drawing.Size(155, 23);
            this.promieniowyCheckBox.TabIndex = 26;
            this.promieniowyCheckBox.Text = "Promieniowo";
            this.promieniowyCheckBox.UseVisualStyleBackColor = true;
            this.promieniowyCheckBox.Click += new System.EventHandler(this.PromieniowyCheckBox_Click);
            // 
            // randomStateButton
            // 
            this.randomStateButton.Location = new System.Drawing.Point(260, 70);
            this.randomStateButton.Name = "randomStateButton";
            this.randomStateButton.Size = new System.Drawing.Size(155, 23);
            this.randomStateButton.TabIndex = 27;
            this.randomStateButton.Text = "Random";
            this.randomStateButton.UseVisualStyleBackColor = true;
            this.randomStateButton.Click += new System.EventHandler(this.RandomStateButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Promień";
            // 
            // promieniowoTextBox
            // 
            this.promieniowoTextBox.Location = new System.Drawing.Point(140, 75);
            this.promieniowoTextBox.Name = "promieniowoTextBox";
            this.promieniowoTextBox.Size = new System.Drawing.Size(100, 20);
            this.promieniowoTextBox.TabIndex = 29;
            this.promieniowoTextBox.Text = "5";
            // 
            // bcComboBox
            // 
            this.bcComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcComboBox.FormattingEnabled = true;
            this.bcComboBox.Items.AddRange(new object[] {
            "periodic",
            "absorbing"});
            this.bcComboBox.Location = new System.Drawing.Point(624, 43);
            this.bcComboBox.Name = "bcComboBox";
            this.bcComboBox.Size = new System.Drawing.Size(121, 21);
            this.bcComboBox.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(640, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Warunki brzegowe";
            // 
            // columnTextBox
            // 
            this.columnTextBox.Location = new System.Drawing.Point(765, 107);
            this.columnTextBox.Name = "columnTextBox";
            this.columnTextBox.Size = new System.Drawing.Size(100, 20);
            this.columnTextBox.TabIndex = 32;
            this.columnTextBox.Text = "6";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(762, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Ilość kolumn";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(762, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Ilość wierszy";
            // 
            // rowsTextBox
            // 
            this.rowsTextBox.Location = new System.Drawing.Point(765, 146);
            this.rowsTextBox.Name = "rowsTextBox";
            this.rowsTextBox.Size = new System.Drawing.Size(100, 20);
            this.rowsTextBox.TabIndex = 35;
            this.rowsTextBox.Text = "10";
            // 
            // neighbourhoodTypeTextBox
            // 
            this.neighbourhoodTypeTextBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.neighbourhoodTypeTextBox.FormattingEnabled = true;
            this.neighbourhoodTypeTextBox.Items.AddRange(new object[] {
            "VonNeuman",
            "Moore\'a",
            "PentagonalneLosowe",
            "HeksagonalneLewe",
            "HeksagonalnePrawe",
            "HeksagonalneRandomowe",
            "Promieniowe"});
            this.neighbourhoodTypeTextBox.Location = new System.Drawing.Point(777, 43);
            this.neighbourhoodTypeTextBox.Name = "neighbourhoodTypeTextBox";
            this.neighbourhoodTypeTextBox.Size = new System.Drawing.Size(147, 21);
            this.neighbourhoodTypeTextBox.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(795, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Sąsiedztwo";
            // 
            // gridCheckBox
            // 
            this.gridCheckBox.AutoSize = true;
            this.gridCheckBox.Location = new System.Drawing.Point(624, 70);
            this.gridCheckBox.Name = "gridCheckBox";
            this.gridCheckBox.Size = new System.Drawing.Size(56, 17);
            this.gridCheckBox.TabIndex = 38;
            this.gridCheckBox.Text = "Siatka";
            this.gridCheckBox.UseVisualStyleBackColor = true;
            // 
            // monteCarloButton
            // 
            this.monteCarloButton.Location = new System.Drawing.Point(765, 222);
            this.monteCarloButton.Name = "monteCarloButton";
            this.monteCarloButton.Size = new System.Drawing.Size(155, 23);
            this.monteCarloButton.TabIndex = 39;
            this.monteCarloButton.Text = "MonteCarlo";
            this.monteCarloButton.UseVisualStyleBackColor = true;
            this.monteCarloButton.Click += new System.EventHandler(this.MonteCarloButton_Click);
            // 
            // mcTextBox
            // 
            this.mcTextBox.Location = new System.Drawing.Point(765, 196);
            this.mcTextBox.Name = "mcTextBox";
            this.mcTextBox.Size = new System.Drawing.Size(100, 20);
            this.mcTextBox.TabIndex = 40;
            this.mcTextBox.Text = "10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(766, 180);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 41;
            this.label9.Text = "Ilość iteracji";
            // 
            // energyButton
            // 
            this.energyButton.Location = new System.Drawing.Point(769, 582);
            this.energyButton.Name = "energyButton";
            this.energyButton.Size = new System.Drawing.Size(155, 23);
            this.energyButton.TabIndex = 42;
            this.energyButton.Text = "Energia/Strukturka";
            this.energyButton.UseVisualStyleBackColor = true;
            this.energyButton.Click += new System.EventHandler(this.EnergyButton_Click);
            // 
            // ktTextBox
            // 
            this.ktTextBox.Location = new System.Drawing.Point(765, 264);
            this.ktTextBox.Name = "ktTextBox";
            this.ktTextBox.Size = new System.Drawing.Size(100, 20);
            this.ktTextBox.TabIndex = 43;
            this.ktTextBox.Text = "0.1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(766, 248);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 44;
            this.label10.Text = "kT";
            // 
            // deltaTTextBox
            // 
            this.deltaTTextBox.Location = new System.Drawing.Point(765, 303);
            this.deltaTTextBox.Name = "deltaTTextBox";
            this.deltaTTextBox.Size = new System.Drawing.Size(100, 20);
            this.deltaTTextBox.TabIndex = 45;
            this.deltaTTextBox.Text = "0.001";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(766, 287);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 46;
            this.label11.Text = "delta T";
            // 
            // dislocationButton
            // 
            this.dislocationButton.Location = new System.Drawing.Point(769, 501);
            this.dislocationButton.Name = "dislocationButton";
            this.dislocationButton.Size = new System.Drawing.Size(155, 23);
            this.dislocationButton.TabIndex = 47;
            this.dislocationButton.Text = "dyslokacja";
            this.dislocationButton.UseVisualStyleBackColor = true;
            this.dislocationButton.Click += new System.EventHandler(this.DislocationButton_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(766, 336);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "A";
            // 
            // AtextBox
            // 
            this.AtextBox.Location = new System.Drawing.Point(765, 352);
            this.AtextBox.Name = "AtextBox";
            this.AtextBox.Size = new System.Drawing.Size(155, 20);
            this.AtextBox.TabIndex = 49;
            this.AtextBox.Text = "86710969050178.5";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(766, 386);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "B";
            // 
            // BtextBox
            // 
            this.BtextBox.Location = new System.Drawing.Point(765, 402);
            this.BtextBox.Name = "BtextBox";
            this.BtextBox.Size = new System.Drawing.Size(155, 20);
            this.BtextBox.TabIndex = 51;
            this.BtextBox.Text = "9.41268203527779";
            // 
            // recrystalisationEnergy
            // 
            this.recrystalisationEnergy.Location = new System.Drawing.Point(769, 553);
            this.recrystalisationEnergy.Name = "recrystalisationEnergy";
            this.recrystalisationEnergy.Size = new System.Drawing.Size(155, 23);
            this.recrystalisationEnergy.TabIndex = 52;
            this.recrystalisationEnergy.Text = "Rekrystalizacja/Energia";
            this.recrystalisationEnergy.UseVisualStyleBackColor = true;
            this.recrystalisationEnergy.Click += new System.EventHandler(this.RecrystalisationEnergy_Click);
            // 
            // precentageTextBox
            // 
            this.precentageTextBox.Location = new System.Drawing.Point(765, 465);
            this.precentageTextBox.Name = "precentageTextBox";
            this.precentageTextBox.Size = new System.Drawing.Size(155, 20);
            this.precentageTextBox.TabIndex = 53;
            this.precentageTextBox.Text = "0.2";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(762, 449);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(15, 13);
            this.label14.TabIndex = 54;
            this.label14.Text = "%";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 622);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.precentageTextBox);
            this.Controls.Add(this.recrystalisationEnergy);
            this.Controls.Add(this.BtextBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.AtextBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dislocationButton);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.deltaTTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ktTextBox);
            this.Controls.Add(this.energyButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.mcTextBox);
            this.Controls.Add(this.monteCarloButton);
            this.Controls.Add(this.gridCheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.neighbourhoodTypeTextBox);
            this.Controls.Add(this.rowsTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.columnTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bcComboBox);
            this.Controls.Add(this.promieniowoTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.randomStateButton);
            this.Controls.Add(this.promieniowyCheckBox);
            this.Controls.Add(this.jednorodnyButton);
            this.Controls.Add(this.liczbaZarodkowCheckBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.simulationButton);
            this.Controls.Add(this.drawGrid);
            this.Controls.Add(this.heightTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.widthTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GridPictureBox);
            this.Name = "Form1";
            this.Text = "Symulacja";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox GridPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox widthTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox heightTextBox;
        private System.Windows.Forms.Button drawGrid;
        private System.Windows.Forms.Button simulationButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox liczbaZarodkowCheckBox;
        private System.Windows.Forms.Button jednorodnyButton;
        private System.Windows.Forms.Button promieniowyCheckBox;
        private System.Windows.Forms.Button randomStateButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox promieniowoTextBox;
        private System.Windows.Forms.ComboBox bcComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox columnTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox rowsTextBox;
        private System.Windows.Forms.ComboBox neighbourhoodTypeTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox gridCheckBox;
        private System.Windows.Forms.Button monteCarloButton;
        private System.Windows.Forms.TextBox mcTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button energyButton;
        private System.Windows.Forms.TextBox ktTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox deltaTTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button dislocationButton;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox AtextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox BtextBox;
        private System.Windows.Forms.Button recrystalisationEnergy;
        private System.Windows.Forms.TextBox precentageTextBox;
        private System.Windows.Forms.Label label14;
    }
}

