using Grpc.Core;

namespace eShop.Basket.UnitTests.Helpers;

public class TestServerCallContext : ServerCallContext
    {
        private readonly Metadata _requestHeaders;
        private readonly CancellationToken _cancellationToken;
        private readonly Metadata _responseTrailers;
        private readonly AuthContext _authContext;
        private readonly Dictionary<object, object> _userState;
        private WriteOptions _writeOptions;

        public Metadata ResponseHeaders { get; private set; }

        private TestServerCallContext(Metadata requestHeaders, CancellationToken cancellationToken)
        {
            this._requestHeaders = requestHeaders;
            this._cancellationToken = cancellationToken;
            this._responseTrailers = [];
            this._authContext = new AuthContext(string.Empty, []);
            this._userState = [];
        }

        protected override string MethodCore => "MethodName";
        protected override string HostCore => "HostName";
        protected override string PeerCore => "PeerName";
        protected override DateTime DeadlineCore { get; }
        protected override Metadata RequestHeadersCore => this._requestHeaders;
        protected override CancellationToken CancellationTokenCore => this._cancellationToken;
        protected override Metadata ResponseTrailersCore => this._responseTrailers;
        protected override Status StatusCore { get; set; }
        protected override WriteOptions WriteOptionsCore { get => this._writeOptions; set { this._writeOptions = value; } }
        protected override AuthContext AuthContextCore => this._authContext;

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions options)
        {
            throw new NotImplementedException();
        }

        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        {
            if (this.ResponseHeaders != null)
            {
                throw new InvalidOperationException("Response headers have already been written.");
            }

            this.ResponseHeaders = responseHeaders;
            return Task.CompletedTask;
        }

        protected override IDictionary<object, object> UserStateCore => this._userState;

        internal void SetUserState(object key, object value)
            => this._userState[key] = value;

        public static TestServerCallContext Create(Metadata requestHeaders = null, CancellationToken cancellationToken = default)
        {
            return new TestServerCallContext(requestHeaders: new Metadata(), cancellationToken);
        }
    }
