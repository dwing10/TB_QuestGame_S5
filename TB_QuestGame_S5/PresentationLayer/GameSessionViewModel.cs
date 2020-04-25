using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame_S1.Models;
using TBQuestGame_S1.DataLayer;
using System.Collections.ObjectModel;

namespace TBQuestGame_S1.PresentationLayer
{
    public class GameSessionViewModel : ObservableObject
    {
        #region Fields

        const string TAB = "\t";
        const string NEW_LINE = "\n";

        private GameSessionViewModel _gameSessionViewModel;

        private Player _player;
        private List<string> _messages;

        private Map _gameMap;
        private Location _currentLocation;
        private string _currentLocationName;
        private ObservableCollection<Location> _accessobleLocations;
        private string _currentLocationInformation;

        private GameItemQuantity _currentGameItem;

        private NPC _currentNPC;

        private Random random = new Random();

        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        #endregion

        #region Properties
        public GameSessionViewModel gameSessionViewModel
        {
            get { return _gameSessionViewModel; }
            set { _gameSessionViewModel = value; }
        }

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public string MessageDisplay
        {
            get { return string.Join("\n\n", _messages); }
        }

        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }

        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                _currentLocationInformation = _currentLocation.Description;
                OnPropertyChange(nameof(CurrentLocation));
                OnPropertyChange(nameof(CurrentLocationInformation));
            }
        }

        public string CurrentLocationName
        {
            get { return _currentLocationName; }
            set
            {
                _currentLocationName = value;
                OnPropertyChange("CurrentLocation");
            }
        }

        public ObservableCollection<Location> AccessibleLocations
        {
            get { return _accessobleLocations; }
            set
            {
                _accessobleLocations = value;
                OnPropertyChange(nameof(AccessibleLocations));
            }
        }

        public string CurrentLocationInformation
        {
            get { return _currentLocationInformation; }
            set
            {
                _currentLocationInformation = value;
                OnPropertyChange(nameof(CurrentLocationInformation));
            }
        }

        public GameItemQuantity CurrentGameItem
        {
            get { return _currentGameItem; }
            set { _currentGameItem = value; }
        }

        public NPC CurrentNPC
        {
            get { return _currentNPC; }
            set
            {
                _currentNPC = value;
                OnPropertyChange(nameof(CurrentNPC));
            }
        }
        #endregion

        #region Constructors

        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(Player player, List<string> initialMessage, Map gameMap, GameSessionViewModel gameSessionViewModel)
        {
            _gameSessionViewModel = gameSessionViewModel;
            _player = player;
            _gameMap = gameMap;
            _currentLocation = _gameMap.CurrentLocation;
            _accessobleLocations = new ObservableCollection<Location>();
            _messages = initialMessage;

            InitializeView();
        }

        #endregion

        #region Methods

        /// <summary>
        /// initialize the view
        /// </summary>
        public void InitializeView()
        {
            _player.UpdateInventoryCategories();
            _currentLocationInformation = CurrentLocation.Description;
        }

        #region Game Item Methods

        /// <summary>
        /// add item from location to player's inventory
        /// </summary>
        public void AddItemToInventory()
        {
            if (_currentGameItem != null && _currentLocation.GameItems.Contains(_currentGameItem))
            {
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.RemoveGameItemQuantityFromLocation(selectedGameItemQuantity);
                _player.AddGameItemToInventory(selectedGameItemQuantity);

            }
        }

        /// <summary>
        /// removes item from player inventory
        /// </summary>
        public void RemoveItemFromInventory()
        {
            if (_currentGameItem != null && _currentGameItem.GameItem.Id != "GLD" && _currentGameItem.GameItem.Id != "GEM" && _currentGameItem.GameItem.Id != "SWD"
                && _currentGameItem.GameItem.Id != "HEl" && _currentGameItem.GameItem.Id != "CUR" && _currentGameItem.GameItem.Id != "BRA" && _currentGameItem.GameItem.Id != "BOO")
            {
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _player.RemoveGameItemFromInventory(selectedGameItemQuantity);
            }
        }

        /// <summary>
        /// calls other methods based on the items used
        /// </summary>
        public void OnUseGameItem()
        {
            switch (_currentGameItem.GameItem)
            {
                case Buff buff:
                    ProcessBuffUse(buff);
                    RemoveItemFromInventory();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// process buff depending on buff ID
        /// </summary>
        private void ProcessBuffUse(Buff buff)
        {
            if (buff.Id == "INS")
            {
                Player.Power = Player.Power + 100;
            }
            if (buff.Id == "BOL")
            {
                _player.LegionnaireNumbers += 100;
                _player.Power += 100;
            }
            if (buff.Id == "TRI")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity += 1000;
                }
            }
        }

        /// <summary>
        /// Process seige weapon use based on ID
        /// </summary>
        private void ProcessSeigeWeaponUse(SeigeWeapon seigeWeapon)
        {
            if (seigeWeapon.Id == "CAT")
            {
                Player.Power += 200;
                Player.NumOfSeigeWeapons += 1;
            }
            if (seigeWeapon.Id == "BAL")
            {
                Player.Power += 100;
                Player.NumOfSeigeWeapons += 1;
            }
        }

        #endregion

        #region Movement methods

        /// <summary>
        /// update the accessible location
        /// </summary>
        private void UpdateAccessibleLocations()
        {
            _accessobleLocations.Clear();

            foreach (Location location in _gameMap.Locations)
            {
                if (location.IsAccessible == true)
                {
                    _accessobleLocations.Add(location);
                }
            }

            _accessobleLocations.Remove(_accessobleLocations.FirstOrDefault(l => l == _currentLocation));

            OnPropertyChange(nameof(AccessibleLocations));
        }

        /// <summary>
        /// timer that is used when traveling
        /// </summary>
        //public void Timer()
        //{
        //    CurrentLocationInformation = "Your Army is on the march.";
        //    Task.Delay(3000).ContinueWith(_ =>
        //    {

        //    }
        //    );
        //}

        /// <summary>
        /// calculate damage if player is attacked on the road
        /// </summary>
        public void AttackedOnTheRoad()
        {
            int r = random.Next(1, 6);
            int deduction = 0;

            if (r == 1 || r == 3 || r == 5)
            {
                if (Player.Power < 500)
                {
                    deduction = random.Next(50, 70);
                    Player.LegionnaireNumbers -= deduction;
                    Player.Power -= deduction;
                    CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine + "You have been attacked on the road by a band of Bloodskull " +
                    "Raiders. You have beaten them back for now, but they may return." + Environment.NewLine +
                    $"You have lost {deduction} Legionnaire.";
                }
                else if (Player.Power > 500)
                {
                    deduction = random.Next(20, 40);
                    Player.LegionnaireNumbers -= deduction;
                    Player.Power -= deduction;
                    CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine + "You have been attacked on the road by a band of Bloodskull " +
                    "Raiders. You have beaten them back for now, but they may return." + Environment.NewLine +
                    $"You have lost {deduction} Legionnaire.";
                }
            }
            else
            {

            }
        }

        public void Move(string tagName)
        {
            switch (tagName)
            {
                case "Alheimurrinn":
                    if (Player.NumOfSeigeWeapons > 5 && Player.PraetorNumbers == 1 && Player.PraetorianNumbers == 5 && Player.CenturionNumbers == 15)
                    {
                        _currentLocation.IsAccessible = true;
                        foreach (Location location in AccessibleLocations)
                        {
                            if (tagName == location.Name)
                            {
                                CurrentLocation = location;
                            }
                        }
                    }
                    else
                    {
                        _currentLocation.IsAccessible = false;
                        CurrentLocationInformation = "You lack the military might to travel to this location";
                    }

                    break;

                case "Qua Redi":
                    if (Player.NumOfSeigeWeapons > 3 && Player.CenturionNumbers > 3)
                    {
                        _currentLocation.IsAccessible = true;
                        foreach (Location location in AccessibleLocations)
                        {
                            if (tagName == location.Name)
                            {
                                CurrentLocation = location;
                            }
                        }
                    }
                    else
                    {
                        _currentLocation.IsAccessible = false;
                        CurrentLocationInformation = "Secure the port of Elkmire, acquire more seige weaponry and stronger troops to travel to this location.";
                    }
                    break;

                case "Dore":
                    if (Player.ElkmireIsDefeated && Player.PlayerHasShips == true)
                    {
                        _currentLocation.IsAccessible = true;
                        foreach (Location location in AccessibleLocations)
                        {
                            if (tagName == location.Name)
                            {
                                CurrentLocation = location;
                            }
                        }
                    }
                    else
                    {
                        _currentLocation.IsAccessible = false;
                        CurrentLocationInformation = "You must secure the port of Elkmire and acquire ships " +
                            "before you can travel to Dore.";
                    }
                    break;

                case "North Bourg":
                    foreach (Location location in AccessibleLocations)
                    {
                        if (tagName == location.Name)
                        {
                            CurrentLocation = location;
                            AttackedOnTheRoad();
                        }
                    }
                    break;

                case "South Bourg":
                    if (Player.NorthBourgIsDefeated == true)
                    {
                        _currentLocation.IsAccessible = true;
                        foreach (Location location in AccessibleLocations)
                        {
                            if (tagName == location.Name)
                            {
                                CurrentLocation = location;
                                AttackedOnTheRoad();
                            }
                        }
                    }
                    else
                    {
                        _currentLocation.IsAccessible = false;
                        CurrentLocationInformation = "You must defeat the warlords of North Bourg before you can " +
                            "march your army to South Bourg";
                    }
                    break;

                case "Elkmire":
                    foreach (Location location in AccessibleLocations)
                    {
                        if (tagName == location.Name)
                        {
                            CurrentLocation = location;
                            AttackedOnTheRoad();
                        }
                    }
                    break;
                default:
                    break;
            }

            UpdateAccessibleLocations();
        }

        #endregion

        #region Button methods

        /// <summary>
        /// Opens barracks window
        /// </summary>
        public void ShowBarracks()
        {
            BarracksView barracksView = new BarracksView(_player);
            barracksView.Show();
        }

        /// <summary>
        /// opens recruitment window
        /// </summary>
        public void DisplayRecruitment()
        {
            RecruitWindow recruitWindow = new RecruitWindow(_player);
            recruitWindow.ShowDialog();
        }

        /// <summary>
        /// opens information window
        /// </summary>
        public void ShowInfo()
        {
            InformationView info = new InformationView();
            info.Show();
        }

        /// <summary>
        /// handle the speak to event in the view
        /// </summary>
        public void OnPlayerTalkTo()
        {
            if (CurrentNPC != null && CurrentNPC is ISpeak)
            {
                ISpeak speakingNpc = CurrentNPC as ISpeak;
                CurrentLocationInformation = speakingNpc.Speak();
                CheckIfNPCHasItem(_currentNPC);
            }
        }

        /// <summary>
        /// handle attack in the view
        /// </summary>
        public void OnPlayerAttack()
        {
            _player.BattleMode = BattleModeName.ATTACK;
            Battle();
        }

        /// <summary>
        /// handle retreat in the view
        /// </summary>
        public void OnPlayerRetreat()
        {
            _player.BattleMode = BattleModeName.RETREAT;
            Battle();
        }

        #endregion

        #region Battle methods

        /// <summary>
        /// calculates player's hitpoints
        /// </summary>
        private int CalculatePlayerHitPoints()
        {
            int hitPoints = 0;

            switch (_player.BattleMode)
            {
                case BattleModeName.ATTACK:
                    hitPoints = _player.Power;
                    break;
                //case BattleModeName.RETREAT:
                //    hitPoints = _player.Retreat();
                //    break;
                default:
                    break;
            }

            return hitPoints;
        }

        private int CalculateNPCHitPoints(IBattle battleNPC)
        {
            int battleNPCHitPoints = 0;

            //switch (NPCBattleResponse())
            //{
            //    case BattleModeName.ATTACK:
            //        battleNPCHitPoints = battleNPC.AttackCalc();
            //        break;
            //    case BattleModeName.RETREAT:
            //        battleNPCHitPoints = battleNPC.Retreat();
            //        break;
            //    default:
            //        break;
            //}

            if (_currentNPC.Rank == 1)
            {
                battleNPCHitPoints = random.Next(300, 499);
            }
            else if (_currentNPC.Rank == 2)
            {
                battleNPCHitPoints = random.Next(500, 749);
            }
            else if (_currentNPC.Rank == 3)
            {
                battleNPCHitPoints = random.Next(750, 999);
            }
            else if (_currentNPC.Rank == 4)
            {
                battleNPCHitPoints = random.Next(1000, 1249);
            }
            else if (_currentNPC.Rank == 5)
            {
                battleNPCHitPoints = random.Next(1249, 1500);
            }
            else
            {
                battleNPCHitPoints = 0;
            }

            return battleNPCHitPoints;
        }

        /// <summary>
        /// Rolls a die to determine npc's response
        /// </summary>
        //private BattleModeName NPCBattleResponse()
        //{
        //    BattleModeName npcBattleResponse = BattleModeName.ATTACK;

        //    switch (DieRoll(6))
        //    {
        //        case 1:
        //            npcBattleResponse = BattleModeName.ATTACK;
        //            break;
        //        case 2:
        //            npcBattleResponse = BattleModeName.ATTACK;
        //            break;
        //        case 3:
        //            npcBattleResponse = BattleModeName.ATTACK;
        //            break;
        //        case 4:
        //            npcBattleResponse = BattleModeName.ATTACK;
        //            break;
        //        case 5:
        //            npcBattleResponse = BattleModeName.ATTACK;
        //            break;
        //        case 6:
        //            npcBattleResponse = BattleModeName.ATTACK;
        //            break;
        //    }

        //    return npcBattleResponse;
        //}

        /// <summary>
        /// battle method
        /// </summary>
        private void Battle()
        {
            if (_currentNPC is IBattle)
            {
                IBattle battleNPC = _currentNPC as IBattle;
                int playerHitPoints = 0;
                int playerTacticalAdvantage = _player.TacticalAdvantage;
                int weightedPlayerHP = 0;
                //int weightedNPCHP = 1;
                int battleNpcHitPoints = 0;
                playerHitPoints = CalculatePlayerHitPoints();
                battleNpcHitPoints = CalculateNPCHitPoints(battleNPC);
                //int NPCPower = battleNpcHitPoints;

                //checks if the stranger is defeated, if so 50 power is subtracted from Dore
                if (_currentNPC.ID == "ENEMY2" && Player.strangerIsDefeated == true)
                {
                    battleNpcHitPoints -= 50;
                }

                //checks if the false merchant is defeated, if so 100 power is subtracted from Qua Redi
                if (_currentNPC.ID == "ENEMY3" && Player.FalseMerchantIsDefeated == true)
                {
                    battleNpcHitPoints -= 100;
                }

                //calculate weighted player power (power divided by tactical advantage)
                if (_currentNPC.Rank == 1)
                {
                    weightedPlayerHP = playerHitPoints / 1;
                }
                else if (_currentNPC.Rank == 2)
                {
                    weightedPlayerHP = playerHitPoints / random.Next(1, 2);
                }
                else if (_currentNPC.Rank == 3)
                {
                    weightedPlayerHP = playerHitPoints / random.Next(2, 3);
                }
                else if (_currentNPC.Rank == 4)
                {
                    weightedPlayerHP = playerHitPoints / random.Next(3, 4);
                }
                else if (_currentNPC.Rank == 5)
                {
                    weightedPlayerHP = playerHitPoints / 5;
                }

                //calculate weighted NPC power
                //weightedNPCHP = NPCPower / playerTacticalAdvantage;


                if (weightedPlayerHP >= (battleNpcHitPoints / playerTacticalAdvantage))
                {
                    DetermineRewards(_currentNPC);
                    _currentLocation.NPCS.Remove(_currentNPC);
                }
                else
                {
                    DetermineLosses(_currentNPC);
                }

            }
            else
            {
                CurrentLocationInformation = "This person cannot be attacked.";
            }
        }

        /// <summary>
        /// Determine rewards for winning
        /// </summary>
        private void DetermineRewards(NPC currentNPC)
        {
            int troopLosses = 0;

            //if location is North Bourg
            if (currentNPC.ID == "ENEMY4")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity += 100;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }
                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(15, 25);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have won and have gained 100 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";

                Player.NorthBourgIsDefeated = true;
                CheckIfAllEnemiesDefeated();
            }

            //if location is south bourg
            else if (currentNPC.ID == "ENEMY5")
            {

                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity += 300;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(20, 35);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                GameItemQuantity gem = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GEM");
                if (gem != null)
                {
                    gem.Quantity += 1;
                    _player.Gems.Clear();
                    _player.Gems.Add(gem);
                    CheckGemInventory();
                }

                CurrentLocationInformation = "You have won and have gained 300 gold!" + Environment.NewLine +
                    "It also looks like your troops found a HELLFIRE GEM!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";

                Player.SouthBourgIsDefeated = true;
                CheckIfAllEnemiesDefeated();
            }

            //if location is elkmire
            else if (currentNPC.ID == "ENEMY6")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity += 550;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(25, 40);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                GameItemQuantity sword = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "SWD");
                if (sword != null)
                {
                    sword.Quantity += 1;
                    _player.ArmorSet.Add(sword);
                    CheckArmorSet();
                }

                CurrentLocationInformation = "You have won and have gained 550 gold!" + Environment.NewLine +
                    "We are also pleased to imform you that the legendary SWORD OF KING MIDAS was found in the king's private quarters." + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";

                Player.ElkmireIsDefeated = true;
                CheckIfAllEnemiesDefeated();
            }

            //if location is dore
            else if (currentNPC.ID == "ENEMY2")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity += 700;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(55, 75);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                GameItemQuantity gem = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GEM");
                if (gem != null)
                {
                    gem.Quantity += 1;
                    _player.Gems.Clear();
                    _player.Gems.Add(gem);
                    CheckGemInventory();
                }


                CurrentLocationInformation = "You have won and have gained 700 gold!" + Environment.NewLine +
                    "A Hellfire Gem was found in Dore's treasure vaults!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";

                Player.DoreIsDefeated = true;
                CheckIfAllEnemiesDefeated();
            }

            //if location is qua redi
            else if (currentNPC.ID == "ENEMY3")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity = 1500;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                GameItemQuantity cuirass = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "CUR");
                if (cuirass != null)
                {
                    cuirass.Quantity += 1;
                    _player.ArmorSet.Add(cuirass);
                    CheckArmorSet();
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(100, 125);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have won and have gained 1500 gold!" + Environment.NewLine +
                    "The cuirass of King Midas was also found in the treasure vaults." + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";

                Player.QuaRediIsDefeated = true;
                CheckIfAllEnemiesDefeated();
            }

            //if locaion is alheimurrinn
            else if (currentNPC.ID == "ENEMY1")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity += 10000;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(150, 225);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have won and have gained 10000 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";

                Player.AlheimurrinnIsDefeated = true;
                CheckIfAllEnemiesDefeated();

            }

            //if npc is the stranger
            else if (currentNPC.ID == "STRANGER")
            {
                Player.strangerIsDefeated = true;
                CheckIfAllSpiesDefeated();

                CurrentLocationInformation = "It would appear this man was indeed an informant for the Dornish nobles. " +
                    "Before he was executed we were able to extract some iformation on the Kingdom of Dore." + Environment.NewLine +
                    "" + Environment.NewLine +
                    "The Kingdom of Dore will lose 50 power.";
            }

            //if npc is the Wanderer
            else if (currentNPC.ID == "WANDERER")
            {
                _player.WandererIsDefeated = true;
                GameItemQuantity bracers = _player.ArmorInventory.FirstOrDefault(i => i.GameItem.Id == "BRA");
                if (bracers != null)
                {
                    bracers.Quantity += 1;
                    _player.ArmorSet.Add(bracers);
                    _player.Inventory.Add(bracers);
                    CheckArmorSet();
                }
                CheckIfAllSpiesDefeated();

                CurrentLocationInformation = "Your suspicions were correct. The women was a spy for the Kingdom of " +
                    "Alheimurrinn. Unfortunately, Alheimurrinn soldiers are fiercely loyal to their king and we" +
                    "could not extract any information from her. However, she did have something of value. The bracers " +
                    "she was wearing are in fact the bracers of King Midas!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "You have found the Bracers of King Midas.";
            }

            //if npc is the false merchant
            else if (currentNPC.ID == "FALSEMERCHANT")
            {
                _player.FalseMerchantIsDefeated = true;
                CheckIfAllSpiesDefeated();

                CurrentLocationInformation = "This elderly man was in fact a Qua Redian spy. We were able to extract some" +
                    "information from him that should give us an advantage when battling the Qua Redi Empire." + Environment.NewLine +
                    "" + Environment.NewLine +
                    "Qua Redi will lose 100 power.";
            }

            //if npc is the horseman
            else if (currentNPC.ID == "HORSEMAN")
            {
                _player.ThiefIsDefeated = true;
                _player.CavalryNumbers += 10;
                _player.Power += 50;

                CurrentLocationInformation = "This man was the thief that stole the emperor's crown! Had he made it over the " +
                    "mountains east of North Bourg, we probably never would have found him." + Environment.NewLine +
                    "The crown has been sent back to the emperor and you have been rewarded for your efforts." + Environment.NewLine +
                    "The emperor has personally purchased an auxiliary unit of cavalrymen for you." + Environment.NewLine +
                    "You have gained 10 cavalrymen." + Environment.NewLine +
                    "Your power has increased by 50.";

                CheckIfThiefIsDefeated();
            }

            //if npc is the trader
            else if (currentNPC.ID == "TRADER")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 200;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                CurrentLocationInformation = "unfortunately this woman was just a poor vegetable farmer. The sack on " +
                    "her waiste was seeds, not gold. We must pay restitution to the womans family" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "You have lost 200 gold.";
            }

            //if npc is the farmer
            else if (currentNPC.ID == "FARMER")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 100;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                CurrentLocationInformation = "unfortunately this man was just a simple farmer. " +
                    "We should pay restitution to the farmers family." + Environment.NewLine +
                    "" + Environment.NewLine +
                    "You have lost 100 gold.";
            }
        }

        /// <summary>
        /// Determines losses for player
        /// </summary>
        private void DetermineLosses(NPC currentNPC)
        {
            int troopLosses = 0;

            //if location is north bourg
            if (currentNPC.ID == "ENEMY4")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 100;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(90, 110);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have been defeated and have lost 100 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";
            }

            //if location is south bourg
            else if (currentNPC.ID == "ENEMY5")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 300;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(100, 120);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have been defeated and have lost 300 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";
            }

            //if location is elkmire
            else if (currentNPC.ID == "ENEMY6")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 550;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(120, 140);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have been defeated and have lost 550 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";
            }

            //if location is dore
            else if (currentNPC.ID == "ENEMY2")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 700;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(150, 200);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have been defeated and have lost 700 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";
            }

            //if location is qua redi
            else if (currentNPC.ID == "ENEMY3")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 1500;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(200, 250);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have been defeated and have lost 1500 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";
            }

            //if location is alheimurrinn
            else if (currentNPC.ID == "ENEMY1")
            {
                GameItemQuantity gameItemQuantity = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                if (gameItemQuantity != null)
                {
                    gameItemQuantity.Quantity -= 10000;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gameItemQuantity);
                }

                if (Player.LegionnaireNumbers > 50)
                {
                    troopLosses = random.Next(300, 400);
                    Player.LegionnaireNumbers -= troopLosses;
                    Player.Power -= troopLosses;
                }

                CurrentLocationInformation = "You have been defeated and have lost 10000 gold!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    $"Legionnaire lost in the battle: {troopLosses}";
            }
        }

        #endregion

        #region Mission Methods

        /// <summary>
        /// opens mission status view
        /// </summary>
        public void OpenMissionStatusView()
        {
            MissionStatusView missionStatusView = new MissionStatusView(InitializeMissionStatusViewModel());

            missionStatusView.Show();
        }

        /// <summary>
        /// call status info methods
        /// </summary>
        private MissionStatusViewModel InitializeMissionStatusViewModel()
        {
            MissionStatusViewModel missionStatusViewModel = new MissionStatusViewModel();

            missionStatusViewModel.MissionInformation = GenerateMissionStatusInformation();

            missionStatusViewModel.Missions = new List<Mission>(_player.Missions);
            foreach (Mission mission in missionStatusViewModel.Missions)
            {
                if (mission is SeigeMission)
                    mission.StatusDetail = GenerateSeigeMissionDetail((SeigeMission)mission);

                if (mission is CrownMission)
                    mission.StatusDetail = GenerateCrownMissionDetail((CrownMission)mission);

                if (mission is LocateMission)
                    mission.StatusDetail = GenerateLocateMissionDetail((LocateMission)mission);

                if (mission is AncientRelicMission)
                    mission.StatusDetail = GenerateMissionRelicDetail((AncientRelicMission)mission);

                if (mission is HellfireGemMission)
                    mission.StatusDetail = GenerateMissionHellfireGemDetail((HellfireGemMission)mission);
            }

            return missionStatusViewModel;
        }

        /// <summary>
        /// Mission status details
        /// </summary>
        private string GenerateMissionStatusInformation()
        {
            string missionStatusInformation;

            double totalMissions = _player.Missions.Count();
            double missionsCompleted = _player.Missions.Where(m => m.Status == Mission.MissionStatus.complete).Count();

            int percentMissionsCompleted = (int)((missionsCompleted / totalMissions) * 100);
            missionStatusInformation = $"Missions Complete: {percentMissionsCompleted}%" + NEW_LINE;

            if (percentMissionsCompleted == 0)
            {
                missionStatusInformation += "Your campaign has just begun!";
            }
            else if (percentMissionsCompleted <= 25)
            {
                missionStatusInformation += "You have made great progress, but there is still much to do.";
            }
            else if (percentMissionsCompleted <= 50)
            {
                missionStatusInformation += "Well done Imperator, our enemies are crumbling under the might of the Empire.";
            }
            else if (percentMissionsCompleted == 100)
            {
                missionStatusInformation += "Congratulations Imperator, you have finished your campaign and defeated all those who would oppose " +
                    "our great empire.";
            }

            return missionStatusInformation;
        }

        /// <summary>
        /// generate seige mission details
        /// </summary>
        private string GenerateSeigeMissionDetail(SeigeMission mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("Your campaign will be completed once" + Environment.NewLine + "all enemies have been destroyed.");

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }


        /// <summary>
        /// generate crown mission details
        /// </summary>
        private string GenerateCrownMissionDetail(CrownMission mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("Find the theif and return" + Environment.NewLine + "the emperor's crown!");

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

        /// <summary>
        /// generate locate mission details
        /// </summary>
        private string GenerateLocateMissionDetail(LocateMission mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("Locate and eliminate enemy spies.");

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

        /// <summary>
        /// ancient relic mission details
        /// </summary>
        private string GenerateMissionRelicDetail(AncientRelicMission mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            if (mission.Status == Mission.MissionStatus.incomplete)
            {
                sb.AppendLine("King Midas Armor Set (Quantity): ");
                foreach (var gameItemQuantity in mission.RequiredGameItemsNotCompleted(_player.Inventory.ToList()))
                {
                    int quantityInInventory = 0;
                    GameItemQuantity gameItemQuantityGatherered = _player.Inventory.FirstOrDefault(gi => gi.GameItem.Id == gameItemQuantity.GameItem.Id);
                    if (gameItemQuantityGatherered != null)
                        quantityInInventory = gameItemQuantityGatherered.Quantity;

                    sb.Append(TAB + gameItemQuantity.GameItem.Name);
                    sb.AppendLine($"  ( {gameItemQuantity.Quantity - quantityInInventory} )");
                }
            }

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString(); ;
        }

        /// <summary>
        /// ancient relic mission details
        /// </summary>
        private string GenerateMissionHellfireGemDetail(HellfireGemMission mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            if (mission.Status == Mission.MissionStatus.incomplete)
            {
                sb.AppendLine("Hellfire Gems yet to find (Quantity): ");
                foreach (var gameItemQuantity in mission.RequiredGameItemsNotCompleted(_player.Inventory.ToList()))
                {
                    int quantityInInventory = 0;
                    GameItemQuantity gameItemQuantityGatherered = _player.Inventory.FirstOrDefault(gi => gi.GameItem.Id == gameItemQuantity.GameItem.Id);
                    if (gameItemQuantityGatherered != null)
                        quantityInInventory = gameItemQuantityGatherered.Quantity;

                    sb.Append(TAB + gameItemQuantity.GameItem.Name);
                    sb.AppendLine($"  ( {gameItemQuantity.Quantity - quantityInInventory} )");
                }
            }

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString(); ;
        }

        /// <summary>
        /// checks to see if all enemies have been defeated
        /// </summary>
        private void CheckIfAllEnemiesDefeated()
        {
            if (Player.NorthBourgIsDefeated == true && Player.SouthBourgIsDefeated == true && Player.ElkmireIsDefeated == true
                && Player.DoreIsDefeated == true && Player.QuaRediIsDefeated == true && Player.AlheimurrinnIsDefeated == true)
            {
                foreach (Mission mission in _player.Missions)
                {
                    if (mission is SeigeMission)
                    {
                        SeigeMission seigeMission = (SeigeMission)mission;
                        seigeMission.AllEnemiesDefeated = true;
                        CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine +
                       "You have defeated all enemy nations and seized Mundas for the Empire!";
                        _player.UpdateMissionStatus();
                    }
                }
            }
        }

        /// <summary>
        /// checks if all spies are defeated
        /// </summary>
        public void CheckIfAllSpiesDefeated()
        {
            if (_player.WandererIsDefeated == true && _player.strangerIsDefeated == true && _player.FalseMerchantIsDefeated == true)
            {
                foreach (Mission mission in _player.Missions)
                {
                    if (mission is LocateMission)
                    {
                        LocateMission locateMission = (LocateMission)mission;
                        locateMission.AllSpiesDefeated = true;
                        GameItemQuantity gold = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                        gold.Quantity += 300;
                        _player.Treasures.Clear();
                        _player.Treasures.Add(gold);
                        CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine +
                       "You have found and eliminated all enemy spies!" + Environment.NewLine +
                       "You have gained 300 gold.";
                        _player.UpdateMissionStatus();
                    }

                }
            }
        }

        /// <summary>
        /// checks if the thief is defeated
        /// </summary>
        public void CheckIfThiefIsDefeated()
        {
            if (_player.ThiefIsDefeated == true)
            {
                foreach (Mission mission in _player.Missions)
                {
                    if (mission is CrownMission)
                    {
                        CrownMission crownMission = (CrownMission)mission;
                        crownMission.ThiefIsCaptured = true;
                        _player.UpdateMissionStatus();
                    }
                }
            }
        }

        /// <summary>
        /// checks if the npc you are speaking to has any important items
        /// </summary>
        public void CheckIfNPCHasItem(NPC currentNPC)
        {
            if (currentNPC.ID == "NOBLE")
            {
                CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine + "The nobleman slipped you " +
                    "a small piece of parchment. Upon reading it you discover that he is actually fleeing Qua Redi and heading to the Aquila " +
                    "Empire. As a token of good faith, the parchment alos explains that a nearby ancient ruin may contain valuable treaure. " +
                    "You take a clutch of men and explore the ruins. The treasure you find inside was a magnificent HELLFIRE GEM.";

                GameItemQuantity gem = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GEM");
                if (gem != null)
                {
                    gem.Quantity += 1;
                    _player.Gems.Clear();
                    _player.Gems.Add(gem);
                    CheckGemInventory();
                }

                _currentLocation.NPCS.Remove(currentNPC);
            }

            if (currentNPC.ID == "ENEMYCOURIER")
            {
                CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine + "An honorable Imperator " +
                    "does not execute couriers, but you do confiscate his sachel to gain intel on the enemy. You find no intel, " +
                    "but at the bottom of the sachel you can see the fiery glow of a HELLFIRE GEM. After further questioning, you discover that the " +
                    "courier was taking the gem to a distant land far beyond the eastern mountains. Who sent the gem and why are still unkown";

                GameItemQuantity gem = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GEM");
                if (gem != null)
                {
                    gem.Quantity += 1;
                    _player.Gems.Clear();
                    _player.Gems.Add(gem);
                    CheckGemInventory();
                }

                _currentLocation.NPCS.Remove(currentNPC);
            }

            if (currentNPC.ID == "WORSHIPPER" && _player.NorthBourgIsDefeated == true)
            {
                CurrentLocationInformation += "Thank you for driving back those heathens. They have desecrated our temple for the last time. " + Environment.NewLine + "" + Environment.NewLine +
                    "For saving their temple, the worshippers have giften you with one of their holy objects." + Environment.NewLine +
                    "You have been given a HELLFIRE GEM.";

                GameItemQuantity gem = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GEM");
                if (gem != null)
                {
                    gem.Quantity += 1;
                    _player.Gems.Clear();
                    _player.Gems.Add(gem);
                    CheckGemInventory();
                }

                _currentLocation.NPCS.Remove(currentNPC);
            }

            if (currentNPC.ID == "PILGRIMS" && _player.SouthBourgIsDefeated == true)
            {
                CurrentLocationInformation += "You have saved us from those oppresive warlords. We cannot thank you enough" + Environment.NewLine + "" + Environment.NewLine +
                    "As a token of gratitude, the pilgrims have given you a helmet that was discovered in a cave sometime ago. You immediately realize that " +
                    "the helmet is the legendary helmet of King Midas.";

                GameItemQuantity helm = _player.ArmorInventory.FirstOrDefault(i => i.GameItem.Id == "HEL");
                if (helm != null)
                {
                    helm.Quantity += 1;
                    _player.Inventory.Add(helm);
                    _player.ArmorSet.Add(helm);
                    CheckArmorSet();
                }

                _currentLocation.NPCS.Remove(currentNPC);
            }

            if (currentNPC.ID == "SCOUT")
            {
                CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine + "The scout lays his backpack at your feet. " +
                    "You open it to find a set of greaves that once belonged to the mighty King Midas. The scout found the greeves while hiding from " +
                    "enemy forces in a dark cave. You assure the scout that he will be rewarded handsomely for his troubles.";

                GameItemQuantity greaves = _player.ArmorInventory.FirstOrDefault(i => i.GameItem.Id == "BOO");
                if (greaves != null)
                {
                    greaves.Quantity += 1;
                    _player.Inventory.Add(greaves);
                    _player.ArmorSet.Add(greaves);
                    CheckArmorSet();
                }

                _currentLocation.NPCS.Remove(currentNPC);
            }

            if (currentNPC.ID == "HAMLET")
            {
                if (_player.ElkmireIsDefeated == true)
                {
                    CurrentLocationInformation = "This small hamlet burned down during your conflict with Elkmire.";
                }
                else if (_player.ElkmireIsDefeated == false)
                {
                    GameItemQuantity gold = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                    gold.Quantity += 50;
                    _player.Treasures.Clear();
                    _player.Treasures.Add(gold);

                    CurrentLocationInformation = "The village elder approaches you and informs you that the villagers at " +
                        "this hamlet want nothing to do with the war. They offer you a small sum of gold and 3 barrels of ale " +
                        "if you decide to place your encampent elsewhere. You agree and march your legion to the western side of " +
                        "Elkmire castle to set up camp." + Environment.NewLine + "" + Environment.NewLine + 
                        "You gained 50 gold.";
                }
            }
        }

        /// <summary>
        /// check inventory for gems
        /// </summary>
        public void CheckGemInventory()
        {
            GameItemQuantity gem = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GEM");
            if (gem.Quantity == 5)
            {
                _player.HasAllGems = true;
                foreach (Mission mission in _player.Missions)
                {
                    if (mission is HellfireGemMission)
                    {
                        HellfireGemMission gemMission = (HellfireGemMission)mission;
                        gemMission.HasAllGems = true;
                        GameItemQuantity gold = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                        gold.Quantity += 500;
                        _player.Treasures.Clear();
                        _player.Treasures.Add(gold);
                        CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine + 
                            "You have found all Hellfire Gems!" + Environment.NewLine + 
                            "You have gained 500 gold.";
                        _player.UpdateMissionStatus();
                    }
                }
            }
        }

        /// <summary>
        /// check for full armor set
        /// </summary>
        public void CheckArmorSet()
        {
            GameItemQuantity sword = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "SWD");
            GameItemQuantity helm = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "HEL");
            GameItemQuantity bracers = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "BRA");
            GameItemQuantity greaves = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "BOO");
            GameItemQuantity cuirass = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "CUR");

            if (sword != null && helm != null && bracers != null && greaves != null && cuirass != null)
            {
                if (sword.Quantity == 1 && helm.Quantity == 1 && bracers.Quantity == 1 && greaves.Quantity == 1 && cuirass.Quantity == 1)
                {
                    _player.HasArmorSet = true;
                    foreach (Mission mission in _player.Missions)
                    {
                        if (mission is AncientRelicMission)
                        {
                            AncientRelicMission relicMission = (AncientRelicMission)mission;
                            relicMission.HasArmorSet = true;
                            GameItemQuantity gold = _player.Inventory.FirstOrDefault(i => i.GameItem.Id == "GLD");
                            gold.Quantity += 500;
                            _player.Treasures.Clear();
                            _player.Treasures.Add(gold);
                            _player.Power += 100;
                            CurrentLocationInformation += "" + Environment.NewLine + "" + Environment.NewLine +
                           "You have found all pieces of King Midas' armor set! Now that you are wearing the legendart " +
                           "armor, your troops will be inspired to fight harder for you." + Environment.NewLine +
                           "You have gained 500 gold." + Environment.NewLine +
                           "Power inreased by 100.";
                            _player.UpdateMissionStatus();
                        }
                    }
                }
            }

        }

        #endregion

        #region Helper Methods

        private int DieRoll(int sides)
        {
            return random.Next(1, sides + 1);
        }

        #endregion

        #endregion
    }
}
