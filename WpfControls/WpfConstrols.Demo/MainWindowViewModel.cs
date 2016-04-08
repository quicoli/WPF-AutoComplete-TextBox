using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using JulMar.Windows.Validations;
using WpfConstrols.Demo.Model;
using WpfControls.Editors;

namespace WpfConstrols.Demo
{
    public class MainWindowViewModel: ValidatingViewModel
    {
        private State _selectedState;
        private string _name;

        [Required]
        public State SelectedState
        {
            get { return _selectedState; }
            set { _selectedState = value; RaisePropertyChanged(()=> SelectedState); }
        }


        [Required]
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(()=>Name); }
        }
    }

   
}