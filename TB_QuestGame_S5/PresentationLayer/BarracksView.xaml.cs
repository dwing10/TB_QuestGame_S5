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
using System.Windows.Shapes;
using TBQuestGame_S1.Models;

namespace TBQuestGame_S1.PresentationLayer
{
    /// <summary>
    /// Interaction logic for BarracksView.xaml
    /// </summary>
    public partial class BarracksView : Window
    {
        Player _player;   
        public BarracksView(Player player)
        {
            _player = player;
            InitializeComponent();

            FillInfo();
        }

        /// <summary>
        /// fills in labels with content
        /// </summary>
        private void FillInfo()
        {
            legionName.Content = _player.LegionName;
            imperatorName.Content = _player.Name;
            power.Content = _player.Power;
            tacticalAdvantage.Content = _player.TacticalAdvantage;
            LegionnaireNumbers.Content = _player.LegionnaireNumbers;
            ArcherNumbers.Content = _player.ArcherNumbers;
            CavalryNumbers.Content = _player.CavalryNumbers;
            CenturionNumbers.Content = _player.CenturionNumbers;
            PraetorNumbers.Content = _player.PraetorNumbers;
            ballistaeNum.Content = _player.NumOfSeigeWeapons;
            if (_player.PlayerHasShips == true)
            {
                navalFleet.IsChecked = true;
            }
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
