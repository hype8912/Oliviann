namespace Oliviann.ServiceModel
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    #endregion Usings

    /// <summary>
    /// Represents a collection of commonly used extension methods for extending
    /// <see cref="ChannelFactory{TChannel}"/>.
    /// </summary>
    public static class ChannelFactoryExtensions
    {
        /// <summary>
        /// Executes the service proxy request using the specified channel
        /// <paramref name="factory"/> to update data.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <param name="factory">The channel factory instance.</param>
        /// <param name="queryDelegate">The query delegate.</param>
        /// <exception cref="Exception">Re-throws any exceptions received by
        /// executing the delegate.</exception>
        public static void ExecuteRequest<TChannel>(
                                                    this ChannelFactory<TChannel> factory,
                                                    Action<TChannel> queryDelegate)
        {
            if (factory == null || queryDelegate == null)
            {
                return;
            }

            TChannel channel = factory.CreateChannel();
#if DEBUG
            // Changes the timeout to 10 minutes so you can step through server
            // code if you need to without getting a timeout error.
            ((IClientChannel)channel).OperationTimeout = TimeSpan.FromMinutes(10);
#endif

            try
            {
                queryDelegate(channel);
                var client = (IClientChannel)channel;
                if (client.State != CommunicationState.Faulted)
                {
                    client.Close();
                }
                else
                {
                    client.Abort();
                }
            }
            catch (Exception)
            {
                ((IClientChannel)channel).Abort();
                throw;
            }
            finally
            {
                ((IClientChannel)channel).DisposeSafe();
            }
        }

        /// <summary>
        /// Executes the service proxy request using the specified channel
        /// <paramref name="factory"/> to update data.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <typeparam name="TIn">The type of the input collection object.
        /// </typeparam>
        /// <param name="factory">The channel factory instance.</param>
        /// <param name="queryDelegate">The query delegate.</param>
        /// <param name="items">The items.</param>
        /// <returns><c>True</c> if the operation completed successfully;
        /// otherwise, <c>false</c>.</returns>
        public static bool ExecuteRequest<TChannel, TIn>(
                                                       this ChannelFactory<TChannel> factory,
                                                       Func<TChannel, TIn[], bool> queryDelegate,
                                                       IEnumerable<TIn> items)
        {
            if (factory == null || queryDelegate == null || items == null)
            {
                return false;
            }

            bool result = factory.ExecuteRequest(q => queryDelegate(q, items.ToArray()));
            return result;
        }

        /// <summary>
        /// Executes the service proxy request using the specified channel
        /// <paramref name="factory"/>.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel factory.
        /// </typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="factory">The channel factory instance.</param>
        /// <param name="readDelegate">The delegate action to be executed using
        /// the factory.</param>
        /// <returns>A single object type <typeparamref name="TResult"/>;
        /// otherwise, the default value.</returns>
        /// <exception cref="Exception">Re-throws any exceptions received by
        /// executing the delegate.</exception>
        public static TResult ExecuteRequest<TChannel, TResult>(
                                                               this ChannelFactory<TChannel> factory,
                                                               Func<TChannel, TResult> readDelegate)
        {
            if (factory == null || readDelegate == null)
            {
                return default;
            }

            TChannel channel = factory.CreateChannel();
#if DEBUG
            // Changes the timeout to 10 minutes so you can step through server
            // code if you need to without getting a timeout error.
            ((IClientChannel)channel).OperationTimeout = TimeSpan.FromMinutes(10);
#endif
            TResult result;
            try
            {
                result = readDelegate(channel);
                var client = (IClientChannel)channel;
                CommunicationState state = client.State;
                if (state != CommunicationState.Faulted)
                {
                    client.Close();
                }
                else
                {
                    client.Abort();
                }
            }
            catch (Exception)
            {
                ((IClientChannel)channel).Abort();
                throw;
            }
            finally
            {
                ((IClientChannel)channel).DisposeSafe();
            }

            return result;
        }
    }
}