using Microsoft.EntityFrameworkCore;
using nookenApp.Helper;
using nookenApp.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace nookenApp.Views
{
    public partial class Gate : Window
    {
        private GateViewModel _viewModel;
        private static DbContextOptions<AppDbContext>? _optionsBuilder;
        private AppDbContext _dbContext;

        public Gate()
        {
            InitializeComponent();
            //Loaded += GateWindow_Loaded;
            InitializeDatabaseAsync();
            DataContext = new GateViewModel(_dbContext);
        }

        private async void GateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeDatabaseAsync();
            // No need to re-initialize the ViewModel here as it's already set in the constructor.
            // Any additional setup can be done here if required.
        }
        
        private void InitializeDatabaseAsync()
        {
            //DatabaseHelper dbHelper = new DatabaseHelper();
            //await dbHelper.InitializeDatabaseAsync();

            _optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(Settings.ConnectionString)
                .Options;

            _dbContext = new AppDbContext(_optionsBuilder);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _viewModel?.Dispose();
        }
    }
}
