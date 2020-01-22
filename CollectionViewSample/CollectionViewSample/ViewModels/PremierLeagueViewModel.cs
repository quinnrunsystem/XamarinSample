using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CollectionViewSample.Models;
using Xamarin.Forms;

namespace CollectionViewSample.ViewModels
{
    public class PremierLeagueViewModel:INotifyPropertyChanged
    {

        public PremierLeagueViewModel()
        {
            CreateClubFootballCollection();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        readonly IList<ClubFootball> _clubFootballs;
        ClubFootball _selectedClubFootball;
        int _selectionCount = 1;

        public ObservableCollection<ClubFootball> ClubFootballs { get; private set; }
        public IList<ClubFootball> EmptyClubFootballs { get; private set; }

        public ClubFootball SelectedClubFootball
        {
            get { return _selectedClubFootball; }
            set {_selectedClubFootball = value; }
        }

        ObservableCollection<ClubFootball> _selectedClubFootballs;
        public ObservableCollection<ClubFootball> SelectedClubFootballs
        {
            get { return _selectedClubFootballs; }
            set { _selectedClubFootballs = value; }
        }

        public string SelectedClubFootballMessgae { get; private set; }

        public ICommand DeleteCommand = new Command<ClubFootball>(RemoveClubFootball);
        public ICommand FavoriteCommand = new Command<ClubFootball>(FavoriteClubFootball);

      

        public ICommand FilterCommand = new Command<ClubFootball>(RemoveClubFootball);
        public ICommand MonkeySelectionChangedCommand = new Command<ClubFootball>(RemoveClubFootball);
       

        private static void RemoveClubFootball(ClubFootball obj)
        {
            //throw new NotImplementedException();
        }

        private static void FavoriteClubFootball(ClubFootball obj)
        {
           // throw new NotImplementedException();
        }

        void CreateClubFootballCollection()
        {
            _clubFootballs = new List<ClubFootball>();

            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Liverpool",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Man City",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Leicester City",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Chelsea",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Man Utd",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Wolves",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Sheffield Utd",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Tottenham",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Southampton",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Arsenal",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Crystal Palace",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Everton",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Newcastle",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Burnley",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Brighton",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Aston Villa",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "WestHam Utd",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Bournemouth",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
            _clubFootballs.Add(new ClubFootball()
            {
                Name = "Watford",
                Logo = "liverpool",
                StadiumName = "Anfield"
            });
        }
    }
}
