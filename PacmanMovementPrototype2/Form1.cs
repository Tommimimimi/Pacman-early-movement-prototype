using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PacmanMovementPrototype2
{
    public partial class Form1 : Form
    {
        int[,] maze = new int[20, 20] {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1 },
            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };

        enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        Direction currentDirection = Direction.Right;
        Direction nextDirection = Direction.Right;
        int gridSize = 40;
        int Speed = 40;
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

            player.Width = gridSize;
            player.Height = gridSize;
            player.BackColor = Color.Yellow;
            player.Left = 40;
            player.Top = 40;
            this.Controls.Add(player);

            Enemy.Width = gridSize;
            Enemy.Height = gridSize;
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
                    nextDirection = Direction.Up;
                    break;
                case Keys.S:
                    nextDirection = Direction.Down;
                    break;
                case Keys.A:
                    nextDirection = Direction.Left;
                    break;
                case Keys.D:
                    nextDirection = Direction.Right;
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
            Enemy.Left = 400;
            Enemy.Top = 400;
            EndScreen.Visible = false;
            GameText.Visible = false;
            Enemy.Visible = true;
            player.Visible = true;
            currentDirection = Direction.None;
            nextDirection = Direction.None;
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
                player.Left = 40;
                player.Top = 40;
                Enemy.Left = 400;
                Enemy.Top = 400;
                currentDirection = Direction.None;
                nextDirection = Direction.None;
            }
        }

        private bool IsCollidingWithWall(int playerX, int playerY)
        {
            Rectangle rect = new Rectangle(playerX, playerY, player.Width, player.Height);
            Point[] corners = new Point[]
            {
                new Point(rect.Left, rect.Top),
                new Point(rect.Right - 1, rect.Top),
                new Point(rect.Left, rect.Bottom - 1),
                new Point(rect.Right - 1, rect.Bottom - 1)
            };

            foreach (var corner in corners)
            {
                int gridX = corner.X / gridSize;
                int gridY = corner.Y / gridSize;

                if (gridX < 0
                    || gridX >= maze.GetLength(1)
                    || gridY < 0
                    || gridY >= maze.GetLength(0))
                    return true;

                if (maze[gridY, gridX] == 1)
                    return true;
            }

            return false;
        }

        private bool CenterCheck(int hori, int vert)
        {
            return (hori % gridSize == 0) && (vert % gridSize == 0);
        }

        private void MovePlayer()
        {
            int centerX = player.Left + player.Width / 2;
            int centerY = player.Top + player.Height / 2;

            if (CenterCheck(player.Left, player.Top))
            {
                int tryX = player.Left;
                int tryY = player.Top;

                switch (nextDirection)
                {
                    case Direction.Up: tryY -= Speed; break;
                    case Direction.Down: tryY += Speed; break;
                    case Direction.Left: tryX -= Speed; break;
                    case Direction.Right: tryX += Speed; break;
                }

                if (!IsCollidingWithWall(tryX, tryY))
                {
                    currentDirection = nextDirection;
                }
            }

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
            int enemySpeed = 30; 

            
            if (Enemy.Left < player.Left)
            {
                int tryX = Enemy.Left + enemySpeed;
                if (!IsCollidingWithWall(tryX, Enemy.Top))
                    Enemy.Left = tryX;
            }
            else if (Enemy.Left > player.Left)
            {
                int tryX = Enemy.Left - enemySpeed;
                if (!IsCollidingWithWall(tryX, Enemy.Top))
                    Enemy.Left = tryX;
            }

            
            if (Enemy.Top < player.Top)
            {
                int tryY = Enemy.Top + enemySpeed;
                if (!IsCollidingWithWall(Enemy.Left, tryY))
                    Enemy.Top = tryY;
            }
            else if (Enemy.Top > player.Top)
            {
                int tryY = Enemy.Top - enemySpeed;
                if (!IsCollidingWithWall(Enemy.Left, tryY))
                    Enemy.Top = tryY;
            }
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            int cellSize = gridSize;

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
