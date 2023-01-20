using Model;
using Sokoban.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sokoban
{
    #region EventDelegates
    public delegate void SimpleFoo();
    public delegate void IntFoo(int i);
    public delegate void StrFoo(string dir);
    public delegate void CellParamsFoo(int x, int y, int fieldType);
    public delegate void GameFieldFoo(GameField field);
    public delegate void FieldFactoryAndLevelNameFoo(IFieldFactory factory, string levelName);
    #endregion

    public class VM
    {
        #region Events
        public event SimpleFoo ChooseLevelEvent;
        public event SimpleFoo ContinueEvent;
        public event StrFoo MoveEvent;
        #endregion

        #region Commands
        private Command chooseLevelCommand;
        public Command ChooseLevelCommand
        {
            get { return chooseLevelCommand ?? (chooseLevelCommand = new Command(obj =>
            {
                ChooseLevelEvent();
            })); }
        }

        private Command continueCommand;
        public Command ContinueCommand
        {
            get
            {
                return continueCommand ?? (continueCommand = new Command(obj =>
                {
                    ContinueEvent();
                }));
            }
        }

        private Command moveCommand;
        public Command MoveCommand
        {
            get
            {
                return moveCommand ?? (moveCommand = new Command(obj =>
                {
                    MoveEvent?.Invoke((string)obj);
                }));
            }
        }
        #endregion
    }
}
