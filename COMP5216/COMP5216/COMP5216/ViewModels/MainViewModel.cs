using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace COMP5216.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int clicks = 0;

        public int Clicks
        {
            get { return clicks; }
            // update the backing field and tell the world something's changed
            // note that because the ClickText property depends on this property, notify that it's changed too
            set { clicks = value; NotifyPropertyChanged(); NotifyPropertyChanged("ClickText"); }
        }

        // Read-only property used for the text on the button - it depends on the number of clicks that have already happened
        public string ClickText
        {
            get { return Clicks == 0 ? "Hello World - Click Me" : $"{Clicks} click{(Clicks == 1 ? "" : "s")} - Click me again!"; }
        }



        public void NotifyPropertyChanged([CallerMemberName]string varName = null)
        {
            // if someone's subscribed to this event, it won't be null, so call invoke
            // the new c# 6 ? operator lets us short-circuit a null check
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(varName));
        }

        // We never use this in the sample, but it's useful for later 
        public void NotifyAllPropertiesChanged()
        {
            foreach (var prop in GetType().GetRuntimeProperties())
            {
                NotifyPropertyChanged(prop.Name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        // Xamarin.Forms.Command implements ICommand for us, so we don't need to :)
        private Command clickCommand;

        // This is the command we bind the button to
        public Command ClickCommand
        // this syntax is a lazy-load singleton pattern. The first time we need to access the property,
        // instantiate the Command object and store it in the backing field. From then on, just return 
        // the contents of the backing field
        { get { clickCommand = clickCommand ?? new Command(doClick); return clickCommand; } }

        private void doClick()
        {
            Clicks++;
        }
    }
}

