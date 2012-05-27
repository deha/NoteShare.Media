using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace Media.Common.Writers
{
    public class WavSink : AudioSink
    {
        private MemoryStream _stream;
        private AudioFormat _format;

        public Stream BackingStream
        {
            get { return _stream; }
        }

        public AudioFormat CurrentFormat
        {
            get { return _format; }
        }



        protected override void OnCaptureStarted()
        {
            _stream = new MemoryStream(1024);
        }

        protected override void OnCaptureStopped()
        {

        }

        protected override void OnFormatChange(AudioFormat audioFormat)
        {
            if (audioFormat.WaveFormat != WaveFormatType.Pcm) throw new InvalidOperationException("WavAudioSink supports only PCM audio format.");
            _format = audioFormat;
        }

        protected override void OnSamples(long sampleTimeInHundredNanoseconds, long sampleDurationInHundredNanoseconds, byte[] sampleData)
        {
            _stream.Write(sampleData, 0, sampleData.Length);
        }
    }
}
