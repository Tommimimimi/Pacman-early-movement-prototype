using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace PacmanMovementPrototype2
{
    public partial class Form1 : Form
    {
        int[,] maze = new int[20, 20] {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,1,1,1,0,0,1,1,1,1,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,1,0,1,0,1,1,1,1,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,0,1,1,1,1,0,1,1,1,0,1,1,1,1,1,1,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1} 
        };
            



        enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        Direction currentDirection = Direction.None;
        int playerSpeed = 40;
        int Speed = 5;
        int Score = 0;
        int Lives = 1;

        Label GameText = new Label();
        PictureBox player = new PictureBox();
        PictureBox Enemy = new PictureBox();
        PictureBox EndScreen = new PictureBox();

        public Form1()
        {
            InitializeComponent();

            this.Width = 1000;
            this.Height = 1000;

            player.Width = playerSpeed;
            player.Height = playerSpeed;
            player.BackColor = Color.Yellow;
            player.Left = 40;
            player.Top = 40;
            this.Controls.Add(player);

            Enemy.Width = playerSpeed;
            Enemy.Height = playerSpeed;
            Enemy.BackColor = Color.Red;
            Enemy.Left = 300;
            Enemy.Top = 300;
            this.Controls.Add(Enemy);

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

            EndScreen.Width = 800;
            EndScreen.Height = 600;
            EndScreen.BackColor = Color.Black;
            EndScreen.Visible = false;
            this.Controls.Add(EndScreen);

            timer1.Start();
            this.KeyDown += Form1_KeyDown;
            this.KeyPreview = true;

            this.DoubleBuffered = true;
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

        private bool IsCollidingWithWall(int playerX, int playerY)
        {
            int gridX = playerX / playerSpeed;
            int gridY = playerY / playerSpeed;

            if (gridX < 0 || gridX >= maze.GetLength(0) || gridY < 0 || gridY >= maze.GetLength(1))
                return false;

            return maze[gridY, gridX] == 1;
        }

        private void MovePlayer()
        {
            int newX = player.Left;
            int newY = player.Top;

            switch (currentDirection)
            {
                case Direction.Up:
                    newY -= Speed;
                    break;
                case Direction.Down:
                    newY += Speed;
                    break;
                case Direction.Left:
                    newX -= Speed;
                    break;
                case Direction.Right:
                    newX += Speed;
                    break;
            }

            if (!IsCollidingWithWall(newX, newY))
            {
                player.Left = newX;
                player.Top = newY;
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
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int cellSize = playerSpeed;


            for (int row = 0; row < maze.GetLength(0); row++) 
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    if (maze[row, col] == 1)
                    {
                        g.FillRectangle(Brushes.Blue, col * cellSize, row * cellSize, cellSize, cellSize);
                    }
                    else if (maze[row, col] == 0)
                    {
                        g.FillRectangle(Brushes.Black, col * cellSize, row * cellSize, cellSize, cellSize);
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MovePlayer();
            MoveEnemyTowardsPlayer();
        }
    }
}
