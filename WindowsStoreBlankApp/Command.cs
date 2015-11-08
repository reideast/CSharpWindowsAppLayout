using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml; //for DispatchTimer

namespace WindowsStoreBlankApp
{
    public class Command : ICommand
    {
        private Action methodToExecute = null;
        private Func<bool> methodToDetectCanExecute = null;
        private DispatcherTimer canExecuteChangedEventTimer = null;

        public Command(Action methodToExecute, Func<bool> methodToDetectCanExecute)
        {
            this.methodToExecute = methodToExecute;
            this.methodToDetectCanExecute = methodToDetectCanExecute;

            //start a timer to raise event CanExecuteChanged once every second, to fit interface ICommand
            this.canExecuteChangedEventTimer = new DispatcherTimer();
            this.canExecuteChangedEventTimer.Tick += canExecuteChangedEventTimer_Tick;
            this.canExecuteChangedEventTimer.Interval = new TimeSpan(0, 0, 1);
            this.canExecuteChangedEventTimer.Start();
        }

        public void Execute(object parameter)
        {
            this.methodToExecute();
        }

        public bool CanExecute(object parameter)
        {
            if (this.methodToDetectCanExecute == null)
                return true;
            else
                return this.methodToDetectCanExecute();
        }

        public event EventHandler CanExecuteChanged;

        void canExecuteChangedEventTimer_Tick(object sender, object e)
        {
            //to do this "manually" without the 1-second timer, see: http://stackoverflow.com/questions/12030697/what-replaces-commandmanager-in-winrt
            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
