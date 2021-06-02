using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Line
{
    public static class Function
    {
        //[FunctionName("Function")]
        //public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req, ILogger log)
        //{
        //    {
        //        try
        //        {
        //            log.LogInformation(req.Content.ReadAsStringAsync().Result);
        //            var channelSecret = Environment.GetEnvironmentVariable("CHANNEL_SEACRET");
        //            var events = await req.GetWebhookEventsAsync(channelSecret);

        //            var app = new LineBotApp();

        //            await app.RunAsync(events);

        //        }
        //        catch (InvalidSignatureException e)
        //        {
        //            return req.CreateResponse(HttpStatusCode.Forbidden, new { e.Message });
        //        }

        //        return req.CreateResponse(HttpStatusCode.OK);
        //    }
        //}
    }
}
