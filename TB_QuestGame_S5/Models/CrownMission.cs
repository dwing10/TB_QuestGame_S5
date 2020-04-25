using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame_S1.Models
{
    public class CrownMission : Mission
    {
        #region Fields

        private string _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private bool _thiefIsCaptured;

        #endregion

        #region Properties

        public bool ThiefIsCaptured
        {
            get { return _thiefIsCaptured; }
            set { _thiefIsCaptured = value; }
        }

        #endregion

        #region Constructors

        public CrownMission()
        {

        }

        public CrownMission(string id, string name, MissionStatus status, bool thiefIsCaptured)
            : base(id, name, status)
        {
            _id = id;
            _name = name;
            _status = status;
            _thiefIsCaptured = thiefIsCaptured;
        }

        #endregion

        #region Methods



        #endregion
    }
}
