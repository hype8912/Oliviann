namespace Oliviann.ServiceModel
{
    #region Usings

    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    #endregion Usings

    /// <summary>
    /// Represents a base class for a web service client.
    /// </summary>
    /// <typeparam name="TChannel">The type of channel produced by the channel
    /// factory. This type must be either <see cref="IOutputChannel"/> or
    /// <see cref="IRequestChannel"/>.</typeparam>
    public abstract class DataMessenger<TChannel>
    {
        #region Fields

        /// <summary>
        /// Place holder for the <see cref="Context"/> property.
        /// </summary>
        private ChannelFactory<TChannel> _context;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DataMessenger{TChannel}"/> class.
        /// </summary>
        /// <param name="endpointAddress">Sets the endpoint address URI property
        /// if defined; otherwise, null.</param>
        protected DataMessenger(string endpointAddress = null)
        {
            if (!endpointAddress.IsNullOrEmpty())
            {
                this.EndpointAddressUri = endpointAddress;
            }
        }

        #endregion Constructor/Destructor

        #region Events

        /// <summary>
        /// Occurs when a new channel factory instance is created. If the
        /// instance has been retrieved from an already created instance then
        /// the event will not be fired.
        /// </summary>
        protected event Action<object, ChannelFactory<TChannel>> OnFactoryCreated;

        #endregion Events

        #region Properties

#if !NET35 && !NETCOREAPP2_0

        /// <summary>
        /// Gets an instance of the current cache engine.
        /// </summary>
        protected System.Runtime.Caching.MemoryCache Cache
        {
            get { return System.Runtime.Caching.MemoryCache.Default; }
        }

#endif

        /// <summary>
        /// Gets the channel factory context for calling the web service.
        /// </summary>
        protected ChannelFactory<TChannel> Context
        {
            get
            {
                if (this._context == null)
                {
                    this._context = this.CreateChannelFactory();
                    this.OnFactoryCreated.RaiseEvent(this, this._context);
                }

                return this._context;
            }
        }

        /// <summary>Gets or sets the endpoint address for the service proxy
        /// creation.</summary>
        /// <value>The service endpoint address.</value>
        protected string EndpointAddressUri { get; set; }

        #endregion Properties

        /// <summary>
        /// Creates a new channel factory for the specified channel type.
        /// </summary>
        /// <returns>
        /// A new channel factory for the specified channel type
        /// </returns>
        protected virtual ChannelFactory<TChannel> CreateChannelFactory()
        {
            return new ChannelFactory<TChannel>(this.GetChannelBindingProtocol(), this.GetChannelEndpointAddress());
        }

        /// <summary>Gets the channel binding protocol.</summary>
        /// <returns>A binding instance for creating a new
        /// <see cref="ChannelFactory{TChannel}"/> instance.</returns>
        protected abstract Binding GetChannelBindingProtocol();

        /// <summary>Gets the channel endpoint address.</summary>
        /// <returns>An endpoint address instance for creating a new
        /// <see cref="ChannelFactory{TChannel}"/> instance.</returns>
        protected virtual EndpointAddress GetChannelEndpointAddress()
        {
            ADP.CheckArgumentNullOrEmpty(this.EndpointAddressUri, "EndpointAddressUri");
            var url = new Uri(this.EndpointAddressUri);
            return new EndpointAddress(url);
        }
    }
}