using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame_S1.Models
{
    class ArmorSet : GameItem
    {
        #region fields



        #endregion

        #region properties



        #endregion

        #region constructors

        public ArmorSet(string id, string name, int value, string description) : base(id, name, value, description)
        {

        }

        #endregion

        #region methods

        public override string InformationString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
