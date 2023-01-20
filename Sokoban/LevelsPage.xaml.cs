using System.Windows.Controls;

namespace Sokoban
{
    /// <summary>
    /// Логика взаимодействия для LevelsPage.xaml
    /// </summary>
    public partial class LevelsPage : Page
    {
        public LevelsPage()
        {
            InitializeComponent();
            (DataContext as VMLevelsPage).CheckLevelsDirectory();
        }
    }
}
