using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Model
{
    public class FieldJsonSerializable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Point Player { get; set; }
        public Point[] Boxes { get; set; }
        public Point[] Destinations { get; set; }
        public Point[] Walls { get; set; }

        public FieldJsonSerializable() { }

        [JsonConstructor]
        public FieldJsonSerializable(int width, int height, Point player, Point[] boxes, Point[] destinations, Point[] walls)
        {
            Width = width;
            Height = height;
            Player = player;
            Boxes = boxes;
            Destinations = destinations;
            Walls = walls;
        }
    }

    public static class FieldJsonSerializer
    {
        public static string FieldJsonSerializableToJson(FieldJsonSerializable fieldJsonSerializable)
        {
            return JsonSerializer.Serialize(fieldJsonSerializable);
        }

        public static FieldJsonSerializable JsonToFieldJsonSerializable(string json)
        {
            return JsonSerializer.Deserialize<FieldJsonSerializable>(json);
        }

        public static FieldJsonSerializable FieldToJsonSerializable(GameField field)
        {
            FieldJsonSerializable json = new FieldJsonSerializable();
            List<Point> boxes = new List<Point>();
            List<Point> dest = new List<Point>();
            List<Point> walls = new List<Point>();

            (json.Width, json.Height) = field.GetFieldShape();
            for (int i = 0; i < json.Width; i++)
                for (int y = 0; y < json.Height; y++)
                {
                    if ((field[i, y].Type & (int)FieldType.Wall) > 0)
                        walls.Add(new Point(i, y));
                    if ((field[i, y].Type & (int)FieldType.Destination) > 0)
                        dest.Add(new Point(i, y));
                    if ((field[i, y].Type & (int)FieldType.Box) > 0)
                        boxes.Add(new Point(i, y));
                    if ((field[i, y].Type & (int)FieldType.Player) > 0)
                        json.Player = new Point(i, y);
                }
            json.Boxes = boxes.ToArray();
            json.Destinations = dest.ToArray();
            json.Walls = walls.ToArray();
            return json;
        }

        public static GameField JsonSerializableToField(FieldJsonSerializable json)
        {
            FieldItem[,] field = new FieldItem[json.Width, json.Height];
            for (int i = 0; i < json.Width; i++)
                for (int y = 0; y < json.Height; y++)
                    field[i, y] = new FieldItem(FieldItem.EmptyCell);

            foreach (var item in json.Walls)
                field[item.X, item.Y] |= FieldItem.Wall;
            foreach (var item in json.Boxes)
                field[item.X, item.Y] |= FieldItem.Box;
            foreach (var item in json.Destinations)
                field[item.X, item.Y] |= FieldItem.Destination;
            field[json.Player.X, json.Player.Y] |= FieldItem.Player;
            return new GameField(field, json.Player, json.Boxes.ToList());
        }
    }

    public static class TurnsJsonSerializer
    {
        public static string CurrrentTurnToJson(Turn turn)
        {
            return JsonSerializer.Serialize(turn);
        }

        public static Turn JsonToCurrentTurn(string json)
        {
            return JsonSerializer.Deserialize<Turn>(json);
        }

        public static string TurnsToJson(Turn[] turns)
        {
            return JsonSerializer.Serialize(turns);
        }

        public static List<Turn> JsonToTurns(string json)
        {
            return JsonSerializer.Deserialize<Turn[]>(json).ToList();
        }
    }

    public static class TurnIndexJsonSerializer
    {
        public static string TurnIndexToJson(int index)
        {
            return JsonSerializer.Serialize(index);
        }

        public static int JsonToTurnIndex(string json)
        {
            return JsonSerializer.Deserialize<int>(json);
        }
    }

    public static class LevelNameSerialazer
    {
        public static string LevelNameToJson(string levelName)
        {
            return JsonSerializer.Serialize(levelName);
        }

        public static string JsonToLevelName(string json)
        {
            return JsonSerializer.Deserialize<string>(json);
        }
    }
}
