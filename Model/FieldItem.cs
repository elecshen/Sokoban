namespace Model
{
    public enum FieldType
    {
        EmptyCell = 1 << 0,
        Wall = 1 << 1,
        Destination = 1 << 2,
        Box = 1 << 3,
        Player = 1 << 4,
    }

    public class FieldItem
    {
        public static readonly FieldItem EmptyCell = new FieldItem((int)FieldType.EmptyCell, false);
        public static readonly FieldItem Wall = new FieldItem((int)FieldType.Wall, true);
        public static readonly FieldItem Destination = new FieldItem((int)FieldType.Destination, false);
        public static readonly FieldItem Box = new FieldItem((int)FieldType.Box, true);
        public static readonly FieldItem Player = new FieldItem((int)FieldType.Player, true);

        public int Type { get; }
        public bool IsBarricade { get; }
        
        public FieldItem(int fieldType, bool isBarricade)
        {
            Type = fieldType;
            IsBarricade = isBarricade;
        }

        public FieldItem(FieldItem fieldItem)
        {
            Type= fieldItem.Type;
            IsBarricade= fieldItem.IsBarricade;
        }

        public static FieldItem operator |(FieldItem left, FieldItem right)
        {
            return new FieldItem(left.Type | right.Type, left.IsBarricade || right.IsBarricade);
        }

        public static FieldItem operator ^(FieldItem left, FieldItem right)
        {
            return new FieldItem(left.Type ^ right.Type, false);
        }
    }
}
