using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Media;
using System.Numerics;
using System.Security.Policy;
using System.Threading;

namespace PacmanMovementPrototype2
{

    public partial class Form1 : Form
    {
        enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        //declaring variables
        Direction currentDirection = Direction.None;
        int playerSpeed = 5;
        int Score = 0;
        int Lives = 1;

        //adding in resources
        Label GameText = new Label();
        PictureBox player = new PictureBox();
        PictureBox Enemy = new PictureBox();
        PictureBox EndScreen = new PictureBox();

        public Form1()
        {
            InitializeComponent();

            //application window settings
            this.Width = 800;
            this.Height = 600;

            //player settings
            player.Width = 40;
            player.Height = 40;
            player.BackColor = Color.Yellow;
            player.Left = 30;
            player.Top = 30;
            this.Controls.Add(player);

            //enemy settings
            Enemy.Width = 40;
            Enemy.Height = 40;
            Enemy.BackColor = Color.Red;
            Enemy.Left = 300;
            Enemy.Top = 300;
            this.Controls.Add(Enemy);

            //label settings
            GameText.Width = 300;
            GameText.Height = 100;
            GameText.Text = "Game Over";
            GameText.ForeColor = Color.Red;
            GameText.BackColor = Color.DarkRed;
            GameText.Font = new Font("Comic Sans MS", 30, FontStyle.Bold | FontStyle.Underline);
            GameText.TextAlign = ContentAlignment.MiddleCenter;
            GameText.Left = (this.ClientSize.Width / 2) - (GameText.Width / 2);
            GameText.Top = (this.ClientSize.Height / 2) - (GameText.Height / 2);
            GameText.Visible = false;
            this.Controls.Add(GameText);

            //end screen settings
            EndScreen.Width = 800;
            EndScreen.Height = 600;
            EndScreen.BackColor = Color.Black;
            EndScreen.Visible = false;
            this.Controls.Add(EndScreen);

            //timer settings
            timer1.Start();

            this.KeyDown += Form1_KeyDown;
            this.KeyPreview = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (EndScreen.Visible) return;

            switch (e.KeyCode)
            {
                case Keys.W:
                    currentDirection = Direction.Up;
                    break;
                case Keys.S:
                    currentDirection = Direction.Down;
                    break;
                case Keys.A:
                    currentDirection = Direction.Left;
                    break;
                case Keys.D:
                    currentDirection = Direction.Right;
                    break;
            }
        }

        private async void GameEndScreen()
        {
            EndScreen.Visible = true;
            GameText.Visible = true;
            EndScreen.BackColor = Color.DarkRed;
            player.Visible = false;
            Enemy.Visible = false;


            await Task.Delay(3000);
            ResetGame();
        }

        private void ResetGame()
        {
            Lives = 1;
            Score = 0;
            player.Left = 30;
            player.Top = 30;

            Enemy.Left = 300;
            Enemy.Top = 300;

            EndScreen.Visible = false;
            GameText.Visible = false;
            Enemy.Visible = true;
            player.Visible = true;
            player.BackColor = Color.Yellow;
            Enemy.BackColor = Color.Red;

            currentDirection = Direction.None;
        }

        private void Death()
        {
            Lives -= 1;
            if (Lives < 1)
            {
                GameEndScreen();
            }
            else
            {
                player.Left = 30;
                player.Top = 30;
                Enemy.Left = 300;
                Enemy.Top = 300;
                currentDirection = Direction.None;
            }
        }
        private void MoveEnemyTowardsPlayer()
        {
            int enemySpeed = 2;

            int horizontalDistance = Math.Abs(Enemy.Left - player.Left);
            int verticalDistance = Math.Abs(Enemy.Top - player.Top);

            if (horizontalDistance > verticalDistance)
            {
                if (Enemy.Left < player.Left)
                {
                    Enemy.Left += enemySpeed;
                }
                else if (Enemy.Left > player.Left)
                {
                    Enemy.Left -= enemySpeed;
                    
                }
            }
            else
            {
                if (Enemy.Top < player.Top)
                {
                    
                    Enemy.Top += enemySpeed;
                    
                }
                else if (Enemy.Top > player.Top)
                {
                    
                    Enemy.Top -= enemySpeed;
                    
                }
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            Rectangle playerRect = new Rectangle(player.Left, player.Top, player.Width, player.Height);
            Rectangle enemyRect = new Rectangle(Enemy.Left, Enemy.Top, Enemy.Width, Enemy.Height);

            MoveEnemyTowardsPlayer();

            bool hitsWall = false;

            if (playerRect.IntersectsWith(enemyRect))
            {
                Death();
            }

            if (player.Left < 0)
            {
                player.Left = 0;
                hitsWall = true;
            }

            if (player.Top < 0)
            {
                player.Top = 0;
                hitsWall = true;
            }

            if (player.Right > this.ClientSize.Width)
            {
                player.Left = this.ClientSize.Width - player.Width;
                hitsWall = true;
            }

            if (player.Bottom > this.ClientSize.Height)
            {
                player.Top = this.ClientSize.Height - player.Height;
                hitsWall = true;
            }

            if (hitsWall == false)
            {
                switch (currentDirection)
                {
                    case Direction.Left:
                        player.Left -= playerSpeed;
                        break;
                    case Direction.Right:
                        player.Left += playerSpeed;
                        break;
                    case Direction.Up:
                        player.Top -= playerSpeed;
                        break;
                    case Direction.Down:
                        player.Top += playerSpeed;
                        break;
                }
            }
            else
            {
                currentDirection = Direction.None;
                hitsWall = false;
            }
        }

    }
}

