using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProducts.Helpers;
using TestProducts.Models;
using TestProducts.Services;

namespace TestProducts.ViewModels
{
    public class SelectMaterialVM : BaseVM
    {
        private DataBaseService dataBaseService;
        private bool? dialogResult;
        public ObservableCollection<Materials> materialsList;
        public ObservableCollection<Materials> MaterialsList { get => materialsList; set { materialsList = value; OnPropertyChanged(); } }
        public Materials SelectedMaterial { get; set; }

        public SelectMaterialVM()
        {
            dataBaseService = new DataBaseService();
            MaterialsList = new ObservableCollection<Materials>(dataBaseService.GetMaterials());
        }
        private string amount;
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(); } }
        private RelayCommand select;
        public RelayCommand Select
        {
            get
            {
                return select ?? (select = new RelayCommand(obj =>
                {
                    Materials material = obj as Materials;
                    SelectedMaterial = material;
                    DialogResult = true;
                }));
            }
        }

        public bool? DialogResult { get => dialogResult; set { dialogResult = value; OnPropertyChanged(); } }
    }
}
