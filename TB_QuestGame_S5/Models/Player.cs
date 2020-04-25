using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TBQuestGame_S1.Models
{
    public class Player : Character, IBattle
    {
        #region Enums
        public enum Gender
        {
            male,
            female
        }

        public enum StartStyle
        {
            neutral,
            offensive,
            deffensive
        }
        #endregion

        private const int MAX_RETREAT_DAMAGE = 100;

        #region Fields

        private string _legionName;
        private int _wealth;
        //soldier numbers
        private int _numOfSeigeWeapons;
        private int _legionnaireNumbers;
        private int _archerNumbers;
        private int _cavalryNumbers;
        private int _praetorianNumbers;
        private int _centurionNumbers;
        private int _praetorNumbers;
        private int _tacticalAdvantage;
        //lists
        private List<NPC> _npcEngaged;
        //enums
        private BattleModeName _battleMode;
        //game items/ observable collection
        private ObservableCollection<GameItemQuantity> _inventory;
        private ObservableCollection<GameItemQuantity> _armorInventory;
        private ObservableCollection<GameItemQuantity> _soldiers;
        private ObservableCollection<GameItemQuantity> _seigeWeapons;
        private ObservableCollection<GameItemQuantity> _treasures;
        private ObservableCollection<GameItemQuantity> _gems;
        private ObservableCollection<GameItemQuantity> _armorSet;
        private ObservableCollection<GameItemQuantity> _buffs;
        private ObservableCollection<Mission> _missions;
        //location bools
        private bool _elkmireIsDefeated;
        private bool _northBourgIsDefeated;
        private bool _southBourgIsDefeated;
        private bool _doreIsDefeated;
        private bool _quaRediIsDefeated;
        private bool _alheimurrinnIsDefeated;
        private bool _playerHasShips;
        private bool _strangerIsDefeated;
        private bool _thiefIsDefeated;
        private bool _falseMerchantIsDefeated;
        private bool _wandererIsDefeated;
        private bool _hasAllGems;
        private bool _hasArmorSet;

        #endregion

        #region Properties
        public string LegionName
        {
            get { return _legionName; }
            set { _legionName = value; }
        }

        public int Wealth
        {
            get { return _wealth; }
            set { _wealth = value; }
        }

        public int NumOfSeigeWeapons
        {
            get { return _numOfSeigeWeapons; }
            set { _numOfSeigeWeapons = value; }
        }

        public int LegionnaireNumbers
        {
            get { return _legionnaireNumbers; }
            set { _legionnaireNumbers = value; }
        }

        public int ArcherNumbers
        {
            get { return _archerNumbers; }
            set { _archerNumbers = value; }
        }

        public int CavalryNumbers
        {
            get { return _cavalryNumbers; }
            set { _cavalryNumbers = value; }
        }

        public int PraetorianNumbers
        {
            get { return _praetorianNumbers; }
            set { _praetorianNumbers = value; }
        }

        public int CenturionNumbers
        {
            get { return _centurionNumbers; }
            set { _centurionNumbers = value; }
        }

        public int PraetorNumbers
        {
            get { return _praetorNumbers; }
            set { _praetorNumbers = value; }
        }

        public int TacticalAdvantage
        {
            get { return _tacticalAdvantage; }
            set { _tacticalAdvantage = value; }
        }

        public List<NPC> NPCEngaged
        {
            get { return _npcEngaged; }
            set { _npcEngaged = value; }
        }

        public BattleModeName BattleMode
        {
            get { return _battleMode; }
            set { _battleMode = value; }
        }

        public ObservableCollection<GameItemQuantity> Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public ObservableCollection<GameItemQuantity> ArmorInventory
        {
            get { return _armorInventory; }
            set { _armorInventory = value; }
        }

        public ObservableCollection<GameItemQuantity> Soldiers
        {
            get { return _soldiers; }
            set { _soldiers = value; }
        }

        public ObservableCollection<GameItemQuantity> SeigeWeapons
        {
            get { return _seigeWeapons; }
            set { _seigeWeapons = value; }
        }

        public ObservableCollection<GameItemQuantity> Treasures
        {
            get { return _treasures; }
            set { _treasures = value; }
        }

        public ObservableCollection<GameItemQuantity> Gems
        {
            get { return _gems; }
            set { _gems = value; }
        }

        public ObservableCollection<GameItemQuantity> ArmorSet
        {
            get { return _armorSet; }
            set { _armorSet = value; }
        }

        public ObservableCollection<GameItemQuantity> Buffs
        {
            get { return _buffs; }
            set { _buffs = value; }
        }

        public ObservableCollection<Mission> Missions
        {
            get { return _missions; }
            set { _missions = value; }
        }

        public bool ElkmireIsDefeated
        {
            get { return _elkmireIsDefeated; }
            set { _elkmireIsDefeated = value; }
        }

        public bool NorthBourgIsDefeated
        {
            get { return _northBourgIsDefeated; }
            set { _northBourgIsDefeated = value; }
        }

        public bool SouthBourgIsDefeated
        {
            get { return _southBourgIsDefeated; }
            set { _southBourgIsDefeated = value; }
        }

        public bool DoreIsDefeated
        {
            get { return _doreIsDefeated; }
            set { _doreIsDefeated = value; }
        }

        public bool QuaRediIsDefeated
        {
            get { return _quaRediIsDefeated; }
            set { _quaRediIsDefeated = value; }
        }

        public bool AlheimurrinnIsDefeated
        {
            get { return _alheimurrinnIsDefeated; }
            set { _alheimurrinnIsDefeated = value; }
        }

        public bool PlayerHasShips
        {
            get { return _playerHasShips; }
            set { _playerHasShips = value; }
        }

        public bool strangerIsDefeated
        {
            get { return _strangerIsDefeated; }
            set { _strangerIsDefeated = value; }
        }

        public bool ThiefIsDefeated
        {
            get { return _thiefIsDefeated; }
            set { _thiefIsDefeated = value; }
        }

        public bool FalseMerchantIsDefeated
        {
            get { return _falseMerchantIsDefeated; }
            set { _falseMerchantIsDefeated = value; }
        }

        public bool WandererIsDefeated
        {
            get { return _wandererIsDefeated; }
            set { _wandererIsDefeated = value; }
        }

        public bool HasAllGems
        {
            get { return _hasAllGems; }
            set { _hasAllGems = value; }
        }

        public bool HasArmorSet
        {
            get { return _hasArmorSet; }
            set { _hasArmorSet = value; }
        }

        #endregion

        #region Constructors

        public Player()
        {
            _npcEngaged = new List<NPC>();

            _soldiers = new ObservableCollection<GameItemQuantity>();
            _seigeWeapons = new ObservableCollection<GameItemQuantity>();
            _treasures = new ObservableCollection<GameItemQuantity>();
            _gems = new ObservableCollection<GameItemQuantity>();
            _armorSet = new ObservableCollection<GameItemQuantity>();
            _buffs = new ObservableCollection<GameItemQuantity>();
            _missions = new ObservableCollection<Mission>();
        }

        #endregion

        #region Methods

        #region Inventory Methods

        /// <summary>
        /// Update the observable collections
        /// </summary>
        public void UpdateInventoryCategories()
        {
            Soldiers.Clear();
            SeigeWeapons.Clear();
            Treasures.Clear();
            Gems.Clear();
            ArmorSet.Clear();
            Buffs.Clear();

            foreach (var gameItemQuantity in _inventory)
            {
                if (gameItemQuantity.GameItem is Soldier) Soldiers.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is SeigeWeapon) SeigeWeapons.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is Treasure) Treasures.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is Gem) Gems.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is ArmorSet) ArmorSet.Add(gameItemQuantity);
                if (gameItemQuantity.GameItem is Buff) Buffs.Add(gameItemQuantity);
            }
        }

        /// <summary>
        /// adds game item to inventory
        /// </summary>
        public void AddGameItemToInventory(GameItemQuantity selectedGameItemQuantity)
        {
            GameItemQuantity gameItemQuantity = _inventory.FirstOrDefault(i => i.GameItem.Id == selectedGameItemQuantity.GameItem.Id);

            if (gameItemQuantity == null)
            {
                GameItemQuantity newGameItemQuantity = new GameItemQuantity();
                newGameItemQuantity.GameItem = selectedGameItemQuantity.GameItem;
                newGameItemQuantity.Quantity = 1;
            }
            else
            {
                gameItemQuantity.Quantity++;
            }
            UpdateInventoryCategories();
        }

        /// <summary>
        /// Removes game item from inventory
        /// </summary>
        public void RemoveGameItemFromInventory(GameItemQuantity selectedGameItemQuantity)
        {
            GameItemQuantity gameItemQuantity = _inventory.FirstOrDefault(i => i.GameItem.Id == selectedGameItemQuantity.GameItem.Id);

            if (gameItemQuantity != null)
            {
                _inventory.Remove(gameItemQuantity);
            }
            else
            {
                gameItemQuantity.Quantity--;
            }

            UpdateInventoryCategories();
        }

        #endregion

        #region Battle Methods

        /// <summary>
        /// calculates attack damage
        /// </summary>
        public int AttackCalc()
        {
            int attack = Power;
            //int randomAttack = random.Next(2, 10);
            //int damage = Attack / randomAttack;

            //if (damage <= 100)
            //{
            //    return damage;
            //}
            //else
            //{
            //    return 0;
            //}

            return attack;
        }

        /*
        public int DefendCalc()
        {
            throw new NotImplementedException();
        } */

        /// <summary>
        /// checks to see if the retreat was successful
        /// </summary>
        public int Retreat()
        {
            int randomAttack = random.Next(2, 10);
            int damage = Power / randomAttack;

            if (damage <= 100)
            {
                return damage;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region Mission Methods

        /// <summary>
        /// Updates the mission status
        /// </summary>
        public void UpdateMissionStatus()
        {
            foreach (Mission mission in _missions.Where(m => m.Status == Mission.MissionStatus.incomplete))
            {
                if (mission is AncientRelicMission)
                {
                    if (((AncientRelicMission)mission).HasArmorSet == true)
                    {
                        mission.Status = Mission.MissionStatus.complete;
                    }
                }
                else if (mission is HellfireGemMission)
                {
                    if (((HellfireGemMission)mission).HasAllGems == true)
                    {
                        mission.Status = Mission.MissionStatus.complete;
                    }
                }
                else if (mission is LocateMission)
                {
                    if (((LocateMission)mission).AllSpiesDefeated == true)
                    {
                        mission.Status = Mission.MissionStatus.complete;
                    }
                }
                else if (mission is SeigeMission)
                {
                    if (((SeigeMission)mission).AllEnemiesDefeated == true)
                    {
                        mission.Status = Mission.MissionStatus.complete;
                    }
                }
                else if (mission is CrownMission)
                {
                    if (((CrownMission)mission).ThiefIsCaptured == true)
                    {
                        mission.Status = Mission.MissionStatus.complete;
                    }
                }

                else
                {
                    throw new Exception("Unknown mission child class.");
                }
            }
        }

        #endregion

        #region Misc. Methods

        /// <summary>
        /// displays message to the barraks window
        /// </summary>
        public string InitialMessage()
        {
            return
                "You have been tasked by your Emperor and the High Council to lay seige on enemy lands. " +
                "Along with the title of Imperator, you have been given a legion and a starting sum of gold. " +
                "Use your newfound status and wealth wisely. The Emperor will not tolerate failure. ";
        }

        #endregion

        #endregion
    }
}
