using DeadLockApp.ViewModels;
using DeadLockApp.Models;
using System;
using Microsoft.Maui.Controls;
namespace DeadLockApp;

public partial class Heroes : ContentPage
{
	public Heroes()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }

   
}