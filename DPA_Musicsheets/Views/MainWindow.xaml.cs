using Microsoft.Win32;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DPA_Musicsheets.Managers;
using System.ComponentModel;
using PSAMWPFControlLibrary;
using PSAMControlLibrary;
using DPA_Musicsheets.Chain_of_responsibility;
using DPA_Musicsheets.New_models_and_patterns;
using DPA_Musicsheets.Builders_Parsers;
using DPA_Musicsheets.Models;
using System.Collections.Generic;
using DPA_Musicsheets.Commands;
using System.Windows.Input;
using System.Threading;

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
            if (!keysPressed.Contains(e.Key.ToString()))
            {
                keysPressed.Add(e.Key.ToString());
                ExecuteCommand();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (keysPressed.Contains(e.Key.ToString()))
            {
                keysPressed.Remove(e.Key.ToString());
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
                  if (keysPressed.Count > 0)
                  {
                      foreach (string key in keysPressed)
                      {
                          keysString += key + "+";
                      }

                      keysString = keysString.Remove(keysString.Length - 1);
                  }

                  controller.executeCommand(keysString);
              }, tokenSource.Token);
            }  
        }
    }
}
