using System.IO.Ports;
using System.Text;

namespace Iot.Device.MT3333
{
    public sealed class MT3333 : IDisposable
    {
        private bool _shouldDispose = false;
        private SerialPort? _serialPort;
        private Stream _serialPortStream;

        public MT3333(Stream stream, bool shouldDispose)
        {
            _serialPortStream = stream ?? throw new ArgumentNullException(nameof(stream));
            _shouldDispose = shouldDispose;
        }

        public MT3333(string uartDevice)
        {
            if (uartDevice is not { Length: > 0 })
            {
                throw new ArgumentException($"{nameof(uartDevice)} can't be null or empty.", nameof(uartDevice));
            }

            _serialPort = new SerialPort(uartDevice, 9600, Parity.None, 8, StopBits.One)
            {
                Encoding = Encoding.ASCII,
                ReadTimeout = 1000,
                WriteTimeout = 1000
            };

            _serialPort.Open();
            _serialPortStream = _serialPort.BaseStream;
            _shouldDispose = true;
        }

        public void SendRequest(string request)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(request);
            try
            {
                _serialPortStream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                throw new IOException("Sensor communication failed", e);
            }
        }

        /// <inheritdoc cref="IDisposable" />
        public void Dispose()
        {
            if (_shouldDispose)
            {
                _serialPortStream?.Dispose();
                _serialPortStream = null!;
            }

            if (_serialPort?.IsOpen ?? false)
            {
                _serialPort.Close();
            }

            _serialPort?.Dispose();
            _serialPort = null;
        }

    }
}
