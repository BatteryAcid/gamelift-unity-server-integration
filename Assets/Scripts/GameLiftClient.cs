using System;
using System.Threading.Tasks;
using UnityEngine;
using Amazon;
using Amazon.GameLift;
using Amazon.GameLift.Model;
using Amazon.CognitoIdentity;

// TODO: add error handling from example
public class GameLiftClient
{
   private AmazonGameLiftClient _amazonGameLiftClient;
   private string _playerUuid;
   private string CognitoIdentityPool = "us-east-1:78235852-4dff-4efa-988a-175041785b42";
   private string FleetId = "fleet-bd8d3627-2e85-461a-b860-82f1e57771a3"; // TODO: probably don't hardcode this, use alias or something?

   public GameLiftClient()
   {
      Debug.Log("Client created");
      _playerUuid = Guid.NewGuid().ToString();

      setup();
   }

   async private void setup()
   {
      Debug.Log("setup");
      CreateGameLiftClient();
      GameSession gameSession = await SearchGameSessionsAsync();

      if (gameSession == null)
      {
         // create one
         gameSession = await CreateGameSessionAsync();
         // TODO: check if this is null and add handling 
      }
   }

   private void CreateGameLiftClient()
   {
      Debug.Log("CreateGameLiftClient");

      CognitoAWSCredentials credentials = new CognitoAWSCredentials(
         CognitoIdentityPool,
         RegionEndpoint.USEast1
      );

      _amazonGameLiftClient = new AmazonGameLiftClient(credentials, RegionEndpoint.USEast1);
   }

   async private Task<GameSession> CreateGameSessionAsync()
   {
      var createGameSessionRequest = new Amazon.GameLift.Model.CreateGameSessionRequest();
      //cgsreq.AliasId = aliasId;
      createGameSessionRequest.FleetId = FleetId;
      createGameSessionRequest.CreatorId = _playerUuid;
      createGameSessionRequest.MaximumPlayerSessionCount = 4;

      Task<CreateGameSessionResponse> createGameSessionRequestTask = _amazonGameLiftClient.CreateGameSessionAsync(createGameSessionRequest);
      CreateGameSessionResponse createGameSessionResponse = await createGameSessionRequestTask;

      string gameSessionId = createGameSessionResponse.GameSession != null ? createGameSessionResponse.GameSession.GameSessionId : "N/A";
      Debug.Log((int)createGameSessionResponse.HttpStatusCode + " GAME SESSION CREATED: " + gameSessionId);

      return createGameSessionResponse.GameSession;
   }

   async private Task<GameSession> SearchGameSessionsAsync()
   {
      Debug.Log("SearchGameSessions");
      var searchGameSessionsRequest = new SearchGameSessionsRequest();

      //searchGameSessionsRequest.AliasId = "TODO alias or fleet id i think"; // only our game
      searchGameSessionsRequest.FleetId = FleetId;
      searchGameSessionsRequest.FilterExpression = "hasAvailablePlayerSessions=true"; // only ones we can join
      searchGameSessionsRequest.SortExpression = "creationTimeMillis ASC"; // return oldest first
      searchGameSessionsRequest.Limit = 1; // only one session even if there are other valid ones

      Task<SearchGameSessionsResponse> SearchGameSessionsResponseTask = _amazonGameLiftClient.SearchGameSessionsAsync(searchGameSessionsRequest);
      SearchGameSessionsResponse searchGameSessionsResponse = await SearchGameSessionsResponseTask;

      int gameSessionCount = searchGameSessionsResponse.GameSessions.Count;
      Debug.Log($"GameSessionCount:  {gameSessionCount}");

      if (gameSessionCount > 0)
      {
         Debug.Log("We have game sessions!");
         return searchGameSessionsResponse.GameSessions[0];
      }
      return null;
   }

}
