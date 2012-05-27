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

namespace Media.Common
{
    public interface IRecorder
    {
        void StartRecord();
        void StopRecord();
        void PauseRecord();

        void WriteToStream(Stream Destination);
    }
}
