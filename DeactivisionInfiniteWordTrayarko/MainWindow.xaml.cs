using CSCore.CoreAudioAPI;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DeactivisionInfiniteWordTrayarko
{
    public partial class MainWindow : Window
    {
        private Settings Settings { get; }
        private ProcessListener Listener { get; }

        private const string RunningMessage = "Deactivision is activated!";

        private bool StopMuting { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            Settings = new Settings();
            Listener = new ProcessListener();
            Listener.AddProcess("Call of Duty®: Modern Warfare®");
            Listener.AddProcess("ModernWarfare");


            Listener.CodLaunched += Listener_CodLaunched;
            Listener.CodExited += Listener_CodExited;
        }

        private void Listener_CodExited(object sender, Process process)
        {
            StatusLabel.Content = RunningMessage;
        }

        private void Listener_CodLaunched(object sender, Process process)
        {
            StatusLabel.Content = "Call of Duty is running.";

            MuteCodIntermittently(process);

            if (Settings.UnmuteOn == UnmuteSetting.Delayed)
            {
                DelayedUnmute();
            }
        }

        private async Task MuteCodIntermittently(Process process)
        {
            while (!StopMuting)
            {
                VolumeMixer.SetApplicationMute(process.Id, true);
                await Task.Delay(50);
            }
            VolumeMixer.SetApplicationMute(process.Id, false);
        }

        private async Task DelayedUnmute()
        {
            await Task.Delay(15000);
            StopMute();
        }

        private void StopMute()
        {
            StopMuting = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeUI();
            Start();
        }

        private void InitializeUI()
        {
            Settings.UnmuteOn = UnmuteSetting.Delayed;
            StatusLabel.Content = RunningMessage;
            SecondsComboBox.ItemsSource = Settings.SecondsAfterOptions;
            SecondsComboBox.SelectedIndex = 2;//30s
        }

        public async Task Start()
        {
            await Listener.ObserveForever();
        }

        //static void Main(string[] args)
        //{
        //    const string app = "Mozilla Firefox";

        //    foreach (string name in EnumerateApplications())
        //    {
        //        Console.WriteLine("name:" + name);
        //        if (name == app)
        //        {
        //            // display mute state & volume level (% of master)
        //            Console.WriteLine("Mute:" + GetApplicationMute(app));
        //            Console.WriteLine("Volume:" + GetApplicationVolume(app));

        //            // mute the application
        //            SetApplicationMute(app, true);

        //            // set the volume to half of master volume (50%)
        //            SetApplicationVolume(app, 50);
        //        }
        //    }
        //}
    }
}