using System;
using System.Collections.Generic;
using ClashEngine.NET.ScreensManager;

namespace Kingdoms_Clash.NET
{
	class GameScreen
		: Screen
	{
		/// <summary>
		/// Poziom gry - czym mniej tym łatwiej(statki będą pojawiać się wolniej).
		/// </summary>
		const double Difficult = 0.1;

		/// <summary>
		/// Czas pomiędzy pojawianiem się nowych przeciwników.
		/// </summary>
		double TimeBetweenNewEnemies = 2.0;
		double ElapsedTime = 2.0;
		List<Enemy> Enemies = new List<Enemy>();
		List<Bullet> Bullets = new List<Bullet>();
		Player Player;

		public GameScreen()
		{
			this.IsFullscreen = true;
			this.Entities.AddEntity(this.Player = new Player());
		}

		public override void Update(double delta)
		{
			base.Update(delta);

			if (this.CheckCollisions())
			{
				this.Manager.AddScreen(new GameOverScreen()); //Wyświetlamy ekran "game over".
			}

			this.CheckEnemyShips(delta);
			this.CheckBullets();
		}

		/// <summary>
		/// Obsługa strzelania. Space - strzał.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public override bool KeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			if (e.Key == OpenTK.Input.Key.Space)
			{
				Bullet bullet = new Bullet(this.Player.Position.Value.X + Player.Size.X / 2 - Bullet.Size.X / 2);
				this.Bullets.Add(bullet);
				this.Entities.AddEntity(bullet);
				return true;
			}
			return false;
		}

		void CheckEnemyShips(double delta)
		{
			for (int i = 0; i < this.Enemies.Count; i++)
			{
				Enemy enemy = this.Enemies[i];
				if (enemy.Position.Value.Y > 600.0f) //Jeśli statek wyszedł poza ekran - usuwamy
				{
					this.Entities.RemoveEntity(enemy);
					this.Enemies.RemoveAt(i);
					--i;
				}
			}

			this.ElapsedTime += delta;
			if (this.ElapsedTime >= this.TimeBetweenNewEnemies)
			{
				Enemy newEnemy = new Enemy();
				this.Entities.AddEntity(newEnemy);
				this.Enemies.Add(newEnemy);
				this.ElapsedTime -= this.TimeBetweenNewEnemies;
				if (this.TimeBetweenNewEnemies - Difficult > 0.0)
				{
					this.TimeBetweenNewEnemies -= Difficult;
				}
			}
		}

		void CheckBullets()
		{
			for (int i = 0; i < this.Bullets.Count; i++)
			{
				Bullet bullet = this.Bullets[i];
				if (bullet.Position.Value.Y < -Bullet.Size.Y) //Wyszedł poza ekran - usuwamy
				{
					this.Entities.RemoveEntity(bullet);
					this.Bullets.RemoveAt(i);
					--i;
				}
			}

			//Sprawdzamy każdy-z-każdym, czy pocisk zderzył się z przeciwnikiem
			for (int i = 0; i < this.Enemies.Count; i++)
			{
				Enemy enemy = this.Enemies[i];
				for (int j = 0; j < this.Bullets.Count; j++)
				{
					Bullet bullet = this.Bullets[j];

					if (RectangleCollision(bullet.Position.Value.X, bullet.Position.Value.X + Bullet.Size.X, bullet.Position.Value.Y, bullet.Position.Value.Y + Bullet.Size.Y,
						enemy.Position.Value.X, enemy.Position.Value.X + Enemy.Size.X, enemy.Position.Value.Y, enemy.Position.Value.Y + Enemy.Size.Y))
					{
						//Kolizja!
						this.Entities.RemoveEntity(enemy);
						this.Entities.RemoveEntity(bullet);
						this.Enemies.RemoveAt(i);
						this.Bullets.RemoveAt(j);
						--i;
						--j;
					}
				}
			}
		}

		bool CheckCollisions()
		{
			foreach (var enemy in this.Enemies)
			{
				if (enemy.Position.Value.Y + Enemy.Size.Y >= this.Player.Position.Value.Y) //Pseudo-kolizja prostokąt-prostokąt
				{
					//Dla ułatwienia sprawdzam tylko czy enemy.Y >= player.Y i czy któryś z dolnych wierzchołków przeciwnika jest pomiędzy naszym statkiem
					return (enemy.Position.Value.X >= this.Player.Position.Value.X && enemy.Position.Value.X <= this.Player.Position.Value.X + Player.Size.X)
						|| (enemy.Position.Value.X + Enemy.Size.X >= this.Player.Position.Value.X && enemy.Position.Value.X + Enemy.Size.X <= this.Player.Position.Value.X + Player.Size.X);
				}
			}
			return false;
		}

		private static bool RectangleCollision(float l1, float r1, float t1, float b1,
			float l2, float r2, float t2, float b2)
		{
			if (b1 < t2 || t1 > b2 || r1 < l2 || l1 > r2)
				return false;
			return true;
		}
	}
}
