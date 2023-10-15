using System.IO.Ports;
using System.Text;
using Iot.Device.MT3333;

string VersionValue = "0.3.0-bata";

string CommandHotStart = "$PMTK101*32\r\n";
//string CommandWarmStart = "$PMTK102*31\r\n";
//string CommandSetGnssSearchMode = "$PMTK353,1,0,1,0,0*2B\r\n";
string CommandSetGnssSearchMode = "$PMTK353,1,1,1,0,0*2A\r\n";
string CommandSetSupportQzss    = "$PMTK351,1*28\r\n";
//string CommandSetNmeaOutput     = "$PMTK314,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0*28\r\n";
string CommandSetNmeaOutput     = "$PMTK314,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0*29\r\n";
//string CommandSetNmeaBaudrate   = "$PMTK251,9600*17\r\n";

// string[] args
if (args.Length == 0)
{
    Console.WriteLine("Please enter a string argument.");
    Console.WriteLine("Usage: SetGpsParam /dev/ttyAMA<num>");
    Console.WriteLine("Usage: SetGpsParam --version, -V");
    return;
}

string args0Value = args[0];
if (args0Value == "--version" || args0Value == "-V")
{
    Console.WriteLine("Version: {0}", VersionValue);
    return;
}

using SerialPort serialPort = new SerialPort(args0Value, 9600, Parity.None, 8, StopBits.One)
{
    Encoding = Encoding.ASCII,
    ReadTimeout = 1000,
    WriteTimeout = 1000
};

try
{
    serialPort.Open();
    using MT3333 gps = new(serialPort.BaseStream, true);

    Thread.Sleep(1000);

    gps.SendRequest(CommandSetGnssSearchMode);
    Console.WriteLine("Command:SetGnssSearchMode");
    Thread.Sleep(1000);
    gps.SendRequest(CommandSetSupportQzss);
    Console.WriteLine("Command:SetSupportQzss");
    Thread.Sleep(1000);
    gps.SendRequest(CommandSetNmeaOutput);
    Console.WriteLine("Command:SetNmeaOutput");
    Thread.Sleep(1000);
    gps.SendRequest(CommandHotStart);
    Console.WriteLine("Command:CommandHotStart");

    Thread.Sleep(1000);

}
catch (IOException e)
{
    Console.WriteLine("GPS couldn't be read");
    Console.WriteLine(e.Message);
    Console.WriteLine(e.InnerException?.Message);
}
catch (Exception e)
{
    Console.WriteLine("SYSTEM couldn't be response");
    Console.WriteLine(e.Message);
    Console.WriteLine(e.InnerException?.Message);
}
