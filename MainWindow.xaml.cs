using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Memory
{
    
    public partial class MainWindow : Window
    {
        float hidden = 0.0f;

        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();
            plansza.Background = Brushes.LightGray;

        }

        private void SetUpGame()
        {
            List<string> pola = new List<string>()
            {
                "/images/czaszka.jpg", "/images/czaszka.jpg",
                "/images/moneta.jpg", "/images/moneta.jpg",
                "/images/zombie.jpg", "/images/zombie.jpg",
                "/images/kamien.png", "/images/kamien.png",

            };
            Random random = new Random();

            foreach (Image image in plansza.Children.OfType<Image>())
            {
                image.IsEnabled = true;
                image.Visibility = Visibility.Visible;
                image.Opacity = hidden;
                int index = random.Next(pola.Count);
                string nextEmoji = pola[index];
                image.Source = new BitmapImage(new Uri(@$"{nextEmoji}", UriKind.Relative));
                Pokaz(image);
                pola.RemoveAt(index);
            }
            matchesFound = 0;
        }

        Image lastImageClicked;
        bool findingMatch = false;


        public async void Pokaz(Image image)
        {
            image.Opacity = 1f;
            await Task.Delay(2500);
            image.Opacity = hidden;
        }

        public async void Znaleziono(Image image)
        {
            lastImageClicked.Opacity = 1f;
            image.Opacity = 1f;
            await Task.Delay(1000);
            image.Visibility = Visibility.Hidden;
            lastImageClicked.Visibility = Visibility.Hidden;
            
        }

        public async void NiePomylka(Image image)
        {
            lastImageClicked.Opacity = 1f;
            image.Opacity = 1f;
            await Task.Delay(1000);
            lastImageClicked.Opacity = hidden;
            image.Opacity = hidden;
        }


        private void Kliknij(object sender, MouseButtonEventArgs e)
        {
            //info.Content = "kliknales";
            Image image = (Image)sender;
            image.Opacity = 1f;
            if (findingMatch == false)
            {
                //info.Content = "nie znaleziono";
                lastImageClicked = image;
                image.IsEnabled = false;
                findingMatch = true;
            }
            else if (Convert.ToString(image.Source) == Convert.ToString(lastImageClicked.Source))
            {
                matchesFound++;
                //info.Content = "znaleziono";
                Znaleziono(image);
                findingMatch = false;
            }
            else
            {
                //info.Content = "nie dla psa kielbaasa";
                lastImageClicked.Opacity = 1f;
                image.Opacity = 1f;
                NiePomylka(image);
                lastImageClicked.IsEnabled = true;
                image.IsEnabled = true;
                findingMatch = false;
            }
            if (matchesFound == 4)
            {
                info.Content = " - Play again?";
            }
            else
            {
                info.Content = $"Matches found: {matchesFound}";
            }

            //info.Content = $"Presed: {image.Source}";
            //info.Content += "\n LAst: " + lastImageClicked.Source;
        }

        private void Wygrana(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 4)
            {
                SetUpGame();
                info.Content = "";
            }
        }
    }

}


