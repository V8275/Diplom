namespace WinFormsOpenTK
{
    public class DbModel
    {
        public string PresetName { get; set; }
        public string ModelPath { get; set; }
        public string? TexturePath { get; set; }
        public string? NormalMapPath { get; set; }
        public string? MetallicMapPath { get; set; }
        public string? RoughnessMapPath { get; set; }
        public string ModelFormat { get; set; }
    }
}
