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
                    Cursor.Current = Cursors.WaitCursor;
            //        progressBar.Show();
                    progressBar.Value = 0;
                    StreamReader reader = new StreamReader(openFileStream);
                    
                    int lineCount = 0;
                    float SliderMultiplier =0;
        //            float BPM;

                    List<TimingPoint> timePointList = new List<TimingPoint>();
                    while (!reader.EndOfStream)
                    {
                        lineCount++;
                        String line = reader.ReadLine();

                        // Searches for spesific parts in the osu file to get the data needed

                        
                        // Looks for SliderMultiplier as it's needed for calculating slider speed
                        if (line.Contains("SliderMultiplier:"))
                        {
                            line = line.Replace("SliderMultiplier:", "");
                            SliderMultiplier = floatParser(line);
 //                           SliderMultiplier = SliderMultiplier;
                            Console.WriteLine("Found SliderMultiplier " + SliderMultiplier.ToString() + ".");
                        }
                        // Looks for TimingPoints to check for important Timingpoint data
                        else if (line == "[TimingPoints]")
                        {
                            int points = 0;
                            while (reader.Peek() != 13)
                            {
                                Console.WriteLine(reader.Peek());
                                points++;
                                Console.WriteLine("points nr"+points.ToString());
                                TimingPoint timingPoint = new TimingPoint();
                                timingPoint.Time = getNextInt(ref reader);
                                if (timingPoint.Time == 0)
                                {
                                    Console.WriteLine("Found all TimingSections");
                                    break;
                                }
                                jumpToNextValue(ref reader,6);

                                if((char)reader.Read() == '1')
                                {
                                    Console.WriteLine("Kiai found");
                                    timingPoint.Kiai = true;
                                }
                                else
                                {
                                    timingPoint.Kiai = false;
                                }

                     //           points

                                timePointList.Add(timingPoint);
                                reader.ReadLine();
                            }
                        }
                        // Searches for the Hitobjects and saves the position on every point the spotlight should follow
                        else if (line == "[HitObjects]")
                        {

                            Console.WriteLine("Found [HitObjects] at line " + lineCount.ToString() + " !");
                            
                            // Get all the beat data needed for the spotlight
                            List<Beat> beatList = new List<Beat>();
                           
                            Beat beat;
                            while (!reader.EndOfStream)
                            {

                                // Create a beat and get the X Y and time of the beat
                                beat = new Beat();


                                beat.x = (short)(getNextInt(ref reader) + xOffset);

                                beat.y = (short)(getNextInt(ref reader) + yOffset);
                                beat.time = getNextInt(ref reader);
                                Console.WriteLine(beat.time.ToString() + " time");
                                // Checks if there is a new combo NOT WORKING
                                
                                if( 3 < getNextInt(ref reader) )
                                {
                                    beat.newCombo = true;
                                }
                                else
                                {
                                    beat.newCombo = false;
                                }
                                
                            //    getNextInt(ref reader);
                               
                                // Get the hitsound but dosn't do anything with it
                                jumpToNextValue(ref reader);
                          //      Console.WriteLine("Gaa");

                                char SliderType = (char)reader.Read();

                                // If PeppySlider
                                if (SliderType == 'P')
                                {

                                    int previousBeatTime = beat.time;
                                    beat.slider = true;

                                    reader.Read();
                                    beatList.Add(beat);

                                    // Add slider middle point
                                    Beat middleSliderBeat = new Beat();
                                    middleSliderBeat.x = (short)(getNextInt(ref reader) + xOffset);
                                    middleSliderBeat.y = (short)(getNextInt(ref reader) + yOffset);

                                    
                                   // Add slider end beat
                                   Beat sliderBeat = new Beat();

                                   sliderBeat.x = (short)(getNextInt(ref reader) + xOffset);

                                   sliderBeat.y = (short)(getNextInt(ref reader) + yOffset);
                                   int repeatTimes = getNextInt(ref reader)-1;

                                   int sliderTime = (int)((getNextInt(ref reader) * 4) / SliderMultiplier);

                                   
                                   middleSliderBeat.time = sliderTime / 2 + previousBeatTime;
                                   sliderBeat.time = sliderTime + previousBeatTime;
                                    
                                   beatList.Add(middleSliderBeat);
                                   beatList.Add(sliderBeat);

                                   
                                   // Add Slider repeats if there are any
                                   bool evenRepeat = false;

                                   for (int i = 0; i < repeatTimes; i++)
                                   {
                                       beat.newCombo = false;

                                       middleSliderBeat.time += sliderTime;
                                       beatList.Add(middleSliderBeat);
                                       if (evenRepeat)
                                       {
                                           sliderBeat.time += sliderTime * 2;
                                           beatList.Add(sliderBeat);
                                            
                                       }
                                       else
                                       {
                                           beat.time += sliderTime * 2;
                                           beatList.Add(beat);
                                     }

                                       evenRepeat = !evenRepeat;
                                   }
                                   


                                }
                                // If Beizer Slider
                                else if (SliderType == 'B')
                                {
                                    int previousBeatTime = beat.time;
                                    beat.slider = true;
                                    
                                    reader.Read();
                                    beatList.Add(beat);

                                    List<Beat> sliderPoints = new List<Beat>();

                                    // Add slider middle point
                                    while(reader.Peek() != ',')
                                    {
                                        Beat SliderBeat = new Beat();
                                        SliderBeat.x = (short)(getNextInt(ref reader) + xOffset);

                                        String tempString = "0";
                                        while ((char)reader.Peek() != ',' && (char)reader.Peek() != ':' && (char)reader.Peek() != '|')
                                        {
                                            tempString += (char)reader.Read();

                                        }
                           
                                        SliderBeat.y = (short)(short.Parse(tempString) + yOffset);
                                        SliderBeat.slider = true;
                                        sliderPoints.Add(SliderBeat);
                                        if(reader.Read() == ',')
                                        {
                               
                                            break;
                                        }
                                    }

                                    int repeatTimes = getNextInt(ref reader) - 1;

                                    int sliderTime = (int)((getNextInt(ref reader) * 4) / SliderMultiplier);
    

                                    for(int i = 0; i < sliderPoints.Count; i++)
                                    {
                                        Beat tempBeat = sliderPoints[i];
                                        tempBeat.time =  (int)(previousBeatTime + (sliderTime * ((float)(i+1) / sliderPoints.Count)));
                                        beatList.Add(tempBeat);
                                    }
                                    /*
                                    middleSliderBeat.time = sliderTime / 2 + previousBeatTime;
                                    sliderBeat.time = sliderTime + previousBeatTime;

                                    beatList.Add(middleSliderBeat);
                                    beatList.Add(sliderBeat);

                                    // Add Slider repeats if there are any
                                    bool evenRepeat = false;

                                    for (int i = 0; i < repeatTimes; i++)
                                    {
                                        beat.newCombo = false;

                                        middleSliderBeat.time += sliderTime;
                                        //        beatList.Add(middleSliderBeat);
                                        Console.WriteLine("repeat nr" + i.ToString());
                                        if (evenRepeat)
                                        {
                                            sliderBeat.time += sliderTime * 2;
                                            beatList.Add(middleSliderBeat);
                                            beatList.Add(sliderBeat);

                                        }
                                        else
                                        {
                                            beat.time += sliderTime * 2;
                                            beatList.Add(middleSliderBeat);
                                            beatList.Add(beat);
                                            Console.WriteLine("middleSliderBeat " + middleSliderBeat.time.ToString());
                                            Console.WriteLine("beat " + beat.time.ToString());
                                        }

                                        evenRepeat = !evenRepeat;
                                    }
                                
            //                        beat = new Beat();
                                     */
                                }
                                // if one point Linear Slider (Multiple not supported yet)
                                else if (SliderType == 'L')
                                {
                                    int previousBeatTime = beat.time;
                                    beat.slider = true;

                                    reader.Read();
                                    beatList.Add(beat);
            
                                    // Add slider end as a beat
                                    Beat sliderBeat = new Beat();

                                    sliderBeat.x = (short)(getNextInt(ref reader) + xOffset);
                                 
                                    sliderBeat.y = (short)(getNextInt(ref reader) + yOffset);
                                    int repeatTimes = getNextInt(ref reader)-1;

                                    int sliderTime = (int)((getNextInt(ref reader) * 4) / SliderMultiplier);
                                    sliderBeat.time = sliderTime + previousBeatTime;

                                    beatList.Add(sliderBeat);

                                    // Add Slider repeats if there are any
                                    bool evenRepeat = false;

                                    for (int i = 0; i < repeatTimes; i++ )
                                    {
                                        if (evenRepeat)
                                        {
                                            sliderBeat.time += sliderTime * 2;
                                            beatList.Add(sliderBeat);
                                        }
                                        else
                                        {
                                            beat.time += sliderTime * 2;
                                            beatList.Add(beat);
                                        }
                                        evenRepeat = !evenRepeat;
                                    }
                                    
                                }
                                else
                                {
                                    beat.slider = true;
                                    beatList.Add(beat);
                                }
                           
                                reader.ReadLine();
                     
                            }
                            progressBar.Value = 60;
                            Console.WriteLine("Found all beats");
                                
                            
                            openFileStream.Position = 0;

                            string finalOsuFile = reader.ReadToEnd();
                            reader.Close() ;
                            Console.WriteLine("Found all beats2");

                            // Prints out the storyboard data for the spotlight
                            short currentTimePoint = 0;
                            string sbText = "Storyboard Layer 3 (Foreground)" + Environment.NewLine
                                + "Sprite,Foreground,Centre," + imgTextBox.Text.ToString() + ",320,320";
                            for (int i = 1; i < beatList.Count; i++)
                            {
                                Console.WriteLine("Beat "+(i+1).ToString()+"/"+beatList.Count.ToString());
                                if (currentTimePoint+1 < timePointList.Count && timePointList[currentTimePoint + 1].Time < beatList[i].time)
                                {
                                    Console.WriteLine(timePointList[currentTimePoint + 1].Time.ToString()+":"+  beatList[i].time.ToString());
                                    currentTimePoint++;
                                    
                                }

                                if (!checkKiai.Checked || timePointList[currentTimePoint].Kiai)
                                {

                                    if (!beatList[i].newCombo || checkBeCo.Checked || checkSliderMov.Checked)
                                    {
                                        if (!checkSliderMov.Checked || !beatList[i].slider)
                                        {
                                            sbText += Environment.NewLine +
                                                " M,0," + beatList[i - 1].time.ToString() + "," + beatList[i].time.ToString()
                                                + "," + beatList[i - 1].x.ToString() + "," + beatList[i - 1].y.ToString()
                                                + "," + beatList[i].x.ToString() + "," + beatList[i].y.ToString();
                                        }
                                        else
                                        {
                                            sbText += Environment.NewLine +
                                                " M,0," + beatList[i - 1].time.ToString() + "," + beatList[i].time.ToString()
                                                + "," + beatList[i - 1].x.ToString() + "," + beatList[i - 1].y.ToString();
                                        }
                                    }
                                    else
                                    {
                                        sbText += Environment.NewLine + "Sprite,Foreground,Centre," + imgTextBox.Text.ToString() + ",320,320";
                                    }
                                }
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
                            Cursor.Current = Cursors.Default;
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
        private void jumpToNextValue(ref StreamReader reader)
        {
            while ((char)reader.Peek() != ',' && (char)reader.Peek() != ':' && (char)reader.Peek() != '|')
            {
                reader.Read();
            }

            reader.Read();
        }
        private void jumpToNextValue(ref StreamReader reader, byte valuesToJump)
        {
            byte valueRead = 0;
            while (valuesToJump > valueRead)
            {
                if ((char)reader.Peek() == ',' || (char)reader.Peek() == ':' || (char)reader.Peek() == '|')
                {
                    valueRead++;
                }
           //     Console.Write(
          //      (char)
                reader.Read();
             //   );
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
            float returnFloat;
            if(int.TryParse(tempString, out returnInt))
            {
                return returnInt;
            }
            else 
            {
      //          Console.WriteLine("Failed to parse into int");
                returnFloat = floatParser(tempString);
                if (returnFloat != 0)
                {
                    return (int)Math.Round( returnFloat);

                }
                else
                {
                    return 0;
                }
            }
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
            if (openFileDialog1.FileName.Contains(mapFolder))
            {
                imgTextBox.Text = openFileDialog1.FileName.Replace(mapFolder, "");
            }
            else
            {
                MessageBox.Show("Image is not in map folder! \nPlease add it to the folder and select it there.");
            }

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

        private void checkKiai_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
    public struct TimingPoint
    {
        public int Time;
        public bool Kiai;
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
