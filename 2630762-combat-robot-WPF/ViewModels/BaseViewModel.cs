using _2630762_combat_robot_wpf;
using _2630762_combat_robot_wpf.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace _2630762_combat_robot_wpf.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Page pageAssociee;

        public BaseViewModel(Page page)
        {
            pageAssociee = page;
        }

        private string erreurMessage = "";
        public string ErreurMessage
        {
            get => erreurMessage;
            set
            {
                erreurMessage = value;
                OnPropertyChanged("ErreurMessage");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
