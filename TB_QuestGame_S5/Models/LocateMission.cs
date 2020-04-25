using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame_S1.Models
{
    public class LocateMission : Mission
    {
        #region fields

        private string _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private string _statusDetail;
        private List<NPC> _requiredNPCs;
        private bool _allSpiesDefeated;

        #endregion

        #region Properties

        public bool AllSpiesDefeated
        {
            get { return _allSpiesDefeated; }
            set { _allSpiesDefeated = value; }
        }

        public List<NPC> RequiredNPCs
        {
            get { return _requiredNPCs; }
            set { _requiredNPCs = value; }
        }

        #endregion

        #region constructors

        public LocateMission()
        {

        }

        public LocateMission(string id, string name, MissionStatus status, bool allSpiesDefeated)
            : base(id, name, status)
        {
            _id = id;
            _name = name;
            _status = status;
            _allSpiesDefeated = allSpiesDefeated;
        }

        #endregion

        #region methods

        /// <summary>
        /// returns a list of NPCs that have not been engaged
        /// </summary>
        public List<NPC> NPCNotEngaged(List<NPC> NPCsEngaged)
        {
            List<NPC> notEngaged = new List<NPC>();

            foreach (var requiredNPC in _requiredNPCs)
            {
                if (!NPCsEngaged.Any(l => l.ID == requiredNPC.ID))
                {
                    notEngaged.Add(requiredNPC);
                }
            }

            return notEngaged;
        }

        #endregion
    }
}
