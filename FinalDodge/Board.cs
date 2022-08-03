using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FinalDodge
{
    class Board
    {

        //array enemy , colisition? , win?, location x + y , score.


        public List<Enemy> Enemies;
        public Player player;
        public int LocationX, LocationY;
        public bool IsCrashed = false;
        public bool IsWin = false;
        public Canvas Game;
        public List<Attacks> attacks;
        public int Score;






        public Board(Canvas board, int score1)
        {
            Game= board;
            Enemies = new List<Enemy>();
            attacks= new List<Attacks>();
            Score = score1;

        }

        public void CreateTheGame(double x, double y, int level)
        {

            Random r = new Random();
            int count = 0;

            LocationX = (int)Game.ActualWidth/2;
            LocationY= (int)Game.ActualHeight/2;

            player = new Player(LocationX, LocationY);//creating player
            Canvas.SetLeft(player.Pic, LocationX);
            Canvas.SetTop(player.Pic, LocationY);
            Game.Children.Add(player.Pic);

            while (count<=10)//creating enemies
            {
                Enemy e;
                e = GetRandomEnemyLocation();
                e.MoveObject(e.X - e.Pic.Width, e.Y-e.Pic.Height);
                e.speed +=(level/10)*2;
                Enemies.Add(e);
                Game.Children.Add(e.Pic);
                count++;
            }

        }


        private Enemy GetRandomEnemyLocation()
        {
            Random rnd = new Random();
            Enemy enemy;
            do
            {
                enemy = new Enemy(
                    rnd.Next(0, (int)Game.ActualWidth),
                    rnd.Next(0, (int)Game.ActualHeight));
            } while (!CheckMinDistFromAll(enemy));
            return enemy;
        }


        private bool CheckMinDistFromAll(Pawns item)
        {
            if (!CheckMinDistFromSpecific(item, player, 300))
                return false;
            foreach (Enemy enemy in Enemies)
            {
                if (!CheckMinDistFromSpecific(item, enemy, 100))
                    return false;
            }
            return true;
        }

        private bool CheckMinDistFromSpecific(Pawns p1, Pawns p2, double min)
        {
            double x = p1.X - p2.X;
            double y = p1.Y - p2.Y;
            double dist = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return dist > min;
        }


        public bool Moving(int Dir)
        {
            foreach (Attacks e in attacks)
            {
                e.MoveArrow();
            }
            switch (Dir)
            {
                case Directions.Right:
                    {
                        MoveRight();
                        break;
                    }
                case Directions.Left:
                    {
                        MoveLeft();
                        break;
                    }
                case Directions.Up:
                    {
                        MoveUp();
                        break;
                    }
                case Directions.Down:
                    {
                        MoveDown();
                        break;
                    }
            }
            foreach (Enemy item in Enemies)
            {
                if (item.X>player.X)
                {
                    EnemyMoveLeft(item);
                }
                if (item.X < player.X)
                {
                    EnemyMoveRight(item);
                }
                if (item.Y > player.Y)
                {
                    EnemyMoveUp(item);
                }
                if (item.Y < player.Y)
                {
                    EnemyMoveDown(item);
                }
            }
            CheckIfCrashed();
            if (IsCrashed)
            {
                return false;
            }
            return true;
        }

        //______________________________________________________________________
        private void CheckIfCrashed()//פונקציה הבודקת האם 2 אויבים נפגשו אחד עם השני, אם כן שניהם נפסלים. בנוסף בודקת האם השחקן מתנגש עם אויב ונפסל
        {
            if (player.X + player.Pic.Width >= Game.ActualWidth)
            {
                MoveLeft();
            }
            if (player.X<= 0)
            {
                MoveRight();
            }
            if (player.Y + player.Pic.Height >= Game.ActualHeight)
            {
                MoveUp();
            }
            if (player.Y<= 0)
            {
                MoveDown();
            }
            foreach (Attacks item in attacks.ToList()) // attack gets out of bounds
            {
                if (item.X<=0)
                {
                    attacks.Remove(item);
                    Game.Children.Remove(item.Pic);
                }
                if (item.X + item.Pic.Width >= Game.ActualWidth)
                {
                    attacks.Remove(item);
                    Game.Children.Remove(item.Pic);
                }
                if (item.Y + item.Pic.Height >= Game.ActualHeight)
                {
                    attacks.Remove(item);
                    Game.Children.Remove(item.Pic);
                }
                if (item.Y <= 0)
                {
                    attacks.Remove(item);
                    Game.Children.Remove(item.Pic);
                }
            }
            foreach (Enemy item in Enemies)
            {
                if (CheckCollision(item, player))//if player hit enemy
                {
                    IsCrashed = true;//game over
                }
                foreach (Attacks attack in attacks)
                {
                    if (CheckCollision(item, attack))//if attack hit enemy
                    {
                        Game.Children.Remove(item.Pic);
                        Enemies.Remove(item);
                        attacks.Remove(attack);
                        Game.Children.Remove(attack.Pic);
                        Score += 10;
                        return;
                    }
                }
                foreach (Enemy item2 in Enemies)
                {
                    if (item != item2)//כפילות
                    {
                        if (CheckCollision(item, item2))//if enemy hit other enemy
                        {
                            Game.Children.Remove(item2.Pic);
                            Enemies.Remove(item2);
                            Score += 5;
                            return;
                        }
                    }

                }
            }
            if (Victory())//if won
                IsWin = true;
        }


        //---------------------------------------------------------------------------------

        private bool CheckCollision(Pawns m1, Pawns m2)
        {
            double x = m1.X - m2.X;
            double y = m1.Y - m2.Y;
            double dist = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return dist < ((((m1.Pic.Width + m2.Pic.Width)/2) + ((m1.Pic.Height + m2.Pic.Height)/2))/2);

        }
        private bool Victory()//check if the player win
        {
            int count = 0;
            foreach (Enemy item in Enemies)//count the alive enemys
                count++;
            if (count<=0)//win
                return true;
            return false;
        }

        private bool CheckIfAppear(double x, double y)
        {
            foreach (Enemy item in Enemies)
            {
                if (item!=null)
                {
                    if (item.X <=x + 40 && item.X >=x - 40 && item.Y <=y + 40 && item.Y >=y - 40)
                        return true;
                }
            }
            return false;
        }

        //----------------------------------------------------
        private void EnemyMoveLeft(Enemy e)
        {
            e.moveEnemy(e.X-e.speed, e.Y);
        }
        private void EnemyMoveRight(Enemy e)
        {
            e.moveEnemy(e.X + e.speed, e.Y);
        }
        private void EnemyMoveUp(Enemy e)
        {
            e.moveEnemy(e.X, e.Y- e.speed);
        }
        private void EnemyMoveDown(Enemy e)
        {
            e.moveEnemy(e.X, e.Y+ e.speed);
        }


        //______________________________________________________________________
        private void MoveLeft()
        {
            LocationX -= 2;
            player.movePlayer(LocationX, LocationY);
        }
        private void MoveRight()
        {
            LocationX += 2;
            player.movePlayer(LocationX, LocationY);
        }
        private void MoveUp()
        {
            LocationY -= 2;
            player.movePlayer(LocationX, LocationY);
        }
        private void MoveDown()
        {
            LocationY  += 2;
            player.movePlayer(LocationX, LocationY);
        }
    }
}
