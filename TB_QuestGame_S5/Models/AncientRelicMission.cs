using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame_S1.Models
{
    class AncientRelicMission : Mission
    {
        #region Fields

        private string _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private List<GameItemQuantity> _requiredGameItems;
        private bool _hasArmorSet;

        #endregion

        #region Properties

        public List<GameItemQuantity> RequiredGameItems
        {
            get { return _requiredGameItems; }
            set { _requiredGameItems = value; }
        }

        public bool HasArmorSet
        {
            get { return _hasArmorSet; }
            set { _hasArmorSet = value; }
        }

        #endregion

        #region Constructors

        public AncientRelicMission()
        {

        }

        public AncientRelicMission(string id, string name, MissionStatus status, List<GameItemQuantity> requiredGameItems, bool hasArmorSet)
            : base(id, name, status)
        {
            _id = id;
            _name = name;
            _status = status;
            _requiredGameItems = requiredGameItems;
            _hasArmorSet = hasArmorSet;
        }

        #endregion

        #region Methods

        public List<GameItemQuantity> RequiredGameItemsNotCompleted(List<GameItemQuantity> inventory)
        {
            List<GameItemQuantity> itemsNotCompleted = new List<GameItemQuantity>();

            foreach (var missionGameItem in _requiredGameItems)
            {
                GameItemQuantity inventoryMatch = inventory.FirstOrDefault(gi => gi.GameItem.Id == missionGameItem.GameItem.Id);

                if (inventoryMatch == null)
                {
                    itemsNotCompleted.Add(missionGameItem);
                }
                else
                {
                    if (inventoryMatch.Quantity < missionGameItem.Quantity)
                    {
                        itemsNotCompleted.Add(missionGameItem);
                    }
                }
            }

            return itemsNotCompleted;
        }

        #endregion
    }
}
