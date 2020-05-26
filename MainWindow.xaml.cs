using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MahAppsTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private volatile bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;

                    NotifyOfPropertyChange(nameof(IsBusy));
                    NotifyOfPropertyChange(nameof(IsNotBusy));
                }
            }
        }
        public bool IsNotBusy => !IsBusy;

        public ICommand TestCommand { get; } = new SimpleCommand(o => true);

        private SimpleCommand switchIsBusyCommand;
        public ICommand SwitchIsBusyCommand => switchIsBusyCommand ?? (switchIsBusyCommand = new SimpleCommand(o => true, async o =>
        {
            IsBusy = true;

            await Task.Delay(2000);

            IsBusy = false;
        }));

		public MainWindow()
		{
			InitializeComponent();

            this.DataContext = this;
		}

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyOfPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SimpleCommand : ICommand
    {
        public SimpleCommand(Func<object, bool> canExecute = null, Action<object> execute = null)
        {
            this.CanExecuteDelegate = canExecute;
            this.ExecuteDelegate = execute;
        }

        public Func<object, bool> CanExecuteDelegate { get; set; }

        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            var canExecute = this.CanExecuteDelegate;
            return canExecute == null || canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter)
        {
            this.ExecuteDelegate?.Invoke(parameter);
        }
    }
}
