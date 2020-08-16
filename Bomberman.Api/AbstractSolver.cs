using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Bomberman.Api
{
    public abstract class AbstractSolver
    {
        private const string ResponsePrefix = "board=";
        private const int MaxRetriesCount = 3;
        private const int RetriestTimeoutInMilliseconds = 10000;

        private readonly string _webSocketUrl;

        private int _retriesCount;

        private bool _shouldExit;

        private WebSocket _gameServer;

        protected AbstractSolver(string serverUrl)
        {
            _webSocketUrl = GetWebSocketUrl(serverUrl);
        }

        public void Play()
        {
            _gameServer = new WebSocket(_webSocketUrl);
            // _gameServer.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;

            _gameServer.OnMessage += Socket_OnMessage;
            _gameServer.OnClose += async (s, e) => await ReconnectAsync(e.WasClean, e.Code);

            _gameServer.Connect();
        }

        public void InitiateExit()
        {
            Console.WriteLine("Exit initiated...");

            _shouldExit = true;

            if (_gameServer.ReadyState == WebSocketState.Open)
            {
                _gameServer.Close();
            }
        }

        protected internal abstract string Get(Board gameBoard);

        protected internal string GetWebSocketUrl(string serverUrl) =>
            serverUrl.Replace("http", "ws")
                .Replace("board/player/", "ws?user=")
                .Replace("?code=", "&code=");

        private void Socket_OnMessage(object sender, MessageEventArgs e)
        {
            if (!_shouldExit)
            {
                var response = e.Data;
                _retriesCount = default;

                if (!response.StartsWith(ResponsePrefix))
                {
                    Console.WriteLine("Something strange is happening on the server... Response:\n{0}", response);
                    InitiateExit();
                }
                else
                {
                    var boardString = response.Substring(ResponsePrefix.Length);
                    var board = new Board(boardString);

                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(board.ToString());

                    var action = Get(board);

                    Console.WriteLine("Answer: " + action);
                    Console.SetCursorPosition(0, 0);

                    ((WebSocket)sender).Send(action);
                }
            }
        }

        private async Task ReconnectAsync(bool wasClean, ushort code)
        {
            if (!wasClean && !_gameServer.IsAlive && IsAllowedToReconnect(code))
            {
                if (_retriesCount < MaxRetriesCount)
                {
                    Console.WriteLine($"Trying to reconnect, attempt {_retriesCount + 1} of {MaxRetriesCount}...");
                    await Task.Delay(RetriestTimeoutInMilliseconds);

                    _retriesCount++;
                    _gameServer.Connect();
                }
                else
                {
                    Console.WriteLine("Could not reconnect to the server, please try again later. Press any key to exit...");
                }
            }
        }

        private bool IsAllowedToReconnect(ushort code)
        {
            var reconnectList = new List<ushort>
            {
                1006, // The connection was closed abnormally, e.g., without sending or receiving a Close control frame.
                1011 // A server is terminating the connection because it encountered an unexpected condition that prevented it from fulfilling the request.
            };

            return reconnectList.Contains(code);
        }
    }
}
