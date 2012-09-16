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
using Media.Common;
using Media.Common.Writers;

namespace Media.Controls
{
    public partial class AudioRecorder : UserControl, IRecorder
    {
        public AudioRecorder()
        {
            InitializeComponent();
            m_source = new CaptureSource();
            m_source.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();
        }

        public void StartRecord()
        {
            if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
            {
                m_sink = new WavSink();
                m_sink.CaptureSource = m_source;
                m_source.Start();
                recCircle.Visibility = Visibility.Visible;

                this.start_button.IsEnabled = false;
                this.stop_button.IsEnabled = true;
            }
            else
            {
                this.start_button.IsEnabled = true;
                this.stop_button.IsEnabled = false;
            }
        }

        public void StopRecord()
        {
            if (m_source.State == CaptureState.Started)
            {
                m_source.Stop();
                recCircle.Visibility = Visibility.Collapsed;
            }
        }

        public void PauseRecord()
        {
            throw new NotImplementedException();
        }

        public void WriteToStream(System.IO.Stream Destination)
        {
            WavWritter.WriteToWav(m_sink.BackingStream, Destination, m_sink.CurrentFormat);
        }

        private CaptureSource m_source;
        private WavSink m_sink;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.StartRecord();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.StopRecord();
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                this.WriteToStream(dialog.OpenFile());
            }

            this.start_button.IsEnabled = true;
            this.stop_button.IsEnabled = false;
        }
    }
}
