using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER

namespace Opc.Ua.Client
{
    /// <summary>
    /// Manages a session with a server.
    /// </summary>
    public class SessionActivitySource : ISession  // Isession to check TODO:
    {
        #region Constructors
        /// <summary>
        /// Constructs a new instance of the <see cref="Session"/> class.
        /// </summary>
        /// <param name="session"></param>
        public SessionActivitySource(Session session)
        {
            m_session = session;
        }

        #endregion
        /// <summary>
        /// Activity Source Name
        /// </summary>
        public const string OpcUaClientActivitySourceName = "Opc.Ua.Client-ActivitySource";

        private bool m_disposedValue;
        private ISession m_session;

        /// <inheritdoc/>
        public event KeepAliveEventHandler KeepAlive;

        /// <inheritdoc/>
        public event NotificationEventHandler Notification;

        /// <inheritdoc/>
        public event PublishErrorEventHandler PublishError;

        /// <inheritdoc/>
        public event PublishSequenceNumbersToAcknowledgeEventHandler PublishSequenceNumbersToAcknowledge;

        /// <inheritdoc/>
        public event EventHandler SubscriptionsChanged;

        /// <inheritdoc/>
        public event EventHandler SessionClosing;

        /// <inheritdoc/>
        public event RenewUserIdentityEventHandler RenewUserIdentity;

        /// <summary>
        /// Activity Source
        /// </summary>
        ///
        public static ActivitySource ActivitySrc { get; } = new ActivitySource(OpcUaClientActivitySourceName);

        /// <inheritdoc/>
        public ISessionFactory SessionFactory => throw new NotImplementedException(); // implement custom session factory for activty source

        /// <inheritdoc/>
        public ConfiguredEndpoint ConfiguredEndpoint => m_session.ConfiguredEndpoint;

        /// <inheritdoc/>
        public string SessionName => m_session.SessionName;

        /// <inheritdoc/>
        public double SessionTimeout => m_session.SessionTimeout;

        /// <inheritdoc/>
        public object Handle => m_session.Handle;

        /// <inheritdoc/>
        public IUserIdentity Identity => m_session.Identity;

        /// <inheritdoc/>
        public IEnumerable<IUserIdentity> IdentityHistory => m_session.IdentityHistory;

        /// <inheritdoc/>
        public NamespaceTable NamespaceUris => m_session.NamespaceUris;

        /// <inheritdoc/>
        public StringTable ServerUris => m_session.ServerUris;

        /// <inheritdoc/>
        public ISystemContext SystemContext => m_session.SystemContext;

        /// <inheritdoc/>
        public IEncodeableFactory Factory => m_session.Factory;

        /// <inheritdoc/>
        public ITypeTable TypeTree => m_session.TypeTree;

        /// <inheritdoc/>
        public INodeCache NodeCache => m_session.NodeCache;

        /// <inheritdoc/>
        public FilterContext FilterContext => m_session.FilterContext;

        /// <inheritdoc/>
        public StringCollection PreferredLocales => m_session.PreferredLocales;

        /// <inheritdoc/>
        public IReadOnlyDictionary<NodeId, DataDictionary> DataTypeSystem => m_session.DataTypeSystem;

        /// <inheritdoc/>
        public IEnumerable<Subscription> Subscriptions => m_session.Subscriptions;

        /// <inheritdoc/>
        public int SubscriptionCount => m_session.SubscriptionCount;

        /// <inheritdoc/>
        public bool DeleteSubscriptionsOnClose
        {
            get => m_session.DeleteSubscriptionsOnClose;
            set => m_session.DeleteSubscriptionsOnClose = value;
        }

        /// <inheritdoc/>
        public Subscription DefaultSubscription
        {
            get => m_session.DefaultSubscription;
            set => m_session.DefaultSubscription = value;
        }

        /// <inheritdoc/>
        public int KeepAliveInterval
        {
            get => m_session.KeepAliveInterval;
            set => m_session.KeepAliveInterval = value;
        }

        /// <inheritdoc/>
        public bool KeepAliveStopped => m_session.KeepAliveStopped;

        /// <inheritdoc/>
        public DateTime LastKeepAliveTime => m_session.LastKeepAliveTime;

        /// <inheritdoc/>
        public int OutstandingRequestCount => m_session.OutstandingRequestCount;

        /// <inheritdoc/>
        public int DefunctRequestCount => m_session.DefunctRequestCount;

        /// <inheritdoc/>
        public int GoodPublishRequestCount => m_session.GoodPublishRequestCount;

        /// <inheritdoc/>
        public int MinPublishRequestCount
        {
            get => m_session.MinPublishRequestCount;
            set => m_session.MinPublishRequestCount = value;
        }

        /// <inheritdoc/>
        public OperationLimits OperationLimits => m_session.OperationLimits;

        /// <inheritdoc/>
        public bool TransferSubscriptionsOnReconnect
        {
            get => m_session.TransferSubscriptionsOnReconnect;
            set => m_session.TransferSubscriptionsOnReconnect = value;
        }

        /// <inheritdoc/>
        public NodeId SessionId => m_session.SessionId;

        /// <inheritdoc/>
        public bool Connected => m_session.Connected;

        /// <inheritdoc/>
        public EndpointDescription Endpoint => m_session.Endpoint;

        /// <inheritdoc/>
        public EndpointConfiguration EndpointConfiguration => m_session.EndpointConfiguration;

        /// <inheritdoc/>
        public IServiceMessageContext MessageContext => m_session.MessageContext;

        /// <inheritdoc/>
        public ITransportChannel TransportChannel => m_session.TransportChannel;

        /// <inheritdoc/>
        public DiagnosticsMasks ReturnDiagnostics
        {
            get => m_session.ReturnDiagnostics;
            set => m_session.ReturnDiagnostics = value;
        }

        /// <inheritdoc/>
        public int OperationTimeout
        {
            get => m_session.OperationTimeout;
            set => m_session.OperationTimeout = value;
        }

        /// <inheritdoc/>
        public bool Disposed => m_session.Disposed;

        /// <inheritdoc/>
        public void Reconnect()
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Reconnect"))
            {
                m_session.Reconnect();
            }
        }

