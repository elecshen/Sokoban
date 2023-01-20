using Model;
using Model.FileManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    public class VMLevelsPage
    {
        public class LevelInfo
        {
            public string LevelName { get; }
            public string FileName { get; }

            public LevelInfo(string levelName, string fileName)
            {
                LevelName = levelName;
                FileName = fileName;
            }
        }

        private ObservableCollection<LevelInfo> levelsNames;
        private Command openLevelCommand;
        private Command returnToMenuCommand;

        public event FieldFactoryAndLevelNameFoo OpenLevelEvent;
        public event SimpleFoo ReturnToMenuEvent;

        public ReadOnlyObservableCollection<LevelInfo> LevelsNames { get; }

        public VMLevelsPage()
        {
            levelsNames = new ObservableCollection<LevelInfo>();
            LevelsNames = new ReadOnlyObservableCollection<LevelInfo>(levelsNames);
        }

        public void CheckLevelsDirectory()
        {
            levelsNames.Clear();
            JsonFileManager.GetFileNamesFromDirectory(JsonFileManager.LevelsPath, out string[] fileNames);
            foreach (string fileName in fileNames)
            {
                string levelName = fileName.Replace(".json", "");
                levelName = levelName.Replace(JsonFileManager.LevelsPath, "");
                levelsNames.Add(new LevelInfo(levelName, fileName));
            }
        }

        public void OpenNextLevel(string levelName)
        {
            var elems = LevelsNames.Where(n => n.LevelName == levelName);
            if (elems.Any())
            {
                var levelInfo = elems.First();
                if (levelInfo != LevelsNames.Last())
                {
                    levelInfo = LevelsNames[LevelsNames.IndexOf(levelInfo) + 1];
                    OpenLevelEvent(GetFieldFactoryOfLevel(levelInfo.FileName), levelInfo.LevelName);
                    return;
                }
            }
            OpenLevelEvent(null, null);
        }

        private IFieldFactory GetFieldFactoryOfLevel(string path)
        {
            if(JsonFileManager.GetJsonStingsFromFile(path, out string[] json))
                return new JsonStringFieldFactory(json[0]);
            return null;
        }

        private void OpenLevel(string levelName)
        {
            var levelInfo = LevelsNames.Where(n => n.LevelName == levelName).First();
            OpenLevelEvent(GetFieldFactoryOfLevel(levelInfo.FileName), levelInfo.LevelName);
        }

        public Command OpenLevelCommand
        {
            get
            {
                return openLevelCommand ?? (openLevelCommand = new Command(obj =>
                {
                    OpenLevel(obj as string);
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
    }
}
