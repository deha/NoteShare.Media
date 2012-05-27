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

namespace Media.Controls
{
    public partial class AudioPlayer : UserControl
    {
        public AudioPlayer()
        {
            m_downloader = new TransferManager();
            m_downloader.DownloadCompleted += new TransferManager.DownloadCompletedEventHandler(Play);
            InitializeComponent();
        }

        private void Play(Stream DownloadedStream)
        {
            MediaStreamSource wavMss = new WaveMediaStreamSource(DownloadedStream);
            mediaElement.SetSource(wavMss);
            mediaElement.Play();
        }

        private void OpenStreamButton_Click(object sender, RoutedEventArgs e)
        {
            this.Source = new Uri("http://localhost/test.wav");
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                mediaElement.Pause();
            }
            else if (mediaElement.CurrentState == MediaElementState.Paused || mediaElement.CurrentState == MediaElementState.Stopped)
            {
                mediaElement.Play();
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

        private Uri m_source;
        public TransferManager m_downloader {get; private set;}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = new TimeSpan(0, 0, 2);
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider1.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }
    }
}
