using Model;
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
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private class ControlContainer
        {
            public ContentControl control;
            public int x, y;
            public FieldType type;
        }

        private readonly List<ControlContainer> containers;

        public GamePage()
        {
            containers = new List<ControlContainer>();
            InitializeComponent();
            ((VMGamePage)DataContext).RefreshFieldEvent += RefreshField;
        }

        public void SetLevel(string levelName, IFieldFactory factory, List<Turn> turns = null, int turnIndex = -1)
        {
            ((VMGamePage)DataContext).LevelName = levelName;
            ((VMGamePage)DataContext).Core.SetLevel(factory.MakeGameField(), turns, turnIndex);
        }

        private void DrawFieldCell(int x, int y, int fieldType)
        {
            foreach (FieldType type in Enum.GetValues(typeof(FieldType)))
            {
                if ((fieldType & (int)type) > 0)
                {
                    ControlContainer container = new ControlContainer()
                    {
                        control = new ContentControl()
                        {
                            Template = (ControlTemplate)TryFindResource(type.ToString()),
                        },
                        x = x,
                        y = y,
                        type = type,
                    };
                    Canvas.SetLeft(container.control, x * 30);
                    Canvas.SetTop(container.control, y * 30);
                    Canvas.SetZIndex(container.control, (int)type);
                    GameField.Children.Add(container.control);
                    containers.Add(container);
                }
            }
        }

        private void RefreshField(GameField field)
        {
            GameField.Children.Clear();
            containers.Clear();
            (int w, int h) = field.GetFieldShape();
            for (int i = 0; i < w; i++)
                for (int y = 0; y < h; y++)
                    DrawFieldCell(i, y, field[i, y].Type);
        }
    }
}
