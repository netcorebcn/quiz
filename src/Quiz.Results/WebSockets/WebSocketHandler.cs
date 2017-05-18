using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Quiz.Results
{
    public class WebSocketHandler
    {
        private readonly WebSocketConnectionManager _webSocketConnectionManager;

        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager) =>
            _webSocketConnectionManager = webSocketConnectionManager;

        public async Task SendMessageToAllAsync(object message) =>
            await Task.WhenAll(
                _webSocketConnectionManager.GetAll()
                    .Where(pair => pair.Value.State == WebSocketState.Open)
                    .Select(pair => SendMessageAsync(pair.Value, message)));
                    
        public async Task OnConnected(WebSocket socket)
        {
            _webSocketConnectionManager.AddSocket(socket);
            await SendMessageAsync(socket, $"Connected with Id: ${_webSocketConnectionManager.GetId(socket)}");
        }

        public async Task OnDisconnected(WebSocket socket) =>
            await _webSocketConnectionManager.RemoveSocket(_webSocketConnectionManager.GetId(socket));

        private async Task SendMessageAsync(WebSocket socket, object message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var serializedMessage = JsonConvert.SerializeObject(message, _jsonSerializerSettings);
            await socket.SendAsync(buffer: new ArraySegment<byte>(
                    array: Encoding.ASCII.GetBytes(serializedMessage),offset: 0,
                    count: serializedMessage.Length),
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None);
        }
    }
}