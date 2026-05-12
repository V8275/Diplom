namespace WinFormsOpenTK
{
    public class DataLoader
    {
        private static string dbPath = "Data Source=ModelViewerDB.db";

        List<DbModel> models = new List<DbModel>();

        private AppDbContext _context = new AppDbContext(dbPath);

        public void LoadData()
        {
            _context.Database.EnsureCreated();

            models = _context.ModelsData.ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void LoadToDataGridView(DataGridView dataGridView)
        {
            _context.Database.EnsureCreated();
            dataGridView.DataSource = _context.ModelsData.Local.ToBindingList();
        }

        public bool AddModel(DbModel model)
        {
            bool exists = _context.ModelsData.Any(m => m.PresetName == model.PresetName);

            if (exists)
            {
                return false;
            }

            _context.ModelsData.Add(model);
            _context.SaveChanges();

            models.Add(model);

            return true;
        }

        public bool AddModel(
            string presetName,
            string modelPath,
            string texturePath,
            string modelFormat,
            string normalPath = null,
            string metallicPath = null,
            string roughnessPath = null)
        {
            var model = new DbModel();
            model.PresetName = presetName;
            model.ModelPath = modelPath;
            model.TexturePath = texturePath;
            model.ModelFormat = modelFormat;
            model.NormalMapPath = normalPath;
            model.MetallicMapPath = metallicPath;
            model.RoughnessMapPath = roughnessPath;

            return AddModel(model);
        }

        public bool DeleteSelected(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count == 0)
                return false;

            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                if (row.DataBoundItem is DbModel model)
                {
                    _context.ModelsData.Remove(model);
                }
            }

            _context.SaveChanges();
            return true;
        }

        public List<DbModel> GetLoadedData()
        {
            return models.ToList();
        }
    }
}
