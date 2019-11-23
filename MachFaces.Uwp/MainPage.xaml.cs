using ProjectClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MachFaces.Uwp
{
   
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();


            using (Windows.Storage.Streams.IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {

                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                SelectedImage.Source = bitmapImage;
            }
            

            var compararfaces =
                new CompareFace("accessKey", "secretkey");            
            

            try
            {
                IsLoading.Visibility = Visibility.Visible;
                ResultTb.Text = "";
                var result = await compararfaces.MachFaces(file.Path, file.Name, NomePessoa.Text);

                if (result)
                {
                    ResultTb.Text = "Esta pessoa está cadastrada!";
                }
                else
                {
                    ResultTb.Text = "Esta pessoa não está cadastrada";
                }
            }
            finally
            {
                IsLoading.Visibility = Visibility.Collapsed;
            }
        }
    }
}
