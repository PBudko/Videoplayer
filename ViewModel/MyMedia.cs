using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using HomeWork_C_sharp_45_wpf.Model;


namespace HomeWork_C_sharp_45_wpf.ViewModel
{
  public class MyMedia : INotifyPropertyChanged
    {
        
        public MediaElement MyProperty { get; set; }
        public ICommand PlaycommandForMedia { get; set; }
        public ICommand StopcommandForMedia { get; set; }
        public ICommand PausecommandForMedia { get; set; }
        public ICommand ChooseFiles { get; set; }
        public ICommand AddOneMediaFiles { get; set; }
        public ICommand MinusMediaFile { get; set; }
        public ICommand PreviewFile { get; set; }
        public ICommand NextFile { get; set; }
        
        public double volume;
        public string time;
        public double maximum;
        private double vol;
        private string source =@"source\volume.png";
        private int index;

        public int Index
        {
            get { return index; }
            set { index = value;
                OnPropertyChanged("index");
            }
        }



        public string Source
        {
            get { return source; }
            set
            {
                source = value;                
                OnPropertyChanged("source");
            }
        }




        Element currentElement;
        public Element CurrentElement
        {
            get
            {
                
                return currentElement;
            }
            set
            {
                currentElement = value;
                if(CurrentElement!=null)
                MyProperty.Source = new Uri(CurrentElement.PathFile);
                OnPropertyChanged("CurrentElemnt");
            }
        }


        ObservableCollection<Element> playList = new ObservableCollection<Element>();      
        public ObservableCollection<Element> OllElement
        {
            get { return playList = PlayList.Playlist; }
            set
            {
                playList = value;
                OnPropertyChanged("playList");
            }
        }

        public bool CanChooseFile(object parametr)
        {
            if(MyProperty != null)
                return true;
           
            return false;
        }

        public void ChooseFile(object parametr)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "Media Files (*.mpeg;*mp4;*mp3;*.png;*jpg)|*.mpeg;*mp4;*mp3;*.png;*jpg|All files (*.*)|*.*";
            open.ShowDialog();
            string[] files = open.FileNames;
            if (files != null)
            {
                
                foreach (var file in files)
                {
                    FileInfo f = new FileInfo(file);
                    OllElement.Add(new Element(f.Name, file));
                }                
                
                timer.Start();
            }

        }

        public double Vol
        {
            get { return vol; }
            set
            {
                vol = value;
                GetSetVol();
                OnPropertyChanged("Vol");
            }
        }


        public string Time
        {
            get { return time; }
            set { time = value;                                   
                OnPropertyChanged("Time");                
            }
        }

        public double Maximum
        {
            get { return maximum; }
            set
            {
                maximum = value;                
                OnPropertyChanged("Maximum");
            }
        }

        DispatcherTimer timer = new DispatcherTimer();

        public MyMedia()
        {

            string.Format(@"hh\:mm\:ss");

            
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            MyProperty = new MediaElement();
            MyProperty.LoadedBehavior = MediaState.Manual;
            MyProperty.UnloadedBehavior = MediaState.Manual;
            MyProperty.MediaOpened += MyProperty_MediaOpened;            
            PlaycommandForMedia = new CommandClass(Play, CanPlay);
            StopcommandForMedia = new CommandClass(Stop, CanStop);
            PausecommandForMedia = new CommandClass(Pause, CanPause);
            ChooseFiles = new CommandClass(ChooseFile, CanChooseFile);
            AddOneMediaFiles = new CommandClass(Add,CanAdd );
            MinusMediaFile = new CommandClass(Minus, CanMinus);
            PreviewFile = new CommandClass(Preview,CanPreview);
            NextFile = new CommandClass(Next,CanNextFile);

           time = "00:00:00";


        }

        private bool CanNextFile(object arg)
        {
            if (MyProperty != null)
                return true;
            return false;
        }

        private void Next(object obj)
        {
            if (OllElement.Count > 0)
            {
                int index = OllElement.IndexOf(CurrentElement);
                if (index < (OllElement.Count-1) && OllElement.Count>1&&index>=0)
                {
                    CurrentElement = OllElement[++index];
                    Index = index;
                    MyProperty.Stop();
                }
            }
        }

        private bool CanPreview(object arg)
        {
            if (MyProperty != null)
                return true;
            return false;
        }

        private void Preview(object obj)
        {
            if (OllElement.Count > 0)
            {
                int index = OllElement.IndexOf(CurrentElement);
                if (index > 0)
                {
                    CurrentElement = OllElement[--index];
                    Index = index;
                    MyProperty.Stop();
                }
            }
        }

        private bool CanMinus(object arg)
        {
            if(MyProperty!=null)
            {
                return true;
            }
            return false;
        }

        private void Minus(object obj)
        {
            
            if (OllElement.Count > 0)
            {
                int index = OllElement.IndexOf(CurrentElement);
                if (index > 0)
                {
                    
                    OllElement.RemoveAt(index);
                    MyProperty.Source = null;
                    MyProperty.Stop();
                    Time = "00:00:00";
                }
                if(OllElement.Count>1 && index == 0)
                {
                    
                    OllElement.RemoveAt(index);
                    MyProperty.Source = null;
                    MyProperty.Stop();
                    Time = "00:00:00";
                }
                else if(OllElement.Count == 1 && index == 0)
                {
                    OllElement.RemoveAt(index);
                    MyProperty.Source = null;
                    MyProperty.Stop();
                    Time = "00:00:00";
                }
                
            }
        }

        private void Add(object arg)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
            string file = open.FileName;
            if(file!= "")
            {
                FileInfo f = new FileInfo(file);                
                OllElement.Add(new Element(f.Name, file));
            }
            timer.Start();
        }

        private bool CanAdd(object parametr)
        {
            if (MyProperty != null)
                return true;
            return false;           
        }

        private void MyProperty_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
             Maximum = MyProperty.NaturalDuration.TimeSpan.TotalSeconds;            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((MyProperty.Source != null) && (MyProperty.NaturalDuration.HasTimeSpan))
            {
                Volume = MyProperty.Position.TotalSeconds;
                Time= TimeSpan.FromSeconds(Volume).ToString(@"hh\:mm\:ss");
                Vol = MyProperty.Volume;
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            if(propertyName!=null)
            {
                
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool CanPlay(object parametr)
        {
            if (MyProperty != null)
            {
                    return true;
            }
            return false;          
        }
        public void Play(object parametr)
        {            
                MyProperty.Play();
        }
        public bool CanStop(object parametr)
        {
            if (MyProperty!= null)
                return true;
            return false;
        }
        public void Stop(object parametr)
        {            
                MyProperty.Stop();
        }
        public bool CanPause(object parametr)
        {
            if (MyProperty!= null)
                return true;
            return false;
        }
        public void Pause(object parametr)
        {           
                MyProperty.Pause();
        }

        public double Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                Dragcompleted();                              
                OnPropertyChanged("Volume");
               
            }
        }

        public void Dragcompleted()
        {
            MyProperty.Position = TimeSpan.FromSeconds(Volume);
            
        }

        public void GetSetVol()
        {
            MyProperty.Volume = Vol;
            if (MyProperty.Volume == 0)
                Source = @"source\NoVolume.png";
            else
                Source = @"source\volume.png";
        }
    }
}
