using System.Threading;

namespace System.Security.Cryptography
{
    public class RoundRobinRNG
    {
        private Thread _t;
        private bool _stop;
        private RandomNumberGenerator _rng;
        private RandomNumberGenerator _rng2;
        private byte[] _buffer;
        private byte[] _buffer2;

        public RoundRobinRNG(int bufferSize)
        {
            _buffer = new byte[bufferSize];
            _buffer2 = new byte[bufferSize];
            _rng = RandomNumberGenerator.Create();
            _rng2 = RandomNumberGenerator.Create();
            _stop = false;
            _t = new Thread(() => {
                Start.Invoke();
                while (!_stop)
                {
                    _rng.GetBytes(_buffer);
                    _rng2.GetBytes(_buffer2);
                }
                Stop.Invoke();
            });
            _t.SetApartmentState(ApartmentState.STA);
            _t.Name = "CryptoRounds";
            _t.Priority = ThreadPriority.Normal;
            _t.Start();
        }

        public delegate void RoundRobinStarted();
        public delegate void RoundRobinStoped();
        public event RoundRobinStarted Start;
        public event RoundRobinStoped Stop;

        public bool StopFlag { get => _stop; set => _stop = value; }
        public byte[] Buffer { get => _buffer; }
        public byte[] Buffer2 { get => _buffer2; }
    }
}
