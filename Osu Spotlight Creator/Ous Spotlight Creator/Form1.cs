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
        int xOffset = 64;
        int yOffset = 56;
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
            openMapDialog.Filter = ".osu Map files|*.osu";
            openMapDialog.Multiselect = false;
            openMapDialog.Title = "Load Map File";
            if (mapTextBox.Text == "")
            {
                openMapDialog.InitialDirectory = Environment.CurrentDirectory;
            }
            else
            {
                openMapDialog.InitialDirectory = mapTextBox.Text;
            }
            DialogResult result = openMapDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            mapTextBox.Text = openMapDialog.FileName;
       //     mapTextBox.Enabled = true;
            LoadImg.Enabled = true;


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
       //             float SliderMultiplier;
       //             float BPM;
                    while (!reader.EndOfStream)
                    {
                        lineCount++;
                        String line = reader.ReadLine();

                        // Searches for spesific parts in the osu file to get the data needed

                        /*
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
                        else 
                            */
                        if (line == "[HitObjects]")
                        {

                            Console.WriteLine("Found [HitObjects] at line " + lineCount.ToString() + " !");
                            
                            // Get all the beat data needed for the spotlight
                            List<Beat> list = new List<Beat>();
                           
                            Beat beat;
                            while (!reader.EndOfStream)
                            {

                                // Create a beat and get the X Y and time of the beat
                                beat = new Beat();


                                beat.x = (short)(getNextInt(ref reader) + xOffset);

                                beat.y = (short)(getNextInt(ref reader) + yOffset);
                                beat.time = getNextInt(ref reader);

                                // Checks if there is a new combo NOT WORKING
                                /*
                                if( 3 < getNextInt(ref reader) )
                                {
                                    beat.newCombo = true;
                                }
                                else
                                {
                                    beat.newCombo = false;
                                }
                                */
                                getNextInt(ref reader);
                               
                                // Get the hitsound but dosn't do anything with it
                                getNextInt(ref reader);
                          //      Console.WriteLine("Gaa");

                                char SliderType = (char)reader.Read();

                                // If PeppySlider
                                if (SliderType == 'P')
                                {
                                    beat.slider = true;
                                    list.Add(beat);
                                    openFileStream.Position++;
                       //             beat = new Beat();
                                }
                                // If Beizer Slider
                                else if (SliderType == 'B')
                                {
                                    beat.slider = true;
                                    list.Add(beat);
                                    openFileStream.Position++;
            //                        beat = new Beat();
                                }
                                // If Linear Slider
                                else if (SliderType == 'L')
                                {
                                    int previousBeatTime = beat.time;
                                    beat.slider = true;

                                    reader.Read();
                                    list.Add(beat);
            

                                    Beat sliderBeat = new Beat();

                                    sliderBeat.x = (short)(getNextInt(ref reader) + xOffset);
                                 
                                    sliderBeat.y = (short)(getNextInt(ref reader) + yOffset);
                                    getNextInt(ref reader);
                                    sliderBeat.time = getNextInt(ref reader)*2 + previousBeatTime;
                                  
                                    list.Add(sliderBeat);

                            
                                    
                                }
                                else
                                {
                                    beat.slider = true;
                                    list.Add(beat);
                                }
                           
                                reader.ReadLine();
                     
                            }
                            progressBar.Value = 60;

                            
                            openFileStream.Position = 0;

                            string finalOsuFile = reader.ReadToEnd();
                            reader.Close() ;
                            
                            // Prints out the storyboard data for the spotlight
                            string sbText = "Storyboard Layer 3 (Foreground)" + Environment.NewLine
                                + "Sprite,Foreground,Centre," + imgTextBox.Text.ToString() + ",320,320" + Environment.NewLine;
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
            while ((char)reader.Peek() != ',' && (char)reader.Peek() != ':' && (char)reader.Peek() != '|')
            {
                tempString += (char)reader.Read();
      
            }

            reader.Read();


            int returnInt;
            if(int.TryParse(tempString, out returnInt))
            {
                return returnInt;
            }
            else
            {
                Console.WriteLine("Failed to parse into int");
                return 0;
            }
       //     return int.Parse(tempString);
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

        private void LoadImg_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".png";
            openFileDialog1.Filter = ".png images|*.png";
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Highlight image";
            openFileDialog1.InitialDirectory = mapTextBox.Text;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result != DialogResult.OK) return;

            string mapFolder = mapTextBox.Text.Remove(mapTextBox.Text.LastIndexOf('\\')+1);
       //     text = mapTextBox.Text.Remove();
            imgTextBox.Text = openFileDialog1.FileName.Replace(mapFolder, "");

    //        generateButton.Enabled = true;
            
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
