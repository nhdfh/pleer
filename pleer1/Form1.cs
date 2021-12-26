using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Windows.Storage.FileProperties;

namespace pleer1
{
    public partial class Form1 : Form
    {
        
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        
        string file = "G:/osu!/Songs/[DOT96 obj：皿魔人] SAMBISTA[皿]/audio.mp3";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(file);
            
            var closing = false;
            outputDevice.PlaybackStopped += (s, a) => { if (closing) { outputDevice.Dispose(); audioFile.Dispose(); } };
            outputDevice.Init(audioFile);
            trackBar1.Value =(int) (audioFile.Volume*100f);
            //var f = new Form();
            //var b = new Button() { Text = "Play" };
            //b.Click += (s, a) => outputDevice.Play();
            //var b2 = new Button() { Text = "Stop", Left = b.Right };
            //b2.Click += (s, a) => outputDevice.Stop();
            //var b3 = new Button { Text = "Rewind", Left = b2.Right };
            //b3.Click += (s, a) => audioFile.Position = 0;
            //var b4 = new Button { Text = "Open", Left = b3.Right };
            //b4.Click += new EventHandler(Open_file);
            //var t = new TrackBar() { Minimum = 0, Maximum = 100, Value = 100, Top = b.Bottom, TickFrequency = 10 };
            //t.Scroll += (s, a) => outputDevice.Volume = t.Value / 100f;
            //// Alternative: t.Scroll += (s, a) => audioFile.Volume = t.Value / 100f;
            //f.Controls.AddRange(new Control[] { b, b2, b3, b4, t });
            //f.FormClosing += (s, a) => { closing = true; outputDevice.Stop(); };
            //f.ShowDialog();
        }
        //private void Open_file(object sender, EventArgs e)
        //{

        //    var filePath = string.Empty;

        //    using (OpenFileDialog openFileDialog = new OpenFileDialog())
        //    {

        //        openFileDialog.RestoreDirectory = true;

        //        if (openFileDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            //Get the path of specified file
        //            filePath = openFileDialog.FileName;
        //            file = filePath;
        //            audioFile = new AudioFileReader(file);
        //            outputDevice = new WaveOutEvent();
        //            outputDevice.Init(audioFile);
        //        }
                

        //    }

        //}
        //private void OnButtonPlayClick(object sender, EventArgs args)
        //{
        //    if (outputDevice == null)
        //    {
        //        outputDevice = new WaveOutEvent();
        //        outputDevice.PlaybackStopped += OnPlaybackStopped;
        //    }
        //    if (audioFile == null)
        //    {
        //        audioFile = new AudioFileReader(@"D:\example.mp3");
        //        outputDevice.Init(audioFile);
        //    }
        //    outputDevice.Play();
        //}
        //private void OnButtonStopClick(object sender, EventArgs args)
        //{
        //    outputDevice?.Stop();
        //}
        //private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        //{
        //    outputDevice.Dispose();
        //    outputDevice = null;
        //    audioFile.Dispose();
        //    audioFile = null;
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                   
                    file = filePath;
                    audioFile = new AudioFileReader(file);
                    outputDevice.Stop();
                    outputDevice.Init(audioFile);
                    TagLib.File file_TAG = TagLib.File.Create(file);
                    if (file_TAG.Tag.Pictures.Length >= 1)
                    {
                        var bin = (byte[])(file_TAG.Tag.Pictures[0].Data.Data);
                        pictureBox1.Image = Image.FromStream(new MemoryStream(bin));
                       
                    }
                    this.Text = audioFile.FileName.Split('\\').Last();
                }


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

       
            outputDevice.Play();
            label1.Text = audioFile.CurrentTime.ToString();
            label2.Text = audioFile.TotalTime.Minutes.ToString() + ":" + audioFile.TotalTime.Seconds.ToString();
            progressBar1.Maximum = (int)audioFile.TotalTime.TotalSeconds;
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            outputDevice.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            audioFile.Position = 0;
            outputDevice.Stop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            outputDevice.Volume = trackBar1.Value / 100f;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = (int)audioFile.CurrentTime.TotalSeconds;
            label1.Text = audioFile.CurrentTime.Minutes.ToString() + ":" + audioFile.CurrentTime.Seconds.ToString();
        }
    }
}
