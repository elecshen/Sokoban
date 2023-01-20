namespace Model
{
    public interface IFieldFactory
    {
        GameField MakeGameField();
    }

    public class JsonStringFieldFactory : IFieldFactory
    {
        private readonly string json;

        public JsonStringFieldFactory(string json)
        {
            this.json = json;
        }

        public GameField MakeGameField()
        {
            var jsonObj = FieldJsonSerializer.JsonToFieldJsonSerializable(json);
            return FieldJsonSerializer.JsonSerializableToField(jsonObj);
        }
    }

    public class VariableFieldFactory : IFieldFactory
    {
        private readonly FieldJsonSerializable field;

        public VariableFieldFactory(FieldJsonSerializable field)
        {
            this.field = field;
        }

        public GameField MakeGameField()
        {
            return FieldJsonSerializer.JsonSerializableToField(field);
        }
    }
}
