using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Model
{
    public class GameField
    {
        private readonly FieldItem[,] field;
        public Point Player { get; private set; }
        private List<Point> boxes;
        public ReadOnlyCollection<Point> Boxes { get; }

        public GameField(FieldItem[,] field, Point player, List<Point> boxes)
        {
            this.field = field;
            Player = player;
            this.boxes = boxes;
            Boxes = new ReadOnlyCollection<Point>(boxes);
        }

        public bool IsInField(Point point)
        {
            return point.X >= 0 && point.Y >= 0 && point.X < field.GetLength(0) && point.Y < field.GetLength(1);
        }

        public void MovePlayer(Point to)
        {
            if (!IsInField(to))
                throw new IndexOutOfRangeException();
            this[Player] ^= FieldItem.Player;
            Player = to;
            this[Player] |= FieldItem.Player;
        }

        public void MoveBox(Point from,Point to)
        {
            if (!IsInField(to) || Boxes.Where(b => b.X == from.X && b.Y == from.Y).Count() == 0)
                throw new IndexOutOfRangeException();
            this[from] ^= FieldItem.Box;
            boxes.Remove(boxes.Where(b => b == from).First());
            boxes.Add(to);
            this[to] |= FieldItem.Box;
        }

        public (int, int) GetFieldShape()
        {
            return (field.GetLength(0), field.GetLength(1));
        }

        public FieldItem this[Point point]
        {
            get
            {
                return field[point.X, point.Y];
            }
            private set
            {
                field[point.X, point.Y] = value;
            }
        }

        public FieldItem this[int r, int c]
        {
            get
            {
                return field[r, c];
            }
            private set
            {
                field[r, c] = value;
            }
        }
    }
}
