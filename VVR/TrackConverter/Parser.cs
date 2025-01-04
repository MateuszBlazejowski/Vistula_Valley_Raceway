namespace Program;

public static class Parser
{
    public static void ParseCSV(string filepath, string filename)
    {
        var destinationPath = Path.Combine("..", "Locations", "TrackAssets", $"{filename}.track");

        using StreamReader sr = new StreamReader(filepath);
        using StreamWriter sw = new StreamWriter(destinationPath);
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if ( line == null ) continue;
            int left = 0;
            int right = 0;
            char leftChar = '\0', rightChar = '\0';
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    if (leftChar == '\0')
                    {
                        left++;
                    }
                    else right++;
                }
                else if (leftChar == '\0')
                {
                    leftChar = line[i];
                }
                else rightChar = line[i];
            }

            sw.WriteLine($"{left}.{right}.{leftChar}.{rightChar}");
        }
    }
}