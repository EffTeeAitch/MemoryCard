using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memory
{
    public partial class MainWindow : Window
    {
        float hidden = 0.0f;
        int matchesFound;
        int howManyPairs;

        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();                //run the game for the first time to declare tiles with pictures
            board.Background = Brushes.LightGray;
        }

        private void SetUpGame()
        {
            List<string> fields = new List<string>()        //list containing (in correct quantity) possible photos 
            {
                "/images/czaszka.jpg", "/images/czaszka.jpg",
                "/images/moneta.jpg", "/images/moneta.jpg",
                "/images/zombie.jpg", "/images/zombie.jpg",
                "/images/kamien.png", "/images/kamien.png",

            };
            howManyPairs = fields.Count() / 2;
            Random random = new Random();               //variable used in process of randomization

            foreach (Image image in board.Children.OfType<Image>())         //preparing board for playing: set tiles with photos -> show tiles at the beginning -> hide tiles -> game begins
            {
                image.IsEnabled = true;
                image.Visibility = Visibility.Visible;
                image.Opacity = hidden;
                int index = random.Next(fields.Count);
                string nextEmoji = fields[index];
                image.Source = new BitmapImage(new Uri(@$"{nextEmoji}", UriKind.Relative));
                ShowTiles(image);
                fields.RemoveAt(index);
            }
            matchesFound = 0;
        }

        Image lastImageClicked;
        bool findingMatch = false;              //declares if comparison has started

        public async void ShowTiles(Image image)        //show tiles for the exact time
        {
            image.Opacity = 1f;
            await Task.Delay(2500);
            image.Opacity = hidden;
        }

        public async void Found(Image image)            //function that shows last and current photo and deactivate them for the rest of the game  
        {                                               //(appears only when both tiles have the same photo on themselves)
            lastImageClicked.Opacity = 1f;
            image.Opacity = 1f;
            await Task.Delay(1000);
            image.Visibility = Visibility.Hidden;
            lastImageClicked.Visibility = Visibility.Hidden;
        }

        public async void Failed(Image image)           //function that shows last and current photo but let them be for the next move
        {                                               //(appears only when both tiles have different photos on themselves)
            lastImageClicked.Opacity = 1f;
            image.Opacity = 1f;
            await Task.Delay(1000);
            lastImageClicked.Opacity = hidden;
            image.Opacity = hidden;
        }


        private void Press(object sender, MouseButtonEventArgs e)       //activates on every image after clicking it (image has to be visible)
        {
            Image image = (Image)sender;
            image.Opacity = 1f;
            if (findingMatch == false)                                  //if it's first move: declares current photo as the lastImageClicked
            {
                lastImageClicked = image;
                image.IsEnabled = false;
                findingMatch = true;
            }
            else if (Convert.ToString(image.Source) == Convert.ToString(lastImageClicked.Source))       //if current tile and last tile has the same image  
            {
                matchesFound++;
                Found(image);
                findingMatch = false;
            }
            else
            {
                lastImageClicked.Opacity = 1f;                      //if tiles has different images on themselves -> lets image still visible for pressing
                image.Opacity = 1f;
                Failed(image);
                lastImageClicked.IsEnabled = true;
                image.IsEnabled = true;
                findingMatch = false;
            }
            if (matchesFound == howManyPairs)                 //if player found every pair
            {
                info.Content = " - Play again?";
            }
            else                                   //for every move displays pairs found
            {
                info.Content = $"Matches found: {matchesFound}";
            }
        }

        private void Won(object sender, MouseButtonEventArgs e)             //creates option for player to continue playing endlessly after win
        {
            if (matchesFound == 4)
            {
                SetUpGame();
                info.Content = "";
            }
        }
    }
}


