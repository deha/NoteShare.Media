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

namespace Media.Library
{
    public static class Generator
    {
        public static string GenerateFilename()
        {
            char[] FileName = new char[fileNameLength];
            for (int i = 0; i < fileNameLength; i++)
            {
                FileName[i] = Convert.ToChar(rand.Next(97, 122));
            }
            return new string(FileName);
        }

        private static Random rand = new Random();
        private static int fileNameLength = 20; 
    }
}
