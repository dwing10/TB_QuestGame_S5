using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame_S1.Models
{
    public class SeigeMission : Mission
    {
        #region fields

        private string _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private bool _allEnemiesDefeated;

        #endregion

        #region Properties

        public bool AllEnemiesDefeated
        {
            get { return _allEnemiesDefeated; }
            set { _allEnemiesDefeated = value; }
        }

        #endregion

        #region Constructors

        public SeigeMission()
        {

        }

        public SeigeMission(string id, string name, MissionStatus status, bool allEnemiesDefeated)
            : base(id, name, status)
        {
            _id = id;
            _name = name;
            _status = status;
            _allEnemiesDefeated = allEnemiesDefeated;
        }

        #endregion

        #region Methods

        #endregion
    }
}
