using VVR.Visuals;

namespace VVR
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Car no longer goes vroom:(((");

            ImageGenerating imageGenerating = new ImageGenerating();
            imageGenerating.GenerateFixedTrack();
        }
    }
}
