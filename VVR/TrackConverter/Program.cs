namespace Program
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

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify file paths to files containing tracks");
                return;
            }

            Parallel.ForEach(args, arg =>
            {
                string path = GetFullPath(arg);
                string filename = Path.GetFileName(arg).Split('.')[0];
                Parser.ParseCSV(path, filename);
            });
        }
    }
}