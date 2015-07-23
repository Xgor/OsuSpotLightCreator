using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Globalization;


namespace osu_Spotlight_Creator
{
    public partial class Form1 : Form
    {
        int xOffset = 48;
        int yOffset = 64;
        public Form1()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        // Ask for the map to add Spotlight for
        private void LoadMap_Click(object sender, EventArgs e)
        {
            openMapDialog.DefaultExt = ".osu";
            openMapDialog.Filter = "Osu Map files|*.osu";
            openMapDialog.Multiselect = false;
            openMapDialog.Title = "Load Map File";
            openMapDialog.InitialDirectory = Environment.CurrentDirectory;
            DialogResult result = openMapDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            mapTextBox.Text = openMapDialog.FileName;

        }

        private void generateButton_Click(object sender, EventArgs e)
        {

            try
            {

                using (Stream openFileStream = openMapDialog.OpenFile())
                {
                    progressBar.Show();
                    progressBar.Value = 0;
                    StreamReader reader = new StreamReader(openFileStream);
                    
                    int lineCount = 0;
                    float SliderMultiplier;
                    float BPM;
                    while (!reader.EndOfStream)
                    {
                        lineCount++;
                        String line = reader.ReadLine();

                        // Searches for spesific parts in the osu file to get the data needed

                        // Looks for SliderMultiplier as it's needed for calculating slider speed
                        if (line.Contains("SliderMultiplier:"))
                        {
                            line = line.Replace("SliderMultiplier:", "");
                            line = line.Replace(".", ",");
                            SliderMultiplier = floatParser(line);
                            progressBar.Value = 10;
                        }

                        // Looks for BPM as it's needed for calculating slider speed
                        else if (line == "[TimingPoints]")
                        {
                            getNextInt(ref reader);
                            BPM =getNextFloat(ref reader);
                            reader.ReadLine();
                            progressBar.Value = 20;
                        }
                        // Searches for the Hitobjects and saves the position on every point the spotlight should follow
                        else if (line == "[HitObjects]")
                        {

                            Console.WriteLine("Found [HitObjects] at line " + lineCount.ToString() + " !");
                            
                            // Get all the beat data needed for the spotlight
                            List<Beat> list = new List<Beat>();

                            Beat beat;
                            while (!reader.EndOfStream)
                            {

                                beat = new Beat();
                                beat.x = (short)(getNextInt(ref reader) + xOffset);
                                beat.y = (short)(getNextInt(ref reader) + yOffset);
                                beat.time = getNextInt(ref reader);
                                if( 3 < getNextInt(ref reader) )
                                {
                                    beat.newCombo = true;
                                }
                                else
                                {
                                    beat.newCombo = false;
                                }
                                
                                list.Add(beat);

                                getNextInt(ref reader);


                                char SliderType = (char)reader.Read();

                                // If PeppySlider
                                if((char)reader.Peek() == 'P')
                                {

                                }
                                // If Beizer Slider
                                else if((char)reader.Peek() == 'B')
                                {

                                }
                                // If Linear Slider
                                else if ((char)reader.Peek() == 'L')
                                {

                                }
                                reader.ReadLine();
                            }
                            progressBar.Value = 60;

                            
                            openFileStream.Position = 0;

                            string finalOsuFile = reader.ReadToEnd();
                            reader.Close() ;
                            
                            // Prints out the storyboard data for the spotlight
                            string sbText = "Storyboard Layer 3 (Foreground)" + Environment.NewLine
                                +"Sprite,Foreground,Centre," +imgTextBox.Text.ToString() +",320,320";
                            for (int i = 1; i < list.Count; i++ )
                            {
                                sbText += " M,0," + list[i - 1].time.ToString() +"," + list[i].time.ToString()
                                    + "," + list[i - 1].x.ToString() +"," + list[i - 1].y.ToString()
                                    + "," + list[i].x.ToString() + "," + list[i].y.ToString()
                                    + Environment.NewLine;
                            }
                            progressBar.Value = 80;


                            finalOsuFile =finalOsuFile.Replace("Storyboard Layer 3 (Foreground)", sbText);

                            progressBar.Value = 90;
                            FileStream iStream = new FileStream(mapTextBox.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); 
                            StreamWriter writer = new StreamWriter(mapTextBox.Text);



                            progressBar.Value = 95;
                            writer.Write(finalOsuFile);
                            writer.Close();

                            progressBar.Value = 100;
                            MessageBox.Show("Spotlight SB is added!");
                            progressBar.Hide();
                            break;
                        }
                    }
              //      Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }
             
        }

        private int getNextInt(ref StreamReader reader)
        {
            String tempString = "0";
            while((char)reader.Peek() != ',')
            {
                tempString += (char)reader.Read();
            }
            reader.Read();
            return int.Parse(tempString);
        }
        private float getNextFloat(ref StreamReader reader)
        {
            String tempString = "0";
            while ((char)reader.Peek() != ',')
            {
                tempString += (char)reader.Read();
            }
            reader.Read();
            return floatParser(tempString);

        }

        private float floatParser(string floatString)
        {
            try
            {
                float number = Single.Parse(floatString, CultureInfo.InvariantCulture);
                Console.WriteLine("{0} -> {1}", floatString, number);
                return number;
            }
            catch (FormatException)
            {
                Console.WriteLine("'{0}' is not in a valid format.", floatString);
            }
            catch (OverflowException)
            {
                Console.WriteLine("{0} is outside the range of a Single.", floatString);

            }
            return 0;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void mapTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void imgTextBox_TextChanged(object sender, EventArgs e)
        {

        }

    }
    public struct Beat
    {
        public short x;
        public short y;
        public int time;
        public bool newCombo;
        public bool slider;
    }
}
