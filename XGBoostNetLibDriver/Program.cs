namespace XGBoostNetLibDriver;

using XGBoostNetLib;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Dealing with XGBoost version {XGBoostUtils.XgbMajorVersion()}.");
        Console.WriteLine("Done!");
    }
}
