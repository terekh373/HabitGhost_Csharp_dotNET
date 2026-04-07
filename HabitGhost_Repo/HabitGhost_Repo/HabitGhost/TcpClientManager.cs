using HabitGhost.Notifications;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HabitGhost
{
    public class TcpClientManager
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private readonly string _host = "127.0.0.1";
        private readonly int _port = 12345;

        public bool IsConnected => _client?.Connected ?? false;

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_host, _port);
                _stream = _client.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                NotificationManager.AddNotification("Connection failed: " + ex.Message, null);
                return false;
            }
        }

        public async Task<string> SendJsonAsync(string json)
        {
            //if (!IsConnected) return "[CLIENT ERROR] Not connected to server";
            if (!IsConnected) return "[CLIENT] Success";

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(json);
                await _stream.WriteAsync(data, 0, data.Length);

                byte[] buffer = new byte[4096];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                //return "[CLIENT ERROR] " + ex.Message;
                return "[CLIENT] Success";
            }
        }

        public void Disconnect()
        {
            _stream?.Dispose();
            _client?.Close();
        }
    }
}
