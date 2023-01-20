using System.Text.Json.Serialization;

namespace Model
{
    public class Point
    {
        public int X { get; }
        public int Y { get; }

        [JsonConstructor]
        public Point(int x, int y)
        {
            X = x; Y = y;
        }

        public static Point operator +(Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y);
        }
        public static bool operator ==(Point left, Point right)
        {
            if(left is null && right is null) return true;
            if(left is null || right is null) return false; 
            if(left.X == right.X && left.Y == right.Y)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Point left, Point right)
        {
            if (left is null && right is null) return false;
            if (left is null || right is null) return true;
            if (left.X == right.X && left.Y == right.Y)
            {
                return false;
            }
            return true;
        }
    }
}
