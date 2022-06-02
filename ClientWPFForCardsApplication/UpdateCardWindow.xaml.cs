using ClientWPFForCardsApplication.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientWPFForCardsApplication
{
    /// <summary>
    /// Логика взаимодействия для UpdateCardWindow.xaml
    /// </summary>
    public partial class UpdateCardWindow : Window
    {
        private static HttpClient client = new HttpClient();

        private MainWindow mainWindow;

        private Card card = new Card();
        public UpdateCardWindow()
        {
            InitializeComponent();
        }

        public UpdateCardWindow(Card card)
        {
            InitializeComponent();
            this.card = card;
        }

        private async void UpdateCardWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var fullFilePath = $@"{API.ImagesAPI + card.ImageName}";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            CurrentImage.Source = bitmap;

            CurrentImage.Width = 150;
            CurrentImage.Height = 150;

            this.TitleTextBox.Text = card.Title;

        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                SelectedImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {

            UpdateCardRequest card = new UpdateCardRequest() { Id = this.card.Id, Title = this.TitleTextBox.Text };
            if (SelectedImage.Source != null)
            {
                var arrayForNewImage = getJPGFromImageControl(SelectedImage.Source as BitmapImage);
                card.NewImage = Convert.ToBase64String(arrayForNewImage);
            }
            else
            {
                var arrayForNewImage = getJPGFromImageControl(CurrentImage.Source as BitmapImage);
                card.NewImage = Convert.ToBase64String(arrayForNewImage);
            }
            card.CurrentImage = this.card.ImageName;

            var myContent = JsonConvert.SerializeObject(card);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PutAsync(API.CardsAPI, byteContent).Result;
            
            this.Close();
        }
    }
}
