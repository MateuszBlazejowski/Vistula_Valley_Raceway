namespace VVR.Locations;

internal static class TrackParser
{
    /// <summary>
    /// Adds a track component to the track
    /// </summary>
    /// <param name="filepath">path to the component</param>
    /// <param name="storage">track to which the component will be appended</param>
    public static void ParseCSV(string filepath, List<TrackPiece> storage)
    {
        StreamReader reader = new StreamReader(filepath);
        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();
            if (line == null) continue;
            string[] values = line.Split('.');
            TrackPiece tp = new TrackPiece(Int32.Parse(values[0]), Int32.Parse(values[1]), values[2][0], values[3][0]);
            storage.Add(tp);
        }
    }
}