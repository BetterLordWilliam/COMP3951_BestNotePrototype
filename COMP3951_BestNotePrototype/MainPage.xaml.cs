﻿using BestNote_3951.ViewModels;

namespace BestNote_3951
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }
}
