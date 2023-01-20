using Model.FileManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Model
{
    public static class CoreFactory
    {
        public static Core MakeCore(IFieldFactory factory, List<Turn> turns = null, int turnIndex = -1)
        {
            Core core = new Core();
            core.SetLevel(factory.MakeGameField(), turns, turnIndex);
            return core;
        }
    }

    public static class Directions
    {
        public static readonly Point Up = new Point(0, -1);
        public static readonly Point Left = new Point(-1, 0);
        public static readonly Point Right = new Point(1, 0);
        public static readonly Point Down = new Point(0, 1);
    }

    public class Core : INotifyPropertyChanged
    {
        private ObservableCollection<Turn> turnsHistory;
        private Turn currentTurn;
        private int currentTurnIndex;
        private bool isGameEnd;
        private bool isFirstTurn;
        private bool isLastTurn;

        public ReadOnlyObservableCollection<Turn> TurnsHistory { get; private set; }
        public Turn CurrentTurn
        {
            get { return currentTurn; }
            private set
            {
                currentTurn = value; OnPropertyChanged();
            }
        }
        public GameField Field { get; private set; }

        public int CurrentTurnIndex
        {
            get { return currentTurnIndex; }
            private set { currentTurnIndex = value; OnPropertyChanged(); }
        }
        public bool IsGameEnd
        {
            get { return isGameEnd; }
            private set { isGameEnd = value; OnPropertyChanged(); }
        }
        public bool IsFirstTurn
        {
            get { return isFirstTurn; }
            private set { isFirstTurn = value; OnPropertyChanged(); }
        }
        public bool IsLastTurn
        {
            get { return isLastTurn; }
            private set { isLastTurn = value; OnPropertyChanged(); }
        }

        public Core()
        {
            PropertyChanged += SetCurrentTurnIndex;
            PropertyChanged += CheckLimits;
            PropertyChanged += CheckGameEnd;
        }

        public void SetLevel(GameField field, List<Turn> turns = null, int turnIndex = -1)
        {
            if (field == null)
                throw new ArgumentNullException("field");
            else
                Field = field;
            if (turns == null)
                turnsHistory = new ObservableCollection<Turn>();
            else
                turnsHistory = new ObservableCollection<Turn>(turns);
            turnsHistory.CollectionChanged += (sender, e) => { OnPropertyChanged("TurnsHistory"); };
            TurnsHistory = new ReadOnlyObservableCollection<Turn>(turnsHistory);
            OnPropertyChanged("TurnsHistory");
            if (turnIndex != -1)
                CurrentTurn = TurnsHistory.ElementAt(turnIndex);
            else
                CurrentTurn = null;
        }

        private bool СanPlayerMove(Point delta)
        {
            Point newPos = Field.Player + delta;
            if (!Field.IsInField(newPos))
                return false;
            if (!Field[newPos].IsBarricade)
                return true;
            if ((Field[newPos].Type & (int)FieldType.Box) > 0)
                return !Field[newPos + delta].IsBarricade;
            return false;
        }

        public bool CreateTurn(Point delta)
        {
            if (IsGameEnd)
                return false;
            if (!СanPlayerMove(delta))
                return false;
            bool isBoxMove = (Field[Field.Player + delta].Type & (int)FieldType.Box) > 0;
            AddTurn(new Turn(Field.Player, delta, isBoxMove));
            return true;
        }

        private void AddTurn(Turn turn)
        {
            while (!IsLastTurn)
                turnsHistory.Remove(TurnsHistory.Last());
            turnsHistory.Add(turn);
        }

        public void RevertTurn()
        {
            if (IsFirstTurn)
                return;
            CurrentTurn.RevertTurn(Field);
            int index = CurrentTurnIndex - 1;
            if (index < 0)
                CurrentTurn = null;
            else
                CurrentTurn = TurnsHistory.ElementAt(index);
        }

        public void NextTurn()
        {
            if (IsLastTurn)
                return;
            Turn turn = TurnsHistory.ElementAt(CurrentTurnIndex + 1);
            turn.MakeTurn(Field);
            CurrentTurn = turn;
        }

        private void SetCurrentTurnIndex(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "CurrentTurn")
            {
                if(TurnsHistory != null)
                    CurrentTurnIndex = TurnsHistory.IndexOf(CurrentTurn);
                else
                    CurrentTurnIndex = -1;
            }
        }

        private void CheckLimits(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentTurn" || e.PropertyName == "TurnsHistory")
            {
                if (TurnsHistory.Count > 0)
                {
                    if (TurnsHistory.Last() == CurrentTurn) IsLastTurn = true;
                    else IsLastTurn = false;
                    if (CurrentTurn == null) IsFirstTurn = true;
                    else IsFirstTurn = false;
                }
                else
                {
                    IsFirstTurn = true;
                    IsLastTurn = true;
                }
            }
        }

        private void CheckGameEnd(object sender, object e)
        {
            if (e is PropertyChangedEventArgs && (e as PropertyChangedEventArgs).PropertyName == "CurrentTurn")
            {
                foreach (var box in Field.Boxes)
                {
                    if ((Field[box].Type & (int)FieldType.Destination) == 0)
                    {
                        IsGameEnd = false;
                        return;
                    }
                }
                IsGameEnd = true;
            }
        }

        public void SaveGame(string levelName)
        {
            if (TurnsHistory.Count == 0) return;
            string[] jsonStrings = new string[4];
            jsonStrings[0] = FieldJsonSerializer.FieldJsonSerializableToJson(FieldJsonSerializer.FieldToJsonSerializable(Field));
            jsonStrings[1] = TurnsJsonSerializer.TurnsToJson(TurnsHistory.ToArray());
            jsonStrings[2] = TurnIndexJsonSerializer.TurnIndexToJson(CurrentTurnIndex);
            jsonStrings[3] = LevelNameSerialazer.LevelNameToJson(levelName);
            JsonFileManager.CreateJsonSaveFile(jsonStrings);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
