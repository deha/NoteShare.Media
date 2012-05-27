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
using System.ComponentModel;

namespace Media.Controls
{
    public class TransferManager : DependencyObject, INotifyPropertyChanged
    {
        public TransferManager()
        {
            m_downloadClient = new WebClient();
            m_downloadClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(m_downloadClient_DownloadProgressChanged);
            m_downloadClient.UploadProgressChanged += new System.Net.UploadProgressChangedEventHandler(m_downloadClient_UploadProgressChanged);
            m_downloadClient.OpenReadCompleted += new OpenReadCompletedEventHandler(m_downloadClient_OpenReadCompleted);
            m_downloadClient.OpenWriteCompleted += new OpenWriteCompletedEventHandler(m_downloadClient_OpenWriteCompleted);
        }

        public void Get(Uri Uri)
        {
            m_downloadClient.OpenReadAsync(Uri);
        }

        public void Put(Uri Uri, byte[] Data)
        {
            m_downloadClient.OpenWriteAsync(Uri, null, Data);
        }

        public int Progress
        {
            get { return (int)this.GetValue(ProgressProperty); }
            private set { this.SetValue(ProgressProperty, value); }
        }

        private WebClient m_downloadClient;

        #region ProgressDependencyProperty

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(int), typeof(TransferManager), new PropertyMetadata(0, new PropertyChangedCallback(OnProgressChange)));

        private static void OnProgressChange(DependencyObject @object, DependencyPropertyChangedEventArgs e)
        {
            TransferManager d = @object as TransferManager;
            if (d.PropertyChanged != null) d.PropertyChanged(d, new PropertyChangedEventArgs("Progress"));
        }

        #endregion

        #region EventHandlers

        void m_downloadClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (DownloadCompleted != null)
            {
                DownloadCompleted(e.Result);
            }
        }

        void m_downloadClient_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
        {
            Stream outputStream = e.Result;
            byte[] fileContent = e.UserState as byte[];
            outputStream.Write(fileContent, 0, fileContent.Length);
            outputStream.Close();
            if (UploadCompleted != null)
            {
                UploadCompleted();
            }
        }
        
        void m_downloadClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
            if (TransferProgressChanged != null)
            {
                TransferProgressChanged(e.ProgressPercentage);
            }
        }

        void m_downloadClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
            if (TransferProgressChanged != null)
            {
                TransferProgressChanged(e.ProgressPercentage);
            }
        }

        #endregion EventHandlers

        #region EventsnDelegates

        public delegate void TransferProgressChangedEventHandler(int Progress);
        public event TransferProgressChangedEventHandler TransferProgressChanged;

        public delegate void UploadCompletedEventHandler();
        public event UploadCompletedEventHandler UploadCompleted;

        public delegate void DownloadCompletedEventHandler(Stream DownloadedStream);
        public event DownloadCompletedEventHandler DownloadCompleted;

        #endregion EventsnDelegates

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
