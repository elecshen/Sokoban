using System;
using System.Text.Json.Serialization;

namespace Model
{
    public class Turn
    {
        public static readonly Turn EmptyTurn = new Turn(new Point(0, 0), new Point(0, 0), false);

        public Point PlayerStartPos { get; }
        public Point PlayerEndPos { get; }
        public Point BoxStartPos { get; }
        public Point BoxEndPos { get; }

        [JsonConstructor]
        public Turn(Point playerStartPos, Point playerEndPos, Point boxStartPos, Point boxEndPos)
        {
            PlayerStartPos = playerStartPos;
            PlayerEndPos = playerEndPos;
            BoxStartPos = boxStartPos;
            BoxEndPos = boxEndPos;
        }

        public Turn(Point playerStartPos, Point delta, bool isBoxTranslated)
        {
            PlayerStartPos = playerStartPos;
            PlayerEndPos = playerStartPos + delta;
            if(isBoxTranslated )
            {
                BoxStartPos = PlayerEndPos;
                BoxEndPos = PlayerEndPos + delta;
            }
            else
            {
                BoxStartPos = null;
                BoxEndPos = null;
            }
        }

        public void MakeTurn(GameField field)
        {
            if (field == null)
                throw new ArgumentNullException("field");
            if ((field[PlayerStartPos].Type & (int)FieldType.Player) == 0)
                throw new Exception("Invalid Player position");
            if (BoxStartPos != null && BoxEndPos !=null 
                && (field[BoxStartPos].Type & (int)FieldType.Box) > 0)
            {
                field.MoveBox(BoxStartPos, BoxEndPos);
            }
            field.MovePlayer(PlayerEndPos);
        }

        public void RevertTurn(GameField field)
        {
            if (field == null)
                throw new ArgumentNullException("field");
            if ((field[PlayerEndPos].Type & (int)FieldType.Player) == 0)
                throw new Exception("Invalid Player position");
            field.MovePlayer(PlayerStartPos);
            if (BoxStartPos != null && BoxEndPos != null
                && (field[BoxEndPos].Type & (int)FieldType.Box) > 0)
            {
                field.MoveBox(BoxEndPos, BoxStartPos);
            }
        }

        public static bool operator ==(Turn left, Turn right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            if (left.PlayerStartPos == right.PlayerStartPos && left.PlayerEndPos == right.PlayerEndPos
                && left.BoxStartPos == right.BoxStartPos && left.BoxEndPos == right.BoxEndPos)
            { return true; }
            return false;
        }

        public static bool operator !=(Turn left, Turn right)
        {
            if (left is null && right is null) return false;
            if (left is null || right is null) return true;
            if (left.PlayerStartPos == right.PlayerStartPos && left.PlayerEndPos == right.PlayerEndPos
                && left.BoxStartPos == right.BoxStartPos && left.BoxEndPos == right.BoxEndPos)
            { return false; }
            return true;
        }
    }
}
