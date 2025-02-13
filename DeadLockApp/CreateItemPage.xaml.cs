namespace DeadLockApp;
using DeadLockApp.ViewModels;
using Microsoft.Maui.Controls;
public partial class CreateItemPage : ContentPage
{
    private CreateItemViewModel ViewModel => BindingContext as CreateItemViewModel;
    public CreateItemPage()
	{
		InitializeComponent();
        BindingContext = new CreateItemViewModel();
    }
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