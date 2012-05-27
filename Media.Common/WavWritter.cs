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
    public static class WavWritter
    {
        public static void WriteToWav(Stream RawData, Stream Output, AudioFormat AudioFormat)
        {
            if (AudioFormat.WaveFormat != WaveFormatType.Pcm) throw new ArgumentException("You can write only to WAV.");

            BinaryWriter bwOutput = new BinaryWriter(Output);

            //WAV header
            bwOutput.Write("RIFF".ToCharArray());
            bwOutput.Write((uint)(RawData.Length + 36));
            bwOutput.Write("WAVE".ToCharArray());
            bwOutput.Write("fmt ".ToCharArray());
            bwOutput.Write((uint)0x10);
            bwOutput.Write((ushort)0x01);
            bwOutput.Write((ushort)AudioFormat.Channels);
            bwOutput.Write((uint)AudioFormat.SamplesPerSecond);
            bwOutput.Write((uint)(AudioFormat.BitsPerSample * AudioFormat.SamplesPerSecond * AudioFormat.Channels / 8));
            bwOutput.Write((ushort)(AudioFormat.BitsPerSample * AudioFormat.Channels / 8));
            bwOutput.Write((ushort)AudioFormat.BitsPerSample);
            bwOutput.Write("data".ToCharArray());
            bwOutput.Write((uint)RawData.Length);


            long originalRawDataStreamPosition = RawData.Position;
            RawData.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[4096];
            int read;
            while ((read = RawData.Read(buffer, 0, 4096)) > 0)
            {
                bwOutput.Write(buffer, 0, read);
            }
            RawData.Seek(originalRawDataStreamPosition, SeekOrigin.Begin);
        }
    }
}
