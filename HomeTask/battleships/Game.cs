using System;
using System.Linq;
using NLog;

namespace battleships
{
	public class ShotInfo
	{
		public ShtEffct Hit;
		public Vector Target;
	}

    public class Game : IGame
    {
		private static readonly Logger log = LogManager.GetCurrentClassLogger();
		private readonly IAi ai;

		public Game(Map map, IAi ai)
		{
			Map = map;
            //��� ���������� �� ����� � ������������ ������, ���������� ��� � ����� �������� ��� ���� ai �� IAi
			this.ai = ai;
			TurnsCount = 0;
			BadShots = 0;
		}

		public Vector LastTarget { get; private set; }
		public int TurnsCount { get; private set; }
		//<summary>���������� ��������� "������" �����</summary>
		public int BadShots { get; private set; }
		public Map Map { get; private set; }
		public ShotInfo LastShotInfo { get; private set; }
		public bool AiCrashed { get; private set; }
		public Exception LastError { get; private set; }

		public bool IsOver()
		{
			return !Map.HasAliveShips() || AiCrashed;
		}

		public void MakeStep()
		{
			if (IsOver()) throw new InvalidOperationException("Game is Over");
			if (!UpdateLastTarget()) return;
			if (IsBadShot(LastTarget)) BadShots++;
			var hit = Map.Badaboom(LastTarget);
			LastShotInfo = new ShotInfo {Target = LastTarget, Hit = hit};
			if (hit == ShtEffct.Miss)
				TurnsCount++;
		}

		private bool UpdateLastTarget()
		{
			try
			{
				LastTarget = LastTarget == null
					? ai.Init(Map.Width, Map.Height, Map.Ships.Select(s => s.Size).ToArray())
					: ai.GetNextShot(LastShotInfo.Target, LastShotInfo.Hit);
				return true;
			}
			catch (Exception e)
			{
				AiCrashed = true;
				log.Info("Ai {0} crashed", ai.Name);
				log.Error(e);
				LastError = e;
				return false;
			}
		}

		private bool IsBadShot(Vector target)
		{
			var cellWasHitAlready = Map[target] != MapCell.Empty && Map[target] != MapCell.Ship;
			var cellIsNearDestroyedShip = Map.Near(target).Any(c => Map.shipsMap[c.X, c.Y] != null && !Map.shipsMap[c.X, c.Y].Alive);
			var diagonals = new[] { new Vector(-1, -1), new Vector(-1, 1), new Vector(1, -1), new Vector(1, 1) };
			var cellHaveWoundedDiagonalNeighbour = diagonals.Any(d => Map[target.Add(d)] == MapCell.DeadOrWoundedShip);
			return cellWasHitAlready || cellIsNearDestroyedShip || cellHaveWoundedDiagonalNeighbour;
		}
	}
}