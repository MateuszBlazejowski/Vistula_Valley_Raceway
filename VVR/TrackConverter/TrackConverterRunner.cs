namespace VVR.TrackConverter
{
    public static class Program
    {

        private static string GetFullPath(string filepath)
        {
            string absolutePath = Path.GetFullPath(filepath);

            if (!File.Exists(absolutePath))
            {
                throw new FileNotFoundException($"file not found at path: {absolutePath}");
            }
            return absolutePath;
        }

        public static void RunTrackConverter(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify file paths to files containing tracks");
                throw new Exception("Too few arguments");
            }

            Parallel.ForEach(args, arg =>
            {
                string path = GetFullPath(arg);
                string filename = Path.GetFileName(arg).Split('.')[0];
                Console.WriteLine($"{filename}: {path}");
                Parser.ParseCSV(path, filename);
            });
        }
    }
}
