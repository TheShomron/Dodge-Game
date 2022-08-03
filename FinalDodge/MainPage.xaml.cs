using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FinalDodge
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int CurrentDirection; // diraction

        Board BoardGame;
        Image player = new Image();
        Image enemy = new Image();
        Image attack = new Image();
        int lastDir = -1;
        int savedPlayer = 1;//player to save
        int savedEnemy = 1;//enemy to save
        

        private int countLevel = 1;
        private int highScore = 0;
        private int score = 0;

        private DispatcherTimer Tmr; // timer
        DispatcherTimer timer;
        int time = 0;

        ImageBrush ib = new ImageBrush();
        Image winningPic;




        public MainPage()
        {
            this.InitializeComponent();


            CreateGame();


        }

        public void CreateGame()
        {
            //default choice
            checkBox1.IsChecked =true;
            checkBox3.IsChecked =true;

            //commend bar
            imagePlayer1.Source=Player.CharmanderPic;
            imagePlayer2.Source = Player.SquirtlePic;
            imageEnemy2.Source = Enemy.GengarPic;
            imageEnemy1.Source = Enemy.SnorlaxPic;


            comboBoxBackground.Items.Add("Route 1");
            comboBoxBackground.Items.Add("Kanto Mansion");
            comboBoxBackground.SelectedIndex = 0;

        }

        public void updatePhotos()//update the photos of all the characters 
        {
            if (savedPlayer== 1)
            {
                BoardGame.player.Pic.Source = Player.CharmanderPic;
            }
            else
            {
                BoardGame.player.Pic.Source = Player.SquirtlePic;
            }

            if (savedEnemy==1)
            {
                foreach (Enemy item in BoardGame.Enemies)
                    item.Pic.Source = Enemy.SnorlaxPic;

            }
            else
            {
                foreach (Enemy item in BoardGame.Enemies)
                    item.Pic.Source = Enemy.GengarPic;
            }
            

        }
        private void focusCanvas()//unfocus on the left canvas elements
        {

            btnStop.IsTabStop = false;
            btnNewGame.IsTabStop = false;

            comboBoxBackground.IsTabStop = false;

            checkBox1.IsTabStop = false;
            checkBox2.IsTabStop = false;
            checkBox3.IsTabStop = false;
            checkBox4.IsTabStop = false;


        }
        public void InitializeGame()
        {
            focusCanvas();
            openScreen.Children.Clear();//clean the board game
            CurrentDirection = -1;//Set direction
            BoardGame = new Board(openScreen, score);//open a new board game
            BoardGame.CreateTheGame(0, 0, countLevel);
            Tmr.Start();//start the timer

        }
        private void timer_Tick(object sender, object e)
        {
            updatePhotos();//update the photos
            focusCanvas();



            if (BoardGame.Moving(CurrentDirection))//moving all pawns
            {
                if (BoardGame.IsWin)//passed level
                    NextLevel();
            }
            else
                EndGame();//game over
            txtBlockHightScore.Text = $"{highScore}";
            txtBlockScore.Text = $"{BoardGame.Score}";
            if (BoardGame.Score > highScore)
                highScore = BoardGame.Score;
        }

        private async void NextLevel()//passed level
        {
            score = BoardGame.Score;

            //gif when u passed
            winningPic = new Image();
            winningPic.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/pika.gif")); 
            Canvas.SetLeft(winningPic, openScreen.ActualWidth / 2);
            Canvas.SetTop(winningPic, 0);
            openScreen.Children.Add(winningPic);

            //timer
            time = 0;
            Tmr.Stop();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_ticktick;
            timer.Start();

            //popup
            MessageDialog md;
            md = new MessageDialog($"Ready for Level {++countLevel} ?", $" Your Scored: {BoardGame.Score} points.");
            UICommand yesCommand = new UICommand("Lets Go!", CurrentLevel);
            UICommand noCommand = new UICommand("No , im done", CloseGameCommand);
            md.Commands.Add(yesCommand);
            md.Commands.Add(noCommand);
            await md.ShowAsync();
        }
        private void CloseGameCommand(IUICommand command)//exit from the app
        {
            Application.Current.Exit();
        }
        private void CurrentLevel(IUICommand command)//level up
        {
            InitializeGame();
            foreach (Enemy item in BoardGame.Enemies)//increase diffucltty
            {
                if (item != null)
                    item.speed += 0.7;
            }
        }
        private async void EndGame()//game over
        {
            Tmr.Stop();
            MessageDialog md;
            md = new MessageDialog("You want a new game?", "Game over");
            UICommand yesCommand = new UICommand("Yes", RestartGame);
            UICommand noCommand = new UICommand("No", CloseGameCommand);
            md.Commands.Add(yesCommand);
            md.Commands.Add(noCommand);
            await md.ShowAsync();
        }
        private void RestartGame(IUICommand command)//restart command
        {
            Restart();
            
        }

        void timer_ticktick(object sender, object e)
        {
            if (time<=4)
            {
                time++;
            }
            else
            {
                openScreen.Children.Remove(winningPic);
                timer.Stop();
            }

        }
        private void Restart()//do a restart to the game
        {
            countLevel = 1;
            score = 0;
            openScreen.Children.Clear();
            InitializeGame();

            if (savedPlayer==1)
            {
                checkBox1.IsChecked = true;
            }
            else
            {
                checkBox2.IsChecked = true;
            }

            if (savedEnemy==1)
            {
                checkBox3.IsChecked = true;
            }
            else
            {
                checkBox4.IsChecked = true;
            }
            

            comboBoxBackground.SelectedIndex = 0;

        }
        private void StopGame()//stop the game
        {
            Tmr.Stop();
            btnStop.Content = "Continue Game";

            battleMusic.Pause();
        }
        private void ContinueGame()//continue the game
        {
            Tmr.Start();
            btnStop.Content = "Stop Game";

            battleMusic.Play();
        }
        private void new_game(object sender, RoutedEventArgs e)// new Game button
        {
            Restart();
            btnStop.Content = "Stop Game";
            btnNewGame.IsTabStop = false;//disable the focus on this radio button
        }
        private void player_KeyDown(CoreWindow sender, KeyEventArgs e)//keyboard
        {
            int NewDirection = -1;
            switch (e.VirtualKey)
            {
                case VirtualKey.Left:
                    NewDirection = Direction.Left;
                    break;
                case VirtualKey.Up:
                    NewDirection = Direction.Up;
                    break;
                case VirtualKey.Right:
                    NewDirection = Direction.Right;
                    break;
                case VirtualKey.Down:
                    NewDirection = Direction.Down;
                    break;
                case VirtualKey.F:
                    if (Tmr.IsEnabled==true)//timer is runing
                    {
                        if (CurrentDirection == -1)//allow to shoot to last dir
                        {

                            Attacks a = new Attacks(BoardGame.LocationX, BoardGame.LocationY, lastDir, savedPlayer);
                            BoardGame.attacks.Add(a);
                            BoardGame.Game.Children.Add(a.Pic);
                            CurrentDirection = lastDir;
                            BoardGame.Score -= 1;
                        }
                        else
                        {
                            Attacks B = new Attacks(BoardGame.LocationX, BoardGame.LocationY, CurrentDirection, savedPlayer);
                            BoardGame.attacks.Add(B);
                            BoardGame.Game.Children.Add(B.Pic);
                            lastDir = CurrentDirection;
                            BoardGame.Score -= 1;
                        }
                    }
                    break;
                case VirtualKey.S:
                    for (int i = 0; i < 4; i++)
                    {
                        Attacks e2 = new Attacks(BoardGame.LocationX, BoardGame.LocationY, i, savedPlayer);
                        BoardGame.attacks.Add(e2);
                        BoardGame.Game.Children.Add(e2.Pic);
                    }
                    BoardGame.Score -= 20;
                    break;
                default:
                    break;
            }
            CurrentDirection = NewDirection;
        }


        private void pause_continue(object sender, RoutedEventArgs e) // pause and continue button
        {
            if (btnStop.Content.ToString().CompareTo("Stop Game") == 0)
                StopGame();
            else
                ContinueGame();
            btnStop.IsTabStop = false;//disable the focus on this radio button
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            Restart();
            btnStop.Content = "Stop Game";
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {

            openScreen.Background = ib;
            openScreen.Children.Clear();//open image

            CurrentDirection = -1; //Default direction

            Tmr = new DispatcherTimer();
            Tmr.Interval = new TimeSpan(0, 0, 0, 0, 1); //Setting the interval to 50 milliseconds
            Tmr.Tick += timer_Tick;//Registering to the Tick Event 

            CoreWindow.GetForCurrentThread().KeyDown += player_KeyDown; //Registering to the KeyDown event
            InitializeGame();

            btnStop.IsEnabled = true;//allow to press
            btnNewGame.IsEnabled=true;

            gameMusic.IsMuted = false;//music 
            gameMusic.Position = TimeSpan.FromSeconds(0);
            gameMusic.Pause();//change music

            battleMusic.IsMuted = false;
            battleMusic.Play();

            focusCanvas();


        }


        private void comboBoxBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxBackground.SelectedIndex == 0)
            {
                ib.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Assets/gym7.png"));
            }
            else if (comboBoxBackground.SelectedIndex == 1)
            {
                ib.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Assets/gym8.jpg"));
            }
            comboBoxBackground.IsTabStop = false;//disable the focus on this combo box
        }

        private void checkBox1_Checked_1(object sender, RoutedEventArgs e)
        {
            savedPlayer = 1;
            checkBox1.IsEnabled = false;
            if (checkBox2!=null)
            {
                checkBox2.IsChecked = false;
                checkBox2.IsEnabled = true;
            }
            player.Source = Player.CharmanderPic;
            checkBox1.IsTabStop = false;//disable the focus on this radio button
        }


        private void checkBox2_Checked_1(object sender, RoutedEventArgs e)
        {
            savedPlayer = 2;
            checkBox2.IsEnabled = false;
            if (checkBox1!=null)
            {
                checkBox1.IsChecked = false;
                checkBox1.IsEnabled = true;
            }
            player.Source = Player.SquirtlePic;
            checkBox2.IsTabStop = false;//disable the focus on this radio button

        }

        private void checkBox3_Checked_1(object sender, RoutedEventArgs e)
        {
            savedEnemy = 1;
            checkBox3.IsEnabled = false;
            if (checkBox4 != null)
            {
                checkBox4.IsChecked = false;
                checkBox4.IsEnabled = true;
            }
            enemy.Source = Enemy.SnorlaxPic;
            checkBox3.IsTabStop = false;//disable the focus on this radio button

        }

        private void checkBox4_Checked_1(object sender, RoutedEventArgs e)
        {
            savedEnemy = 2;
            checkBox4.IsEnabled = false;
            if (checkBox3 != null)
            {
                checkBox3.IsChecked = false;
                checkBox3.IsEnabled = true;
            }
            enemy.Source = Enemy.GengarPic;
            checkBox4.IsTabStop = false;//disable the focus on this radio button
        }


    }



}
