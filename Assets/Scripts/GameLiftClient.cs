using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.GameLift;

public class GameLiftClient : MonoBehaviour
{
   private AmazonGameLiftClient amazonGameLiftClient;

   // Start is called before the first frame update
   void Start()
   {
      amazonGameLiftClient = new AmazonGameLiftClient(RegionEndpoint.USEast1);
      Debug.Log("Client created");
      // amazonGameLiftClient.init();
      
   }

   // Update is called once per frame
   void Update()
   {

   }
}
