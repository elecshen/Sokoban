using Model;
using Model.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sokoban
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private MainPage mainPage;
        private GamePage gamePage;
        private LevelsPage levelsPage;

        public MainWindow()
        {
            InitializeComponent();
            mainPage = new MainPage
            {
                DataContext = DataContext
            };
            gamePage = new GamePage();
            levelsPage= new LevelsPage();


            #region MainPage
            ((VM)DataContext).ChooseLevelEvent += OpenChooseLevelPage;
            ((VM)DataContext).ContinueEvent += ContinueGame;
            #endregion

            #region LevelsPage
            ((VMLevelsPage)levelsPage.DataContext).OpenLevelEvent += OpenGamePage;
            ((VMLevelsPage)levelsPage.DataContext).ReturnToMenuEvent += OpenMainPage;
            #endregion

            #region GamePage
            ((VM)DataContext).MoveEvent += ((VMGamePage)gamePage.DataContext).Move;
            ((VMGamePage)gamePage.DataContext).ReturnToMenuEvent += OpenMainPage;
            ((VMGamePage)gamePage.DataContext).ToNextLevelEvent += ((VMLevelsPage)levelsPage.DataContext).OpenNextLevel;
            #endregion

            OpenMainPage();
        }

        private void OpenMainPage()
        {
            frame.Navigate(mainPage);
        }

        private void OpenChooseLevelPage()
        {
            ((VMLevelsPage)levelsPage.DataContext).CheckLevelsDirectory();
            frame.Navigate(levelsPage);
        }

        private void ContinueGame()
        {
            if(!JsonFileManager.GetJsonStingsFromFile(JsonFileManager.GameSavePath, out string[] jsonStrs))
            {
                MessageBox.Show("Не удалось открыть файл сохранения");
                return;
            }
            var factory = new JsonStringFieldFactory(jsonStrs[0]);
            var turns = TurnsJsonSerializer.JsonToTurns(jsonStrs[1]);
            var turnIndex = TurnIndexJsonSerializer.JsonToTurnIndex(jsonStrs[2]);
            var levelName = LevelNameSerialazer.JsonToLevelName(jsonStrs[3]);
            gamePage.SetLevel(levelName, factory, turns, turnIndex);

            frame.Navigate(gamePage);
        }

        private void OpenGamePage(IFieldFactory factory, string levelName)
        {
            if(factory== null)
            {
                OpenChooseLevelPage();
                return;
            }
            gamePage.SetLevel(levelName, factory);
            frame.Navigate(gamePage);
        }
    }
}
