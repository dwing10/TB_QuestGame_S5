using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame_S1.Models;

namespace TBQuestGame_S1.PresentationLayer
{
    public class MissionStatusViewModel : ObservableObject
    {
        #region fields

        private string _missionInformation;
        private List<Mission> _missions;

        #endregion

        #region properties

        public string MissionInformation
        {
            get { return _missionInformation; }
            set
            {
                _missionInformation = value;
                OnPropertyChange(nameof(MissionInformation));
            }
        }

        public List<Mission> Missions
        {
            get { return _missions; }
            set { _missions = value; }
        }

        #endregion
    }
}
