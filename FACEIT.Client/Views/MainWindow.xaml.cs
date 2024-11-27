using System;
using System.ComponentModel;
using System.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FACEIT.Client.Messages;
using FACEIT.Client.ViewModels;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace FACEIT.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private readonly VideoCapture capture;
        private readonly CascadeClassifier cascadeClassifier;

        private readonly BackgroundWorker bkgWorker;
        private IMessenger _messenger;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
            _messenger = Ioc.Default.GetRequiredService<IMessenger>();

            _messenger.Register<OpenNewWindowMessage>(this, OpenNewWindowMessageReceived);
            _messenger.Register<PersonRecognizedMessage>(this, PersonRecognizedMessageReceived);
            capture = new VideoCapture();
            cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");

            bkgWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            bkgWorker.DoWork += Worker_DoWork;

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void PersonRecognizedMessageReceived(object recipient, PersonRecognizedMessage message)
        {
            if (message.Value == null)
            {
                this.DetectedPersonPanel.Background = System.Windows.Media.Brushes.Red;
            }
            else
            {
                if (message.Value.Person.Enabled)
                {
                    this.DetectedPersonPanel.Background = System.Windows.Media.Brushes.LightGreen;
                }
                else
                {
                    this.DetectedPersonPanel.Background = System.Windows.Media.Brushes.Yellow;
                }   
            }
        }

        private void OpenNewWindowMessageReceived(object recipient, OpenNewWindowMessage message)
        {
            switch (message.Value)
            {
                case nameof(GroupsManagementWindow):
                    var groupWindow = new GroupsManagementWindow();
                    groupWindow.Show();
                    break;
                case nameof(PersonsManagementWindow):
                    var personWindow = new PersonsManagementWindow();
                    personWindow.Show();
                    break;
            }
        }

        internal MainViewModel ViewModel => (MainViewModel)DataContext;

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            capture.Open(0, VideoCaptureAPIs.ANY);
            if (!capture.IsOpened())
            {
                Close();
                return;
            }

            bkgWorker.RunWorkerAsync();
            _messenger.Send(new CameraReadyMessage(true));
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            bkgWorker.CancelAsync();

            capture.Dispose();
            cascadeClassifier.Dispose();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                using (var frameMat = capture.RetrieveMat())
                {
                    //var rects = cascadeClassifier.DetectMultiScale(frameMat, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));

                    //foreach (var rect in rects)
                    //{
                    //    Cv2.Rectangle(frameMat, rect, Scalar.Red);
                    //}

                    // Must create and use WriteableBitmap in the same thread(UI Thread).
                    Dispatcher.Invoke(() =>
                    {
                        var message = new FrameCapturedMessage(frameMat.ToWriteableBitmap());
                        _messenger.Send(message);
                    });
                }

                Thread.Sleep(30);
            }
        }
    }
}