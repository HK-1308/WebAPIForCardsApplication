using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientWPFForCardsApplication.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace ClientWPFForCardsApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string checkBoxName = "cardCheckBox";

        private const string cardButtonName = "cardButton";

        private static HttpClient client = new HttpClient();

        private static List<CheckBox> allCheckBoxes = new List<CheckBox>();

        private static List<Button> allCardsButtons = new List<Button>();

        private List<Card> cards = new List<Card>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.cardsStackPanel.Children.Clear();
            allCheckBoxes.Clear();
            var button = sender as Button;

            if(button!= null && button.Name == "Sort")
            {
                cards = await GetAllCardsAsync(API.CardsAPI);
                cards = cards.OrderBy(c => c.Title).ToList();
            }
            else cards = await GetAllCardsAsync(API.CardsAPI);

            if(cards.Count != 0)
            foreach (var card in cards)
                {
                    Sort.Visibility = Visibility.Visible;
                    Delete.Visibility = Visibility.Visible;
                    var image = new Image();
                    var fullFilePath = $@"{API.ImagesAPI + card.ImageName}";

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                    bitmap.EndInit();

                    image.Source = bitmap;
                    image.Width = 150;
                    image.Height = 150;

                    var cardStackPanel = new StackPanel() { Name = $"cardStackPanel{card.Id}", Orientation = Orientation.Vertical };
                    cardStackPanel.Children.Add(image);
                    var elementsGrid = new Grid();
                    elementsGrid.Children.Add(new Label() { Content = $"{card.Title}", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom });
                    var checkBox = new CheckBox() { Name = $"{checkBoxName + card.Id}", HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom };
                    allCheckBoxes.Add(checkBox);
                    elementsGrid.Children.Add(checkBox);
                    var cardButton = new Button() { Content = "Details", Name = $"{cardButtonName + card.Id}", HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom };
                    cardButton.Click += CardButton_Click;
                    allCardsButtons.Add(cardButton);
                    elementsGrid.Children.Add(cardButton);
                    cardStackPanel.Children.Add(elementsGrid);
                    this.cardsStackPanel.Children.Add(cardStackPanel);

                }
            else
            {
                Sort.Visibility = Visibility.Hidden;
                Delete.Visibility = Visibility.Hidden;
            }    

        }

        private async void CardButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string id = button.Name.Replace(cardButtonName, "");
            Card card = await GetCard(id);
            UpdateCardWindow cardWindow = new UpdateCardWindow(card);
            cardWindow.Closed += CardWindowClosed;
            cardWindow.ShowDialog();
        }

        private void CardWindowClosed (object sender, EventArgs e)
        {
            var eventArgs = new RoutedEventArgs();
            this.MainWindow_Loaded(sender, eventArgs);
        }

        private static async Task<Card> GetCard(string id)
        {
            Card card = null;
            HttpResponseMessage response = await client.GetAsync(API.CardsAPI + id);
            if (response.IsSuccessStatusCode)
            {
                card = await response.Content.ReadAsAsync<Card>();
            }
            return card;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        static async Task<List<Card>> GetAllCardsAsync(string path) 
        {
            List <Card> cardList = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                cardList = await response.Content.ReadAsAsync<List<Card>>();
            }
            return cardList;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {   
            foreach (var checkBox in allCheckBoxes)
            {
                if ((bool)checkBox.IsChecked)
                {
                    string id = checkBox.Name.Replace(checkBoxName, ""); 
                    var response = client.DeleteAsync(API.CardsAPI + id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("Success");
                    }
                    else
                        Console.Write("Error");

                }
            }
            this.MainWindow_Loaded(sender, e);
        }


        private void AddCardButton_Click(object sender, RoutedEventArgs e)
        {
            bool isAllDataPresent = SelectedImage.Source != null && !string.IsNullOrEmpty(TextBoxForTitle.Text);
            bool isTextInCorrectFormat = Regex.Match(this.TextBoxForTitle.Text, @"[a-zA-Z]").Success;
                
            if (isAllDataPresent && isTextInCorrectFormat)
            {
                int lastId = 0;
                if (cards.Count != 0)
                {
                    lastId = Convert.ToInt32(cards.Last().Id);
                    lastId++;
                }
                string newCardId = Convert.ToString(lastId);

                UploadNewCardRequest card = new UploadNewCardRequest() { Id = newCardId, Title = TextBoxForTitle.Text };
                var array = getJPGFromImageControl(SelectedImage.Source as BitmapImage);
                card.Image = Convert.ToBase64String(array);

                var myContent = JsonConvert.SerializeObject(card);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(API.CardsAPI, byteContent).Result;
                this.MainWindow_Loaded(sender, e);
            }
            else
            {
                string messageBoxText = "Please select image and enter correct title(onle latin)";
                string caption = "Word Processor";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
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

        private void ImageSelection_Click(object sender, RoutedEventArgs e)
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

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            this.MainWindow_Loaded(sender, e);
        }
    }
}
