using Urho;

namespace RenderCube.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new MyGame(new ApplicationOptions("Data")).Run();
            }
            catch (System.Exception e) { }
        }
    }
}
