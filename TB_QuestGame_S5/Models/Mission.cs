using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame_S1.Models
{
    public class Mission
    {
        public enum MissionStatus
        {
            unassigned,
            incomplete,
            complete
        }

        #region Fields

        private string _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private string _statusDetail;

        #endregion

        #region Properties

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public MissionStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string StatusDetail
        {
            get { return _statusDetail; }
            set { _statusDetail = value; }
        }

        #endregion

        #region Constructors

        public Mission()
        {

        }

        public Mission(string id, string name, MissionStatus status)
        {
            _id = id;
            _name = name;
            _status = status;
        }

        #endregion
    }
}
