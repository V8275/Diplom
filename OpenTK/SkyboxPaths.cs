namespace WinFormsOpenTK
{
    public class SkyboxPaths
    {
        string right = "Models/Textures/Sky/SkyCloudsRight.jpg";
        string left = "Models/Textures/Sky/SkyCloudsLeft.jpg";
        string top = "Models/Textures/Sky/SkyCloudsTop.jpg";
        string bottom = "Models/Textures/Sky/SkyCloudsBottom.jpg";
        string front = "Models/Textures/Sky/SkyCloudsFront.jpg";
        string back = "Models/Textures/Sky/SkyCloudsBack.jpg";

        public string[] GetPaths()
        {
            return new string[]
            { right, left, top, bottom, front, back };
        }
    }
}
