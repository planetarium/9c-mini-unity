using System;
using Cysharp.Threading.Tasks;
using Mini9C.ScriptableObjects.EventChannels;
using Newtonsoft.Json.Linq;
using SimpleGraphQL;
using UniRx;
using UnityEngine;

namespace Mini9C.Network
{
    public sealed class GraphQLWorker
    {
        private static readonly Lazy<GraphQLWorker> Singleton =
            new(() => new GraphQLWorker());

        public static GraphQLWorker Instance => Singleton.Value;

        private GraphQLClient _client;

        public void Initialize(GraphQLConfig config)
        {
            _client = new GraphQLClient(config);
        }

        public async UniTask GetBlockTipAsync(Subject<long> blockTipSubject)
        {
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetBlockTipAsync)}");
            var query = _client.FindQuery("ChainQueries", "GetBlockTip");
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetBlockTipAsync)}" +
                      $" query: {query.Source}");
            var request = query.ToRequest();
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetBlockTipAsync)}" +
                      $" request: {request.ToJson(true)}");
            var response = await _client.Send(query.ToRequest());
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetBlockTipAsync)}" +
                      $" response errors: {response}");
            try
            {
                var jsonObj = JObject.Parse(response);
                Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetBlockTipAsync)}" +
                          $" jsonObj: {jsonObj}");

                var blockTip = jsonObj["data"]["chainQuery"]["blockQuery"]["blocks"][0]["index"].Value<long>();
                blockTipSubject?.OnNext(blockTip);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async UniTask GetAgentStateAsync(
            string agentAddress,
            AgentStateEventChannel agentStateEventChannel)
        {
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetAgentStateAsync)}");
            if (string.IsNullOrEmpty(agentAddress))
            {
                Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetAgentStateAsync)}" +
                          $" agentAddress is null or empty.");
                return;
            }

            var query = _client.FindQuery("StateQueries", "GetNCG");
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetAgentStateAsync)}" +
                      $" query: {query.Source}");
            var request = query.ToRequest(new
            {
                address = agentAddress,
            });
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetAgentStateAsync)}" +
                      $" request: {request.ToJson(true)}");
            var response = await _client.Send(request);
            Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetAgentStateAsync)}" +
                      $" response errors: {response}");
            try
            {
                var jsonObj = JObject.Parse(response);
                Debug.Log($"{nameof(GraphQLWorker)} {nameof(GetAgentStateAsync)}" +
                          $" jsonObj: {jsonObj}");

                var ncg = jsonObj["data"]["stateQuery"]["balance"]["quantity"].Value<decimal>();
                agentStateEventChannel.NCG.OnNext(ncg);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
