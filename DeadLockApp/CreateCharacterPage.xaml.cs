namespace DeadLockApp;
using DeadLockApp.ViewModels;
using Microsoft.Maui.Controls;

public partial class CreateCharacterPage : ContentPage
{
    private CreateCharacterViewModel ViewModel => BindingContext as CreateCharacterViewModel;

    public CreateCharacterPage()
    {
        InitializeComponent();
        BindingContext = new CreateCharacterViewModel();
    }

    // ���������� ������ �����������
    private async void OnSelectImageClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "�������� �����������",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            ViewModel.ImagePath = result.FullPath; // ������������� ���� � ViewModel
            SelectedImage.Source = ImageSource.FromFile(result.FullPath); // ���������� ��������� �����������
        }
    }
}