        /// <inheritdoc/>
        public void Reconnect(ITransportWaitingConnection connection)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Reconnect"))
            {
                m_session.Reconnect(connection);
            }
        }

        /// <inheritdoc/>
        public void Reconnect(ITransportChannel channel)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Reconnect"))
            {
                m_session.Reconnect(channel);
            }
        }

        /// <inheritdoc/>
        public void Save(string filePath)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Save"))
            {
                m_session.Save(filePath);
            }
        }

        /// <inheritdoc/>
        public void Save(Stream stream, IEnumerable<Subscription> subscriptions)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Save"))
            {
                m_session.Save(stream, subscriptions);
            }
        }

        /// <inheritdoc/>
        public void Save(string filePath, IEnumerable<Subscription> subscriptions)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Save"))
            {
                m_session.Save(filePath, subscriptions);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Subscription> Load(Stream stream, bool transferSubscriptions = false)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Load"))
            {
                return m_session.Load(stream, transferSubscriptions);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Subscription> Load(string filePath, bool transferSubscriptions = false)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Load"))
            {
                return m_session.Load(filePath, transferSubscriptions);
            }
        }

        /// <inheritdoc/>
        public void FetchNamespaceTables()
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FetchNamespaceTables"))
            {
                m_session.FetchNamespaceTables();
            }
        }

        /// <inheritdoc/>
        public void FetchTypeTree(ExpandedNodeId typeId)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FetchTypeTree"))
            {
                m_session.FetchTypeTree(typeId);
            }
        }

        /// <inheritdoc/>
        public void FetchTypeTree(ExpandedNodeIdCollection typeIds)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FetchTypeTree"))
            {
                m_session.FetchTypeTree(typeIds);
            }
        }

        /// <inheritdoc/>
        public ReferenceDescriptionCollection ReadAvailableEncodings(NodeId variableId)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadAvailableEncodings"))
            {
                return m_session.ReadAvailableEncodings(variableId);
            }
        }

        /// <inheritdoc/>
        public ReferenceDescription FindDataDescription(NodeId encodingId)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FindDataDescription"))
            {
                return m_session.FindDataDescription(encodingId);
            }
        }

        /// <inheritdoc/>
        public async Task<DataDictionary> FindDataDictionary(NodeId descriptionId)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FindDataDictionary"))
            {
                return await m_session.FindDataDictionary(descriptionId).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<DataDictionary> LoadDataDictionary(ReferenceDescription dictionaryNode, bool forceReload = false)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("LoadDataDictionary"))
            {
                return await m_session.LoadDataDictionary(dictionaryNode, forceReload).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<NodeId, DataDictionary>> LoadDataTypeSystem(NodeId dataTypeSystem = null)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("LoadDataTypeSystem"))
            {
                return await m_session.LoadDataTypeSystem(dataTypeSystem).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public Node ReadNode(NodeId nodeId)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNode"))
            {
                return m_session.ReadNode(nodeId);
            }
        }

        /// <inheritdoc/>
        public Node ReadNode(NodeId nodeId, NodeClass nodeClass, bool optionalAttributes = true)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNode"))
            {
                return m_session.ReadNode(nodeId, nodeClass, optionalAttributes);
            }
        }

        /// <inheritdoc/>
        public void ReadNodes(IList<NodeId> nodeIds, out IList<Node> nodeCollection, out IList<ServiceResult> errors, bool optionalAttributes = false)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNodes"))
            {
                m_session.ReadNodes(nodeIds, out nodeCollection, out errors, optionalAttributes);
            }
        }

        /// <inheritdoc/>
        public void ReadNodes(IList<NodeId> nodeIds, NodeClass nodeClass, out IList<Node> nodeCollection, out IList<ServiceResult> errors, bool optionalAttributes = false)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNodes"))
            {
                m_session.ReadNodes(nodeIds, nodeClass, out nodeCollection, out errors, optionalAttributes);
            }
        }

        /// <inheritdoc/>
        public DataValue ReadValue(NodeId nodeId)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadValue"))
            {
                return m_session.ReadValue(nodeId);
            }
        }

        /// <inheritdoc/>
        public object ReadValue(NodeId nodeId, Type expectedType)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadValue"))
            {
                return m_session.ReadValue(nodeId, expectedType);
            }
        }

        /// <inheritdoc/>
        public void ReadValues(IList<NodeId> nodeIds, out DataValueCollection values, out IList<ServiceResult> errors)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadValues"))
            {
                m_session.ReadValues(nodeIds, out values, out errors);
            }
        }

        /// <inheritdoc/>
        public ReferenceDescriptionCollection FetchReferences(NodeId nodeId)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FetchReferences"))
            {
                return m_session.FetchReferences(nodeId);
            }
        }

        /// <inheritdoc/>
        public void FetchReferences(IList<NodeId> nodeIds, out IList<ReferenceDescriptionCollection> referenceDescriptions, out IList<ServiceResult> errors)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FetchReferences"))
            {
                m_session.FetchReferences(nodeIds, out referenceDescriptions, out errors);
            }
        }

        /// <inheritdoc/>
        public void Open(string sessionName, IUserIdentity identity)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Open"))
            {
                m_session.Open(sessionName, identity);
            }
        }

        /// <inheritdoc/>
        public void Open(string sessionName, uint sessionTimeout, IUserIdentity identity, IList<string> preferredLocales)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Open"))
            {
                m_session.Open(sessionName, sessionTimeout, identity, preferredLocales);
            }
        }

        /// <inheritdoc/>
        public void Open(string sessionName, uint sessionTimeout, IUserIdentity identity, IList<string> preferredLocales, bool checkDomain)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Open"))
            {
                m_session.Open(sessionName, sessionTimeout, identity, preferredLocales, checkDomain);
            }
        }

        /// <inheritdoc/>
        public void ChangePreferredLocales(StringCollection preferredLocales)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ChangePreferredLocales"))
            {
                m_session.ChangePreferredLocales(preferredLocales);
            }
        }

        /// <inheritdoc/>
        public void UpdateSession(IUserIdentity identity, StringCollection preferredLocales)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("UpdateSession"))
            {
                m_session.UpdateSession(identity, preferredLocales);
            }
        }

        /// <inheritdoc/>
        public void FindComponentIds(NodeId instanceId, IList<string> componentPaths, out NodeIdCollection componentIds, out List<ServiceResult> errors)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("FindComponentIds"))
            {
                m_session.FindComponentIds(instanceId, componentPaths, out componentIds, out errors);
            }
        }

        /// <inheritdoc/>
        public void ReadValues(IList<NodeId> variableIds, IList<Type> expectedTypes, out List<object> values, out List<ServiceResult> errors)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadValues"))
            {
                m_session.ReadValues(variableIds, expectedTypes, out values, out errors);
            }
        }

        /// <inheritdoc/>
        public void ReadDisplayName(IList<NodeId> nodeIds, out IList<string> displayNames, out IList<ServiceResult> errors)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadDisplayName"))
            {
                m_session.ReadDisplayName(nodeIds, out displayNames, out errors);
            }
        }
        /// <inheritdoc/>
        public async Task<(IList<Node>, IList<ServiceResult>)> ReadNodesAsync(IList<NodeId> nodeIds, NodeClass nodeClass, bool optionalAttributes = false, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNodesAsync"))
            {
                return await m_session.ReadNodesAsync(nodeIds, nodeClass, optionalAttributes, ct);
            }
        }

        /// <inheritdoc/>
        public async Task<DataValue> ReadValueAsync(NodeId nodeId, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadValueAsync"))
            {
                return await m_session.ReadValueAsync(nodeId, ct);
            }
        }

        /// <inheritdoc/>
        public async Task<Node> ReadNodeAsync(NodeId nodeId, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNodeAsync"))
            {
                return await m_session.ReadNodeAsync(nodeId, ct);
            }
        }

        /// <inheritdoc/>
        public async Task<Node> ReadNodeAsync(NodeId nodeId, NodeClass nodeClass, bool optionalAttributes = true, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNodeAsync"))
            {
                return await m_session.ReadNodeAsync(nodeId, nodeClass, optionalAttributes, ct);
            }
        }

        /// <inheritdoc/>
        public async Task<(IList<Node>, IList<ServiceResult>)> ReadNodesAsync(IList<NodeId> nodeIds, bool optionalAttributes = false, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadNodesAsync"))
            {
                return await m_session.ReadNodesAsync(nodeIds, optionalAttributes, ct);
            }
        }

        /// <inheritdoc/>
        public async Task<(DataValueCollection, IList<ServiceResult>)> ReadValuesAsync(IList<NodeId> nodeIds, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadValuesAsync"))
            {
                return await m_session.ReadValuesAsync(nodeIds, ct);
            }
        }

        /// <inheritdoc/>
        public StatusCode Close(int timeout)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Close"))
            {
                return m_session.Close(timeout);
            }
        }

        /// <inheritdoc/>
        public StatusCode Close(bool closeChannel)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Close"))
            {
                return m_session.Close(closeChannel);
            }
        }

        /// <inheritdoc/>
        public StatusCode Close(int timeout, bool closeChannel)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Close"))
            {
                return m_session.Close(timeout, closeChannel);
            }
        }

        /// <inheritdoc/>
        public async Task<StatusCode> CloseAsync(CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CloseAsync"))
            {
                return await m_session.CloseAsync(ct);
            }
        }

        /// <inheritdoc/>
        public async Task<StatusCode> CloseAsync(bool closeChannel, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CloseAsync"))
            {
                return await m_session.CloseAsync(closeChannel, ct);
            }
        }

        /// <inheritdoc/>
        public async Task<StatusCode> CloseAsync(int timeout, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CloseAsync"))
            {
                return await m_session.CloseAsync(timeout, ct);
            }
        }

        /// <inheritdoc/>
        public async Task<StatusCode> CloseAsync(int timeout, bool closeChannel, CancellationToken ct = default)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CloseAsync"))
            {
                return await m_session.CloseAsync(timeout, closeChannel, ct);
            }
        }

        /// <inheritdoc/>
        public bool AddSubscription(Subscription subscription)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("AddSubscription"))
            {
                return m_session.AddSubscription(subscription);
            }
        }

        /// <inheritdoc/>
        public bool RemoveSubscription(Subscription subscription)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RemoveSubscription"))
            {
                return m_session.RemoveSubscription(subscription);
            }
        }

        /// <inheritdoc/>
        public bool RemoveSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RemoveSubscriptions"))
            {
                return m_session.RemoveSubscriptions(subscriptions);
            }
        }

        /// <inheritdoc/>
        public bool TransferSubscriptions(SubscriptionCollection subscriptions, bool sendInitialValues)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("TransferSubscriptions"))
            {
                return m_session.TransferSubscriptions(subscriptions, sendInitialValues);
            }
        }

        /// <inheritdoc/>
        public bool RemoveTransferredSubscription(Subscription subscription)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RemoveTransferredSubscription"))
            {
                return m_session.RemoveTransferredSubscription(subscription);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveSubscriptionAsync(Subscription subscription)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RemoveSubscriptionAsync"))
            {
                return await m_session.RemoveSubscriptionAsync(subscription);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveSubscriptionsAsync(IEnumerable<Subscription> subscriptions)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RemoveSubscriptionsAsync"))
            {
                return await m_session.RemoveSubscriptionsAsync(subscriptions);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader Browse(RequestHeader requestHeader, ViewDescription view, NodeId nodeToBrowse, uint maxResultsToReturn, BrowseDirection browseDirection, NodeId referenceTypeId, bool includeSubtypes, uint nodeClassMask, out byte[] continuationPoint, out ReferenceDescriptionCollection references)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Browse"))
            {
                return m_session.Browse(requestHeader, view, nodeToBrowse, maxResultsToReturn, browseDirection, referenceTypeId, includeSubtypes, nodeClassMask, out continuationPoint, out references);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginBrowse(RequestHeader requestHeader, ViewDescription view, NodeId nodeToBrowse, uint maxResultsToReturn, BrowseDirection browseDirection, NodeId referenceTypeId, bool includeSubtypes, uint nodeClassMask, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndBrowse(IAsyncResult result, out byte[] continuationPoint, out ReferenceDescriptionCollection references)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader BrowseNext(RequestHeader requestHeader, bool releaseContinuationPoint, byte[] continuationPoint, out byte[] revisedContinuationPoint, out ReferenceDescriptionCollection references)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("BrowseNext"))
            {
                return m_session.BrowseNext(requestHeader, releaseContinuationPoint, continuationPoint, out revisedContinuationPoint, out references);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginBrowseNext(RequestHeader requestHeader, bool releaseContinuationPoint, byte[] continuationPoint, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndBrowseNext(IAsyncResult result, out byte[] revisedContinuationPoint, out ReferenceDescriptionCollection references)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<object> Call(NodeId objectId, NodeId methodId, params object[] args)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Call"))
            {
                return m_session.Call(objectId, methodId, args);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginPublish(int timeout)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Republish(uint subscriptionId, uint sequenceNumber)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Republish"))
            {
                return m_session.Republish(subscriptionId, sequenceNumber);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader CreateSession(RequestHeader requestHeader, ApplicationDescription clientDescription, string serverUri, string endpointUrl, string sessionName, byte[] clientNonce, byte[] clientCertificate, double requestedSessionTimeout, uint maxResponseMessageSize, out NodeId sessionId, out NodeId authenticationToken, out double revisedSessionTimeout, out byte[] serverNonce, out byte[] serverCertificate, out EndpointDescriptionCollection serverEndpoints, out SignedSoftwareCertificateCollection serverSoftwareCertificates, out SignatureData serverSignature, out uint maxRequestMessageSize)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CreateSession"))
            {
                return m_session.CreateSession(requestHeader, clientDescription, serverUri, endpointUrl, sessionName, clientNonce, clientCertificate, requestedSessionTimeout, maxResponseMessageSize, out sessionId, out authenticationToken, out revisedSessionTimeout, out serverNonce, out serverCertificate, out serverEndpoints, out serverSoftwareCertificates, out serverSignature, out maxRequestMessageSize);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCreateSession(RequestHeader requestHeader, ApplicationDescription clientDescription, string serverUri, string endpointUrl, string sessionName, byte[] clientNonce, byte[] clientCertificate, double requestedSessionTimeout, uint maxResponseMessageSize, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndCreateSession(IAsyncResult result, out NodeId sessionId, out NodeId authenticationToken, out double revisedSessionTimeout, out byte[] serverNonce, out byte[] serverCertificate, out EndpointDescriptionCollection serverEndpoints, out SignedSoftwareCertificateCollection serverSoftwareCertificates, out SignatureData serverSignature, out uint maxRequestMessageSize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<CreateSessionResponse> CreateSessionAsync(RequestHeader requestHeader, ApplicationDescription clientDescription, string serverUri, string endpointUrl, string sessionName, byte[] clientNonce, byte[] clientCertificate, double requestedSessionTimeout, uint maxResponseMessageSize, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CreateSessionAsync"))
            {
                return await m_session.CreateSessionAsync(requestHeader, clientDescription, serverUri, endpointUrl, sessionName, clientNonce, clientCertificate, requestedSessionTimeout, maxResponseMessageSize, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader ActivateSession(RequestHeader requestHeader, SignatureData clientSignature, SignedSoftwareCertificateCollection clientSoftwareCertificates, StringCollection localeIds, ExtensionObject userIdentityToken, SignatureData userTokenSignature, out byte[] serverNonce, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ActivateSession"))
            {
                return m_session.ActivateSession(requestHeader, clientSignature, clientSoftwareCertificates, localeIds, userIdentityToken, userTokenSignature, out serverNonce, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginActivateSession(RequestHeader requestHeader, SignatureData clientSignature, SignedSoftwareCertificateCollection clientSoftwareCertificates, StringCollection localeIds, ExtensionObject userIdentityToken, SignatureData userTokenSignature, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndActivateSession(IAsyncResult result, out byte[] serverNonce, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<ActivateSessionResponse> ActivateSessionAsync(RequestHeader requestHeader, SignatureData clientSignature, SignedSoftwareCertificateCollection clientSoftwareCertificates, StringCollection localeIds, ExtensionObject userIdentityToken, SignatureData userTokenSignature, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ActivateSessionAsync"))
            {
                return await m_session.ActivateSessionAsync(requestHeader, clientSignature, clientSoftwareCertificates, localeIds, userIdentityToken, userTokenSignature, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader CloseSession(RequestHeader requestHeader, bool deleteSubscriptions)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CloseSession"))
            {
                return m_session.CloseSession(requestHeader, deleteSubscriptions);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCloseSession(RequestHeader requestHeader, bool deleteSubscriptions, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public ResponseHeader EndCloseSession(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<CloseSessionResponse> CloseSessionAsync(RequestHeader requestHeader, bool deleteSubscriptions, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CloseSessionAsync"))
            {
                return await m_session.CloseSessionAsync(requestHeader, deleteSubscriptions, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader Cancel(RequestHeader requestHeader, uint requestHandle, out uint cancelCount)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Cancel"))
            {
                return m_session.Cancel(requestHeader, requestHandle, out cancelCount);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCancel(RequestHeader requestHeader, uint requestHandle, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndCancel(IAsyncResult result, out uint cancelCount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<CancelResponse> CancelAsync(RequestHeader requestHeader, uint requestHandle, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CancelAsync"))
            {
                return m_session.CancelAsync(requestHeader, requestHandle, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader AddNodes(RequestHeader requestHeader, AddNodesItemCollection nodesToAdd, out AddNodesResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("AddNodes"))
            {
                return m_session.AddNodes(requestHeader, nodesToAdd, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginAddNodes(RequestHeader requestHeader, AddNodesItemCollection nodesToAdd, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndAddNodes(IAsyncResult result, out AddNodesResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<AddNodesResponse> AddNodesAsync(RequestHeader requestHeader, AddNodesItemCollection nodesToAdd, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("AddNodesAsync"))
            {
                return m_session.AddNodesAsync(requestHeader, nodesToAdd, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader AddReferences(RequestHeader requestHeader, AddReferencesItemCollection referencesToAdd, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("AddReferences"))
            {
                return m_session.AddReferences(requestHeader, referencesToAdd, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginAddReferences(RequestHeader requestHeader, AddReferencesItemCollection referencesToAdd, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndAddReferences(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<AddReferencesResponse> AddReferencesAsync(RequestHeader requestHeader, AddReferencesItemCollection referencesToAdd, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("AddReferencesAsync"))
            {
                return m_session.AddReferencesAsync(requestHeader, referencesToAdd, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader DeleteNodes(RequestHeader requestHeader, DeleteNodesItemCollection nodesToDelete, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteNodes"))
            {
                return m_session.DeleteNodes(requestHeader, nodesToDelete, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginDeleteNodes(RequestHeader requestHeader, DeleteNodesItemCollection nodesToDelete, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndDeleteNodes(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<DeleteNodesResponse> DeleteNodesAsync(RequestHeader requestHeader, DeleteNodesItemCollection nodesToDelete, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteNodesAsync"))
            {
                return m_session.DeleteNodesAsync(requestHeader, nodesToDelete, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader DeleteReferences(RequestHeader requestHeader, DeleteReferencesItemCollection referencesToDelete, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteReferences"))
            {
                return m_session.DeleteReferences(requestHeader, referencesToDelete, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginDeleteReferences(RequestHeader requestHeader, DeleteReferencesItemCollection referencesToDelete, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndDeleteReferences(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<DeleteReferencesResponse> DeleteReferencesAsync(RequestHeader requestHeader, DeleteReferencesItemCollection referencesToDelete, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteReferencesAsync"))
            {
                return m_session.DeleteReferencesAsync(requestHeader, referencesToDelete, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader Browse(RequestHeader requestHeader, ViewDescription view, uint requestedMaxReferencesPerNode, BrowseDescriptionCollection nodesToBrowse, out BrowseResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Browse"))
            {
                return m_session.Browse(requestHeader, view, requestedMaxReferencesPerNode, nodesToBrowse, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginBrowse(RequestHeader requestHeader, ViewDescription view, uint requestedMaxReferencesPerNode, BrowseDescriptionCollection nodesToBrowse, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndBrowse(IAsyncResult result, out BrowseResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<BrowseResponse> BrowseAsync(RequestHeader requestHeader, ViewDescription view, uint requestedMaxReferencesPerNode, BrowseDescriptionCollection nodesToBrowse, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("BrowseAsync"))
            {
                return m_session.BrowseAsync(requestHeader, view, requestedMaxReferencesPerNode, nodesToBrowse, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader BrowseNext(RequestHeader requestHeader, bool releaseContinuationPoints, ByteStringCollection continuationPoints, out BrowseResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("BrowseNext"))
            {
                return m_session.BrowseNext(requestHeader, releaseContinuationPoints, continuationPoints, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginBrowseNext(RequestHeader requestHeader, bool releaseContinuationPoints, ByteStringCollection continuationPoints, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndBrowseNext(IAsyncResult result, out BrowseResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<BrowseNextResponse> BrowseNextAsync(RequestHeader requestHeader, bool releaseContinuationPoints, ByteStringCollection continuationPoints, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("BrowseNextAsync"))
            {
                return m_session.BrowseNextAsync(requestHeader, releaseContinuationPoints, continuationPoints, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader TranslateBrowsePathsToNodeIds(RequestHeader requestHeader, BrowsePathCollection browsePaths, out BrowsePathResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("TranslateBrowsePathsToNodeIds"))
            {
                return m_session.TranslateBrowsePathsToNodeIds(requestHeader, browsePaths, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginTranslateBrowsePathsToNodeIds(RequestHeader requestHeader, BrowsePathCollection browsePaths, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndTranslateBrowsePathsToNodeIds(IAsyncResult result, out BrowsePathResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TranslateBrowsePathsToNodeIdsResponse> TranslateBrowsePathsToNodeIdsAsync(RequestHeader requestHeader, BrowsePathCollection browsePaths, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("TranslateBrowsePathsToNodeIdsAsync"))
            {
                return m_session.TranslateBrowsePathsToNodeIdsAsync(requestHeader, browsePaths, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader RegisterNodes(RequestHeader requestHeader, NodeIdCollection nodesToRegister, out NodeIdCollection registeredNodeIds)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RegisterNodes"))
            {
                return m_session.RegisterNodes(requestHeader, nodesToRegister, out registeredNodeIds);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginRegisterNodes(RequestHeader requestHeader, NodeIdCollection nodesToRegister, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndRegisterNodes(IAsyncResult result, out NodeIdCollection registeredNodeIds)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<RegisterNodesResponse> RegisterNodesAsync(RequestHeader requestHeader, NodeIdCollection nodesToRegister, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RegisterNodesAsync"))
            {
                return m_session.RegisterNodesAsync(requestHeader, nodesToRegister, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader UnregisterNodes(RequestHeader requestHeader, NodeIdCollection nodesToUnregister)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("UnregisterNodes"))
            {
                return m_session.UnregisterNodes(requestHeader, nodesToUnregister);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginUnregisterNodes(RequestHeader requestHeader, NodeIdCollection nodesToUnregister, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndUnregisterNodes(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<UnregisterNodesResponse> UnregisterNodesAsync(RequestHeader requestHeader, NodeIdCollection nodesToUnregister, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("UnregisterNodesAsync"))
            {
                return m_session.UnregisterNodesAsync(requestHeader, nodesToUnregister, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader QueryFirst(RequestHeader requestHeader, ViewDescription view, NodeTypeDescriptionCollection nodeTypes, ContentFilter filter, uint maxDataSetsToReturn, uint maxReferencesToReturn, out QueryDataSetCollection queryDataSets, out byte[] continuationPoint, out ParsingResultCollection parsingResults, out DiagnosticInfoCollection diagnosticInfos, out ContentFilterResult filterResult)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("QueryFirst"))
            {
                return m_session.QueryFirst(requestHeader, view, nodeTypes, filter, maxDataSetsToReturn, maxReferencesToReturn, out queryDataSets, out continuationPoint, out parsingResults, out diagnosticInfos, out filterResult);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginQueryFirst(RequestHeader requestHeader, ViewDescription view, NodeTypeDescriptionCollection nodeTypes, ContentFilter filter, uint maxDataSetsToReturn, uint maxReferencesToReturn, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndQueryFirst(IAsyncResult result, out QueryDataSetCollection queryDataSets, out byte[] continuationPoint, out ParsingResultCollection parsingResults, out DiagnosticInfoCollection diagnosticInfos, out ContentFilterResult filterResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<QueryFirstResponse> QueryFirstAsync(RequestHeader requestHeader, ViewDescription view, NodeTypeDescriptionCollection nodeTypes, ContentFilter filter, uint maxDataSetsToReturn, uint maxReferencesToReturn, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("QueryFirstAsync"))
            {
                return m_session.QueryFirstAsync(requestHeader, view, nodeTypes, filter, maxDataSetsToReturn, maxReferencesToReturn, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader QueryNext(RequestHeader requestHeader, bool releaseContinuationPoint, byte[] continuationPoint, out QueryDataSetCollection queryDataSets, out byte[] revisedContinuationPoint)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("QueryNext"))
            {
                return m_session.QueryNext(requestHeader, releaseContinuationPoint, continuationPoint, out queryDataSets, out revisedContinuationPoint);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginQueryNext(RequestHeader requestHeader, bool releaseContinuationPoint, byte[] continuationPoint, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndQueryNext(IAsyncResult result, out QueryDataSetCollection queryDataSets, out byte[] revisedContinuationPoint)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<QueryNextResponse> QueryNextAsync(RequestHeader requestHeader, bool releaseContinuationPoint, byte[] continuationPoint, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("QueryNextAsync"))
            {
                return m_session.QueryNextAsync(requestHeader, releaseContinuationPoint, continuationPoint, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader Read(RequestHeader requestHeader, double maxAge, TimestampsToReturn timestampsToReturn, ReadValueIdCollection nodesToRead, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Read"))
            {
                return m_session.Read(requestHeader, maxAge, timestampsToReturn, nodesToRead, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginRead(RequestHeader requestHeader, double maxAge, TimestampsToReturn timestampsToReturn, ReadValueIdCollection nodesToRead, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public ResponseHeader EndRead(IAsyncResult result, out DataValueCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ReadResponse> ReadAsync(RequestHeader requestHeader, double maxAge, TimestampsToReturn timestampsToReturn, ReadValueIdCollection nodesToRead, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ReadAsync"))
            {
                return m_session.ReadAsync(requestHeader, maxAge, timestampsToReturn, nodesToRead, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader HistoryRead(RequestHeader requestHeader, ExtensionObject historyReadDetails, TimestampsToReturn timestampsToReturn, bool releaseContinuationPoints, HistoryReadValueIdCollection nodesToRead, out HistoryReadResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("HistoryRead"))
            {
                return m_session.HistoryRead(requestHeader, historyReadDetails, timestampsToReturn, releaseContinuationPoints, nodesToRead, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginHistoryRead(RequestHeader requestHeader, ExtensionObject historyReadDetails, TimestampsToReturn timestampsToReturn, bool releaseContinuationPoints, HistoryReadValueIdCollection nodesToRead, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndHistoryRead(IAsyncResult result, out HistoryReadResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<HistoryReadResponse> HistoryReadAsync(RequestHeader requestHeader, ExtensionObject historyReadDetails, TimestampsToReturn timestampsToReturn, bool releaseContinuationPoints, HistoryReadValueIdCollection nodesToRead, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("HistoryReadAsync"))
            {
                return m_session.HistoryReadAsync(requestHeader, historyReadDetails, timestampsToReturn, releaseContinuationPoints, nodesToRead, ct);
            } 
        }

        /// <inheritdoc/>
        public ResponseHeader Write(RequestHeader requestHeader, WriteValueCollection nodesToWrite, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Write"))
            {
                return m_session.Write(requestHeader, nodesToWrite, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginWrite(RequestHeader requestHeader, WriteValueCollection nodesToWrite, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndWrite(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<WriteResponse> WriteAsync(RequestHeader requestHeader, WriteValueCollection nodesToWrite, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("WriteAsync"))
            {
                return m_session.WriteAsync(requestHeader, nodesToWrite, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader HistoryUpdate(RequestHeader requestHeader, ExtensionObjectCollection historyUpdateDetails, out HistoryUpdateResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("HistoryUpdate"))
            {
                return m_session.HistoryUpdate(requestHeader, historyUpdateDetails, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginHistoryUpdate(RequestHeader requestHeader, ExtensionObjectCollection historyUpdateDetails, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndHistoryUpdate(IAsyncResult result, out HistoryUpdateResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<HistoryUpdateResponse> HistoryUpdateAsync(RequestHeader requestHeader, ExtensionObjectCollection historyUpdateDetails, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("HistoryUpdateAsync"))
            {
                return m_session.HistoryUpdateAsync(requestHeader, historyUpdateDetails, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader Call(RequestHeader requestHeader, CallMethodRequestCollection methodsToCall, out CallMethodResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Call"))
            {
                return m_session.Call(requestHeader, methodsToCall, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCall(RequestHeader requestHeader, CallMethodRequestCollection methodsToCall, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndCall(IAsyncResult result, out CallMethodResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<CallResponse> CallAsync(RequestHeader requestHeader, CallMethodRequestCollection methodsToCall, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CallAsync"))
            {
                return m_session.CallAsync(requestHeader, methodsToCall, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader CreateMonitoredItems(RequestHeader requestHeader, uint subscriptionId, TimestampsToReturn timestampsToReturn, MonitoredItemCreateRequestCollection itemsToCreate, out MonitoredItemCreateResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CreateMonitoredItems"))
            {
                return m_session.CreateMonitoredItems(requestHeader, subscriptionId, timestampsToReturn, itemsToCreate, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCreateMonitoredItems(RequestHeader requestHeader, uint subscriptionId, TimestampsToReturn timestampsToReturn, MonitoredItemCreateRequestCollection itemsToCreate, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndCreateMonitoredItems(IAsyncResult result, out MonitoredItemCreateResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<CreateMonitoredItemsResponse> CreateMonitoredItemsAsync(RequestHeader requestHeader, uint subscriptionId, TimestampsToReturn timestampsToReturn, MonitoredItemCreateRequestCollection itemsToCreate, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CreateMonitoredItemsAsync"))
            {
                return m_session.CreateMonitoredItemsAsync(requestHeader, subscriptionId, timestampsToReturn, itemsToCreate, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader ModifyMonitoredItems(RequestHeader requestHeader, uint subscriptionId, TimestampsToReturn timestampsToReturn, MonitoredItemModifyRequestCollection itemsToModify, out MonitoredItemModifyResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ModifyMonitoredItems"))
            {
                return m_session.ModifyMonitoredItems(requestHeader, subscriptionId, timestampsToReturn, itemsToModify, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginModifyMonitoredItems(RequestHeader requestHeader, uint subscriptionId, TimestampsToReturn timestampsToReturn, MonitoredItemModifyRequestCollection itemsToModify, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndModifyMonitoredItems(IAsyncResult result, out MonitoredItemModifyResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ModifyMonitoredItemsResponse> ModifyMonitoredItemsAsync(RequestHeader requestHeader, uint subscriptionId, TimestampsToReturn timestampsToReturn, MonitoredItemModifyRequestCollection itemsToModify, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ModifyMonitoredItemsAsync"))
            {
                return m_session.ModifyMonitoredItemsAsync(requestHeader, subscriptionId, timestampsToReturn, itemsToModify, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader SetMonitoringMode(RequestHeader requestHeader, uint subscriptionId, MonitoringMode monitoringMode, UInt32Collection monitoredItemIds, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("SetMonitoringMode"))
            {
                return m_session.SetMonitoringMode(requestHeader, subscriptionId, monitoringMode, monitoredItemIds, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginSetMonitoringMode(RequestHeader requestHeader, uint subscriptionId, MonitoringMode monitoringMode, UInt32Collection monitoredItemIds, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndSetMonitoringMode(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SetMonitoringModeResponse> SetMonitoringModeAsync(RequestHeader requestHeader, uint subscriptionId, MonitoringMode monitoringMode, UInt32Collection monitoredItemIds, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("SetMonitoringModeAsync"))
            {
                return m_session.SetMonitoringModeAsync(requestHeader, subscriptionId, monitoringMode, monitoredItemIds, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader SetTriggering(RequestHeader requestHeader, uint subscriptionId, uint triggeringItemId, UInt32Collection linksToAdd, UInt32Collection linksToRemove, out StatusCodeCollection addResults, out DiagnosticInfoCollection addDiagnosticInfos, out StatusCodeCollection removeResults, out DiagnosticInfoCollection removeDiagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("SetTriggering"))
            {
                return m_session.SetTriggering(requestHeader, subscriptionId, triggeringItemId, linksToAdd, linksToRemove, out addResults, out addDiagnosticInfos, out removeResults, out removeDiagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginSetTriggering(RequestHeader requestHeader, uint subscriptionId, uint triggeringItemId, UInt32Collection linksToAdd, UInt32Collection linksToRemove, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndSetTriggering(IAsyncResult result, out StatusCodeCollection addResults, out DiagnosticInfoCollection addDiagnosticInfos, out StatusCodeCollection removeResults, out DiagnosticInfoCollection removeDiagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SetTriggeringResponse> SetTriggeringAsync(RequestHeader requestHeader, uint subscriptionId, uint triggeringItemId, UInt32Collection linksToAdd, UInt32Collection linksToRemove, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("SetTriggeringAsync"))
            {
                return m_session.SetTriggeringAsync(requestHeader, subscriptionId, triggeringItemId, linksToAdd, linksToRemove, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader DeleteMonitoredItems(RequestHeader requestHeader, uint subscriptionId, UInt32Collection monitoredItemIds, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteMonitoredItems"))
            {
                return m_session.DeleteMonitoredItems(requestHeader, subscriptionId, monitoredItemIds, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginDeleteMonitoredItems(RequestHeader requestHeader, uint subscriptionId, UInt32Collection monitoredItemIds, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndDeleteMonitoredItems(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<DeleteMonitoredItemsResponse> DeleteMonitoredItemsAsync(RequestHeader requestHeader, uint subscriptionId, UInt32Collection monitoredItemIds, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteMonitoredItemsAsync"))
            {
                return m_session.DeleteMonitoredItemsAsync(requestHeader, subscriptionId, monitoredItemIds, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader CreateSubscription(RequestHeader requestHeader, double requestedPublishingInterval, uint requestedLifetimeCount, uint requestedMaxKeepAliveCount, uint maxNotificationsPerPublish, bool publishingEnabled, byte priority, out uint subscriptionId, out double revisedPublishingInterval, out uint revisedLifetimeCount, out uint revisedMaxKeepAliveCount)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CreateSubscription"))
            {
                return m_session.CreateSubscription(requestHeader, requestedPublishingInterval, requestedLifetimeCount, requestedMaxKeepAliveCount, maxNotificationsPerPublish, publishingEnabled, priority, out subscriptionId, out revisedPublishingInterval, out revisedLifetimeCount, out revisedMaxKeepAliveCount);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginCreateSubscription(RequestHeader requestHeader, double requestedPublishingInterval, uint requestedLifetimeCount, uint requestedMaxKeepAliveCount, uint maxNotificationsPerPublish, bool publishingEnabled, byte priority, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndCreateSubscription(IAsyncResult result, out uint subscriptionId, out double revisedPublishingInterval, out uint revisedLifetimeCount, out uint revisedMaxKeepAliveCount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<CreateSubscriptionResponse> CreateSubscriptionAsync(RequestHeader requestHeader, double requestedPublishingInterval, uint requestedLifetimeCount, uint requestedMaxKeepAliveCount, uint maxNotificationsPerPublish, bool publishingEnabled, byte priority, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("CreateSubscriptionAsync"))
            {
                return m_session.CreateSubscriptionAsync(requestHeader, requestedPublishingInterval, requestedLifetimeCount, requestedMaxKeepAliveCount, maxNotificationsPerPublish, publishingEnabled, priority, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader ModifySubscription(RequestHeader requestHeader, uint subscriptionId, double requestedPublishingInterval, uint requestedLifetimeCount, uint requestedMaxKeepAliveCount, uint maxNotificationsPerPublish, byte priority, out double revisedPublishingInterval, out uint revisedLifetimeCount, out uint revisedMaxKeepAliveCount)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ModifySubscription"))
            {
                return m_session.ModifySubscription(requestHeader, subscriptionId, requestedPublishingInterval, requestedLifetimeCount, requestedMaxKeepAliveCount, maxNotificationsPerPublish, priority, out revisedPublishingInterval, out revisedLifetimeCount, out revisedMaxKeepAliveCount);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginModifySubscription(RequestHeader requestHeader, uint subscriptionId, double requestedPublishingInterval, uint requestedLifetimeCount, uint requestedMaxKeepAliveCount, uint maxNotificationsPerPublish, byte priority, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndModifySubscription(IAsyncResult result, out double revisedPublishingInterval, out uint revisedLifetimeCount, out uint revisedMaxKeepAliveCount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ModifySubscriptionResponse> ModifySubscriptionAsync(RequestHeader requestHeader, uint subscriptionId, double requestedPublishingInterval, uint requestedLifetimeCount, uint requestedMaxKeepAliveCount, uint maxNotificationsPerPublish, byte priority, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("ModifySubscriptionAsync"))
            {
                return m_session.ModifySubscriptionAsync(requestHeader, subscriptionId, requestedPublishingInterval, requestedLifetimeCount, requestedMaxKeepAliveCount, maxNotificationsPerPublish, priority, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader SetPublishingMode(RequestHeader requestHeader, bool publishingEnabled, UInt32Collection subscriptionIds, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("SetPublishingMode"))
            {
                return m_session.SetPublishingMode(requestHeader, publishingEnabled, subscriptionIds, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginSetPublishingMode(RequestHeader requestHeader, bool publishingEnabled, UInt32Collection subscriptionIds, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndSetPublishingMode(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SetPublishingModeResponse> SetPublishingModeAsync(RequestHeader requestHeader, bool publishingEnabled, UInt32Collection subscriptionIds, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("SetPublishingModeAsync"))
            {
                return m_session.SetPublishingModeAsync(requestHeader, publishingEnabled, subscriptionIds, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader Publish(RequestHeader requestHeader, SubscriptionAcknowledgementCollection subscriptionAcknowledgements, out uint subscriptionId, out UInt32Collection availableSequenceNumbers, out bool moreNotifications, out NotificationMessage notificationMessage, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Publish"))
            {
                return m_session.Publish(requestHeader, subscriptionAcknowledgements, out subscriptionId, out availableSequenceNumbers, out moreNotifications, out notificationMessage, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginPublish(RequestHeader requestHeader, SubscriptionAcknowledgementCollection subscriptionAcknowledgements, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndPublish(IAsyncResult result, out uint subscriptionId, out UInt32Collection availableSequenceNumbers, out bool moreNotifications, out NotificationMessage notificationMessage, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<PublishResponse> PublishAsync(RequestHeader requestHeader, SubscriptionAcknowledgementCollection subscriptionAcknowledgements, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("PublishAsync"))
            {
                return m_session.PublishAsync(requestHeader, subscriptionAcknowledgements, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader Republish(RequestHeader requestHeader, uint subscriptionId, uint retransmitSequenceNumber, out NotificationMessage notificationMessage)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Republish"))
            {
                return m_session.Republish(requestHeader, subscriptionId, retransmitSequenceNumber, out notificationMessage);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginRepublish(RequestHeader requestHeader, uint subscriptionId, uint retransmitSequenceNumber, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndRepublish(IAsyncResult result, out NotificationMessage notificationMessage)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<RepublishResponse> RepublishAsync(RequestHeader requestHeader, uint subscriptionId, uint retransmitSequenceNumber, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("RepublishAsync"))
            {
                return m_session.RepublishAsync(requestHeader, subscriptionId, retransmitSequenceNumber, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader TransferSubscriptions(RequestHeader requestHeader, UInt32Collection subscriptionIds, bool sendInitialValues, out TransferResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("TransferSubscriptions"))
            {
                return m_session.TransferSubscriptions(requestHeader, subscriptionIds, sendInitialValues, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginTransferSubscriptions(RequestHeader requestHeader, UInt32Collection subscriptionIds, bool sendInitialValues, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndTransferSubscriptions(IAsyncResult result, out TransferResultCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TransferSubscriptionsResponse> TransferSubscriptionsAsync(RequestHeader requestHeader, UInt32Collection subscriptionIds, bool sendInitialValues, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("TransferSubscriptionsAsync"))
            {
                return m_session.TransferSubscriptionsAsync(requestHeader, subscriptionIds, sendInitialValues, ct);
            }
        }

        /// <inheritdoc/>
        public ResponseHeader DeleteSubscriptions(RequestHeader requestHeader, UInt32Collection subscriptionIds, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteSubscriptions"))
            {
                return m_session.DeleteSubscriptions(requestHeader, subscriptionIds, out results, out diagnosticInfos);
            }
        }

        /// <inheritdoc/>
        public IAsyncResult BeginDeleteSubscriptions(RequestHeader requestHeader, UInt32Collection subscriptionIds, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ResponseHeader EndDeleteSubscriptions(IAsyncResult result, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<DeleteSubscriptionsResponse> DeleteSubscriptionsAsync(RequestHeader requestHeader, UInt32Collection subscriptionIds, CancellationToken ct)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DeleteSubscriptionsAsync"))
            {
                return m_session.DeleteSubscriptionsAsync(requestHeader, subscriptionIds, ct);
            }
        }

        /// <inheritdoc/>
        public void AttachChannel(ITransportChannel channel)
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("AttachChannel"))
            {
                m_session.AttachChannel(channel);
            }
        }

        /// <inheritdoc/>
        public void DetachChannel()
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("DetachChannel"))
            {
                m_session.DetachChannel();
            }
        }

        /// <inheritdoc/>
        public StatusCode Close()
        {
            // Start an Activity.
            using (Activity activity = ActivitySrc.StartActivity("Close"))
            {
                return m_session.Close();
            }
        }

        /// <inheritdoc/>
        public uint NewRequestHandle()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                m_disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SessionActivitySource()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
#endif
