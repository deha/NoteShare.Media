using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using WaveMSS;
using System.ComponentModel;

namespace Media.Controls
{
    public partial class AudioPlayer : UserControl, INotifyPropertyChanged
    {
        public AudioPlayer()
        {
            m_downloader = new TransferManager();
            m_downloader.DownloadCompleted += new TransferManager.DownloadCompletedEventHandler(Play);
            InitializeComponent();
        }

        private void Play(Stream DownloadedStream)
        {
            stream = new WaveMediaStreamSource(DownloadedStream);
            mediaElement.SetSource(stream);
            //mediaElement.Play();
            PauseIcon.Visibility = Visibility.Collapsed;
            PlayIcon.Visibility = Visibility.Visible;
        }

        private void OpenStreamButton_Click(object sender, RoutedEventArgs e)
        {
            this.Source = new Uri("http://localhost/Store/test.wav");
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                mediaElement.Pause();
                PauseIcon.Visibility = Visibility.Visible;
                PlayIcon.Visibility = Visibility.Collapsed;
            }
            else if (mediaElement.CurrentState == MediaElementState.Paused || mediaElement.CurrentState == MediaElementState.Stopped)
            {
                mediaElement.Play();
                PauseIcon.Visibility = Visibility.Collapsed;
                PlayIcon.Visibility = Visibility.Visible;
            }
        }

        private void DownloadStream(Uri Uri)
        {
            m_downloader.Get(Uri);
        }

        public Uri Source
        {
            get
            {
                return m_source;
            }
            set
            {
                m_source = value;
                DownloadStream(m_source);
            }
        }

        public TimeSpan Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Length"));
            }
        }
        private TimeSpan _length;


        private Uri m_source;
        public TransferManager m_downloader { get; private set; }
        private MediaStreamSource stream;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(1);
            mediaElement.Play();
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider1.Maximum = (int)mediaElement.NaturalDuration.TimeSpan.TotalSeconds * 1000;
            mediaElement.Play();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }
    }
}
