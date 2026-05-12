namespace OpenTKProject
{
    public class LightManager
    {
        private List<LightModule> _lights = new List<LightModule>();
        private const int MAX_LIGHTS = 10;

        public void AddLight(LightModule light)
        {
            if (_lights.Count < MAX_LIGHTS)
                _lights.Add(light);
        }

        public void AddLights(LightModule[] light)
        {
            if (_lights.Count < MAX_LIGHTS)
                _lights.AddRange(light);
        }

        public void RemoveLight(LightModule light)
        {
            _lights.Remove(light);
        }

        public List<LightModule> GetLights() => _lights;
    }
}