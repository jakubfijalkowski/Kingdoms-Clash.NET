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
				//TODO zaimplementować obsługę kolizji.
				throw new NotImplementedException();
			}

			this.CheckEnemyShips(delta);
		}

		override Ke

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
				this.Enemies.Add(newEnemy);
				this.Entities.AddEntity(newEnemy);
				this.ElapsedTime -= this.TimeBetweenNewEnemies;
				if (this.TimeBetweenNewEnemies - Difficult > 0.0)
				{
					this.TimeBetweenNewEnemies -= Difficult;
				}
			}
		}

		bool CheckCollisions()
		{
			foreach (var enemy in this.Enemies)
			{
				if (enemy.Position.Value.Y >= this.Player.Position.Value.Y) //Pseudo-kolizja prostokąt-prostokąt
				{
					//Dla ułatwienia sprawdzam tylko czy enemy.Y >= player.Y i czy któryś z dolnych wierzchołków przeciwnika jest pomiędzy naszym statkiem
					return (enemy.Position.Value.X >= this.Player.Position.Value.X && enemy.Position.Value.X <= this.Player.Position.Value.X + Player.Size.X)
						|| (enemy.Position.Value.X + Enemy.Size.X >= this.Player.Position.Value.X && enemy.Position.Value.X + Enemy.Size.X <= this.Player.Position.Value.X + Player.Size.X);
				}
			}
			return false;
		}
	}
}
