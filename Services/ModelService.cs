using Grpc.Core;
using csDemo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Google.Protobuf;


namespace csDemo.Services;

public class ModelService : Model.ModelBase
{
    private readonly ILogger<ModelService> _logger;
    public ModelService(ILogger<ModelService> logger)
    {
        _logger = logger;
    }

    public override Task<Response> Exec(Request request, ServerCallContext context)
    {
        var v= new Dictionary<string, object>(); 
        var payload = request.Payload; // 获取请求的 payload
        //string jsonPayload = System.Text.Encoding.UTF8.GetString(request.Payload.ToByteArray());
        string jsonPayload = payload.ToStringUtf8();
        //dynamic dynamicPayload = JsonConverter.DeserializeObject(jsonPayload);
        _logger.LogWarning($"request Name: {request.Name},jsonPayload:{jsonPayload}");
        if(jsonPayload.Length>0){
            try{
                //JObject jObject = JObject.Parse(jsonPayload);
                switch (request.Name)
                {          

                    case "hello":
                        if (payload.Length<1)
                        {
                            v = new Dictionary<string, object> { { "code", 400 }, { "message", "参数不足，需要一个参数" } };
                            _logger.LogError("参数不足，需要一个参数" );
                            break;
                        }
                        else
                        {
                            //JProperty firstProperty = jObject.Properties().First();
                            //JToken firstValue = firstProperty.Value;                   
                            v = new Dictionary<string, object> { { "name", "Hello" }, { "value", jsonPayload } };                    
                        }
                        break;

                    default:
                        v = new Dictionary<string, object> { { "name", request.Name }, { "args", jsonPayload } };
                        break;
                }

                // 输出前需要将数据转换成字节数组
                //byte[] bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(v);
                var byteString = ByteString.CopyFromUtf8(JsonConvert.SerializeObject(v));

                // 设置输出数据的类型
                // 支持的类型：Dictionary<string, object>, string, int, float, double, 数组/列表
                return  Task.FromResult(new Response { Response_ = byteString, Type = "map" });
            }catch(Exception ex){
                _logger.LogError("JSON 解析失败：" + ex.Message);
                 return  Task.FromResult(new Response { Response_ = request.Payload, Type = "map" });
            }
        }
        return  Task.FromResult(new Response { Response_ = request.Payload, Type = "map" });
        
    }
}
