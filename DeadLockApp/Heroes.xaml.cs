using DeadLockApp.Models;
using DeadLockApp.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace DeadLockApp
{
    public partial class Heroes : ContentPage
    {
        // ��������, ������� ������������� ������ � ViewModel
        private HeroesViewModel ViewModel => BindingContext as HeroesViewModel;

        // ����������� ��������, � ������� ���������������� ViewModel
        public Heroes()
        {
            InitializeComponent(); // �������������� ���������� ��������
            Shell.SetNavBarIsVisible(this, false);
            BindingContext = new HeroesViewModel(); // ����������� ViewModel � ��������
        }

        // ���������� ������ ��������� �� ���������
        private async void OnCharacterSelected(object sender, SelectionChangedEventArgs e)
        {
            // ���������, ������ �� ��������
            if (e.CurrentSelection.FirstOrDefault() is Character selectedCharacter)
            {
                // ������������� ���������� ��������� � ���������� ������ Data (��� ����������� �������������)
                Data.CurrentCharacter = selectedCharacter;

                // ����������� ��� ���������� �������� ������ ��� CollectionView
                // ��� ������ ��� ����, ����� ������ ����������, ���� ��������� ���������
                charactersCollectionView.ItemsSource = null;
                charactersCollectionView.ItemsSource = ViewModel.Characters;

                // ��������� �� �������� � ������ ���������� ���������
                // �������� ��������� ����� URL ��� ������������� ����������� ������ ������
                await Shell.Current.GoToAsync($"{nameof(BuildsPage)}?characterId={selectedCharacter.Id}&name={selectedCharacter.Name}&timestamp={DateTime.Now.Ticks}");
            }
        }

        // ���� ����� ����������, ����� �������� ���������� �� ������
        // ����� �� ���������� � ��������� �������� ������ ��� CollectionView, ����� �������������, ��� ������������ ���������� ������
        protected override void OnAppearing()
        {
            base.OnAppearing(); // �������� ����� �������� ������ ��� ���������� ������ ���������� ����� ��������

            // ���������, ���� CollectionView ����������, �� ���������� � ����� ����������� �������� ������
            if (charactersCollectionView != null)
            {
                charactersCollectionView.ItemsSource = null; // ������� �������� ������
                charactersCollectionView.ItemsSource = ViewModel.Characters; // ����������� ����������� ������ �� ViewModel
            }
        }

        // ���� ����� ����������, ����� �������� �������� � ������
        // �� ���������� ��������� � ������� �������� ������, ����� �������� ������ ������ ��� ������������� ���������
        protected override void OnDisappearing()
        {
            base.OnDisappearing(); // �������� ����� �������� ������

            // ���������, ���� CollectionView ����������, ���������� ��������� � ������� �������� ������
            if (charactersCollectionView != null)
            {
                charactersCollectionView.SelectedItem = null; // ���������� ��������� �������� ��������
                charactersCollectionView.ItemsSource = null; // ������� �������� ������
            }
        }
        private async void OnCreateCharacterButtonClicked(object sender, EventArgs e)
        {
            // ������� �� �������� �������� ���������
            await Shell.Current.GoToAsync("///createCharacterPage");
        }
    }
}
