using Microsoft.Win32;
using Sanford.Multimedia.Midi;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Commands;


namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        private MainController controller;
        private List<string> keysPressed;

        private Task inputWaitTask;
        private CancellationTokenSource tokenSource;

        public MainWindow()
        {
            InitializeComponent();

            controller = new MainController(fileNameText, LilyTextBox);
            keysPressed = new List<string>();
            tokenSource = new CancellationTokenSource();

            this.DataContext = controller;
        }


        private void OpenFile(object sender, RoutedEventArgs e)
        {
            controller.executeCommand(CKey.Open);
        }
        private void SaveContent(object sender, RoutedEventArgs e)
        {
            controller.executeCommand(CKey.Save);
        }

        private void PlayContent(object sender, RoutedEventArgs e)
        {
            controller.player.play();
        }
        private void PauseContent(object sender, RoutedEventArgs e)
        {
            controller.player.pause();
        }
        private void StopContent(object sender, RoutedEventArgs e)
        {
            controller.player.stop();
        }
        private void UndoContent(object sender, RoutedEventArgs e)
        {
            controller.lilypondEditor.undo();
        }
        private void RedoContent(object sender, RoutedEventArgs e)
        {
            controller.lilypondEditor.redo();
        }

        private void ChangedContent(object sender, RoutedEventArgs e)
        {
            if (controller != null) controller.lilypondEditor.stateChanged();
        }

        private void ExitWindow(object sender, CancelEventArgs e)
        {
            controller.player.destroy();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            var key = e.Key;
            if (key == Key.System)
                key = e.SystemKey;

            var keyname = Enum.GetName(typeof(Key), key);
            if (!keysPressed.Contains(keyname))
            {
                keysPressed.Add(keyname);
                ExecuteCommand();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            var key = e.Key;
            if (key == Key.System)
                key = e.SystemKey;

            var keyname = Enum.GetName(typeof(Key), key);
            if (keysPressed.Contains(keyname))
            {
                keysPressed.Remove(keyname);
            }
        }

        private void ExecuteCommand()
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();

            if (inputWaitTask == null || inputWaitTask.IsCompleted)
            {
                inputWaitTask = Task.Delay(200).ContinueWith(
              (task, obj) =>
              {
                  string keysString = string.Empty;
                  List<string> tempKeys = keysPressed;
                  if (tempKeys.Count > 0)
                  {
                      
                      foreach (string key in tempKeys)
                      {
                          keysString += key + "+";
                      }

                      keysString = keysString.Remove(keysString.Length - 1);

                      Dispatcher.Invoke(() =>
                      {
                          controller.executeCommand(keysString);
                      });
                  }

              }, tokenSource.Token);
            }  
        }
    }
}
