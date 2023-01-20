using Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sokoban
{
    internal class VMGamePage : INotifyPropertyChanged
    {
        private Core core;
        private string levelName;
        private int numberOfTurns;
        private int currentTurnNumber;
        private System.Windows.Visibility gameEndVisibility;
        private bool revertIsEnable;
        private bool nextIsEnable;

        private Command revertTurnCommand;
        private Command nextTurnCommand;
        private Command nextLevelCommand;
        private Command saveCommand;
        private Command returnToMenuCommand;

        public event SimpleFoo ReturnToMenuEvent;
        public event StrFoo ToNextLevelEvent;
        public event GameFieldFoo RefreshFieldEvent;

        public Core Core
        {
            get { return core; }
            set { core = value; OnPropertyChanged(); }
        }
        public string LevelName
        {
            get { return levelName; }
            set
            {
                levelName = value;
                OnPropertyChanged();
            }
        }
        public int NumberOfTurns
        {
            get { return numberOfTurns; }
            private set
            {
                numberOfTurns = value;
                OnPropertyChanged();
            }
        }
        public int CurrentTurnNumber
        {
            get { return currentTurnNumber; }
            private set
            {
                currentTurnNumber = value;
                OnPropertyChanged();
            }
        }
        public System.Windows.Visibility GameEndVisibility
        {
            get { return gameEndVisibility; }
            private set { gameEndVisibility = value; OnPropertyChanged(); }
        }
        public bool RevertIsEnable
        {
            get { return revertIsEnable; }
            private set { revertIsEnable = value; OnPropertyChanged(); }
        }
        public bool NextIsEnable
        {
            get { return nextIsEnable; }
            private set { nextIsEnable = value; OnPropertyChanged();}
        }

        public VMGamePage() 
        {
            PropertyChanged += InitCore;
            Core = new Core();
        }

        private void InitCore(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Core")
            {
                GameEndVisibility = System.Windows.Visibility.Hidden;
                if (Core == null)
                    return;
                Core.PropertyChanged += CalculateTurns;
                Core.PropertyChanged += CalculateCurrentTurn;
                Core.PropertyChanged += SetGameEndVisibility;
                Core.PropertyChanged += SetRevetIsEnable;
                Core.PropertyChanged += SetNextIsEnable;
            }
        }

        public void Move(string strDirection)
        {
            Point direction;
            if (strDirection == "Up")
                direction = Directions.Up;
            else if (strDirection == "Left")
                direction = Directions.Left;
            else if (strDirection == "Right")
                direction = Directions.Right;
            else if (strDirection == "Down")
                direction = Directions.Down;
            else return;
            if(Core.CreateTurn(direction))
                Core.NextTurn();
        }

        private void CalculateTurns(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TurnsHistory")
                NumberOfTurns = Core.TurnsHistory.Count;
        }

        private void CalculateCurrentTurn(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "CurrentTurn")
            {
                CurrentTurnNumber = Core.CurrentTurnIndex + 1;
                RefreshFieldEvent(Core.Field);
            }
        }

        private void SetGameEndVisibility(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGameEnd")
            {
                if(Core.IsGameEnd)
                    GameEndVisibility = System.Windows.Visibility.Visible;
                else
                    GameEndVisibility= System.Windows.Visibility.Hidden;
            }
        }

        private void SetRevetIsEnable(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsFirstTurn")
            {
                RevertIsEnable = !Core.IsFirstTurn;
            }
        }

        private void SetNextIsEnable(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLastTurn")
            {
                NextIsEnable = !Core.IsLastTurn;
            }
        }

        public Command RevertTurnCommand
        {
            get
            {
                return revertTurnCommand ?? (revertTurnCommand = new Command(obj =>
                {
                    Core.RevertTurn();
                }));
            }
        }
        public Command NextTurnCommand
        {
            get
            {
                return nextTurnCommand ?? (nextTurnCommand = new Command(obj =>
                {
                    Core.NextTurn();
                }));
            }
        }
        public Command NextLevelCommand
        {
            get
            {
                return nextLevelCommand ?? (nextLevelCommand = new Command(obj =>
                {
                    ToNextLevelEvent(LevelName);
                }));
            }
        }
        public Command SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new Command(obj =>
                {
                    Core.SaveGame(LevelName);
                }));
            }
        }
        public Command ReturnToMenuCommand
        {
            get
            {
                return returnToMenuCommand ?? (returnToMenuCommand = new Command(obj =>
                {
                    ReturnToMenuEvent();
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
